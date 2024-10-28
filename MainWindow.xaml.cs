using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Space_Shooters.classes;
using Space_Shooters.views;
using static Space_Shooters.classes.General.User_DataHandling.PlayerDataHandling;
using static Space_Shooters.classes.General.User_DataHandling.UserKeyBinds;
using static Space_Shooters.classes.General.Config;
using Space_Shooters.classes.Game.Game_DataHandling;
using Space_Shooters.classes.Game.Game_EntityHandling;

namespace Space_Shooters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewHandler VarViewHandler { get; set; }
        internal static Border BoDuplicateKeybind;
        internal static Border BoSelectItem;
        internal static Border BoNoInputGiven;
        internal static Border BoInvalidInput;
        internal static Viewbox VbUpdate;

        internal static OutlinedTextControl InvalidInputText;
        internal static int UserId;
        public MainWindow()
        {
            InitializeComponent();
            InitializeConfiguration();
            InitializeObjects();
            // Get the stats from the database
            VarViewHandler = new ViewHandler();
            DataContext = VarViewHandler;

            if (CheckUserDataFile())
            {
                VarViewHandler.GoToMainMenu();
                GetStatsFromDB();
                GetDataFromDB();
                EquipmentStatBoost();
            }
            else
            {
                VarViewHandler.GoToRegister();
                return;
            }
          

        }
        private void InitializeObjects()
        {
          

            BoDuplicateKeybind = boDuplicateKeyBind;
            BoSelectItem = boSelectItem;
            BoNoInputGiven = boNoInputGiven;
            BoInvalidInput = boInvalidInput;

            InvalidInputText = otInvalidInputReason;
            VbUpdate = vbUpdate;
        }
        internal static async void ShowInvalidInput(string reason, SolidColorBrush color)
        {
            InvalidInputText.Text = reason;
            BoInvalidInput.BorderBrush = color;
            InvalidInputText.Stroke = color;

            BoInvalidInput.Visibility = Visibility.Visible;
            await Task.Delay(2000);
            BoInvalidInput.Visibility = Visibility.Collapsed;
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

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            vbUpdate.Visibility = Visibility.Collapsed;
        }
    }
}

