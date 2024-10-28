using Space_Shooters.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace Space_Shooters.classes.Game.Game_PlayerHandling
{
    internal class PlayerGameHPHandler : INotifyPropertyChanged
    {
        public PlayerGameHPHandler()
        {
            _hp = _UserModel.UserStatBoosted.Health; // Initialize _hp in the constructor
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private static int _hp;
        public static int PlayerHP
        {
            get => _hp;
            set
            {
                if (_hp != value)
                {
                    _hp = value;
                    OnStaticPropertyChanged();
                }
            }
        }

        public static event PropertyChangedEventHandler? StaticPropertyChanged;

        private static void OnStaticPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }
        public static void DecreaseHP(int hpToLose)
        {
            PlayerHP -= hpToLose;
        }
    }
}
