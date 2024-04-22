using System;
using System.Collections.Generic;

namespace ohj1v0._1.Models;

public partial class Lasku
{
    public int LaskuId { get; set; }

    public uint VarausId { get; set; }

    public double Summa { get; set; }

    public double Alv { get; set; }

    public sbyte Maksettu { get; set; }

    public virtual Varau Varaus { get; set; } = null!;
}
