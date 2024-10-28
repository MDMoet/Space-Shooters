using Space_Shooters.classes.Game.Game_EntityHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using static Space_Shooters.classes.Game.Game_PlayerHandling.PlayerGameHPHandler;
using static Space_Shooters.classes.Game.Game_VariableHandling.DifficultyHandling;
using static Space_Shooters.views.Game;
using Space_Shooters.classes.Game.Game_UIHandling;
using Space_Shooters.Models;
using System.Windows.Controls;
using System.Windows;

namespace Space_Shooters.classes.Game.Game_PlayerHandling
{
    internal class PlayerDamageHandler
    { 
        internal static void HandlePlayerDamage(int damage)
        {
            // Increase the damage by the difficulty
            damage = DamageIncrease(damage);
            if (PlayerHP - damage <= 0)
            {
                PlayerHP = 0;
                _UserModel.UserGameStat.Deaths++;
                GameOver();
            }
            else
            {
                DecreaseHP(damage);
            }
        }
    }
}
