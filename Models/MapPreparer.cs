using System;
using System.Collections.Generic;
using System.Windows;

namespace Game_Sudoku.Models
{
    public class MapPreparer
    {
        private const int Size = 9;
        private readonly int[,] _generatedMap;
        private readonly int[,] _preparedMap;
        private readonly int[,] _map;
        private readonly int _hiddenCount;
        private readonly Random _random;

        public MapPreparer(int[,] generatedMap, int[,] preparedMap, int[,] map, int hiddenCount, Random random)
        {
            _generatedMap = generatedMap;
            _preparedMap = preparedMap;
            _map = map;
            _hiddenCount = hiddenCount;
            _random = random;
        }

        public void PrepareMap()
        {
            List<Point> shown = new List<Point>();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _generatedMap[i, j] = _map[i, j];
                    _preparedMap[i, j] = 0;
                    shown.Add(new Point(i, j));
                }
            }

            HideRandomCells(shown);
            CopyPreparedToMap();
        }

        private void HideRandomCells(List<Point> shown)
        {
            for (int i = 0; i < _hiddenCount; i++)
            {
                if (shown.Count > 0)
                {
                    int indexToRemove = _random.Next(shown.Count);
                    shown.RemoveAt(indexToRemove);
                }
            }

            foreach (Point p in shown)
            {
                _preparedMap[(int)p.X, (int)p.Y] = _generatedMap[(int)p.X, (int)p.Y];
            }
        }

        private void CopyPreparedToMap()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _map[i, j] = _preparedMap[i, j];
                }
            }
        }
    }
}
