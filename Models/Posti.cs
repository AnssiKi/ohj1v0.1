using System;
using System.Collections.Generic;

namespace ohj1v0._1.Models;

public partial class Posti
{
    public string Postinro { get; set; } = null!;

    public string Toimipaikka { get; set; }

    public virtual ICollection<Asiaka> Asiakas { get; set; } = new List<Asiaka>();

    public virtual ICollection<Mokki> Mokkis { get; set; } = new List<Mokki>();
}
