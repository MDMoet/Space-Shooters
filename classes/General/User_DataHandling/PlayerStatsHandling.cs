using Space_Shooters.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using static Space_Shooters.classes.Game.Game_VariableHandling.Variables;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace Space_Shooters.classes.General.User_DataHandling
{
    internal class PlayerHPHandler : INotifyPropertyChanged
    {
        public PlayerHPHandler()
        {
            _hp = _UserModel.UserStat.Health; // Initialize _hp in the constructor
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
    }
    internal class PlayerDamageHandler : INotifyPropertyChanged
    {
        public PlayerDamageHandler()
        {
            _damage = _UserModel.UserStat.BaseDamage; // Initialize _damage in the constructor
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private static int _damage;
        public static int PlayerDamage
        {
            get => _damage;
            set
            {
                if (_damage != value)
                {
                    _damage = value;
                    OnStaticPropertyChanged();
                }
            }
        }

        public static event PropertyChangedEventHandler? StaticPropertyChanged;

        private static void OnStaticPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }
    }
    internal class PlayerASHandler : INotifyPropertyChanged
    {
        public PlayerASHandler()
        {
            _as = _UserModel.UserStat.BaseAttackSpeed; // Initialize _as in the constructor
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private static int _as;
        public static int PlayerAS
        {
            get => _as;
            set
            {
                if (_as != value)
                {
                    _as = value;
                    OnStaticPropertyChanged();
                }
            }
        }

        public static event PropertyChangedEventHandler? StaticPropertyChanged;

        private static void OnStaticPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }
    }
    internal class PlayerMSHanlder : INotifyPropertyChanged
    {
        public PlayerMSHanlder()
        {
            _ms = _UserModel.UserStat.BaseSpeed; // Initialize _as in the constructor
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private static int _ms;
        public static int PlayerMS
        {
            get => _ms;
            set
            {
                if (_ms != value)
                {
                    _ms = value;
                    OnStaticPropertyChanged();
                }
            }
        }

        public static event PropertyChangedEventHandler? StaticPropertyChanged;

        private static void OnStaticPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }
    }
    internal class PlayerLevelHandler : INotifyPropertyChanged
    {
        public PlayerLevelHandler()
        {
            _lvl = _UserModel.UserStat.Level; // Initialize _as in the constructor
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private static int _lvl;
        public static int PlayerLevel
        {
            get => _lvl;
            set
            {
                if (_lvl != value)
                {
                    _lvl = value;
                    OnStaticPropertyChanged();
                }
            }
        }

        public static event PropertyChangedEventHandler? StaticPropertyChanged;

        private static void OnStaticPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }
    }
    internal class PlayerLPHandler : INotifyPropertyChanged
    {
        public PlayerLPHandler()
        {
            _lp = _UserModel.UserStat.LevelProgression; // Initialize _as in the constructor
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        internal static int TillLevelUp = Convert.ToInt32(CalculateProgression());
        private static int _lp;
        public static int PlayerLP
        {
            get => _lp;
            set
            {
                if (_lp != value)
                {
                    _lp = value;
                    OnStaticPropertyChanged();
                }
            }
        }

        public static event PropertyChangedEventHandler? StaticPropertyChanged;

        private static void OnStaticPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        internal static double CalculateProgression()
        {
            double progression = 100;

            for (int i = _UserModel.UserStat.Level; i > 0; i--)
            {
                progression = +(progression * Progression_Requirement_Multiplier);
            }
            return progression;
        }
    }
    internal class PlayerPointsHandler : INotifyPropertyChanged
    {
        public PlayerPointsHandler()
        {
            _lvlp = _UserModel.UserStat.LevelPoints; // Initialize _as in the constructor
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private static int _lvlp;
        public static int PlayerPoints
        {
            get => _lvlp;
            set
            {
                if (_lvlp != value)
                {
                    _lvlp = value;
                    OnStaticPropertyChanged();
                }
            }
        }

        public static event PropertyChangedEventHandler? StaticPropertyChanged;

        private static void OnStaticPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }
    }
}
