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
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Space_Shooters
{
    /// <summary>
    /// Interaction logic for ChangePasswordDialog.xaml
    /// </summary>
    public partial class ChangePasswordDialog : Window
    {
        public string CurrentPassword => pbCurrentPassword.Password;
        public string NewPassword => pbNewPassword.Password;
        public string ConfirmNewPassword => pbConfirmNewPassword.Password;

        public ChangePasswordDialog()
        {
            InitializeComponent();
        }

        private async void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (NewPassword != ConfirmNewPassword)
            {
                Visibility = Visibility.Hidden;
                MainWindow.ShowInvalidInput("Passwords don't match", Brushes.DarkOrange);
                await Task.Delay(2000);
                Close();
                return;
            }
            else
            {
                DialogResult = true;
                Close();
            }

        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}