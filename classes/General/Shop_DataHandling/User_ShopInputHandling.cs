using Space_Shooters.classes.General.User_DataHandling;
using Space_Shooters.views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using static Space_Shooters.classes.ItemShop.ItemShopHandling;

namespace Space_Shooters.classes.General.Shop_DataHandling
{
    internal class AmountInputHandler : INotifyPropertyChanged
    {
        private string _amount = "0";
        private string _amountForSale = "0";
        private string _amountOwned = "0";// Default value for the amount of items to sell
        private string _cost = "0";
        private string _worth = "0";
        private string _goldHeld = UserGold().ToString();
        private string _goldLeft = "0";
        private string _goldGain = "0";

        private Visibility visibility = Visibility.Collapsed;

        public string Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = Validation(value);
                    OnPropertyChanged(nameof(Amount));
                }
            }
        }
        public string AmountForSale
        {
            get { return _amountForSale; }
            set
            {
                if (_amountForSale != value)
                {
                    _amountForSale = value;
                    OnPropertyChanged(nameof(AmountForSale));
                }
            }
        }
        public string AmountOwned
        {
            get { return _amountOwned; }
            set
            {
                if (_amountOwned != value)
                {
                    _amountOwned = ValidationSell(value);
                    OnPropertyChanged(nameof(AmountOwned));
                }
            }
        }

        public string Cost
        {
            get { return _cost; }
            set
            {
                if (_cost != value)
                {
                    _cost = value;
                    OnPropertyChanged(nameof(Cost));
                }
            }
        }
        public string Worth
        {
            get { return _worth; }
            set
            {
                if (_worth != value)
                {
                    _worth = value;
                    OnPropertyChanged(nameof(Worth));
                }
            }
        }
        public string GoldHeld
        {
            get { return GoldNumberConvert.Convert(_goldHeld); }
            set
            {
                if (_goldHeld != value)
                {
                    _goldHeld = GoldNumberConvert.Convert(value);
                    OnPropertyChanged(nameof(GoldHeld));
                }
            }
        }
        public string GoldLeft
        {
            get { return _goldLeft; }
            set
            {
                if (_goldLeft != value)
                {
                    _goldLeft = value;
                    OnPropertyChanged(nameof(GoldLeft));
                }
            }
        }
        public string GoldGain
        {
            get { return _goldGain; }
            set
            {
                if (_goldGain != value)
                {
                    _goldGain = value;
                    OnPropertyChanged(nameof(GoldGain));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string Validation(string amount)
        {
            if (int.TryParse(amount, out int result) && result > 0 && result < int.MaxValue)
            {
                return result.ToString(); // Return the valid number if it's greater than 0
            }
            else if (result >= int.MaxValue)
            {
                return $"{int.MaxValue}";
            }
            else if (string.IsNullOrEmpty(amount))
            {
                return ""; // Return "" if the input is invalid or empty
            }
            else
            {
                return "0"; // Return 0 if the input is invalid or empty
            }
        }
        private string ValidationSell(string amount)
        {
            if (int.TryParse(amount, out int result) && result > 0 && result <= Convert.ToInt32(ItemsOwned))
            {
                return result.ToString(); // Return the valid number if it's greater than 0
            }
            else if (result >= Convert.ToInt32(ItemsOwned))
            {
                return ItemsOwned;
            }
            else if (string.IsNullOrEmpty(amount))
            {
                return ""; // Return "" if the input is invalid or empty
            }
            else
            {
                return "0"; // Return 0 if the input is invalid or empty
            }
        }
    }
}
