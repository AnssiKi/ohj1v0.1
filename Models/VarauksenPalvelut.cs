using System;
using System.Collections.Generic;

namespace ohj1v0._1.Models;

public partial class VarauksenPalvelut
{
    public uint VarausId { get; set; }

    public uint PalveluId { get; set; }

    public int Lkm { get; set; }

    public virtual Palvelu Palvelu { get; set; } = null!;

    public virtual Varau Varaus { get; set; } = null!;
}
