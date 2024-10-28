using Space_Shooters.views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static Space_Shooters.classes.Game.Game_EntityHandling.WaveNumber;
using static Space_Shooters.classes.General.User_DataHandling.PlayerDataHandling;
using static Space_Shooters.classes.Game.Game_VariableHandling.GameTick;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using static Space_Shooters.classes.Game.Game_VariableHandling.Variables;
using Space_Shooters.classes.Game.Game_VariableHandling;
using static System.Net.Mime.MediaTypeNames;
using Space_Shooters.classes.Game.Game_UIHandling;


namespace Space_Shooters.classes.Game.Game_EntityHandling
{
    internal class WaveClearHandling
    {
       
        public WaveClearHandling() { }

        internal static async void WaveCleared()
        {
            while (true)
            {
                if (Paused)
                {
                    // Wait for 200ms to reduce the CPU usage
                    await Task.Delay(200);
                    // Skip the rest of the loop but don't break it
                    continue;
                }
                // Set the center text to "Wave Cleared!"
                if (!_WaveModel.GameEnded) CenterTextHandling.UpdateCenterText($"Wave Cleared!");
                else break;// Increase the wave number
                IncreaseWave();
                UpdateDatabase();
                // Wait for 2 seconds
                if (!_WaveModel.GameEnded) await Wait.WaitInSeconds(2);
                else break;
                // Set the center text to "Wave: {Wave}"
                if (!_WaveModel.GameEnded) CenterTextHandling.UpdateCenterText($"Wave: {Wave}");
                else break;
                // Wait for 2 seconds
                if (!_WaveModel.GameEnded) await Wait.WaitInSeconds(2);
                else break;
                // Clear the center text
                if (!_WaveModel.GameEnded) _WindowModel.CenterBlock.Visibility = Visibility.Collapsed;
                else break;
                // Wait for 1 second
                if (!_WaveModel.GameEnded) await Wait.WaitInSeconds(1);
                else break;
                // Start spawning the next wave
                if (!_WaveModel.GameEnded) EntityWave.InitiateSpawn.StartSpawning();
                else break;
                
                break;
            }
        }
    }
    internal class WaveCheck
    {
        public WaveCheck()
        {

        }
        internal static void CheckForEntities()
        {
            if (EntitiesLeft == 0 && !_WaveModel.GameEnded)
            {
                WaveClearHandling.WaveCleared();
            } 
        }
    }
}
