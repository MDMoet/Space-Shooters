using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Space_Shooters.classes.General.User_DataHandling;
using static Space_Shooters.classes.Game.Game_UIHandling.CenterTextHandling;
using Space_Shooters.views;

namespace Space_Shooters.classes
{
    public class ViewHandler : INotifyPropertyChanged
    {
        private UserControl _view;

        public UserControl View
        {
            get { return _view; }
            set
            {
                _view = value;
                OnPropertyChanged();
            }
        }

        public ViewHandler()
        {
            UserModels.ViewHistory = [];
             _view = new views.Register(this);
            SetPreviousView(_view);
        }

        public void GoToMainMenu()
        {
            View = new MainMenu(this);
            SetPreviousView(View);
        }

        public void GoToGameMenu()
        {
            View = new GameMenu(this);
            SetPreviousView(View);
        }

        public void GoToCharacter()
        {
            View = new Character(this);
            SetPreviousView(View);
        }

        public void GoToInventory()
        {
            View = new views.Inventory(this);
            SetPreviousView(View);
        }

        public void GoToItemShop()
        {
            View = new views.ItemShop(this);
            SetPreviousView(View);
        }

        public void GoToSettings()
        {
            View = new views.Settings(this);
            SetPreviousView(View);
        }

        public void GoToEquipment()
        {
            View = new views.Equipment(this);
            SetPreviousView(View);
        }

        public void GoToSkins()
        {
            View = new Skins(this);
            SetPreviousView(View);
        }

        public void GoToGame()
        {
            View = new views.Game(this);
            CheckConfiguration();
            SetPreviousView(View);
        }
        public void GoToUserShop()
        {
            View = new views.UserShop(this);
            SetPreviousView(View);
        }
        public void GoToLogin()
        {
            View = new Login(this);
        }
        public void GoToRegister()
        {
            View = new Register(this);
        }

        public void Return()
        {
            if (UserModels.ViewHistory.Count > 1)
            {
                // Remove the current view
                UserModels.ViewHistory.RemoveAt(UserModels.ViewHistory.Count - 1);
                // Set the previous view
                View = UserModels.ViewHistory[UserModels.ViewHistory.Count - 1];
            }
            else
            {
                MessageBox.Show("No previous view to return to.");
            }
        }

        private void SetPreviousView(UserControl view)
        {   
            if (view != null)
            {
                UserModels.ViewHistory.Add(view);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
