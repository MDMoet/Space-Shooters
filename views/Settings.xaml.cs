using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
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
using System.Windows.Shapes;
using static Space_Shooters.classes.General.User_DataHandling.UserKeyBinds;

namespace Space_Shooters.views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        string selectedAction;
        private readonly ViewHandler VarViewHandler;
        Brush DFC900 = (Brush)new BrushConverter().ConvertFromString("#DFC900");
        public Settings(ViewHandler VarViewHandler)
        {
            InitializeComponent();
            this.VarViewHandler = VarViewHandler;
            LoadKeybinds();
            tbEmail.Text = UserModels.Email;
            tbUserName.Text = UserModels.Username;
        }

        private async void ChangePass(object sender, RoutedEventArgs e)
        {
            ChangePasswordDialog dialog = new();
            if (dialog.ShowDialog() == true)
            {
                // Handle the password change logic here
                string currentPassword = dialog.CurrentPassword;
                string newPassword = dialog.NewPassword;
                string confirmPassword = dialog.ConfirmNewPassword;

                bool success = PlayerDataHandling.UpdatePassword(currentPassword, newPassword);
                bool match = (bool)dialog.DialogResult;
                if (success && match)
                {
                    MainWindow.ShowInvalidInput("Password changed.", Brushes.Green);
                }
                else if (!match)
                {
                    return;
                }
                else if (!success)
                {
                    MainWindow.ShowInvalidInput("Password incorrect", Brushes.DarkRed);
                    await Task.Delay(2500);
                    // Recreate the dialog instead of modifying visibility
                    dialog = new ChangePasswordDialog();
                    dialog.ShowDialog();
                }
            }
        }

        private void Logout(object sender, RoutedEventArgs e) 
        {
            string filePath = "user_data.txt";
            File.Delete(filePath);
            VarViewHandler.GoToRegister();
        }
        public void Return(object sender, RoutedEventArgs e)
        {
            VarViewHandler.Return();
        }
        private void CustomScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            if (scrollViewer != null)
            {
                // Adjust the scroll speed by changing the multiplier
                double scrollSpeed = 0.2; // Decrease this value to slow down the scroll speed
                double offset = scrollViewer.VerticalOffset - (e.Delta * scrollSpeed);
                scrollViewer.ScrollToVerticalOffset(offset);
                e.Handled = true;
            }
        }
        private void LoadKeybinds()
        {
            spKeybindContainer.Children.Clear();
            FontFamily montserratText = GetFontFamily("Montserrat Text");
            // Create the Grid
            Grid titleGrid = new()
            {
                Margin = new Thickness(10)
            };

            // Define column definitions
            titleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            titleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });

            // Define row definitions
            titleGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });

            // Create the first Border with OutlinedTextControl
            Border actionTitleLabelBorder = new()
            {
                Height = 40,
                Width = 114,
                BorderBrush = new SolidColorBrush(Color.FromRgb(223, 201, 0)), // Equivalent to #DFC900
                BorderThickness = new Thickness(1)
            };
            Viewbox actionTitleLabelViewbox = new()
            {
                Width = 114,
                Height = 40
            };
            var actionTitleLabelText = new OutlinedTextControl
            {
                Text = "Action:",
                Width = 75,
                FontSize = 15,
                FontFamily = montserratText,
                Stroke = new SolidColorBrush(Color.FromRgb(223, 201, 0)),
                StrokeThickness = 1,
                Margin = new Thickness(4)
            };
            actionTitleLabelViewbox.Child = actionTitleLabelText;
            actionTitleLabelBorder.Child = actionTitleLabelViewbox;
            Grid.SetColumn(actionTitleLabelBorder, 0);
            titleGrid.Children.Add(actionTitleLabelBorder);

            // Create the second Border with OutlinedTextControl
            Border keybindTitleLabelBorder = new()
            {
                Height = 40,
                Width = 114,
                BorderBrush = new SolidColorBrush(Color.FromRgb(223, 201, 0)),
                BorderThickness = new Thickness(1)
            };
            Viewbox keybindTitleLabelViewbox = new()
            {
                Width = 114,
                Height = 40
            };
            var keybindTitleLabelText = new OutlinedTextControl
            {
                Text = "Keybind:",
                Width = 75,
                FontSize = 15,
                FontFamily = montserratText,
                Stroke = new SolidColorBrush(Color.FromRgb(223, 201, 0)),
                StrokeThickness = 1,
                Margin = new Thickness(4)
            };
            keybindTitleLabelViewbox.Child = keybindTitleLabelText;
            keybindTitleLabelBorder.Child = keybindTitleLabelViewbox;
            Grid.SetColumn(keybindTitleLabelBorder, 1);
            titleGrid.Children.Add(keybindTitleLabelBorder);

            // Add the title grid to the parent container (e.g., a StackPanel or another Grid)
            spKeybindContainer.Children.Add(titleGrid);

            foreach (string action in Actions)
            {
                Grid keybindGrid = new()
                {
                    Margin = new Thickness(10)
                };

                // Define column definitions
                keybindGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
                keybindGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });

                // Define row definitions
                keybindGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });

                // Create the first Border with OutlinedTextControl
                Border actionLabelBorder = new()
                {
                    Width = 114,
                    Height = 50,
                    BorderBrush = new SolidColorBrush(Color.FromRgb(223, 201, 0)), // Equivalent to #DFC900
                    BorderThickness = new Thickness(1),
                    HorizontalAlignment = HorizontalAlignment.Right
                };
                Viewbox actionLabelViewbox = new()
                {
                    Width = 114,
                    Height = 50,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                var actionLabelText = new OutlinedTextControl
                {
                    Text = action.Replace("_", " ") + ':',
                    Width = 75,
                    FontSize = 10,
                    FontFamily = montserratText,
                    Stroke = new SolidColorBrush(Color.FromRgb(223, 201, 0)),
                    StrokeThickness = 0.8,
                    Margin = new Thickness(4),
                    HorizontalAlignment = HorizontalAlignment.Right
                };
                actionLabelViewbox.Child = actionLabelText;
                actionLabelBorder.Child = actionLabelViewbox;
                Grid.SetColumn(actionLabelBorder, 0);
                keybindGrid.Children.Add(actionLabelBorder);

                // Create the second Border for keybind input
                Border keybindInputBorder = new()
                {
                    Width = 114,
                    Height = 50,
                    BorderBrush = new SolidColorBrush(Color.FromRgb(223, 201, 0)),
                    BorderThickness = new Thickness(1),
                };
                Border keybindInputInnerBorder = new()
                {
                    Background = Brushes.Transparent,
                    BorderBrush = new SolidColorBrush(Color.FromRgb(223, 201, 0)),
                    BorderThickness = new Thickness(1),
                    Width = 35,
                    Height = 35,
                    Cursor = Cursors.Hand,
                    IsHitTestVisible = true,
                    Tag = action
                };
                Viewbox keybindLabelViewBox = new();
                var keybindLabelText = new OutlinedTextControl
                {
                    Text = PlayerKeyBindsDict[action].Item1,
                    FontSize = 32,
                    FontFamily = montserratText,
                    Stroke = new SolidColorBrush(Color.FromRgb(223, 201, 0)),
                    StrokeThickness = 1.6,
                    Margin = new Thickness(4),
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                keybindLabelViewBox.Child = keybindLabelText;
                keybindInputInnerBorder.Child = keybindLabelViewBox;
                keybindInputInnerBorder.MouseLeftButtonDown += KeybindSelect_MouseDown; // Event handler for key down
                keybindInputBorder.Child = keybindInputInnerBorder;
                Grid.SetColumn(keybindInputBorder, 1);
                keybindGrid.Children.Add(keybindInputBorder);

                // Add the grid to the parent container (e.g., a StackPanel or another Grid)
                spKeybindContainer.Children.Add(keybindGrid);
            }

        }
        private void KeybindSelect_MouseDown(object sender, MouseEventArgs e)
        {
            boKeyInput.Visibility = Visibility.Visible;
            boKeyInputDark.Visibility = Visibility.Visible;
            if (sender is Border border)
            {
                selectedAction = border.Tag.ToString();
            }
            this.KeyDown += Settings_KeyDown;
        }

        private void Settings_KeyDown(object sender, KeyEventArgs e)
        {
            // Handle the key press
            string keyPressed = e.Key.ToString();

            // Update the keybind (you can add your logic here to update the keybind)
            // For example, you can update the text of the keybind input
            // keybindLabelText.Text = keyPressed;

            // Hide the input overlay
            boKeyInput.Visibility = Visibility.Collapsed;
            boKeyInputDark.Visibility = Visibility.Collapsed;
            // Unsubscribe from the KeyDown event
            this.KeyDown -= Settings_KeyDown;

            SetNewKeybind(selectedAction, keyPressed);
            LoadKeybinds();
    }
        public static FontFamily GetFontFamily(string key)
        {
            if (Application.Current.Resources.Contains(key))
            {
                return Application.Current.Resources[key] as FontFamily;
            }
            return null;
        }
    }
}
