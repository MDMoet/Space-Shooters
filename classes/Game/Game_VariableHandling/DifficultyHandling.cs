using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Space_Shooters.classes.Game.Game_VariableHandling.Variables;
using static Space_Shooters.classes.Game.Game_VariableHandling.DifficultyVariable;

namespace Space_Shooters.classes.Game.Game_VariableHandling
{
    internal class DifficultyHandling
    {
        internal static void DifficultyVariables()
        {
            // Create temporary variables to store the original values 
            double tempMovement_ = Enemy_ms_Movement;
           double tempSpawn_ = Enemy_ms_Spawning;
            // Decrease the values of the variables based on the difficulty
            tempMovement_ /= ((Convert.ToDouble(Difficulty) / 4) + 1);
           tempSpawn_ /= ((Convert.ToDouble(Difficulty) / 2) + 1);
            // Convert the temporary variables back to integers
            Enemy_ms_Movement = Convert.ToInt32(tempMovement_);
            Enemy_ms_Spawning = Convert.ToInt32(tempSpawn_);
        }

        internal static int LifeIncrease(int lifeIncrease)
        {
            double tempLife = lifeIncrease;
            tempLife *= ((Difficulty / 2) + 1);
            lifeIncrease = Convert.ToInt32(tempLife);
            return lifeIncrease;
        }

        internal static int DamageIncrease(int damageIncrease)
        {
            // Create a temporary variable to store the original value
            double tempDamage = damageIncrease;
            // Increase the value of the variable based on the difficulty
            tempDamage *= ((Convert.ToDouble(Difficulty) / 8) + 1);
            // Convert the temporary variable back to an integer
            damageIncrease = Convert.ToInt32(tempDamage);
            // Return the increased damage value
            return damageIncrease;
        }
    }
}
