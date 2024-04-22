using System;
using System.Collections.Generic;

namespace ohj1v0._1.Models;

public partial class Varau
{
    public uint VarausId { get; set; }

    public uint AsiakasId { get; set; }

    public uint MokkiId { get; set; }

    public DateTime? VarattuPvm { get; set; }

    public DateTime? VahvistusPvm { get; set; }

    public DateTime? VarattuAlkupvm { get; set; }

    public DateTime? VarattuLoppupvm { get; set; }

    public virtual Asiaka Asiakas { get; set; } = null!;

    public virtual ICollection<Lasku> Laskus { get; set; } = new List<Lasku>();

    public virtual Mokki Mokki { get; set; } = null!;

    public virtual ICollection<VarauksenPalvelut> VarauksenPalveluts { get; set; } = new List<VarauksenPalvelut>();
}
