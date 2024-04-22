using System;
using System.Collections.Generic;

namespace ohj1v0._1.Models;

public partial class Alue
{
    public uint AlueId { get; set; }

    public string Nimi { get; set; }

    public virtual ICollection<Mokki> Mokkis { get; set; } = new List<Mokki>();

    public virtual ICollection<Palvelu> Palvelus { get; set; } = new List<Palvelu>();
}
