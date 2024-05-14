using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Game_Sudoku
{
    public class Sudoku
    {
        // Фабрика для створення інстанцій класу Sudoku
        public class SudokuFactory
        {
            public static Sudoku CreateSudoku(TextBox[,] textBoxes, int hiddenCount = 45)
            {
                return new Sudoku(textBoxes, hiddenCount);
            }
        }

        // Декоратор для обрання числа в клітинці Sudoku
        public class SudokuCellDecorator
        {
            public static void DecorateCell(TextBox textBox, int number)
            {
                textBox.Text = number == 0 ? "" : number.ToString();

                if (number != 0)
                {
                    textBox.IsReadOnly = true;
                    textBox.FontWeight = FontWeights.Bold;
                }
                else
                {
                    textBox.IsReadOnly = false;
                    textBox.FontWeight = FontWeights.Regular;
                }
            }
        }

        // Зберігання стану Sudoku
        public class SudokuMemento
        {
            private readonly int[,] _state;

            public SudokuMemento(int[,] state)
            {
                _state = state.Clone() as int[,];
            }

            public int[,] GetState()
            {
                return _state.Clone() as int[,];
            }
        }

            public readonly TextBox[,] _textBoxes;
            public int[,] _map;
            public readonly int[,] _preparedMap;
            public readonly int[,] _generatedMap;
            public readonly int _hiddenCount;
            public readonly Random _random;

            public Sudoku(TextBox[,] textBoxes, int hiddenCount = 45)
            {
                _textBoxes = textBoxes;
                _hiddenCount = hiddenCount;
                _random = new Random(Guid.NewGuid().GetHashCode());

                _map = new int[9, 9];
                _preparedMap = new int[9, 9];
                _generatedMap = new int[9, 9];

                GenerateMap();
            }

            //Генерація поля
            public void GenerateMap()
            {
                int[] firstRow = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                int a, b, c, swaps = _random.Next(3, 10), j = 0, shift;
                for (int i = 0; i < swaps; i += 1)
                {
                    a = _random.Next(0, 9); b = _random.Next(0, 9);
                    (firstRow[a], firstRow[b]) = (firstRow[b], firstRow[a]);
                }

                for (int i = 0; i < 9; i += 1)
                {
                    _map[j, i] = firstRow[i];
                }

                for (j = 1; j < 9; j += 1)
                {
                    if (j % 3 == 0) shift = 1; else shift = 3;
                    for (int i = 0; i < 9; i += 1)
                    {
                        _map[j, i] = _map[j - 1, (i + shift) % 9];
                    }
                }

                swaps = _random.Next(0, 3);
                for (int i = 0; i < swaps; i += 1)
                {
                    c = _random.Next(0, 9);

                    a = _random.Next(3 * (c / 3), 3 * (c / 3 + 1)); b = _random.Next(3 * (c / 3), 3 * (c / 3 + 1));
                    RowSwap(a, b);
                }

                swaps = _random.Next(0, 3);
                for (int i = 0; i < swaps; i += 1)
                {
                    c = _random.Next(0, 9);

                    a = _random.Next(3 * (c / 3), 3 * (c / 3 + 1)); b = _random.Next(3 * (c / 3), 3 * (c / 3 + 1));
                    ColumnSwap(a, b);
                }

                PrepareMap();
                UpdateNumbers();
            }

            //Обміни рядків і стовпчиків
            public void RowSwap(int a, int b)
            {
                for (int i = 0; i < 9; i += 1)
                {
                    (_map[a, i], _map[b, i]) = (_map[b, i], _map[a, i]);
                }
            }
            public void ColumnSwap(int a, int b)
            {
                for (int i = 0; i < 9; i += 1)
                {
                    (_map[i, a], _map[i, b]) = (_map[i, b], _map[i, a]);
                }
            }

            //Запис чисел в текстбокси
            public void UpdateNumbers()
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (_map[i, j] == 0)
                            _textBoxes[i, j].Text = "";
                        else
                            _textBoxes[i, j].Text = _map[i, j].ToString();

                        if (_preparedMap[i, j] != 0)
                        {
                            _textBoxes[i, j].IsReadOnly = true;
                            _textBoxes[i, j].FontWeight = FontWeights.Bold;
                        }
                        else
                        {
                            _textBoxes[i, j].IsReadOnly = false;
                            _textBoxes[i, j].FontWeight = FontWeights.Regular;
                        }
                    }
                }
            }

            //Приховування чисел
            public void PrepareMap()
            {
                List<Point> shown = new List<Point>();

                for (int i = 0; i < 9; i += 1)
                {
                    for (int j = 0; j < 9; j += 1)
                    {
                        _generatedMap[i, j] = _map[i, j];
                        _preparedMap[i, j] = 0;
                        shown.Add(new Point(i, j));
                    }
                }

                for (int i = 0; i < _hiddenCount; i += 1)
                {
                    shown.RemoveAt(_random.Next(0, shown.Count));
                }

                foreach (Point p in shown)
                {
                    _preparedMap[(int)p.X, (int)p.Y] = _generatedMap[(int)p.X, (int)p.Y];
                }

                for (int i = 0; i < 9; i += 1)
                {
                    for (int j = 0; j < 9; j += 1)
                    {
                        _map[i, j] = _preparedMap[i, j];
                    }
                }
            }
            public void ClearMap()
            {
                for (int i = 0; i < 9; i += 1)
                {
                    for (int j = 0; j < 9; j += 1)
                    {
                        _map[i, j] = _preparedMap[i, j];
                    }
                }
                UpdateNumbers();
            }

            public void Hint()
            {
                List<Point> hidden = new List<Point>();
                for (int i = 0; i < 9; i += 1)
                {
                    for (int j = 0; j < 9; j += 1)
                    {
                        if (_generatedMap[j, i] != _map[j, i])
                            hidden.Add(new Point(j, i));
                    }
                }

                Point gift = hidden[_random.Next(0, hidden.Count)];

                int x = (int)gift.X, y = (int)gift.Y;

                _map[x, y] = _generatedMap[x, y];
                _textBoxes[x, y].Text = _map[x, y].ToString();
            }

            public bool CheckMap()
            {
                bool[] appearances = { false, false, false, false, false, false, false, false, false };
                for (int i = 0; i < 9; i += 1)
                {
                    for (int j = 0; j < 9; j += 1)
                    {
                        if (_map[i, j] != 0)
                        {
                            if (appearances[_map[i, j] - 1])
                            {
                                return false;
                            }
                            else
                            {
                                appearances[_map[i, j] - 1] = true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    appearances = new[] { false, false, false, false, false, false, false, false, false };
                }

                for (int i = 0; i < 9; i += 1)
                {
                    for (int j = 0; j < 9; j += 1)
                    {
                        if (_map[j, i] != 0)
                        {
                            if (appearances[_map[j, i] - 1])
                            {
                                return false;
                            }
                            else
                            {
                                appearances[_map[j, i] - 1] = true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    appearances = new[] { false, false, false, false, false, false, false, false, false };
                }

                for (int i = 0; i < 3; i += 1)
                {
                    for (int j = 0; j < 3; j += 1)
                    {
                        for (int x = 0; x < 3; x += 1)
                        {
                            for (int y = 0; y < 3; y += 1)
                            {
                                if (_map[i * 3 + x, j * 3 + y] != 0)
                                {
                                    if (appearances[_map[i * 3 + x, j * 3 + y] - 1])
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        appearances[_map[i * 3 + x, j * 3 + y] - 1] = true;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                        appearances = new[] { false, false, false, false, false, false, false, false, false };
                    }
                }

                return true;
            }
        }
    }





