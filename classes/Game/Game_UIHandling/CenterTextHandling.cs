using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using Space_Shooters.classes.Game.Game_DataHandling;
using System.IO;
using System.Windows.Controls;
using static Space_Shooters.classes.Game.Game_VariableHandling.Variables;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;  
using static Space_Shooters.classes.General.Config;

namespace Space_Shooters.classes.Game.Game_UIHandling
{
    internal class CenterTextHandling
    {
        public static void UpdateCenterText(string text)
        {
            BindingOperations.ClearBinding(_WindowModel.CenterBlock, OutlinedTextControl.TextProperty);
            _WindowModel.CenterBlock.Visibility = Visibility.Visible;
            _WindowModel.CenterBlock.Text = text?.ToString() ?? string.Empty;
        }
        public static void CheckConfiguration()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            // Compare static values with config values
            if (Enemy_ms_Movement != Configuration.Default_Enemy_ms_Movement ||
                Enemy_ms_Spawning != Configuration.Default_Enemy_ms_Spawning ||
                Entity_Wave_Amount != Configuration.Entity_Wave_Amount ||
                KeyDownSubscribed != Configuration.KeyDownSubscribed ||
                EntitiesLeft != Configuration.EntitiesLeft ||
                Base_Experience_Multiplier != Configuration.Base_Experience_Multiplier ||
                Progression_Requirement_Multiplier != Configuration.Progression_Requirement_Multiplier ||
                Difficulty_Factor_Increase != Configuration.Difficulty_Factor_Increase)
            {
                // Show message box
                MessageBox.Show("You snooped around where you shouldn't have. You know that's not allowed right?", "Configuration Mismatch", MessageBoxButton.OK, MessageBoxImage.Error);
                File.Delete(filePath); // Delete the configuration file (to prevent tampering)
                InitializeConfiguration(); // Reload the configuration
                Application.Current.Shutdown(); // Close the application
            }
        }
    }
}
