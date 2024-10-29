using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Space_Shooters.classes;
using Space_Shooters.classes.General;
using Space_Shooters.classes.General.User_DataHandling;
using Space_Shooters.views;

namespace Space_Shooters.views
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        private readonly ViewHandler VarViewHandler;

        public MainMenu(ViewHandler VarViewHandler)
        {
            InitializeComponent();
            tbVersion.Text = "Version: " + Assembly.GetExecutingAssembly().GetName().Version;
            this.VarViewHandler = VarViewHandler;
        }
        public void PlayGame(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToGameMenu(); // Handle button click to navigate to the game menu
        }
        public void Character(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToCharacter(); // Handle button click to navigate to the game menu
        }
        public void Inventory(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToInventory(); // Handle button click to navigate to the game 
        }
        public void ItemShop(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToItemShop(); // Handle button click to navigate to the game menu
        }
        public void Settings(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToSettings(); // Handle button click to navigate to the game menu
        }
        private void ExitToDesktop(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(MainWindow.VbUpdate.Visibility == Visibility.Visible)
            {
                MainWindow.VbUpdate.Visibility = Visibility.Collapsed;
            }
            else if (MainWindow.VbUpdate.Visibility == Visibility.Collapsed)
            {
                MainWindow.VbUpdate.Visibility = Visibility.Visible;
            }
        }
    }
}
