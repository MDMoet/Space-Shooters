using Space_Shooters.classes.Game.Game_DataHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Space_Shooters.classes.Game.Game_VariableHandling.GameTick;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;

namespace Space_Shooters.classes.Game.Game_VariableHandling
{
    internal class Wait
    {
        public Wait() { }
        public static async Task
        WaitInSeconds(double seconds)
        {
            // Loops through this for every tick, making you wait 1 second if 'seconds' = 1.
            for (double i = 0; i < _tickRate * seconds; i++)
            {
                // Check if the game is paused
                if (Paused)
                {
                    // Wait for 200ms to reduce the CPU usage
                    if (_WaveModel.GameEnded) break;
                    await Task.Delay(200);
                    // Minus the i to ensure that the loop stays running even while paused
                    i--;
                    // Skip the rest of the loop and wait for the next tick
                    continue;
                }
                // Wait for the tick
                if (_WaveModel.GameEnded) break;
                await Task.Delay(tickMs);
            }
        }
    }
}
