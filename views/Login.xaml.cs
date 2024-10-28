using Space_Shooters.classes;
using Space_Shooters.classes.General.User_DataHandling;
using System;
using System.Collections.Generic;
using System.IO;
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
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using static Space_Shooters.classes.General.User_DataHandling.PlayerDataHandling;
using System.Windows.Shapes;
using System.Reflection;

namespace Space_Shooters.views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        private readonly ViewHandler VarViewHandler;
        public Login(ViewHandler VarViewHandler)
        {
            InitializeComponent();
            this.VarViewHandler = VarViewHandler;
        }

        public void LoginClick(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text;
            string password = pbPassword.Password;

            if (VerifyUser(username, password))
            {
                VarViewHandler.GoToMainMenu();
            }
            else
            {
                MainWindow.ShowInvalidInput("Invalid username or password.", Brushes.DarkRed);
            }
        }

        public void NoAccount(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToRegister();
        }
    }
}
