namespace Game_Sudoku.Utilities.Memento
{
    public interface IMemento
    {
        int[,] GetState();
    }
}