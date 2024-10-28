using Space_Shooters.classes.Game.Game_EntityHandling;
using Space_Shooters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Space_Shooters.classes.Game.Game_DataHandling
{
    internal class WaveModel
    {
        internal int SpawnIndex { get; set; }
        internal int WaveMin { get; set; }
        internal int WaveMax { get; set; }
        internal bool WaveStarted { get; set; }
        internal bool GameEnded { get; set; }

    }
    
    internal class WindowModel
    {
        internal Grid MainGrid { get; set; }
        internal Border BoPause { get; set; }
        internal Border BoUser { get; set; }
        internal OutlinedTextControl CenterBlock { get; set; }
        internal StackPanel ItemLog { get; set; }
    }
    internal class ItemModel
    {
        internal int[] ItemArray { get; set; }
    }
   
    internal class EntityModel
    {
        internal Entity Entity { get; set; }
        internal EntitySkin EntitySkin { get; set; }
        internal EntityStat EntityStat { get; set; }
        internal EntityEquipment EntityEquipment { get; set; }
    }
    internal class Enemy
    {
        internal string Name { get; set; }
        internal int Level { get; set; }
        internal int Health { get; set; }
        internal int Base_damage { get; set; }
        internal int Base_speed { get; set; }
        internal int Base_attack_speed { get; set; }
        internal string Skin { get; set; }
        internal string? Item { get; set; }
        internal string? Equipment { get; set; }
    }
}
