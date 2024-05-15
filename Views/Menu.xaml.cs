using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Game_Sudoku.Views;

namespace Game_Sudoku
{
    /// <summary>
    /// Логика взаимодействия для Sudoku.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void PlayButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Game.CheckFile())
            {
                NavigationService.Navigate(new NewOrLoadPage());
            }
            else
            {
                NavigationService.Navigate(new DifficultyPage());
            }
        }
        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Rules());
        }
        private void ExitButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
