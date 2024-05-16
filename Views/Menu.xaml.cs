using System.Windows;
using System.Windows.Controls;
using Game_Sudoku.Utilities;

namespace Game_Sudoku.Views
{
    /// <summary>
    /// Логика взаимодействия для Sudoku.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void PlayButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Game.CheckFile())
            {
                NavigationHelper.NavigateToPage(NavigationService, new NewOrLoadPage());
            }
            else
            {
                NavigationHelper.NavigateToPage(NavigationService, new DifficultyPage());
            }
        }
        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateToPage(NavigationService, new Rules());
        }
        private void ExitButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
