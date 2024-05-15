using Game_Sudoku.Utilities.Memento;

namespace Game_Sudoku.Utilities
{
    public class SudokuMemento : IMemento
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
}