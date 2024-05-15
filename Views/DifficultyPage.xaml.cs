using System.Windows;
using System.Windows.Controls;
using Game_Sudoku.Views;

namespace Game_Sudoku
{
    public partial class DifficultyPage : Page
    {
        public DifficultyPage()
        {
            InitializeComponent();
        }

        private void BeginnerButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Game("Beginner", 31, 0, 5));
        }

        private void ComfortButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Game("Comfort", 36, 0, 3));
        }

        private void NormalButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Game("Normal", 41, 80, 3));
        }

        private void HardButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Game("Hard", 56, 60, 1));
        }
    }
}