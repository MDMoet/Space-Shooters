using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Space_Shooters.views;
using static Space_Shooters.classes.General.User_DataHandling.UserKeyBinds;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using Space_Shooters.classes.Game.Game_VariableHandling;
using Space_Shooters.classes.Game.Game_PlayerHandling;
using System.Runtime.CompilerServices;
using Space_Shooters.Models;


namespace Space_Shooters.classes.Game.Game_UIHandling
{
    public class GameKeyDown
    {
        // Create a border object for the pause menu
        private static bool wait = false;
        private static readonly UserActions userActions = new();

        public static async void KeyPressed(object sender, KeyEventArgs e)
        {
            
            // Check if the escape key is pressed and the pause menu is visible
            if (e.Key == Pause_Game)
            {
                PausePressed(sender, e);
            }
            // Check if the game is paused
            if (GameTick.Paused == true)
            {
                // Skip the rest of the method if game is paused
                return;
            }
            // Check if the movement keys are pressed
            if (e.Key == Move_Left)
            {
                // Move the user to the left
                userActions.MoveLeft();
            }
            if (e.Key == Move_Right)
            {
                // Move the user to the right
                userActions.MoveRight();
            }
            // Check if the attack key is pressed
            if (e.Key == Attack_1)
            {
                if (!wait)
                {
                    userActions.Attack1();
                    wait = true;
                    await Task.Delay(_UserModel.UserStatBoosted.BaseAttackSpeed);
                    wait = false;
                }
            }
        }

        // Method to handle the pause button
        public static void PausePressed(object sender, KeyEventArgs e)
        {
            if (!_WaveModel.GameEnded)
            {
                // Check if the escape key is pressed
                if (_WindowModel.BoPause.Visibility == Visibility.Visible)
                {
                    // Hide the pause menu
                    ContinueGame(sender, e);
                }
                else
                {
                    // Show the pause menu
                    PauseGame(sender, e);
                }
            }
        }
        public static void ContinueGame(object sender, RoutedEventArgs e)
        {
            // Hide the pause menu
            GameTick.Paused = false;
            _WindowModel.BoPause.Visibility = Visibility.Collapsed;
        }
        public static void PauseGame(object sender, RoutedEventArgs e)
        {
            // Show the pause menu
            GameTick.Paused = true;
            _WindowModel.BoPause.Visibility = Visibility.Visible;
        }
        
    }
}

