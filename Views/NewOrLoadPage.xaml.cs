using System.Windows;
using System.Windows.Controls;
using Game_Sudoku.Views;

namespace Game_Sudoku
{
    public partial class NewOrLoadPage : Page
    {
        public NewOrLoadPage()
        {
            InitializeComponent();
        }

        private void NewGameButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DifficultyPage());
        }

        private void ContinueButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Game("", 1, 1, 1, true));
        }
    }
}