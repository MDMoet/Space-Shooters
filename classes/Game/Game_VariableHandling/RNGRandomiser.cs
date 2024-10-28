using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mysqlx.Notice.Warning.Types;
using static Space_Shooters.classes.Game.Game_VariableHandling.DifficultyVariable;
using static Space_Shooters.classes.Game.Game_VariableHandling.Variables;
using static Space_Shooters.classes.Game.Game_EntityHandling.WaveNumber;
using System.Windows.Forms;

namespace Space_Shooters.classes.Game.Game_VariableHandling
{
    internal class RNGRandomiser
    {
        internal static bool LayeredRandomise(int requiredLevel, int worth)
        {
            // Adjust this factor as needed to fit your game's balance.
            double difficultyFactor = (1 + Difficulty) * Difficulty_Factor_Increase;
            
            // Calculate a base chance based on worth and level.
            double baseChance = (100.0 / worth) * requiredLevel;

            // Apply the difficulty factor to the base chance.
            double finalChance = baseChance  * difficultyFactor;

            // Ensure the final chance does not exceed 100%.
            finalChance = Math.Min(finalChance, 100.0);

            // Generate a random number to determine if the item drops.
            double roll = Random.Shared.NextDouble() * 100;

            return roll <= finalChance;
        }
        internal static int GoldDropped(int level)
        {
            // Level_ * Difficulty determines the center of the range of gold that can be dropped.
            level = level * Difficulty;

            // The minimum gold dropped is half the level.
            double min = level / 2;

            // The maximum gold dropped is 1.5 times the level.
            double max = level * (1.5 + (Wave / 10));

            // Generate a random number between the min and max values.
            int gold = Random.Shared.Next(Convert.ToInt32(min), Convert.ToInt32(max));


            // Ensure that at least 1 gold is dropped.
            gold = (gold == 0) ? 1 : gold;

            // Return the gold value.
            return gold;
        }
    }
}
