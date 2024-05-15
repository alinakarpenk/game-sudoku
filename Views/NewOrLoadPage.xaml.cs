using System.Windows;
using System.Windows.Controls;

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
            if (NavigationService != null) 
                NavigationService.Navigate(new DifficultyPage());
        }

        private void ContinueButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null) 
                NavigationService.Navigate(new Game("", 1, 1, 1, true));
        }
    }
}