using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows.Input;
using System.Reflection;
using System.Collections.Generic;
using Space_Shooters.Models;
using System.Linq;
using System.Windows;
using Space_Shooters.Context;
using Space_Shooters.classes.Game.Game_VariableHandling;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using System.Collections.Frozen;

namespace Space_Shooters.classes.General.User_DataHandling
{
    public class UserKeyBinds
    {
        public static Key Pause_Game;
        public static Key Move_Left;
        public static Key Move_Right;
        public static Key Attack_1;
        public static Key Attack_2;
        public static Key Attack_3;

        public static List<string> Actions = [];
        public static Dictionary<string, Tuple<string, int>> PlayerKeyBindsDict = [];

        public static void GetDataFromDB()
        {
            PlayerKeyBindsDict = [];
            Actions = [];
            try
            {
                using var context = new GameContext();
                int currentUserId = MainWindow.UserId; // Assuming MainWindow.UserId holds the current user's ID

                var keyBinds = context.DefaultKeybinds
                           .GroupJoin(
                           context.UserKeybinds.Where(uk => uk.UserId == currentUserId), // Filter by current user's UserId
                           defaultKeybind => defaultKeybind.ActionId,
                           userKeybind => userKeybind.ActionId,
                           (defaultKeybind, userKeybinds) => new { defaultKeybind, userKeybinds })
                           .SelectMany(
                               x => x.userKeybinds.DefaultIfEmpty(),
                               (x, userKeybind) => new
                               {
                                   Keybind = userKeybind != null ? userKeybind.Keybind.Keybind : x.defaultKeybind.DefaultKeybindNavigation.Keybind,
                                   Action = userKeybind != null ? userKeybind.Action.Action : x.defaultKeybind.Action.Action,
                                   Enum = userKeybind != null ? userKeybind.Keybind.KeybindEnum1 : x.defaultKeybind.DefaultKeybindNavigation.KeybindEnum1
                               })
                           .ToList();

                foreach (var keyBind in keyBinds)
                {
                    Actions.Add(keyBind.Action);
                    PlayerKeyBindsDict.Add(keyBind.Action, new Tuple<string, int>(keyBind.Keybind, keyBind.Enum));

                    if (Enum.TryParse(keyBind.Enum.ToString(), out Key key))
                    {
                        FieldInfo? field = typeof(UserKeyBinds).GetField(keyBind.Action, BindingFlags.Public | BindingFlags.Static);
                        field?.SetValue(null, key);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }
        public static async void SetNewKeybind(string action, string pressedKey)
        {
            foreach (var key in PlayerKeyBindsDict)
            {
                if (PlayerKeyBindsDict[key.Key].Item1 == pressedKey)
                {
                    MainWindow.BoDuplicateKeybind.Visibility = Visibility.Visible;
                    await Task.Delay(2000);
                    MainWindow.BoDuplicateKeybind.Visibility = Visibility.Collapsed;
                    return;
                }
            }

            try
            {
                using var context = new GameContext();
                // Read

                var userKeybind = context.UserKeybinds
                    .Where(ugs => ugs.Action.Action == action)
                    .FirstOrDefault(ugs => ugs.UserId == MainWindow.UserId);

                var keyBind = context.KeybindEnums
                   .Where(ugs => ugs.Keybind == pressedKey)
                   .Select(ugs => ugs.KeybindId)
                   .First();

                // Update
                if (userKeybind == null)
                {
                   context.UserKeybinds.Add(new UserKeybind
                   {
                       UserId = MainWindow.UserId,
                       ActionId = context.UserActions.Where(ugs => ugs.Action == action).Select(ugs => ugs.ActionId).First(),
                       KeybindId = keyBind
                   });
                }
                else
                {
                    userKeybind.KeybindId = keyBind;
                }
               

                context.SaveChanges();

                GetDataFromDB();
            }
            catch (Exception e)
            {
                    MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }

        }
    }
}
