using System;
using System.Collections.Generic;

namespace ohj1v0._1.Models;

public partial class Asiaka
{
    public uint AsiakasId { get; set; }

    public string Postinro { get; set; } = null!;

    public string Etunimi { get; set; }

    public string Sukunimi { get; set; }

    public string Lahiosoite { get; set; }

    public string Email { get; set; }

    public string Puhelinnro { get; set; }

    public virtual Posti PostinroNavigation { get; set; } = null!;

    public virtual ICollection<Varau> Varaus { get; set; } = new List<Varau>();
}
