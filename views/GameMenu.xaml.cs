using Space_Shooters.classes;
using static Space_Shooters.classes.Game.Game_VariableHandling.DifficultyVariable;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
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
using Space_Shooters.classes.General.User_DataHandling;
using Space_Shooters.classes.Game.Game_VariableHandling;


namespace Space_Shooters.views
{
    /// <summary>
    /// Interaction logic for GameMenu.xaml
    /// </summary>
    public partial class GameMenu : UserControl
    {
        private readonly ViewHandler VarViewHandler;

        private static Button btEasyStatic;
        private static Button btMediumStatic;
        private static Button btHardStatic;
        private static Button btExtremeStatic;

        internal static Image EquippedSkin;

        private static bool DifficultySelected;
        public GameMenu(ViewHandler VarViewHandler)
        {
            InitializeComponent();
            this.VarViewHandler = VarViewHandler;
            
            btEasyStatic = btEasy;
            btMediumStatic = btMedium;
            btHardStatic = btHard;
            btExtremeStatic = btExtreme;

            EquippedSkin = imgEquippedSkin;

            imgEquippedSkin.Source = new BitmapImage(new Uri($"..\\img\\skins\\User_Skins\\{_UserModel.Skin}.png", UriKind.RelativeOrAbsolute));

            DifficultySelected = false;
        }
        internal static void UpdateSkinStatic()
        {
            if (EquippedSkin == null) return;
            EquippedSkin.Source = new BitmapImage(new Uri($"..\\img\\skins\\User_Skins\\{_UserModel.Skin}.png", UriKind.RelativeOrAbsolute));
        }
        public void StartGame(object sender, RoutedEventArgs e)
        {
            if (!DifficultySelected) { MainWindow.ShowInvalidInput("No difficulty selected", Brushes.DarkOrange); return; }
            if (_UserModel.Skin == "Efteling")
            {
                Difficulty = 50;
            }
            VarViewHandler.GoToGame();
        }
        public void Equipment(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToEquipment();
        }
        public void Skins(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToSkins();
        }
        public void Return(object sender, RoutedEventArgs e)
        {
            VarViewHandler.Return();
        }

        private void btExtreme_Click(object sender, RoutedEventArgs e)
        {
            Difficulty = 4;
            Selected();
            btExtremeStatic.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ACA700"));
        }
        private void btHard_Click(object sender, RoutedEventArgs e)
        {
            Difficulty = 3;
            Selected();
            btHardStatic.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ACA700"));
        }
        private void btMedium_Click(object sender, RoutedEventArgs e)
        {
            Difficulty = 2;
            Selected();
            btMediumStatic.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ACA700"));
        }

        private void btEasy_Click(object sender, RoutedEventArgs e)
        {
            Difficulty = 1;
            Selected();
            btEasyStatic.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ACA700"));
        }
        private static void Selected()
        {
            btEasyStatic.Background = new SolidColorBrush(Colors.Transparent);
            btMediumStatic.Background = new SolidColorBrush(Colors.Transparent);
            btHardStatic.Background = new SolidColorBrush(Colors.Transparent);
            btExtremeStatic.Background = new SolidColorBrush(Colors.Transparent);

            DifficultySelected = true;
        }
    }
}
