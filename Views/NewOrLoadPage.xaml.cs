using System.Windows;
using System.Windows.Controls;
using Game_Sudoku.Utilities;

namespace Game_Sudoku.Views
{
    public partial class NewOrLoadPage : Page
    {
        public NewOrLoadPage()
        {
            InitializeComponent();
        }

        private void NewGameButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateToPage(NavigationService, new DifficultyPage());
        }

        private void ContinueButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateToPage(NavigationService, new Game("", 1, 1, 1, true));
        }
    }
}