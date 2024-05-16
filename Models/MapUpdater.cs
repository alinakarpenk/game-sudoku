using System.Windows;

namespace Game_Sudoku.Models
{
    public class MapUpdater
    {
        private const int Size = 9;
        private readonly int[,] _map;
        private readonly int[,] _preparedMap;
        private readonly ISudokuUi _sudokuUi;

        public MapUpdater(int[,] map, int[,] preparedMap, ISudokuUi sudokuUi)
        {
            _map = map;
            _preparedMap = preparedMap;
            _sudokuUi = sudokuUi;
        }

        public void UpdateNumbers()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    string text = _map[i, j] == 0 ? "" : _map[i, j].ToString();
                    bool isReadOnly = _preparedMap[i, j] != 0;
                    FontWeight fontWeight = isReadOnly ? FontWeights.Bold : FontWeights.Regular;

                    _sudokuUi.UpdateCell(i, j, text, isReadOnly, fontWeight);
                }
            }
        }
        
        public void ClearMap()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _map[i, j] = _preparedMap[i, j];
                }
            }
            UpdateNumbers();
        }
    }
}