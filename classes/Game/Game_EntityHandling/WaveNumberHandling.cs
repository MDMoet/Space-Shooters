using Space_Shooters.views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using System.Windows.Controls;

namespace Space_Shooters.classes.Game.Game_EntityHandling
{
    public class WaveNumberHandling
    {
        internal bool _waveStarted = true;
    }

    public class WaveNumber : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private static int _wave = 1;

        internal static int Wave
        {
            get => _wave;
            set
            {
                if (_wave != value)
                {
                    _wave = value;
                    OnStaticPropertyChanged();
                }
            }
        }

        public static event PropertyChangedEventHandler? StaticPropertyChanged;

        private static void OnStaticPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        public static void IncreaseWave()
        {
            Wave++;
        }
    }

    public class GameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int Wave
        {
            // Get the wave number from the static property
            get => WaveNumber.Wave;
            set
            {
                if (WaveNumber.Wave != value)
                {
                    WaveNumber.Wave = value;
                    OnPropertyChanged(nameof(Wave));
                }
            }
        }

        public GameViewModel()
        {
            // Subscribe to the static property changed event
            WaveNumber.StaticPropertyChanged += (sender, args) =>
        {
            // Check if the property that changed is the Wave property
            if (args.PropertyName == nameof(WaveNumber.Wave))
            {
                // Raise the PropertyChanged event for the Wave property
                OnPropertyChanged(nameof(Wave));
            }
        };
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}