using System.Windows.Controls;
using System.Windows.Navigation;

namespace Game_Sudoku.Utilities
{
    public static class NavigationHelper
    {
        public static void NavigateToPage(NavigationService navigationService, Page page)
        {
            navigationService?.Navigate(page);
        }
        
        public static void GoBack(NavigationService navigationService)
        {
            navigationService?.GoBack();
        }
    }
}