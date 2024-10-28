using Space_Shooters.classes.General.Shop_DataHandling;
using Space_Shooters.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Space_Shooters.classes.General.User_DataHandling
{
    internal class UserModels
    {
        internal static UserModel _UserModel;

        internal static int ItemSlot;
        internal static List<UserControl> ViewHistory = [];
        internal static string SelectAction = "";

        internal static AmountInputHandler VarAmountInputHandler;
        internal static string ItemsOwned = "0";

        internal static string Email;
        internal static string Username;
        internal class UserModel
        {
            internal Dictionary<string, int> OwnedUserSkins { get; set; }
            internal Dictionary<string, int> LockedUserSkins { get; set; }
            internal string Skin { get; set; }
            internal int SkinId { get; set; }
            internal UserStat UserStat { get; set; }
            internal UserstatBoosted UserStatBoosted { get; set; }
            internal UserGameStat UserGameStat { get; set; }

        }
        internal class UserstatBoosted
        {
            internal int BaseDamage { get; set; }
            internal int BaseSpeed { get; set; }
            internal int BaseAttackSpeed { get; set; }
            internal int Health { get; set; }
        }
    }
}
