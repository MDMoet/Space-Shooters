using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class UserGameStat
{
    public int UserId { get; set; }

    public int WavePr { get; set; }

    public int Deaths { get; set; }

    public int Kills { get; set; }

    public int DamageDone { get; set; }

    public int MissedShots { get; set; }

    public int HitShots { get; set; }

    public int AverageAccuracy { get; set; }

    public virtual User User { get; set; } = null!;
}
