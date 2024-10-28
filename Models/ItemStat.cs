using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class ItemStat
{
    public int ItemId { get; set; }

    public int Level { get; set; }

    public int? Damage { get; set; }

    public int? Healing { get; set; }

    public int Durability { get; set; }

    public int AttackSpeed { get; set; }

    public virtual Item Item { get; set; } = null!;
}
