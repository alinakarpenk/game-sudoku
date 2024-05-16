namespace Game_Sudoku.Models
{
    public class MapChecker
    {
        private const int Size = 9;
        private const int BlockSize = 3;
        private readonly int[,] _map;

        public MapChecker(int[,] map)
        {
            _map = map;
        }

        public bool CheckMap()
        {
            return CheckRows() && CheckColumns() && CheckBlocks();
        }

        private bool CheckRows()
        {
            for (int i = 0; i < Size; i++)
            {
                if (!CheckGroup(i, 0, 0, 1)) return false;
            }
            return true;
        }

        private bool CheckColumns()
        {
            for (int j = 0; j < Size; j++)
            {
                if (!CheckGroup(0, j, 1, 0)) return false;
            }
            return true;
        }

        private bool CheckBlocks()
        {
            for (int i = 0; i < Size; i += BlockSize)
            {
                for (int j = 0; j < Size; j += BlockSize)
                {
                    if (!CheckGroup(i, j, 1, 1)) return false;
                }
            }
            return true;
        }

        private bool CheckGroup(int startRow, int startCol, int stepRow, int stepCol)
        {
            bool[] appearances = new bool[Size];
            for (int x = 0; x < BlockSize; x++)
            {
                for (int y = 0; y < BlockSize; y++)
                {
                    int value = _map[startRow + x * stepRow, startCol + y * stepCol];
                    if (value == 0 || appearances[value - 1]) return false;
                    appearances[value - 1] = true;
                }
            }
            return true;
        }
    }

}