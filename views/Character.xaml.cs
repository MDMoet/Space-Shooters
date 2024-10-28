using Space_Shooters.classes;
using Space_Shooters.classes.General.User_DataHandling;
using static Space_Shooters.classes.General.User_DataHandling.PlayerDataHandling;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Space_Shooters.views
{
    /// <summary>
    /// Interaction logic for Character.xaml
    /// </summary>
    public partial class Character : UserControl
    {
        private readonly ViewHandler VarViewHandler;

        private static PlayerPointsHandler playerPointsHandler = new();
        private static PlayerHPHandler playerHPHandler = new();
        private static PlayerDamageHandler playerDamageHandler = new();
        private static PlayerASHandler playerASHandler = new();
        private static PlayerMSHanlder playerMSHanlder = new();
        private static PlayerLevelHandler playerLevelHandler = new();
        private static PlayerLPHandler playerLPHandler = new();
        public Character(ViewHandler VarViewHandler)
        {
            InitializeComponent();


            // Set the datacontext of the progress bar to the playerLPHandler
            pbLevelProgress.DataContext = playerLPHandler;
            // Set the maximum value of the progress bar to the TillLevelUp property in the playerLPHandler
            pbLevelProgress.Maximum = PlayerLPHandler.TillLevelUp;
            // Set the datacontext of the textblock to the playerLevelHandler
            tbCurrentLevel.DataContext = playerLevelHandler;
            // Set the text of the textblock to the current level + 1
            tbNextLevel.Text = $"{Convert.ToInt32(tbCurrentLevel.Text) + 1}";

            HandleUpdate();

            this.VarViewHandler = VarViewHandler;
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
        public void UpgradeHealth(object sender, RoutedEventArgs e)
        {
            _UserModel.UserStat.Health = _UserModel.UserStat.LevelPoints <= 0 ? _UserModel.UserStat.Health : _UserModel.UserStat.Health += 15;
            _UserModel.UserStat.LevelPoints = _UserModel.UserStat.LevelPoints <= 0 ? _UserModel.UserStat.LevelPoints = 0 : _UserModel.UserStat.LevelPoints -= 1;
            HandleUpdate();
        }
        public void UpgradeDamage(object sender, RoutedEventArgs e)
        {
            _UserModel.UserStat.BaseDamage = _UserModel.UserStat.LevelPoints <= 0 ? _UserModel.UserStat.BaseDamage : _UserModel.UserStat.BaseDamage += 3;
            _UserModel.UserStat.LevelPoints = _UserModel.UserStat.LevelPoints <= 0 ? _UserModel.UserStat.LevelPoints = 0 : _UserModel.UserStat.LevelPoints -= 1;
            HandleUpdate();
        }
            public void UpgradeAS(object sender, RoutedEventArgs e)
        {

            _UserModel.UserStat.BaseAttackSpeed = _UserModel.UserStat.LevelPoints <= 0 ? _UserModel.UserStat.BaseAttackSpeed : _UserModel.UserStat.BaseAttackSpeed -= 25;
            _UserModel.UserStat.LevelPoints = _UserModel.UserStat.LevelPoints <= 0 ? _UserModel.UserStat.LevelPoints = 0 : _UserModel.UserStat.LevelPoints -= 1;
            HandleUpdate();
        }
        public void UpgradeMS(object sender, RoutedEventArgs e)
        {
            _UserModel.UserStat.BaseSpeed = _UserModel.UserStat.LevelPoints <= 0 ? _UserModel.UserStat.BaseSpeed : _UserModel.UserStat.BaseSpeed += 3;
            _UserModel.UserStat.LevelPoints = _UserModel.UserStat.LevelPoints <= 0 ? _UserModel.UserStat.LevelPoints = 0 : _UserModel.UserStat.LevelPoints -= 1;
            HandleUpdate();
        }
        private void HandleUpdate()
        {
            UpdateStats();
            tbPoints.DataContext = playerPointsHandler = new();
            tbHealth.DataContext = playerHPHandler = new();
            tbDamage.DataContext = playerDamageHandler = new();
            tbAS.DataContext = playerASHandler = new();
            tbMS.DataContext = playerMSHanlder = new();
         
        }
    }
}
