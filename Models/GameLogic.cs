using System;
using System.Collections.Generic;
using System.Windows; // Для Point
using System.Windows.Controls; // Для TextBox

namespace Game_Sudoku.Models
{
    public class GameLogic
    {
        private readonly int[,] _generatedMap;
        private readonly int[,] _map;
        private readonly TextBox[,] _textBoxes;
        private readonly Random _random;

        public GameLogic(int[,] generatedMap, int[,] map, TextBox[,] textBoxes)
        {
            _generatedMap = generatedMap;
            _map = map;
            _textBoxes = textBoxes;
            _random = new Random();
        }

        public void Hint()
        {
            List<Point> hidden = new List<Point>();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (_generatedMap[j, i] != _map[j, i])
                        hidden.Add(new Point(j, i));
                }
            }

            if (hidden.Count > 0)
            {
                Point gift = hidden[_random.Next(hidden.Count)];
                int x = (int)gift.X, y = (int)gift.Y;
                _map[x, y] = _generatedMap[x, y];
                _textBoxes[x, y].Text = _map[x, y].ToString();
            }
        }
    }
}