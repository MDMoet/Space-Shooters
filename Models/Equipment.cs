using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class Equipment
{
    public int EquipmentId { get; set; }

    public string Name { get; set; } = null!;

    public string Skin { get; set; } = null!;

    public int Worth { get; set; }

    public int Health { get; set; }

    public int Damage { get; set; }

    public int AttackSpeed { get; set; }

    public int Speed { get; set; }

    public virtual ICollection<Itemshop> Itemshops { get; set; } = new List<Itemshop>();

    public virtual ICollection<UserEquipmentInventory> UserEquipmentInventories { get; set; } = new List<UserEquipmentInventory>();

    public virtual ICollection<UserEquipment> UserEquipments { get; set; } = new List<UserEquipment>();
}
