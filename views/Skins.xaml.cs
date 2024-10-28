using Space_Shooters.classes;
using Space_Shooters.classes.General.User_DataHandling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using static Space_Shooters.classes.General.User_DataHandling.PlayerDataHandling;
using System.Windows.Shapes;
using System.Reflection;
using Space_Shooters.classes.ItemShop;
using Space_Shooters.Context;
using Space_Shooters.classes.Game.Game_VariableHandling;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Space_Shooters.views
{
    /// <summary>
    /// Interaction logic for Skins.xaml
    /// </summary>
    public partial class Skins : UserControl
    {
        private readonly ViewHandler VarViewHandler;
        bool result = false;
        public Skins(ViewHandler VarViewHandler)
        {
            InitializeComponent();
            CheckPurchasedSkins();
            LoadSkins();    
            this.VarViewHandler = VarViewHandler;
        }
        public void LoadSkins()
        {
            lbSkinsInventory.Items?.Clear();
            
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            // Adjust the path to point to the project directory
            var d = new DirectoryInfo(System.IO.Path.Combine(basePath, "..\\..\\..\\img\\skins\\User_Skins\\"));
            int i = 0;
            if (!d.Exists)
            {
                // Handle the case where the directory does not exist
                MessageBox.Show($"The directory {d.FullName} does not exist. Please check the path.", "Directory Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (FileInfo fi in d.GetFiles())
            {
                string skinName = fi.Name.Remove(fi.Name.Length - 4);
                if (!_UserModel.OwnedUserSkins.ContainsKey(skinName) && !_UserModel.LockedUserSkins.ContainsKey(skinName))
                {
                    continue;
                }
                else
                {
                    ListBoxItem container = new()
                    {
                        Height = 75,
                        Width = 75,
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFC900")),
                        Background = Brushes.Transparent,
                        IsHitTestVisible = true,
                        Tag = $"{skinName},{(_UserModel.OwnedUserSkins.ContainsKey(skinName) ? _UserModel.OwnedUserSkins[skinName] : _UserModel.LockedUserSkins[skinName])}",
                        Margin = new Thickness(5, 2, 0, 3)
                    };

                    Image image = new()
                    {
                        Width = 70,
                        Height = 70,
                        Source = new BitmapImage(new Uri(System.IO.Path.Combine(basePath, "..\\..\\..\\img\\skins\\User_Skins\\", fi.Name), UriKind.RelativeOrAbsolute)),
                        Margin = new Thickness(0, 2, 0, 0)
                    };
                    container.Selected += SkinSelect;
                    container.Content = image;

                    if(_UserModel.LockedUserSkins.ContainsKey(skinName))
                    {
                        container.Opacity = 0.4;
                        lbSkinsInventory.Items.Add(container);
                    }
                    else
                    {
                        lbSkinsInventory.Items.Insert(i, container);
                        i++;
                    }                   
                    imgSelectedSkin.Source = new BitmapImage(new Uri(System.IO.Path.Combine(basePath, "..\\..\\..\\img\\skins\\User_Skins\\" + _UserModel.Skin + ".png"), UriKind.RelativeOrAbsolute));
                    tbSkinName.Text = _UserModel.Skin.Replace("_", " ");
                    otEquip.Text = "Equipped";
                }
            }
        }

        private async void SkinSelect(object sender, RoutedEventArgs e)
        {
            if (sender is ListBoxItem listBoxItem)
            {
                string tag = listBoxItem.Tag.ToString();
                string[] values = tag.Split(',');

                string skinName = values[0];
                int skinId = Convert.ToInt32(values[1]);

                if (_UserModel.LockedUserSkins.ContainsKey(skinName))
                {
                    // Prompt the user to buy the skin
                    boConfirmPurchase.Visibility = Visibility.Visible;
                    // Wait for user response
                    var result = await ShowConfirmationDialog(); // New method for confirmation dialog

                    if (!result)
                    {
                        return;
                    }
                    else
                    {
                        // Check if the user has enough gold
                        if (ItemShopHandling.UserGold() >= 1000)
                        {
                            using var context = new GameContext();
                            var buyerGoldInventory = context.UserItemInventories.First(ui => ui.UserId == MainWindow.UserId && ui.ItemId == 1);
                            buyerGoldInventory.Amount -= 1000;
                            context.UserItemInventories.Update(buyerGoldInventory);
                            var skinInventory = context.UserSkinsInventories.First(ui => ui.UserId == MainWindow.UserId && ui.SkinId == skinId);
                            skinInventory.Purchased = 1;
                            context.SaveChanges();

                            _UserModel.OwnedUserSkins[skinName] = skinId;
                            _UserModel.LockedUserSkins.Remove(skinName);

                            boPurchaseMessage.Visibility = Visibility.Visible;
                            await Task.Delay(1500);
                            boPurchaseMessage.Visibility = Visibility.Collapsed;
                            LoadSkins();
                        }
                        else
                        {
                            boNotEnough.Visibility = Visibility.Visible;
                            await Task.Delay(2500);
                            boNotEnough.Visibility = Visibility.Collapsed;
                        }
                    }
                }
                else
                {
                    _UserModel.SkinId = skinId;
                    // Handle the button click event
                    if (skinName != _UserModel.Skin)
                    {
                        otEquip.Text = "Equip";
                    }
                    else
                    {
                        otEquip.Text = "Equipped";
                    }
                    imgSelectedSkin.Source = ((Image)listBoxItem.Content).Source;
                    tbSkinName.Text = skinName.Replace("_", " ");
                }
            }
        }

        private Task<bool> ShowConfirmationDialog()
        {
            var tcs = new TaskCompletionSource<bool>();
            YesButton.Click += (s, e) =>
            {
                tcs.TrySetResult(true);
                boConfirmPurchase.Visibility = Visibility.Collapsed; // Hide the confirmation dialog
            };

            NoButton.Click += (s, e) =>
            {
                
                tcs.TrySetResult(false);
                boConfirmPurchase.Visibility = Visibility.Collapsed; // Hide the confirmation dialog
            };

            return tcs.Task;
        }

        public void Equip(object sender, RoutedEventArgs e)
        {
            _UserModel.Skin = tbSkinName.Text.Replace(" ", "_");
            otEquip.Text = "Equipped";
            GameMenu.UpdateSkinStatic();
            UpdateSkin(_UserModel.SkinId);
        }
        public void Return(object sender, RoutedEventArgs e)
        {
            VarViewHandler.Return();
        }
    }
}
