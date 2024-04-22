using System;
using System.Collections.Generic;

namespace ohj1v0._1.Models;

public partial class Mokki
{
    public uint MokkiId { get; set; }

    public uint AlueId { get; set; }

    public string Postinro { get; set; } = null!;

    public string Mokkinimi { get; set; }

    public string Katuosoite { get; set; }

    public double Hinta { get; set; }

    public string Kuvaus { get; set; }

    public int? Henkilomaara { get; set; }

    public string Varustelu { get; set; }

    public virtual Alue Alue { get; set; } = null!;

    public virtual Posti PostinroNavigation { get; set; } = null!;

    public virtual ICollection<Varau> Varaus { get; set; } = new List<Varau>();
}
