using Space_Shooters.classes;
using Space_Shooters.classes.General.User_DataHandling;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using static Space_Shooters.classes.General.User_DataHandling.PlayerDataHandling;

namespace Space_Shooters.views
{
    public partial class Register : UserControl
    {
        private readonly ViewHandler VarViewHandler;

        public Register(ViewHandler VarViewHandler)
        {
            InitializeComponent();
            this.VarViewHandler = VarViewHandler;
        }

        public void RegisterClick(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text;
            string email = tbEmail.Text;
            string password = tbPassword.Password;
            string passwordRepeat = tbPasswordRepeat.Password;
         
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordRepeat))
            {
                MainWindow.ShowInvalidInput("All fields are required.", Brushes.DarkRed);
                return;
            }

            if (!IsValidEmail(email))
            {
                MainWindow.ShowInvalidInput($"Invalid email format.", Brushes.DarkRed);
                return;
            }

            if (password != passwordRepeat)
            {
                MainWindow.ShowInvalidInput($"Passwords do not match.", Brushes.DarkRed);
                return;
            }
           
            bool isRegistered = RegisterUser(username, email, password); // Pass plain password

            if (isRegistered)
            {
                VarViewHandler.GoToMainMenu();
            }
            else
            {
                MainWindow.ShowInvalidInput("Username or email already taken.", Brushes.DarkRed);
            }
        }

        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        public void HaveAccount(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToLogin();
        }
    }
}
