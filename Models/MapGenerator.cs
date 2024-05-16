using System;

namespace Game_Sudoku.Models
{
    public class MapGenerator
    {
        private const int Size = 9;
        private const int BlockSize = 3;
        private readonly Random _random = new Random();
        private readonly int[,] _map = new int[Size, Size];
        
        public void GenerateMap()
        {
            int[] firstRow = GenerateFirstRow();
            InitializeFirstRow(firstRow);
            FillMapWithShiftedRows();
            PerformRandomSwaps();
        }
        
        private int[] GenerateFirstRow()
        {
            int[] firstRow = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int swaps = _random.Next(3, 10);
            for (int i = 0; i < swaps; i++)
            {
                int a = _random.Next(Size);
                int b = _random.Next(Size);
                Swap(ref firstRow[a], ref firstRow[b]);
            }
            return firstRow;
        }

        private void InitializeFirstRow(int[] firstRow)
        {
            for (int i = 0; i < Size; i++)
            {
                _map[0, i] = firstRow[i];
            }
        }
        
        private void FillMapWithShiftedRows()
        {
            for (int j = 1; j < Size; j++)
            {
                int shift = (j % BlockSize == 0) ? 1 : BlockSize;
                for (int i = 0; i < Size; i++)
                {
                    _map[j, i] = _map[j - 1, (i + shift) % Size];
                }
            }
        }
        
        private void PerformRandomSwaps()
        {
            PerformRandomRowSwaps();
            PerformRandomColumnSwaps();
        }
        
        private void PerformRandomRowSwaps()
        {
            int swaps = _random.Next(BlockSize);
            for (int i = 0; i < swaps; i++)
            {
                int block = _random.Next(BlockSize);
                int a = _random.Next(block * BlockSize, (block + 1) * BlockSize);
                int b = _random.Next(block * BlockSize, (block + 1) * BlockSize);
                SwapRows(a, b);
            }
        }
        
        private void PerformRandomColumnSwaps()
        {
            int swaps = _random.Next(BlockSize);
            for (int i = 0; i < swaps; i++)
            {
                int block = _random.Next(BlockSize);
                int a = _random.Next(block * BlockSize, (block + 1) * BlockSize);
                int b = _random.Next(block * BlockSize, (block + 1) * BlockSize);
                SwapColumns(a, b);
            }
        }
        
        private void Swap(ref int a, ref int b)
        {
            (a, b) = (b, a);
        }
        
        private void SwapRows(int a, int b)
        {
            for (int i = 0; i < Size; i++)
            {
                Swap(ref _map[a, i], ref _map[b, i]);
            }
        }
        
        private void SwapColumns(int a, int b)
        {
            for (int i = 0; i < Size; i++)
            {
                Swap(ref _map[i, a], ref _map[i, b]);
            }
        }
    }
}
