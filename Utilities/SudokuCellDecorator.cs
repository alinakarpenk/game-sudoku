using System.Windows;
using System.Windows.Controls;

namespace Game_Sudoku.Utilities
{
    public class SudokuCellDecorator
    {
        public static void DecorateCell(TextBox textBox, int number)
        {
            textBox.Text = number == 0 ? "" : number.ToString();

            if (number != 0)
            {
                textBox.IsReadOnly = true;
                textBox.FontWeight = FontWeights.Bold;
            }
            else
            {
                textBox.IsReadOnly = false;
                textBox.FontWeight = FontWeights.Regular;
            }
        }
    }
}