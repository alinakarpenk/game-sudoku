using System.Windows;
using System.Windows.Navigation;
using Game_Sudoku.Utilities;

namespace Game_Sudoku
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static NavigationService NavigationService { get; set; }
    }
}
