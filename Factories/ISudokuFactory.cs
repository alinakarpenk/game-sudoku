using System.Windows.Controls;
using Game_Sudoku.Models;

namespace Game_Sudoku.Factories
{
    public interface ISudokuFactory
    {
        Sudoku CreateSudoku(TextBox[,] textBoxes, int hiddenCount);
    }
}