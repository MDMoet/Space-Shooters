using Microsoft.EntityFrameworkCore.Query.Internal;
using Space_Shooters.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using Space_Shooters.classes.General;
using Space_Shooters.classes.Game.Game_DataHandling;
using Space_Shooters.classes.Game.Game_EntityHandling;

namespace Space_Shooters.classes.Game.Game_VariableHandling
{
    internal class DifficultyVariable
    {
        public static int Difficulty = Config.Configuration.Difficulty;
    }
    internal class Variables
    {
        public static int Enemy_ms_Movement = Config.Configuration.Default_Enemy_ms_Movement;
        public static int Enemy_ms_Spawning = Config.Configuration.Default_Enemy_ms_Spawning;
        
        public static int Entity_Wave_Amount = Config.Configuration.Entity_Wave_Amount;

        private static readonly int Bullet_Damage_Multiplier = Config.Configuration.Bullet_Damage_Multiplier;
        private static readonly int Bullet_BaseDamage = _UserModel.UserStatBoosted.BaseDamage; 
        public static int Bullet_Damage = Bullet_BaseDamage * Bullet_Damage_Multiplier;

        internal static bool KeyDownSubscribed = Config.Configuration.KeyDownSubscribed;
        internal static int EntitiesLeft = Config.Configuration.EntitiesLeft;

        public static int Base_Experience_Multiplier = Config.Configuration.Base_Experience_Multiplier;
        public static double Progression_Requirement_Multiplier = Config.Configuration.Progression_Requirement_Multiplier;
        public static double Difficulty_Factor_Increase = Config.Configuration.Difficulty_Factor_Increase;

        internal static List<Grid> EntitiesList = [];
        internal static List<Border> BulletsList = [];
    }
    internal class PassableVariables
    {
        internal static WaveModel _WaveModel;
        internal static WindowModel _WindowModel;
        internal static EntityModel _EntityModel;

        internal static ItemModel _ItemModel;

        internal static Enemy _Enemy;
    }
}
