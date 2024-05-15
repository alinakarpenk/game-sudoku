using System.Windows;
using System.Windows.Controls;

namespace Game_Sudoku.Models
{
    public class SudokuUi : ISudokuUi
    {
        private readonly TextBox[,] _textBoxes;

        public SudokuUi(TextBox[,] textBoxes)
        {
            _textBoxes = textBoxes;
        }

        public void UpdateCell(int row, int col, string text, bool isReadOnly, FontWeight fontWeight)
        {
            _textBoxes[row, col].Text = text;
            _textBoxes[row, col].IsReadOnly = isReadOnly;
            _textBoxes[row, col].FontWeight = fontWeight;
        }
    }
}