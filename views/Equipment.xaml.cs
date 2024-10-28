using Space_Shooters.classes;
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
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using System.Windows.Shapes;
using Space_Shooters.classes.General.User_DataHandling;
using Space_Shooters.Models;
using Space_Shooters.classes.Game.Game_PlayerHandling;

namespace Space_Shooters.views
{
    /// <summary>
    /// Interaction logic for Equipment.xaml
    /// </summary>
    public partial class Equipment : UserControl
    {
        private readonly ViewHandler VarViewHandler;
        internal static Border EquipmentBorder;
        internal static ListBox EquipmentList;

        internal static Image EquipmetSlot1;
        internal static Image EquipmetSlot2;
        internal static Image EquipmetSlot3;
        internal static Image EquipmetSlot4;

        internal static OutlinedTextControl NameSlot1;
        internal static OutlinedTextControl NameSlot2;
        internal static OutlinedTextControl NameSlot3;
        internal static OutlinedTextControl NameSlot4;

        internal static OutlinedTextControl HealthExtra;
        internal static OutlinedTextControl DamageExtra;
        internal static OutlinedTextControl ASExtra;
        internal static OutlinedTextControl MSExtra;
                              
        private static PlayerPointsHandler playerPointsHandler = new();
        internal static PlayerHPHandler playerHPHandler = new();
        internal static classes.General.User_DataHandling.PlayerDamageHandler playerDamageHandler = new();
        internal static PlayerASHandler playerASHandler = new();
        internal static PlayerMSHanlder playerMSHandler = new();
        private static PlayerLevelHandler playerLevelHandler = new();
        private static PlayerLPHandler playerLPHandler = new();

        private static ProgressBar LevelProgress;
        private static OutlinedTextControl CurrentLevel;
        private static OutlinedTextControl NextLevel;
        private static OutlinedTextControl AS;
        private static OutlinedTextControl Health;
        private static OutlinedTextControl Damage;
        private static OutlinedTextControl MS;
        public Equipment(ViewHandler VarViewHandler)
        {
            InitializeComponent();
            LevelChecker();
            InitializeData();
            SetDataContext();

            PlayerDataHandling.LoadEquipment();
            this.VarViewHandler = VarViewHandler;
        }
        internal static void SetDataContext()
        {
            LevelProgress.DataContext = playerLPHandler;
            LevelProgress.Maximum = PlayerLPHandler.TillLevelUp;
            CurrentLevel.DataContext = playerLevelHandler;
            NextLevel.Text = $"{Convert.ToInt32(CurrentLevel.Text) + 1}";
            AS.DataContext = playerASHandler;
            Health.DataContext = playerHPHandler;
            Damage.DataContext = playerDamageHandler;
            MS.DataContext = playerMSHandler;
        }
        private void InitializeData()
        {
            EquipmetSlot1 = imgEquipmentSlot1;
            EquipmetSlot2 = imgEquipmentSlot2;
            EquipmetSlot3 = imgEquipmentSlot3;
            EquipmetSlot4 = imgEquipmentSlot4;

            NameSlot1 = tbSlot1;
            NameSlot2 = tbSlot2;
            NameSlot3 = tbSlot3;
            NameSlot4 = tbSlot4;

            HealthExtra = tbHealthExtra;
            DamageExtra = tbDamageExtra;
            ASExtra = tbASExtra;
            MSExtra = tbMSExtra;

            LevelProgress = pbLevelProgress;
            CurrentLevel = tbCurrentLevel;
            NextLevel = tbNextLevel;
            AS = tbAS;
            Health = tbHealth;
            Damage = tbDamage;
            MS = tbMS;

            ItemSlot = 1;
            EquipmentBorder = boEquipmentInventory;

            SelectAction = "Equipment";

            PlayerDataHandling.InitializeDictionary();
        }
        public void Equipmentbt(object sender, RoutedEventArgs e)
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
        private void LevelChecker()
        {
            if (_UserModel.UserStat.Level >= 5)
            {
                boLvl5.Visibility = Visibility.Collapsed;
                if (_UserModel.UserStat.Level >= 10)
                {
                    boLvl10.Visibility = Visibility.Collapsed;
                    if (_UserModel.UserStat.Level >= 20)
                    {
                        boLvl20.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
        private void ItemSlotOne(object sender, MouseButtonEventArgs e)
        {
            ItemSlot = 1;
            PostRetrieve();
        }
        private void ItemSlotTwo(object sender, MouseButtonEventArgs e)
        {
            ItemSlot = 2;
            PostRetrieve();
        }
        private void ItemSlotThree(object sender, MouseButtonEventArgs e)
        {
            ItemSlot = 3;
            PostRetrieve();
        }
        private void ItemSlotFour(object sender, MouseButtonEventArgs e)
        {
            ItemSlot = 4;
            PostRetrieve();
        }
        private void Unequip(object sender, MouseButtonEventArgs e)
        {
            PlayerDataHandling.UnequipEquipment();
        }
        private void PostRetrieve()
        {
            if (PlayerDataHandling.RetrieveInventoryEquipment().Items.Count == 0) { MainWindow.ShowInvalidInput("No equipment owned", Brushes.DarkOrange); return; }
            boEquipmentInventory.Child = PlayerDataHandling.RetrieveInventoryEquipment();
            EquipmentList = (ListBox)boEquipmentInventory.Child;
            boEquipmentInventory.Visibility = Visibility.Visible;
        }
    }
}
