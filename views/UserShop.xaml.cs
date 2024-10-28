using Space_Shooters.classes;
using Space_Shooters.classes.General.User_DataHandling;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Space_Shooters.classes.UserShop.UserShopHandling;
using static Space_Shooters.classes.General.User_DataHandling.PlayerDataHandling;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Space_Shooters.Models;
using Space_Shooters.classes.General.Shop_DataHandling;
using Azure.Core;
using Org.BouncyCastle.Bcpg.Sig;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Space_Shooters.views
{
    /// <summary>
    /// Interaction logic for ItemShop.xaml
    /// </summary>
    public partial class UserShop : UserControl
    {
        internal static ListBox ItemShopList;

        internal static StackPanel PreviousItem;
        internal static StackPanel SelectedItem;

        internal static Image SellItemImage;
        internal static TextBlock SellItemName;

        internal static Grid SellItemGrid;

        private readonly ViewHandler VarViewHandler;

        public UserShop(ViewHandler VarViewHandler)
        {
            PreviousItem = SelectedItem = null;
            InitializeComponent();
            InitializeData();
            
            this.VarViewHandler = VarViewHandler;
        }

        private void InitializeData()
        {
            boUserShop.Child = null;
            VarAmountInputHandler = new AmountInputHandler();

            SellItemImage = imgPopUpItemSell;
            SellItemName = tbPopUpItemSell;
            SellItemGrid = grSellItemContainer;

            otGoldHeld.DataContext = otCost.DataContext = otForSale.DataContext = tbPopUpAmount.DataContext = 
            otPopUpGoldLeft.DataContext = otPopUpCost.DataContext = otPopUpGoldHeld.DataContext = otPopUpCostSell.DataContext = 
            tbPopUpAmountSell.DataContext = otPopUpGoldHeldSell.DataContext = otPopUpGoldLeftSell.DataContext = VarAmountInputHandler;

            boUserShop.Child = RetrieveUserShop();
            SetBuyFunctions();
            SelectAction = "UserShop";
        }

        public void Return(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToMainMenu();
        }

        public void ItemShop(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToItemShop(); // Handle button click to navigate to the game menu
        }

        private async void Buy_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem == null)
            {
                MainWindow.BoSelectItem.Visibility = Visibility.Visible;
                await Task.Delay(1500);
                MainWindow.BoSelectItem.Visibility = Visibility.Collapsed;
                return;
            }
            else
            {
                boConfirmation.Visibility = Visibility.Visible;
                boConfirmationDark.Visibility = Visibility.Visible;
                ChangeGoldLeft();
            }
        }

        private void Sell_Click(object sender, RoutedEventArgs e)
        {
            
            boSell.Visibility = Visibility.Visible;
            boConfirmationDark.Visibility = Visibility.Visible;
            ChangeGoldLeft();
        }

        private void btPopUpReturn_Click(object sender, RoutedEventArgs e)
        {
            imgPopUpItemSell.Source = new BitmapImage(new Uri("../img/select.png", UriKind.Relative));
            tbPopUpItemSell.Text = "Select Item";
            VarAmountInputHandler.AmountOwned = VarAmountInputHandler.Amount = VarAmountInputHandler.Worth = "0";
            ClosePopUp();
        }

        private async void btPopUpBuy_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbPopUpAmount.Text))
            {
                MainWindow.BoNoInputGiven.Visibility = Visibility.Visible;
                await Task.Delay(1500);
                MainWindow.BoNoInputGiven.Visibility = Visibility.Collapsed;
                return;
            }
            if(UserGold() == 0)
            {
                VarAmountInputHandler.Amount = "0";
                return;
            }
            UserBuyItem(GetIntFromTag(imgPopUpItem), GetIntFromString(VarAmountInputHandler.Amount), GetIntFromString(VarAmountInputHandler.GoldLeft), GetIntFromTag(tbPopUpItem), otUserName.Text);
            ChangeGoldLeft();
            InitializeData();
            ClosePopUp();
        }
        private void btDecrement_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(tbPopUpAmount.Text, out int amount) && amount > 0)
            {
                VarAmountInputHandler.Amount = (amount - 1).ToString();
                ChangeGoldLeft();
            }
        }

        private void btIncrement_Click(object sender, RoutedEventArgs e)
        {
            if (UserGold() >= Convert.ToInt32(otPopUpCost.Text) && int.TryParse(tbPopUpAmount.Text, out int amount) && amount < Convert.ToInt32(VarAmountInputHandler.AmountForSale))
            {
                VarAmountInputHandler.Amount = (amount + 1).ToString();
                ChangeGoldLeft();
            }
        }

        internal void ChangeGoldLeft()
        {
            if (!int.TryParse(VarAmountInputHandler.Amount, out int amount)) return;
            int cost = GetIntFromString(VarAmountInputHandler.Cost);

            int answer = UserGold() - (cost * amount);
            if (UserGold() <= 0)
            {
                VarAmountInputHandler.Amount = tbPopUpAmount.Text = "0";
                return;
            }else if (amount > Convert.ToInt32(VarAmountInputHandler.AmountForSale))
            {
                VarAmountInputHandler.Amount = VarAmountInputHandler.AmountForSale;
                otGoldHeld.Text = answer.ToString();
            } else if (answer < 0)
            {
                VarAmountInputHandler.Amount = (UserGold() / cost).ToString();
                tbPopUpAmount.Text = VarAmountInputHandler.Amount;
                otGoldHeld.Text = VarAmountInputHandler.GoldHeld;
            }
            else
            {
                VarAmountInputHandler.GoldLeft = answer.ToString();
            }
        }
        private void imgPopUpItemSell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (RetrieveInventoryEquipment().Items.Count == 0 && RetrieveInventoryItems().Items.Count == 0) { MainWindow.ShowInvalidInput("Nothing owned", Brushes.DarkOrange); return; }
            boItemInventory.Child = RetrieveInventoryItems();
            boEquipmentInventory.Child = RetrieveInventoryEquipment();
            grSellItemContainer.Visibility = Visibility.Visible;
        }
        private async void btPopUpSell_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbPopUpAmountSell.Text))
            {
                MainWindow.BoNoInputGiven.Visibility = Visibility.Visible;
                await Task.Delay(1500);
                MainWindow.BoNoInputGiven.Visibility = Visibility.Collapsed;
                return;
            }
            UserSellItem(GetIntFromTag(imgPopUpItemSell), GetIntFromString(VarAmountInputHandler.AmountOwned), GetIntFromString(VarAmountInputHandler.GoldGain), GetIntFromTag(tbPopUpItemSell), otUserName.Text);
            ChangeGoldGain();
            InitializeData();
            imgPopUpItemSell.Source = new BitmapImage(new Uri("../img/select.png", UriKind.Relative));
            tbPopUpItemSell.Text = "Select Item";
            ClosePopUp();
        }

        private void btDecrementSell_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(tbPopUpAmountSell.Text, out int amount) && amount > 1)
            {
                VarAmountInputHandler.AmountOwned = (amount - 1).ToString();
                ChangeGoldGain();
            }
        }

        private void btIncrementSell_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(tbPopUpAmountSell.Text, out int amount) && amount < Convert.ToInt32(ItemsOwned))
            {
                VarAmountInputHandler.AmountOwned = (amount + 1).ToString();
                ChangeGoldGain();
            }
        }

        internal static void ChangeGoldGain()
        {
            if (!int.TryParse(VarAmountInputHandler.AmountOwned, out int amount)) return;
            int cost = Convert.ToInt32(VarAmountInputHandler.Worth);

            int answer = UserGold() + (cost * amount);
            if (answer < 0)
            {
                VarAmountInputHandler.AmountOwned = (UserGold() / cost).ToString();
                MessageBox.Show(VarAmountInputHandler.GoldHeld);
            }
            else
            {
                VarAmountInputHandler.GoldGain = answer.ToString();
            }
        }


        private void ClosePopUp()
        {
            boConfirmation.Visibility = Visibility.Collapsed;
            boSell.Visibility = Visibility.Collapsed;
            boConfirmationDark.Visibility = Visibility.Collapsed;

            if (SelectedItem != null)
            {
                SelectedItem.Background = null;
                SelectedItem = null;
                PreviousItem = null;
            }
        }

        internal void ItemShopItemClick(object sender, MouseButtonEventArgs e)
        {
            // Deselect the previous item if it exists
            if (PreviousItem != null) PreviousItem.Background = null;

            if (sender is StackPanel stackPanel)
            {
                // Highlight the selected item
                stackPanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ACA700"));

                SelectedItem = stackPanel;

                // Check if the selected item is the same as the previous item
                if (SelectedItem == PreviousItem)
                {
                    // Deselect the item if it is clicked again
                    SelectedItem.Background = null;
                    SelectedItem = null;
                    PreviousItem = null;
                }
                else
                {
                    // Update the previous item to the currently selected item
                    PreviousItem = stackPanel;
                }

                // Retrieve item data from the stack panel's Tag property
                string[] itemData = stackPanel.Tag.ToString().Split(',');
                ImageSource imgSource = null;
                if (stackPanel.Children[0] is Image image)
                {
                    imgSource = image.Source;
                }
                string id = itemData[0];
                string amountForSale = itemData[1];
                string worth = itemData[2];
                string name = itemData[3];
                string isEquipment = itemData[4];
                string userName = itemData[5];

                // Call the method to show item details
                SetBuyDetails(name, amountForSale, worth, id, imgSource, isEquipment, userName);
            }
        }

        private void SetBuyDetails(string name, string amountForSale, string worth, string id, ImageSource imgSource, string isEquipment, string userName)
        {
            VarAmountInputHandler.Cost = worth;

            VarAmountInputHandler.AmountForSale = amountForSale;

            otUserName.Text = userName;
            tbPopUpItem.Text = name;
            tbPopUpItem.Tag = tbPopUpItemSell.Tag = isEquipment;

            imgPopUpItem.Source = imgSource;
            imgPopUpItem.Tag = imgPopUpItemSell.Tag = id;
        }

        private void SetBuyFunctions()
        {
            foreach (ListBoxItem container in ((ListBox)boUserShop.Child).Items)
            {
                if (container.Content is StackPanel stackPanel)
                {
                    stackPanel.MouseDown += ItemShopItemClick;
                }
            }
        }
        private void tbPopUpAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeGoldLeft();
        }
        private void tbPopUpAmountSell_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeGoldGain();
        }

        private int GetIntFromTag(FrameworkElement element)
        {
            return element.Tag != null ? int.Parse(element.Tag.ToString()) : 0;
        }

        private int GetIntFromString(string value)
        {
            return int.TryParse(value, out int result) ? result : 0;
        }
    }
}
