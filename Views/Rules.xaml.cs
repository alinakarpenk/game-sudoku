using System.Windows;
using System.Windows.Controls;
using Game_Sudoku.Utilities;

namespace Game_Sudoku.Views
{
    /// <summary>
    /// Логика взаимодействия для Rules.xaml
    /// </summary>
    public partial class Rules : Page
    {
        public Rules()
        {
            InitializeComponent();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateToPage(NavigationService, new Menu());
        }
    }
}
