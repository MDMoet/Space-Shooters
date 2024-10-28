using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class EntityStat
{
    public int EntityId { get; set; }

    public int MinLevel { get; set; }

    public int MaxLevel { get; set; }

    public int Health { get; set; }

    public int BaseDamage { get; set; }

    public int BaseSpeed { get; set; }

    public int BaseAttackSpeed { get; set; }

    public virtual Entity Entity { get; set; } = null!;
}
