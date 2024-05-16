using System.Windows;

namespace Game_Sudoku.Models
{
    public interface ISudokuUi
    {
        void UpdateCell(int row, int col, string text, bool isReadOnly, FontWeight fontWeight);
    }
}