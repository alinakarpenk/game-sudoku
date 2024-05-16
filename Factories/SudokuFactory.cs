using System.Windows.Controls;
using Game_Sudoku.Models;

namespace Game_Sudoku.Factories
{
    public class SudokuFactory : ISudokuFactory
    {
        public Sudoku CreateSudoku(TextBox[,] textBoxes, int hiddenCount)
        {
            return new Sudoku(textBoxes, hiddenCount);
        }
    }
}