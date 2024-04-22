using System;
using System.Collections.Generic;

namespace ohj1v0._1.Models;

public partial class Palvelu
{
    public uint PalveluId { get; set; }

    public uint AlueId { get; set; }

    public string Nimi { get; set; }

    public string Kuvaus { get; set; }

    public double Hinta { get; set; }

    public double Alv { get; set; }

    public virtual Alue Alue { get; set; } = null!;

    public virtual ICollection<VarauksenPalvelut> VarauksenPalveluts { get; set; } = new List<VarauksenPalvelut>();
}
