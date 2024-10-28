using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using static Space_Shooters.classes.Game.Game_VariableHandling.Variables;
using static Space_Shooters.classes.Game.Game_VariableHandling.DifficultyVariable;
using System.Windows;

namespace Space_Shooters.classes.Game.Game_DataHandling
{
    internal class LevelHandling
    {
        internal static void LevelProgression(Grid entity)
        {
            _UserModel.UserStat.LevelProgression += Convert.ToInt32(entity.Tag) * (Base_Experience_Multiplier + (Difficulty / 2));
            LevelUp();
        }
        internal static void LevelUp()
        {
            double levelRequirement_ = CalculateProgression() - _UserModel.UserStat.LevelProgression;
            if (_UserModel.UserStat.LevelProgression >= CalculateProgression())
            {
                if (Convert.ToInt32(levelRequirement_) < 0) 
                {
                    // If the level requirement is negative, make it positive
                    levelRequirement_ *= -1;
                }
                _UserModel.UserStat.Level++;
                _UserModel.UserStat.LevelProgression = Convert.ToInt32(levelRequirement_);
                _UserModel.UserStat.LevelPoints += 3;
                _UserModel.UserStat.Health += 5;
            }
        }
        internal static double CalculateProgression()
        {
            double progression = 100;

            for (int i = _UserModel.UserStat.Level; i > 0; i--)
            {
                progression =+ (progression * Progression_Requirement_Multiplier);   
            }
            return progression;
        }
    }
}
