using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public sbyte Admin { get; set; }

    public virtual ICollection<UserEquipmentInventory> UserEquipmentInventories { get; set; } = new List<UserEquipmentInventory>();

    public virtual ICollection<UserEquipment> UserEquipments { get; set; } = new List<UserEquipment>();

    public virtual UserGameStat? UserGameStat { get; set; }

    public virtual ICollection<UserItemInventory> UserItemInventories { get; set; } = new List<UserItemInventory>();

    public virtual ICollection<UserKeybind> UserKeybinds { get; set; } = new List<UserKeybind>();

    public virtual UserSkin? UserSkin { get; set; }

    public virtual ICollection<UserSkinsInventory> UserSkinsInventories { get; set; } = new List<UserSkinsInventory>();

    public virtual UserStat? UserStat { get; set; }

    public virtual ICollection<Usershop> Usershops { get; set; } = new List<Usershop>();
}
