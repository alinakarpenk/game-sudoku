using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Game_Sudoku.Models
{
    public class Sudoku
    {
        public readonly TextBox[,] TextBoxes;
        public int[,] _map;
        public readonly int[,] _preparedMap;
        public readonly int[,] _generatedMap;
        public readonly int _hiddenCount;
        public readonly Random _random;

        public Sudoku(TextBox[,] textBoxes, int hiddenCount = 45)
        {
            TextBoxes = textBoxes;
            _hiddenCount = hiddenCount;
            _random = new Random(Guid.NewGuid().GetHashCode());

            _map = new int[9, 9];
            _preparedMap = new int[9, 9];
            _generatedMap = new int[9, 9];

            GenerateMap();
        }

        
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

        private void SwapElements(int[,] array, int a, int b, int size, string type)
        {
            for (int i = 0; i < size; i++)
            {
                if (type == "Row")
                    (array[a, i], array[b, i]) = (array[b, i], array[a, i]);
                else if (type == "Column")
                    (array[i, a], array[i, b]) = (array[i, b], array[i, a]);
            }
        }

        public void RowSwap(int a, int b)
        {
            SwapElements(_map, a, b, 9, "Row");
        }

        public void ColumnSwap(int a, int b)
        {
            SwapElements(_map, a, b, 9, "Column");
        }
        
        public void UpdateNumbers()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (_map[i, j] == 0)
                        TextBoxes[i, j].Text = "";
                    else
                        TextBoxes[i, j].Text = _map[i, j].ToString();

                    if (_preparedMap[i, j] != 0)
                    {
                        TextBoxes[i, j].IsReadOnly = true;
                        TextBoxes[i, j].FontWeight = FontWeights.Bold;
                    }
                    else
                    {
                        TextBoxes[i, j].IsReadOnly = false;
                        TextBoxes[i, j].FontWeight = FontWeights.Regular;
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
            TextBoxes[x, y].Text = _map[x, y].ToString();
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





