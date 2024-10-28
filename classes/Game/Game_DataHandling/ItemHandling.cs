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
using Space_Shooters.Context;
using Space_Shooters.classes.Game.Game_VariableHandling;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using static Space_Shooters.classes.Game.Game_VariableHandling.RNGRandomiser;
using static Space_Shooters.classes.General.User_DataHandling.PlayerDataHandling;
using static Space_Shooters.classes.Game.Game_UIHandling.ItemLogHandler;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Windows.Controls;



namespace Space_Shooters.classes.Game.Game_DataHandling
{
    internal class ItemHandling
    {
        internal static void GetItemData(Grid entity)
        {
            int entityLevel_ = Convert.ToInt32(entity.Tag);
            int itemsNum_ = _ItemModel.ItemArray.Length - 1;
            Item ItemClass_ = new();
            using var context = new GameContext();
            // Assuming you have a primary key 'Id' in your UserStats table
            foreach (int itemId_ in _ItemModel.ItemArray)
            {
                var Item_ = context.Items.FirstOrDefault(ugs => ugs.ItemId == itemId_);
                if (Item_.Name == "Gold")
                {
                    int goldDropped = GoldDropped(entityLevel_);
                   
                    AddItemsToInventory(itemId_, goldDropped);
                    AddItem(Item_.Name, goldDropped);
                    return;
                }
                if (LayeredRandomise(Item_.RequiredLevel, Item_.Worth))
                {
                    AddItemsToInventory(itemId_, 1);
                    AddItem(Item_.Name, 1);
                }
            }
        }
    }
}
