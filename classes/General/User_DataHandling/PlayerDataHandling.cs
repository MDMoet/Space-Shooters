using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Space_Shooters.Models;
using Space_Shooters.classes.Game.Game_VariableHandling;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using static Space_Shooters.classes.Game.Game_EntityHandling.WaveNumber;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Space_Shooters.views;
using Microsoft.Identity.Client.NativeInterop;
using Microsoft.VisualBasic.ApplicationServices;
using Azure.Core;
using Space_Shooters.Context;
using System.Windows.Markup.Localizer;
using System.Windows.Input;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Runtime.CompilerServices;
using Microsoft.Web.WebView2.Core;
using Space_Shooters.classes.ItemShop;
using Space_Shooters.classes.UserShop;
using Space_Shooters.classes.General.Shop_DataHandling;
using System.IO;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Space_Shooters.classes.General.User_DataHandling
{
    internal class PlayerDataHandling
    {
        static bool ran = false;
        internal static Dictionary<int, Tuple<Image, OutlinedTextControl>> EquipmentSlotMap = new();
        internal static Tuple<int, int, int, int> PreviousBoosts;

        public static void GetStatsFromDB()
        {
            try
            {
                using var context = new GameContext();
                var userStats = context.UserStats.FirstOrDefault(us => us.UserId == MainWindow.UserId);
                var userGameStats = context.UserGameStats.FirstOrDefault(ugs => ugs.UserId == MainWindow.UserId);
                var userSkin = context.UserSkins.FirstOrDefault(us => us.UserId == MainWindow.UserId);

                _UserModel = new()
                {
                    UserStat = userStats == null ? new UserStat() : new UserStat
                    {
                        Level = userStats.Level,
                        LevelProgression = userStats.LevelProgression,
                        LevelPoints = userStats.LevelPoints,
                        Health = userStats.Health,
                        BaseDamage = userStats.BaseDamage,
                        BaseSpeed = userStats.BaseSpeed,
                        BaseAttackSpeed = userStats.BaseAttackSpeed
                    },
                    UserStatBoosted = userStats == null ? new UserstatBoosted() : new UserstatBoosted
                    {
                        BaseDamage = userStats.BaseDamage,
                        BaseSpeed = userStats.BaseSpeed,
                        BaseAttackSpeed = userStats.BaseAttackSpeed,
                        Health = userStats.Health
                    },
                    UserGameStat = userGameStats == null ? new UserGameStat() : new UserGameStat
                    {
                        WavePr = userGameStats.WavePr,
                        Deaths = userGameStats.Deaths,
                        Kills = userGameStats.Kills,
                        DamageDone = userGameStats.DamageDone,
                        MissedShots = userGameStats.MissedShots,
                        HitShots = userGameStats.HitShots,
                        AverageAccuracy = userGameStats.AverageAccuracy
                    },
                    Skin = userSkin == null ? string.Empty : context.Skins
                    .Where(s => s.SkinId == userSkin.SkinId)
                    .Select(s => s.Skin1)
                    .FirstOrDefault() ?? string.Empty,
                    // Ensure Skin is not null
                    SkinId = userSkin?.SkinId ?? 0,
                    OwnedUserSkins = [], // Initialize empty list
                    LockedUserSkins = [],// Initialize empty list
                };
               
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }

        public static void CheckPurchasedSkins()
        {
            try
            {
                _UserModel.OwnedUserSkins.Clear();
                _UserModel.LockedUserSkins.Clear();
                using var context = new GameContext();
                var userInventorySkins = context.UserSkinsInventories
                    .Where(uis => uis.UserId == MainWindow.UserId)
                    .Select(ui => new { ui.Skin.Skin1, ui.SkinId, ui.Purchased })
                    .ToList();

                foreach (var skin in userInventorySkins)
                {
                    var targetList = skin.Purchased == 1 ? _UserModel.OwnedUserSkins : _UserModel.LockedUserSkins;
                    targetList.Add(skin.Skin1, skin.SkinId);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }

        public static void UpdateSkin(int equippedSkin)
        {
            try
            {
                using var context = new GameContext();
                var userSkins = context.UserSkins.FirstOrDefault(ugs => ugs.UserId == MainWindow.UserId);
                if (userSkins != null)
                {
                    userSkins.SkinId = equippedSkin;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }

        public static void UpdateDatabase()
        {
            UpdatePlayerLevel();
            UpdateGameStats();
        }

        public static void UpdateGameStats()
        {
            try
            {
                using var context = new GameContext();
                var userGameStats = context.UserGameStats.FirstOrDefault(ugs => ugs.UserId == MainWindow.UserId);
                if (userGameStats != null)
                {
                    userGameStats.Kills = _UserModel.UserGameStat.Kills;
                    userGameStats.Deaths = _UserModel.UserGameStat.Deaths;
                    userGameStats.DamageDone = _UserModel.UserGameStat.DamageDone;
                    userGameStats.MissedShots = _UserModel.UserGameStat.MissedShots;
                    userGameStats.HitShots = _UserModel.UserGameStat.HitShots;
                    userGameStats.AverageAccuracy = Convert.ToInt32(AverageCalculator.AverageCalculation(userGameStats.HitShots, userGameStats.MissedShots));
                    if (userGameStats.WavePr < Wave) userGameStats.WavePr = Wave;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }

        public static void UpdateStats()
        {
            try
            {
                using var context = new GameContext();
                var userStats = context.UserStats.FirstOrDefault(ugs => ugs.UserId == MainWindow.UserId);
                if (userStats != null)
                {
                    userStats.LevelPoints = _UserModel.UserStat.LevelPoints;
                    userStats.BaseDamage = _UserModel.UserStat.BaseDamage;
                    userStats.BaseSpeed = _UserModel.UserStat.BaseSpeed;
                    userStats.BaseAttackSpeed = _UserModel.UserStat.BaseAttackSpeed;
                    userStats.Health = _UserModel.UserStat.Health;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }

        public static void UpdatePlayerLevel()
        {
            try
            {
                using var context = new GameContext();
                var userStats = context.UserStats.FirstOrDefault(ugs => ugs.UserId == MainWindow.UserId);
                if (userStats != null)
                {
                    userStats.LevelProgression = _UserModel.UserStat.LevelProgression;
                    userStats.Level = _UserModel.UserStat.Level;
                    userStats.LevelPoints = _UserModel.UserStat.LevelPoints;
                    userStats.Health = _UserModel.UserStat.Health;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }

        /*
        *                  ITEM HANDLING
        */
        public class InventoryItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Amount { get; set; }
            public string Skin { get; set; }
            public int Worth { get; set; }

            public InventoryItem(int equipmentId, string name, int amount, string skin, int worth)
            {
                Id = equipmentId;
                Name = name;
                Amount = amount;
                Skin = skin;
                Worth = worth;
            }
        }

        private static Style ListBoxItemStyle()
        {

            // Create a custom style for ListBoxItem
            Style listBoxItemStyle = new(typeof(ListBoxItem));

            // Set the ControlTemplate to remove the highlight
            ControlTemplate template = new(typeof(ListBoxItem));
            FrameworkElementFactory borderFactory = new(typeof(Border));
            borderFactory.AppendChild(new FrameworkElementFactory(typeof(ContentPresenter)));
            template.VisualTree = borderFactory;

            listBoxItemStyle.Setters.Add(new Setter(Control.TemplateProperty, template));

            // Apply the custom style to the ListBox
            return listBoxItemStyle;
        }
        private static ListBox InventoryListBox(InventoryItem[] userEquipmentInventory, InventoryItem[] userItemInventory)
        {
            // Use the userEquipmentInventory if available, otherwise fall back to userItemInventory
            InventoryItem[] userInventory = [];
            int isEquipment = 0;
            string file = "";

            if (userEquipmentInventory != null)
            {
                userInventory = userEquipmentInventory;
                isEquipment = 1;
                file = "Equipment_Skins";
            }
            else if (userItemInventory != null)
            {
                userInventory = userItemInventory;
                isEquipment = 0;
                file = "Item_Skins";
                userInventory = userInventory.Where(ui => ui.Id != 1).ToArray();
            }
            else
            {
                return null;
            }

            // Create a WrapPanel for the ListBox
            FrameworkElementFactory wrapPanelFactory = new(typeof(WrapPanel));
            wrapPanelFactory.SetValue(WrapPanel.OrientationProperty, Orientation.Horizontal);

            // Create the ListBox
            var listBox = new ListBox
            {
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                ItemsPanel = new ItemsPanelTemplate
                {
                    VisualTree = wrapPanelFactory
                },
                ItemContainerStyle = new Style(typeof(ListBoxItem))
                {
                    BasedOn = ListBoxItemStyle() // Ensure this style is defined
                },
            };

            // Set scroll bar visibility
            listBox.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
            listBox.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);

            // Populate the ListBox with items from the user inventory
            foreach (var item in userInventory)
            {
                // Create a ListBoxItem container
                ListBoxItem container = new()
                {
                    Height = 75,
                    Width = 75,
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFC900")),
                    Background = Brushes.Transparent,
                    Cursor = Cursors.Hand,
                    Margin = new Thickness(5, 2, 0, 3),
                };

                // Create a StackPanel for the item display
                StackPanel stackPanel = new()
                {
                    Tag = $"{item.Id},{isEquipment},{item.Worth}"
                };

                // Add the item image
                stackPanel.Children.Add(new Image
                {
                    Width = 50,
                    Height = 50,
                    Source = new BitmapImage(new Uri($"pack://application:,,,/img/skins/{file}/{item.Skin}.png")), // Adjusted URI format
                    Margin = new Thickness(0, 2, 0, 0)
                });

                // Add item name and amount text blocks
                stackPanel.Children.Add(CreateTextBlock(item.Name, 9)); // Ensure CreateTextBlock is defined
                stackPanel.Children.Add(CreateTextBlock($"{item.Amount}x", 8));

                // Attach event handler
                stackPanel.MouseLeftButtonDown += InventorySelect;

                // Set the content of the ListBoxItem
                container.Content = stackPanel;

                // Add the container to the ListBox
                listBox.Items.Add(container);
            }

            return listBox;
        }

        public static ListBox RetrieveInventoryItems()
        {
            try
            {
                using var context = new GameContext();

                var userItemInventory = context.UserItemInventories
                   .Where(ui => ui.UserId == MainWindow.UserId)
                .Select(ui => new InventoryItem(
                    ui.ItemId,
                    ui.Item.Name,
                    ui.Amount,
                    ui.Item.Skin,
                    ui.Item.Worth))
                .ToArray();

                // Call InventoryListBox with the populated inventory
                var listBox = InventoryListBox([], userItemInventory);

                return listBox;
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
                return new ListBox();
            }
        }

        public static void AddItemsToInventory(int itemId, int itemAmount)
        {
            try
            {
                using var context = new GameContext();
                var userInventory = context.UserItemInventories
                    .FirstOrDefault(ui => ui.UserId == MainWindow.UserId && ui.ItemId == itemId);

                if (userInventory == null)
                {
                    context.UserItemInventories.Add(new UserItemInventory { UserId = MainWindow.UserId, ItemId = itemId, Amount = itemAmount });
                }
                else
                {
                    userInventory.Amount += itemAmount;
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }

        public static void RemoveItemsFromInventory(int itemId, int itemAmount)
        {
            try
            {
                using var context = new GameContext();
                var userInventory = context.UserItemInventories
                    .FirstOrDefault(ui => ui.UserId == MainWindow.UserId && ui.ItemId == itemId);

                if (userInventory != null)
                {
                    userInventory.Amount -= itemAmount;
                    if (userInventory.Amount <= 0) context.UserItemInventories.Remove(userInventory);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }

        /*
         *                  EQUIPMENT HANDLING
         */
        public static void AddEquipmentToInventory(int id, int amount)
        {
            using var context = new GameContext();
            var userInventory = context.UserEquipmentInventories
                .FirstOrDefault(ui => ui.UserId == MainWindow.UserId && ui.EquipmentId == id);

            if (userInventory == null)
            {
                context.UserEquipmentInventories.Add(new UserEquipmentInventory
                {
                    UserId = MainWindow.UserId,
                    EquipmentId = id,
                    Amount = amount
                });
            }
            else
            {
                userInventory.Amount += amount;
            }
            context.SaveChanges();
        }

        public static void RemoveEquipmentFromInventory(int id, int amount)
        {
            using var context = new GameContext();
            var userInventory = context.UserEquipmentInventories
                .FirstOrDefault(ui => ui.UserId == MainWindow.UserId && ui.EquipmentId == id);

            if (userInventory != null)
            {
                userInventory.Amount -= amount;
                if (userInventory.Amount <= 0)
                {
                    context.UserEquipmentInventories.Remove(userInventory);
                }
                context.SaveChanges();
            }
        }

        public static ListBox RetrieveInventoryEquipment()
        {
            using var context = new GameContext();

            // Select and create InventoryItem instances from the UserEquipmentInventories
            var userEquipmentInventory = context.UserEquipmentInventories
                .Where(ui => ui.UserId == MainWindow.UserId)
                .Select(ui => new InventoryItem(
                    ui.EquipmentId,
                    ui.Equipment.Name,
                    ui.Amount,
                    ui.Equipment.Skin,
                    ui.Equipment.Worth))
                .ToArray();

            // Call InventoryListBox with the populated inventory
            var listBox = InventoryListBox(userEquipmentInventory, []);

            return listBox;
        }


        private static TextBlock CreateTextBlock(string text, double fontSize)
        {
            return new TextBlock
            {
                Text = text,
                FontFamily = new FontFamily("Inter"),
                Width = 75,
                TextAlignment = TextAlignment.Center,
                FontSize = fontSize,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFC900")),
                Opacity = 0.6,
                Margin = new Thickness(0)
            };
        }

        private static void InventorySelect(object sender, MouseButtonEventArgs e)
        {

            // Example logic for handling the equipment selection
            if (sender is StackPanel stackPanel)
            {
                if (stackPanel.Tag != null)
                {
                    object[] itemData = stackPanel.Tag.ToString().Split(',');
                    if (SelectAction == "Equipment")
                    {
                        // Retrieve the associated equipment data from the border's DataContext or Tag
                        EquipEquipment(Convert.ToInt32(itemData[0]));
                    }
                    else if (SelectAction == "ItemShop")
                    {
                        // Image
                        // Name
                        // Amount
                        views.ItemShop.SellItemImage.Source = ((Image)stackPanel.Children[0]).Source;
                        views.ItemShop.SellItemName.Text = ((TextBlock)stackPanel.Children[1]).Text;
                        views.ItemShop.SellItemImage.Tag = (string)itemData[0];
                        views.ItemShop.SellItemName.Tag = (string)itemData[1];

                        ItemsOwned = ItemShopHandling.GetAmountOwned((string)itemData[0], (string)itemData[1]);
                        VarAmountInputHandler.AmountOwned = ItemShopHandling.GetAmountOwned((string)itemData[0], (string)itemData[1]);

                        VarAmountInputHandler.Worth = (Convert.ToInt32(itemData[2]) * 0.8).ToString();

                        views.ItemShop.SellItemGrid.Visibility = Visibility.Collapsed;
                        views.ItemShop.ChangeGoldGain();

                    }
                    else if (SelectAction == "UserShop")
                    {
                        views.UserShop.SellItemImage.Source = ((Image)stackPanel.Children[0]).Source;
                        views.UserShop.SellItemName.Text = ((TextBlock)stackPanel.Children[1]).Text;
                        views.UserShop.SellItemImage.Tag = (string)itemData[0];
                        views.UserShop.SellItemName.Tag = (string)itemData[1];

                        ItemsOwned = UserShopHandling.GetAmountOwned((string)itemData[0], (string)itemData[1]);
                        VarAmountInputHandler.AmountOwned = UserShopHandling.GetAmountOwned((string)itemData[0], (string)itemData[1]);

                        VarAmountInputHandler.Worth = (string)itemData[2];

                        views.UserShop.SellItemGrid.Visibility = Visibility.Collapsed;
                        views.UserShop.ChangeGoldGain();
                    }
                    else if (SelectAction == "Inventory")
                    {

                    }
                }

            }
        }


        public static void EquipEquipment(int equipmentId)
        {
            using var context = new GameContext();

            // Get the currently equipped item in the specified slot
            var userEquipment = context.UserEquipments
                .FirstOrDefault(ui => ui.UserId == MainWindow.UserId && ui.Itemslot == ItemSlot);

            // Get the item to equip and its inventory record
            var equipment = context.Equipment.Find(equipmentId);
            var userEquipmentInventory = context.UserEquipmentInventories
                .FirstOrDefault(ui => ui.UserId == MainWindow.UserId && ui.EquipmentId == equipmentId);

            // If the same equipment is already equipped, unequip it
            if (userEquipment != null && userEquipment.EquipmentId == equipmentId)
            {
                UnequipEquipment(); // Call unequip if it's the same item
                return; // Exit the method after unequipping
            }

            // If a different item is equipped, remove it from the slot first
            if (userEquipment != null)
            {
                // Add the currently equipped item back to the inventory
                var currentEquipmentInventory = context.UserEquipmentInventories
                    .FirstOrDefault(ui => ui.UserId == MainWindow.UserId && ui.EquipmentId == userEquipment.EquipmentId);

                if (currentEquipmentInventory != null)
                {
                    currentEquipmentInventory.Amount++;
                }
                else
                {
                    context.UserEquipmentInventories.Add(new UserEquipmentInventory
                    {
                        UserId = MainWindow.UserId,
                        EquipmentId = (int)userEquipment.EquipmentId,
                        Amount = 1
                    });
                }

                // Remove the currently equipped item
                context.UserEquipments.Remove(userEquipment);
            }

            // Now equip the new item if it's available in the inventory
            if (userEquipmentInventory != null && userEquipmentInventory.Amount > 0)
            {
                // Decrement the inventory amount
                userEquipmentInventory.Amount--;

                // If the amount reaches zero, remove it from inventory
                if (userEquipmentInventory.Amount == 0)
                {
                    context.UserEquipmentInventories.Remove(userEquipmentInventory);
                }

                // Add the new equipment to the user's equipment
                context.UserEquipments.Add(new UserEquipment
                {
                    UserId = MainWindow.UserId,
                    EquipmentId = equipmentId,
                    Itemslot = ItemSlot
                });

                context.SaveChanges();
                UpdateEquipmentSlot(equipment);
                EquipmentStatBoost();
                UpdateBoostText();
                views.Equipment.EquipmentBorder.Visibility = Visibility.Collapsed;
            }
        }

        public static void UnequipEquipment()
        {
            using var context = new GameContext();
            
            // Get the currently equipped item in the specified slot
            var userEquipment = context.UserEquipments
                .FirstOrDefault(ui => ui.UserId == MainWindow.UserId && ui.Itemslot == ItemSlot);

            if (userEquipment != null)
            {
                // Get the inventory record for the unequipped item
                var userEquipmentInventory = context.UserEquipmentInventories
                    .FirstOrDefault(ui => ui.UserId == MainWindow.UserId && ui.EquipmentId == userEquipment.EquipmentId);

                // Remove the equipment from the user's equipped items
                context.UserEquipments.Remove(userEquipment);

                // Add it back to the inventory or increment the amount
                if (userEquipmentInventory != null)
                {
                    userEquipmentInventory.Amount++;
                }
                else
                {
                    context.UserEquipmentInventories.Add(new UserEquipmentInventory
                    {
                        UserId = MainWindow.UserId,
                        EquipmentId = (int)userEquipment.EquipmentId,
                        Amount = 1
                    });
                }

                context.SaveChanges();
                UpdateEquipmentSlot(null);
                EquipmentStatBoost();
                UpdateBoostText();
                views.Equipment.EquipmentBorder.Visibility = Visibility.Collapsed;
            }
        }

        private static void UpdateEquipmentSlot(Models.Equipment? equipment)
        {
            if(equipment == null)
            {
                EquipmentSlotMap[ItemSlot].Item1.Source = null;
                EquipmentSlotMap[ItemSlot].Item2.Text = "";
            }
            else
            {
                EquipmentSlotMap[ItemSlot].Item1.Source = new BitmapImage(new Uri($"\\..\\img\\skins\\Equipment_Skins\\{equipment.Skin}.png", UriKind.Relative));
                EquipmentSlotMap[ItemSlot].Item2.Text = equipment.Name;
            }
        }

        public static void LoadEquipment()
        {
            using var context = new GameContext();

            var userEquipments = context.UserEquipments.Where(ui => ui.UserId == MainWindow.UserId).ToList();

            foreach (var item in userEquipments)
            {
                var equipment = context.Equipment.Find(item.EquipmentId);
                if (item.Itemslot != 0)
                {
                    EquipmentSlotMap[item.Itemslot].Item1.Source = new BitmapImage(new Uri($"\\..\\img\\skins\\Equipment_Skins\\{equipment.Skin}.png", UriKind.Relative));
                    EquipmentSlotMap[item.Itemslot].Item2.Text = equipment.Name;
                }
            }
            EquipmentStatBoost();
            UpdateBoostText();
        }

        public static void EquipmentStatBoost()
        {
            using var context = new GameContext();

            var userEquipments = context.UserEquipments.Where(ui => ui.UserId == MainWindow.UserId).ToList();
            var equipmentIds = userEquipments.Select(ui => ui.EquipmentId).ToList();
            var equipments = context.Equipment.Where(eq => equipmentIds.Contains(eq.EquipmentId)).ToDictionary(eq => eq.EquipmentId);

            int totalDamageBoost = 0, totalSpeedBoost = 0, totalAttackSpeedBoost = 0, totalHealthBoost = 0;

            foreach (var userEquipment in userEquipments)
            {
                if (equipments.TryGetValue((int)userEquipment.EquipmentId, out var equipment))
                {
                    totalDamageBoost += equipment.Damage;
                    totalSpeedBoost += equipment.Speed;
                    totalAttackSpeedBoost -= equipment.AttackSpeed;
                    totalHealthBoost += equipment.Health;
                }
            }

            _UserModel.UserStatBoosted.BaseDamage = _UserModel.UserStat.BaseDamage + totalDamageBoost;
            _UserModel.UserStatBoosted.BaseSpeed = _UserModel.UserStat.BaseSpeed + totalSpeedBoost;
            _UserModel.UserStatBoosted.BaseAttackSpeed = _UserModel.UserStat.BaseAttackSpeed + totalAttackSpeedBoost;
            _UserModel.UserStatBoosted.Health = _UserModel.UserStat.Health + totalHealthBoost;

            if (totalDamageBoost > 0 || totalSpeedBoost > 0 || totalAttackSpeedBoost < 0 || totalHealthBoost > 0)
            {
                PreviousBoosts = Tuple.Create(totalDamageBoost, totalSpeedBoost, totalAttackSpeedBoost, totalHealthBoost);
            }
            else
            {
                PreviousBoosts = null;
            }
        }

        private static void UpdateBoostText()
        {
            using var context = new GameContext();
            var userStats = context.UserStats.FirstOrDefault(us => us.UserId == MainWindow.UserId);
            var userEquipments = context.UserEquipments.Where(ui => ui.UserId == MainWindow.UserId).ToList();

            if (userStats != null && PreviousBoosts != null)
            {
                UpdateExtraStatText(PreviousBoosts.Item1, views.Equipment.DamageExtra);
                UpdateExtraStatText(PreviousBoosts.Item2, views.Equipment.MSExtra);
                UpdateExtraStatText(PreviousBoosts.Item3, views.Equipment.ASExtra);
                UpdateExtraStatText(PreviousBoosts.Item4, views.Equipment.HealthExtra);
            }
            else
            {
                views.Equipment.DamageExtra.Visibility = Visibility.Collapsed;
                views.Equipment.MSExtra.Visibility = Visibility.Collapsed;
                views.Equipment.ASExtra.Visibility = Visibility.Collapsed;
                views.Equipment.HealthExtra.Visibility = Visibility.Collapsed;
            }
        }

        private static void UpdateExtraStatText(int statValue, OutlinedTextControl textBlock)
        {
            if (statValue != 0)
            {
                textBlock.Visibility = Visibility.Visible;
                textBlock.Text = statValue > 0 ? $"+{statValue}" : statValue.ToString();
            }
            else
            {
                textBlock.Visibility = Visibility.Collapsed;
            }
        }

        internal static void InitializeDictionary()
        {
            ran = false;
            EquipmentSlotMap = new()
                  {
           { 1, new Tuple<Image, OutlinedTextControl>(views.Equipment.EquipmetSlot1, views.Equipment.NameSlot1) },
           { 2, new Tuple<Image, OutlinedTextControl>(views.Equipment.EquipmetSlot2, views.Equipment.NameSlot2) },
           { 3, new Tuple<Image, OutlinedTextControl>(views.Equipment.EquipmetSlot3, views.Equipment.NameSlot3) },
           { 4, new Tuple<Image, OutlinedTextControl>(views.Equipment.EquipmetSlot4, views.Equipment.NameSlot4) },
                };
        }


        /*
         *                  SHOP HANDLING
         */
        internal static void BuyItem(int id, int amount, int goldLeft, int isEquipment)
        {
            if (amount == 0) return;

            using var context = new GameContext();
            var userId = MainWindow.UserId;

            if (isEquipment == 1)
            {
                AddEquipmentToInventory(id, amount);
            }
            else
            {
                var item = context.Items.Find(id);
                if (item == null || item.RequiredLevel > _UserModel.UserStat.Level)
                {
                    MessageBox.Show("You do not have the required level to purchase this item.");
                    return;
                }
                AddItemsToInventory(id, amount);
            }

            var itemInventory = context.UserItemInventories.First(ui => ui.UserId == userId && ui.ItemId == 1);
            itemInventory.Amount = goldLeft;
            context.UserItemInventories.Update(itemInventory);
            context.SaveChanges();
        }

        internal static void SellItem(int id, int amount, int goldGain, int isEquipment)
        {
            if (amount == 0) return;

            using var context = new GameContext();
            var userId = MainWindow.UserId;

            if (isEquipment == 1)
            {
                RemoveEquipmentFromInventory(id, amount);
            }
            else
            {
                var item = context.Items.Find(id);
                var itemInventory = context.UserItemInventories.FirstOrDefault(ui => ui.UserId == userId && ui.ItemId == id);
                if (itemInventory == null || item.RequiredLevel > _UserModel.UserStat.Level)
                {
                    MessageBox.Show("You do not have the required level to sell this item.");
                    return;
                }
                RemoveItemsFromInventory(id, amount);
            }

            var itemToUpdate = context.UserItemInventories.First(ui => ui.UserId == userId && ui.ItemId == 1);
            itemToUpdate.Amount = goldGain;
            context.UserItemInventories.Update(itemToUpdate);
            context.SaveChanges();
        }

        internal static void UserBuyItem(int id, int amount, int goldLeft, int isEquipment, string userName)
        {
            if (amount == 0) return;

            using var context = new GameContext();
            var userId = MainWindow.UserId;

            if (isEquipment == 1)
            {
                AddEquipmentToInventory(id, amount);
            }
            else
            {
                var item = context.Items.Find(id);
                if (item == null || item.RequiredLevel > _UserModel.UserStat.Level)
                {
                    MessageBox.Show("You do not have the required level to purchase this item.");
                    return;
                }
                AddItemsToInventory(id, amount);
            }

            var sellerId = context.Users.First(us => us.Username == userName).UserId;
            var userShopItem = context.Usershops.FirstOrDefault(us => us.UserId == sellerId && (isEquipment == 1 ? us.EquipmentId == id : us.ItemId == id));

            if (userShopItem != null)
            {
                if (amount >= userShopItem.Amount) context.Usershops.Remove(userShopItem);
                else userShopItem.Amount -= amount;
                context.Usershops.Update(userShopItem);
            }

            UpdateGoldInventory(context, userId, sellerId, amount);
            context.SaveChanges();
        }

        internal static void UserSellItem(int id, int amount, int goldGain, int isEquipment, string userName)
        {
            if (amount == 0) return;

            using var context = new GameContext();
            var userId = MainWindow.UserId;

            if (isEquipment == 1)
            {
                RemoveEquipmentFromInventory(id, amount);
            }
            else
            {
                var item = context.Items.Find(id);
                var itemInventory = context.UserItemInventories.FirstOrDefault(ui => ui.UserId == userId && ui.ItemId == id);
                if (itemInventory == null || item.RequiredLevel > _UserModel.UserStat.Level)
                {
                    MessageBox.Show("You do not have the required level to sell this item.");
                    return;
                }
                RemoveItemsFromInventory(id, amount);
            }

            var userShopItem = context.Usershops.FirstOrDefault(us => us.UserId == userId && (isEquipment == 1 ? us.EquipmentId == id : us.ItemId == id));
            if (userShopItem != null)
            {
                userShopItem.Amount += amount;
                context.Usershops.Update(userShopItem);
            }
            else
            {
                context.Usershops.Add(new Usershop
                {
                    UserId = userId,
                    ItemId = isEquipment == 1 ? 1 : id,
                    EquipmentId = isEquipment == 1 ? id : 1,
                    Isequipment = (sbyte)isEquipment,
                    Amount = amount
                });
            }

            context.SaveChanges();
        }

        private static void UpdateGoldInventory(GameContext context, int buyerId, int sellerId, int amount)
        {
            var buyerGoldInventory = context.UserItemInventories.First(ui => ui.UserId == buyerId && ui.ItemId == 1);
            buyerGoldInventory.Amount -= Convert.ToInt32(VarAmountInputHandler.Cost) * amount;
            context.UserItemInventories.Update(buyerGoldInventory);

            var sellerGoldInventory = context.UserItemInventories.First(ui => ui.UserId == sellerId && ui.ItemId == 1);
            sellerGoldInventory.Amount += Convert.ToInt32(VarAmountInputHandler.Cost) * amount;
            context.UserItemInventories.Update(sellerGoldInventory);
        }

        /*
       *                  AUTHENTICATION HANDLING
       */
        public static bool RegisterUser(string username, string email, string password)
        {
            using var context = new GameContext();
            if (UserExists(username, email, context))
            {
                return false;
            }

            // Hash the password using SHA-256
            var hashedPassword = Hash(password.Trim());

            var user = new Models.User
            {
                Username = username,
                Email = email,
                Password = hashedPassword // Store the hashed password
            };
            context.Users.Add(user);
            context.SaveChanges(); // Save to generate UserId

            var userStats = new Models.UserStat
            {
                UserId = user.UserId,
                Level = 1,
                LevelPoints = 0,
                LevelProgression = 0,
                BaseDamage = 20,
                BaseSpeed = 25,
                BaseAttackSpeed = 1000,
                Health = 100
            };

            var userGameStats = new Models.UserGameStat
            {
                UserId = user.UserId,
                Kills = 0,
                Deaths = 0,
                DamageDone = 0,
                MissedShots = 0,
                HitShots = 0,
                AverageAccuracy = 0,
                WavePr = 0
            };

            var userSkinInventoryList = CreateUserSkinInventory(user.UserId);

            var userSkin = new Models.UserSkin
            {
                UserId = user.UserId,
                SkinId = 1
            };

            var userItemInventory = new Models.UserItemInventory
            {
                UserId = user.UserId,
                ItemId = 1,
                Amount = 100
            };

            context.UserItemInventories.Add(userItemInventory);
            context.UserStats.Add(userStats);
            context.UserGameStats.Add(userGameStats);
            context.UserSkinsInventories.AddRange(userSkinInventoryList);
            context.UserSkins.Add(userSkin);

            context.SaveChanges();

            // Securely retrieve the UserId after saving changes
            MainWindow.UserId = user.UserId;
            UserModels.Username = user.Username;
            UserModels.Email = user.Email;

            GetStatsFromDB();
            UserKeyBinds.GetDataFromDB();
            EquipmentStatBoost();

            // Create or update the user data file
            CreateOrUpdateUserDataFile(user.UserId, user.Password);

            return true;
        }
        public static bool UpdatePassword(string OldPassword, string Newpassword)
        {
            using var context = new GameContext();
            var userData = context.Users.Where(u => u.UserId == MainWindow.UserId).First();

            if (userData != null)
            {
                if (userData.Password == Hash(OldPassword))
                {
                    userData.Password = Hash(Newpassword);
                    context.Update(userData);
                    context.SaveChanges();
                    CreateOrUpdateUserDataFile(MainWindow.UserId, Hash(Newpassword));
                    return true;
                }
            }
            return false;
        }

        private static List<Models.UserSkinsInventory> CreateUserSkinInventory(int userId)
        {
            var userSkinInventoryList = new List<Models.UserSkinsInventory>
    {
        new Models.UserSkinsInventory
        {
            UserId = userId,
            SkinId = 1,
            Purchased = 1
        }
    };

            for (int skinId = 3; skinId <= 16; skinId++)
            {
                userSkinInventoryList.Add(new Models.UserSkinsInventory
                {
                    UserId = userId,
                    SkinId = skinId,
                    Purchased = (sbyte)(skinId == 16 ? 1 : 0)
                });
                if (skinId == 3) skinId = 8;
            }

            return userSkinInventoryList;
        }

        public static bool VerifyUser(string username, string password)
        {
            using var context = new GameContext();
            var user = context.Users.SingleOrDefault(u => u.Username == username);

            if (user == null)
            {
                return false;
            }

            // Hash the entered password
            var enteredPasswordHash = Hash(password.Trim());

            return user.Password == enteredPasswordHash;
        }

        private static string Hash(string input)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }

        private static void CreateOrUpdateUserDataFile(int userId, string encryptedPassword)
        {
            string filePath = "user_data.txt";
            string userData = $"UserId: {userId}, Encrypted Password: {encryptedPassword}";
            File.WriteAllText(filePath, userData + Environment.NewLine);
        }

        internal static bool CheckUserDataFile()
        {
            string filePath = "user_data.txt";
            if (!File.Exists(filePath))
            {
                return false;
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ", " }, StringSplitOptions.None);
                if (parts.Length == 2)
                {
                    var idPart = parts[0].Split(new[] { ": " }, StringSplitOptions.None);
                    var passwordPart = parts[1].Split(new[] { ": " }, StringSplitOptions.None);

                    if (idPart.Length == 2 && passwordPart.Length == 2)
                    {
                        if (int.TryParse(idPart[1], out int fileUserId))
                        {
                            string fileHashedPassword = passwordPart[1];

                            using var context = new GameContext();
                            
                            var user = context.Users.SingleOrDefault(u => u.UserId == fileUserId);
                            if (user != null && user.Password == fileHashedPassword)
                            {
                                MainWindow.UserId = user.UserId;
                                UserModels.Email = user.Email;
                                UserModels.Username = user.Username;

                                GetStatsFromDB();
                                UserKeyBinds.GetDataFromDB();
                                EquipmentStatBoost();
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private static bool UserExists(string username, string email, GameContext context)
        {
            return context.Users.Any(u => u.Username == username || u.Email == email);
        }

    }
}

