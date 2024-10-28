using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using static Space_Shooters.classes.Game.Game_VariableHandling.Wait;
using static Space_Shooters.classes.Game.Game_VariableHandling.GameTick;

namespace Space_Shooters.classes.Game.Game_UIHandling
{
    internal class ItemLogHandler
    {
        internal static void AddItem(string itemName, int amount)
        {
            OutlinedTextControl logMessage_ = new();

            logMessage_ = new()
            {
                Text = $"+ {itemName}: {amount}",
                FontSize = 14,
                StrokeThickness = 1,
                Stroke = "#DFC900",
                BorderBrush = System.Windows.Media.Brushes.Yellow
            };

            _WindowModel.ItemLog.Children.Add(logMessage_);
            RemoveItem(logMessage_);

        }
        internal static async void RemoveItem(OutlinedTextControl l)
        {
            while(_WindowModel.ItemLog.Children.Count > 0)
            {
                if (Paused)
                {
                    // Wait for 200ms to reduce the CPU usage
                    await Task.Delay(200);
                    // Skip the rest of the loop but don't break it
                    continue;
                }
                // Wait for 3 seconds
                await WaitInSeconds(3);
                // Remove the first item in the ItemLog
                _WindowModel.ItemLog.Children.Remove(l);
            }
        }
    }
}
