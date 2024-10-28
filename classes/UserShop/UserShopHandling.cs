using Space_Shooters.Context;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Input;
using System.Collections.Frozen;
using Space_Shooters.classes.General.Shop_DataHandling;
using System.Windows.Markup;

namespace Space_Shooters.classes.UserShop
{
    internal class UserShopHandling
    {
        internal static string GetAmountOwned(string id, string isEquipment)
        {
            using var context = new GameContext();
            if (int.TryParse(isEquipment, out int num) && num == 1)
            {
                var userInventory = context.UserEquipmentInventories
               .Where(ui => ui.UserId == MainWindow.UserId)
               .Where(ui => ui.EquipmentId == Convert.ToInt32(id))
               .FirstOrDefault();
                if (userInventory == null) return "0";
                return userInventory.Amount.ToString();
            }
            else
            {
                var userInventory = context.UserItemInventories
                .Where(ui => ui.UserId == MainWindow.UserId)
                .Where(ui => ui.ItemId == Convert.ToInt32(id))
                .FirstOrDefault();
                if (userInventory == null) return "0";
                return userInventory.Amount.ToString();
            }

        }
        public static ListBox RetrieveUserShop()
        {
            try
            {
                using var context = new GameContext();

                var itemShop = context.Usershops
                    .Where(us => us.UserId != MainWindow.UserId)
                    .Select(ui => new
                    {
                        ui.User.Username,
                        ui.Isequipment,
                        Id = ui.Isequipment == 1 ? ui.EquipmentId : ui.ItemId,
                        ui.Amount,
                        Name = ui.Isequipment == 1 ? context.Equipment.Where(eqp => eqp.EquipmentId == ui.EquipmentId).First().Name : context.Items.Where(it => it.ItemId == ui.ItemId).First().Name,
                        Skin = ui.Isequipment == 1 ? context.Equipment.Where(eqp => eqp.EquipmentId == ui.EquipmentId).First().Skin : context.Items.Where(it => it.ItemId == ui.ItemId).First().Skin,
                        ui.Worth

                    })
                    .ToList();

                ListBox listBox = new()
                {
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0)
                };
                listBox.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
                listBox.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                ItemsPanelTemplate itemsPanelTemplate = new();
                FrameworkElementFactory wrapPanelFactory = new(typeof(WrapPanel));
                wrapPanelFactory.SetValue(WrapPanel.OrientationProperty, Orientation.Horizontal);
                itemsPanelTemplate.VisualTree = wrapPanelFactory;
                listBox.ItemsPanel = itemsPanelTemplate;

                // Create a custom style for ListBoxItem
                Style listBoxItemStyle = new(typeof(ListBoxItem));

                // Set the ControlTemplate to remove the highlight
                ControlTemplate template = new(typeof(ListBoxItem));
                FrameworkElementFactory borderFactory = new(typeof(Border));
                borderFactory.AppendChild(new FrameworkElementFactory(typeof(ContentPresenter)));
                template.VisualTree = borderFactory;

                listBoxItemStyle.Setters.Add(new Setter(Control.TemplateProperty, template));

                // Apply the custom style to the ListBox
                listBox.ItemContainerStyle = listBoxItemStyle;

                foreach (var item in itemShop)
                {
                    string skinFile = item.Isequipment switch
                    {
                        0 => "Item_Skins",
                        1 => "Equipment_Skins",
                    };

                    ListBoxItem container = new()
                    {
                        Height = 75,
                        Width = 75,
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFC900")),
                        Background = Brushes.Transparent,
                        IsHitTestVisible = true,
                        Margin = new Thickness(5, 2, 0, 3),
                        Cursor = Cursors.Hand
                    };

                    StackPanel stackPanel = new()
                    {
                        Tag = $"{item.Id},{item.Amount},{item.Worth},{item.Name},{item.Isequipment},{item.Username}"
                    };

                    Image image = new()
                    {
                        Width = 50,
                        Height = 50,
                        Source = new BitmapImage(new Uri($"..\\img\\skins\\{skinFile}\\" + item.Skin + ".png", UriKind.Relative)),
                        Margin = new Thickness(0, 2, 0, 0)
                    };

                    Viewbox viewbox = new();
                    TextBlock textBlock = new()
                    {
                        Text = $"{item.Name}",
                        FontFamily = new FontFamily("Inter"),
                        Width = 75,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 9,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFC900")),
                        Opacity = 0.6,
                        Margin = new Thickness(0, 0, 0, 0)
                    };

                    viewbox.Child = textBlock;
                    stackPanel.Children.Add(image);
                    stackPanel.Children.Add(viewbox);
                    container.Content = stackPanel;
                    listBox.Items.Add(container);
                }
                return listBox;
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
                return null;
            }
        }
        internal static int UserGold()
        {
            using var context = new GameContext();

            var userGold = context.UserItemInventories
                .Where(ui => ui.UserId == MainWindow.UserId)
                .Where(ui => ui.ItemId == 1)
                .Select(ui => ui.Amount)
                .FirstOrDefault();

            return userGold;
        }
    }
}
