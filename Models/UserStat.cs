using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class UserStat
{
    public int UserId { get; set; }

    public int Level { get; set; }

    public int LevelProgression { get; set; }

    public int LevelPoints { get; set; }

    public int Health { get; set; }

    public int BaseDamage { get; set; }

    public int BaseSpeed { get; set; }

    public int BaseAttackSpeed { get; set; }

    public virtual User User { get; set; } = null!;
}
