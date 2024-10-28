using Space_Shooters.classes;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;
using static Space_Shooters.classes.Game.Game_VariableHandling.GameTick;
using static Space_Shooters.classes.General.User_DataHandling.PlayerDataHandling;
using static Space_Shooters.classes.Game.Game_VariableHandling.Variables;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using static Space_Shooters.classes.Game.Game_VariableHandling.DifficultyHandling;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using Space_Shooters.classes.Game.Game_EntityHandling;
using Space_Shooters.classes.Game.Game_CollisionHandling;
using Space_Shooters.classes.Game.Game_PlayerHandling;
using Space_Shooters.classes.Game.Game_VariableHandling;
using Space_Shooters.classes.Game.Game_UIHandling;
using Space_Shooters.Models;
using Space_Shooters.classes.General.User_DataHandling;

namespace Space_Shooters.views
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : UserControl
    {
        private static ViewHandler StatVarViewHandler = new();

        private readonly ViewHandler VarViewHandler;

        public Game(ViewHandler PassedViewHandler)
        {
            InitializeComponent();
            VariableInitialize();

            // Set the view in the ViewHandler to this view
            this.VarViewHandler = PassedViewHandler;
            StatVarViewHandler = PassedViewHandler;
            // Create a new instance of the StartingTimer class
            StartingTimer timer = new();
            // Set the datacontext of the textblock to the timer (Time in StartingTimer class)
            tbCenterText.DataContext = timer;
            // Create a new instance of the GameViewModel class
            GameViewModel gameViewModel = new();
            // Set the datacontext of the textblock to the waveCounter (Wave in WaveNumber class)
            tbWaveNum.DataContext = gameViewModel;
            tbWaveCounter.DataContext = gameViewModel;
            // Start the collision loop
            CollisionLoop.Start();
            // Start the game loop
            Tick();
            // Set the datacontext of the progressbar to the hpAmount (PlayerHP in PlayerHPHandler class)
            PlayerGameHPHandler playerHPHandler = new();
            // Set the datacontext of the progressbar to the hpAmount (PlayerHP in PlayerHPHandler class)
            pbUserHealth.DataContext = playerHPHandler;
            // Set the maximum value of the progressbar to the player's max health
            pbUserHealth.Maximum = PlayerGameHPHandler.PlayerHP;

            EquipmentStatBoost();
        }
        internal static async void StartCountDownCompleted()
        {
            while (!_WaveModel.GameEnded)
            {
                if (Paused)
                {
                    if (_WaveModel.GameEnded) break;
                    // Wait for 200ms to reduce the CPU usage
                    await Task.Delay(200);
                    // Skip the rest of the loop but don't break it
                    continue;
                }
                if (_WaveModel.GameEnded) break;
                // Clear the binding to avoid an error when setting the text
                BindingOperations.ClearBinding(_WindowModel.CenterBlock, OutlinedTextControl.TextProperty);
                // Set the text to "Start!"
                CenterTextHandling.UpdateCenterText("Start!");
                if (_WaveModel.GameEnded) break;
                await Wait.WaitInSeconds(1); // Wait for 1 second
                if (_WaveModel.GameEnded) break;
                _WindowModel.CenterBlock.Visibility = Visibility.Collapsed;
                if (!_WaveModel.WaveStarted && !_WaveModel.GameEnded)
                {
                    EntityWave.InitiateSpawn.StartSpawning();
                }
                break;
            }
        }
        private void VariableInitialize()
        {
            WaveNumber.Wave = 1;
            Paused = false;
            imgUser.Source = new BitmapImage(new Uri($"..\\img\\skins\\User_Skins\\{ _UserModel.Skin }.png", UriKind.RelativeOrAbsolute));
            //MessageBox.Show(imgUser.Source.ToString());

            if (_WindowModel != null && _WaveModel != null)
            {
                _WindowModel.MainGrid = MainGrid;
                _WindowModel.BoPause = boPauseGame;
                _WindowModel.BoUser = boUserHitBox;
                _WindowModel.CenterBlock = tbCenterText;
                _WindowModel.ItemLog = spItemLog;

                _WaveModel.SpawnIndex = 0;
                _WaveModel.WaveMin = 1;
                _WaveModel.WaveMax = 3;
                _WaveModel.WaveStarted = false;
                _WaveModel.GameEnded = false;

                _ItemModel.ItemArray = new int[0];
            }
            else
            {
                _WindowModel = new()
                {
                    MainGrid = MainGrid,
                    BoPause = boPauseGame,
                    BoUser = boUserHitBox,
                    CenterBlock = tbCenterText,
                    ItemLog = spItemLog
                };
                _WaveModel = new()
                {
                    SpawnIndex = 0,
                    WaveMin = 1,
                    WaveMax = 3,
                    WaveStarted = false,
                    GameEnded = false
                };
                _ItemModel = new()
                {
                    ItemArray = new int[0]
                };
            }
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Subscribe to the KeyDown event
            if (!KeyDownSubscribed)
            {
                DifficultyVariables();
                Window.GetWindow(this).KeyDown += GameKeyDown.KeyPressed;
                KeyDownSubscribed = true;
            }
        }
        public void ContinueGame(object sender, RoutedEventArgs e)
        {
            // Hide the pause menu
            boPauseGame.Visibility = Visibility.Collapsed;
            Paused = false;
        }

        private void GiveUp(object sender, RoutedEventArgs e)
        {
            GameOver();
        }
        private void ExitToDesktop(object sender, RoutedEventArgs e)
        {
            UpdateDatabase();
            // Close the application
            Application.Current.Shutdown();
        }
        internal static async void GameOver()
        {
            UpdateDatabase();
            _WaveModel.GameEnded = true;
            Paused = true;
            _WindowModel.BoPause.Visibility = Visibility.Collapsed;
            CenterTextHandling.UpdateCenterText("Game over!");
            // Wait for 3 seconds
            await Task.Delay(3 * 1000);
            StatVarViewHandler.GoToMainMenu();
        }

    }
}
