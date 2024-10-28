using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string Name { get; set; } = null!;

    public string ItemType { get; set; } = null!;

    public int RequiredLevel { get; set; }

    public string Skin { get; set; } = null!;

    public int Worth { get; set; }

    public virtual ICollection<EntityEquipment> EntityEquipments { get; set; } = new List<EntityEquipment>();

    public virtual ICollection<Itemshop> Itemshops { get; set; } = new List<Itemshop>();

    public virtual ICollection<UserItemInventory> UserItemInventories { get; set; } = new List<UserItemInventory>();
}
