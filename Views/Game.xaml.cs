using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Game_Sudoku.Models;

namespace Game_Sudoku.Views
{
    /// <summary>
    /// Логика взаимодействия для Start.xaml
    /// </summary>

    public partial class Game : Page
    {
        //public Random r = new Random(Guid.NewGuid().GetHashCode());
        //public int[,] map = new int[9, 9];
        TextBox[,] TextBoxes = new TextBox[9, 9];

        private bool[,] Painted = new bool[,]
        {
            { true, true, true, false, false, false, true, true, true },
            { true, true, true, false, false, false, true, true, true },
            { true, true, true, false, false, false, true, true, true },
            { false, false, false, true, true, true, false, false, false },
            { false, false, false, true, true, true, false, false, false },
            { false, false, false, true, true, true, false, false, false },
            { true, true, true, false, false, false, true, true, true },
            { true, true, true, false, false, false, true, true, true },
            { true, true, true, false, false, false, true, true, true },
        };

        public Sudoku game;

        public int hiddenCount { get; set; }
        public int turnLimit { get; set; }
        public int turns = 0;
        public int hints = 3;
        
        DispatcherTimer UpdateTimer = new DispatcherTimer();
        
        public long TotalTime = 0;
        public long StartTime;
        public long PauseTime = 0;
        public long PauseStartTime;
        public long SaveTime = 0;
        
        public bool fromFile { get; set; }

        public string DifficultyName { get; set; }

        public Game(string difName = "", int hiddenCount = 45, int turnLimit = 0, int hints = 3, bool fromFile = false)
        {
            InitializeComponent();
            this.DifficultyName = difName;
            this.hiddenCount = hiddenCount;
            this.turnLimit = turnLimit;
            this.hints = hints;
            this.fromFile = fromFile;
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SolidColorBrush PaintedBrush = new SolidColorBrush(Color.FromRgb(222, 246, 250));

            for (int i = 0; i < 9; i += 1)
            {
                for (int j = 0; j < 9; j += 1)
                {
                    TextBoxes[i, j] = new TextBox();
                    TextBoxes[i, j].Name = $"textBox_{i}_{j}";
                    TextBoxes[i, j].MaxLength = 1;
                    TextBoxes[i, j].MaxLines = 1;
                    TextBoxes[i, j].ContextMenu = null;
                    TextBoxes[i, j].HorizontalContentAlignment = HorizontalAlignment.Center;
                    TextBoxes[i, j].VerticalContentAlignment = VerticalAlignment.Center;
                    if (Painted[i, j])
                    {
                        TextBoxes[i, j].Background = PaintedBrush;
                    }

                    MapGrid.Children.Add(TextBoxes[i, j]);
                    Grid.SetColumn(TextBoxes[i, j], j + (j / 3));
                    Grid.SetRow(TextBoxes[i, j], i + (i / 3));
                }
            }

            game = new Sudoku(TextBoxes, hiddenCount);
            
            if(fromFile) LoadFile();
            
            StartTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            
            for (int i = 0; i < 9; i += 1)
            {
                for (int j = 0; j < 9; j += 1)
                {
                    TextBoxes[i, j].TextChanged += (object o, TextChangedEventArgs args) =>
                    {
                        TextBox textBox = args.Source as TextBox;
                        if (textBox == null || textBox.Name == null)
                            return;
                        string[] splits = textBox.Name.Split('_');
                        int x = int.Parse(splits[1]);
                        int y = int.Parse(splits[2]);

                        if (textBox.Text != "")
                        {
                            game._map[x, y] = int.Parse(textBox.Text);
                            turns += 1;
                            LabelTRN.Content = $"Turn: {turns}";
                            if (turnLimit > 0) 
                                LabelTRN.Content += $"/{turnLimit}";

                            
                        }
                        else
                            game._map[x, y] = 0;
                        
                        SaveFile();
                        
                        if (game.SudokuChecker.CheckMap())
                        {
                            string details = $"Difficulty: {DifficultyName}";
                            details += $"\r\nTime: {(TotalTime / 60)}:{(TotalTime % 60).ToString("00")}";
                            details += $"\r\nTurns: {turns}";
                            if (turnLimit > 0) 
                                details += $"/{turnLimit}";
                            FinishGame("You Win!", details);
                        } else if (turns >= turnLimit && turnLimit > 0)
                        {
                            FinishGame("You Lose...", "You've reached the limit of turns");
                        }
                    };

                    TextBoxes[i, j].PreviewTextInput += (object o, TextCompositionEventArgs args) =>
                    {
                        Regex rgx = new Regex("[1-9]");
                        args.Handled = !(rgx.IsMatch(args.Text) || args.Text == "");
                    };
                    
                    TextBoxes[i, j].CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, (object o, ExecutedRoutedEventArgs args) =>
                    {
                        args.Handled = true;
                    }));
                }
               
                UpdateTimer.Interval = TimeSpan.FromMilliseconds(100);
                UpdateTimer.Tick += (_, __) =>
                {
                    if (PauseMenu.Visibility == Visibility.Visible || ClearMenu.Visibility == Visibility.Visible)
                        return;
                    TotalTime = ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - StartTime) / 1000;
                    TotalTime -= PauseTime;
                    TotalTime += SaveTime;

                    LabelTime.Content = $"Time: {(TotalTime / 60)}:{(TotalTime % 60).ToString("00")}";
                    
                    SaveFile();
                };
                UpdateTimer.Start();
                
                HintButton.Content = $"(Hint {hints})";
                if (hints <= 0) HintButton.Visibility = Visibility.Collapsed;
                
                LabelTRN.Content = $"Turn: {turns}";
                if (turnLimit > 0) 
                    LabelTRN.Content += $"/{turnLimit}";
            }
        }

        public void FinishGame(string message, string details)
        {
            GameEndMessageLabel.Content = message;
            GameEndDetailsLabel.Content = details;
            GameEnd.Visibility = Visibility.Visible;
            UpdateTimer.Stop();
            ResetFile();
        }
        
        public static bool CheckFile()
        {
            try
            {
                StreamReader sr = new StreamReader("data.sudoku");
                string fileData = sr.ReadLine();
                sr.Close();

                string[] data = fileData.Split(' ');

                long test;
                int[,] testMap = new int[9, 9];
                
                test = int.Parse(data[0]);
                test = int.Parse(data[1]);
                test = int.Parse(data[2]);
                test = int.Parse(data[3]);
                test = long.Parse(data[4]);
                test = long.Parse(data[5]);
                string testString = data[6];

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        testMap[i, j] = int.Parse(data[7 + i * 9 + j]);
                    }
                }

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        testMap[i, j] = int.Parse(data[88 + i * 9 + j]);
                    }
                }

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        testMap[i, j] = int.Parse(data[169 + i * 9 + j]);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void LoadFile()
        {
            try
            {
                StreamReader sr = new StreamReader("data.sudoku");
                string fileData = sr.ReadLine();
                sr.Close();

                string[] data = fileData.Split(' ');

                hiddenCount = int.Parse(data[0]);
                turnLimit = int.Parse(data[1]);
                turns = int.Parse(data[2]);
                hints = int.Parse(data[3]);
                SaveTime = long.Parse(data[4]);
                PauseTime = long.Parse(data[5]);
                DifficultyName = data[6];

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        game._map[i, j] = int.Parse(data[7 + i * 9 + j]);
                    }
                }

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        game._preparedMap[i, j] = int.Parse(data[88 + i * 9 + j]);
                    }
                }

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        game._generatedMap[i, j] = int.Parse(data[169 + i * 9 + j]);
                    }
                }
                
                game.UpdateNumbers();
            }
            catch (Exception)
            {
                
            }
        }

        public void SaveFile()
        {
            try
            {
                StreamWriter sw = new StreamWriter("data.sudoku");

                sw.Write($"{hiddenCount} ");
                sw.Write($"{turnLimit} ");
                sw.Write($"{turns} ");
                sw.Write($"{hints} ");
                sw.Write($"{TotalTime} ");
                sw.Write($"{PauseTime} ");
                sw.Write($"{DifficultyName} ");

                string mapString = "", preparedMapString = "", generatetMapString = "";

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        mapString += $"{game._map[i, j]} ";
                        preparedMapString += $"{game._preparedMap[i, j]} ";
                        generatetMapString += $"{game._generatedMap[i, j]} ";
                    }
                }

                sw.Write(mapString);
                sw.Write(preparedMapString);
                sw.Write(generatetMapString);

                sw.Close();
            }
            catch (Exception)
            {
                
            }
        }

        public void ResetFile()
        {
            try
            {
                StreamWriter sw = new StreamWriter("data.sudoku");

                sw.WriteLine("NoData");

                sw.Close();
            }
            catch (Exception)
            {
                
            }
        }

        private void PauseButton_OnClick(object sender, RoutedEventArgs e)
        {
            PauseMenu.Visibility = Visibility.Visible;
            PauseStartTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void ContinueButton_OnClick(object sender, RoutedEventArgs e)
        {
            PauseMenu.Visibility = Visibility.Collapsed;
            PauseTime += ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - PauseStartTime) / 1000;
        }

        private void ClearButton_OnClick(object sender, RoutedEventArgs e)
        {
            ClearMenu.Visibility = Visibility.Visible;
            PauseStartTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void ClearYesButton_OnClick(object sender, RoutedEventArgs e)
        {
            ClearMenu.Visibility = Visibility.Collapsed;
            game.ClearMap();
            PauseTime += ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - PauseStartTime) / 1000;
        }
        
        private void ClearNoButton_OnClick(object sender, RoutedEventArgs e)
        {
            ClearMenu.Visibility = Visibility.Collapsed;
            PauseTime += ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - PauseStartTime) / 1000;
        }

        private void MainMenuButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Menu());
            UpdateTimer.Stop();
        }

        private void PlayAgainButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DifficultyPage());
            UpdateTimer.Stop();
        }

        private void RestartButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Game(DifficultyName, hiddenCount, turnLimit, hints));
            UpdateTimer.Stop();
        }

        private void HintButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (hints <= 0) return;
            hints -= 1;
            if (hints == 0) HintButton.Visibility = Visibility.Collapsed;

            HintButton.Content = $"(Hint {hints})";
            
            game.Hint();
            if (game.SudokuChecker.CheckMap())
            {
                string details = $"Difficulty: {DifficultyName}";
                details += $"\r\nTime: {(TotalTime / 60)}:{(TotalTime % 60).ToString("00")}";
                details += $"\r\nTurns: {turns}";
                if (turnLimit > 0) 
                    details += $"/{turnLimit}";
                FinishGame("You Win!", details);
            }
        }
    }
}

