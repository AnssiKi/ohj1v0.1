using System;
using System.Collections.Generic;

namespace ohj1v0._1.Models;
/*Luotu luokka 22042024 KA
 *Lisätty ALV laskuri 29042024 KA
 * 
 * 
 */
public partial class Palvelu
{
    public uint PalveluId { get; set; }

    public uint AlueId { get; set; }

    public string Nimi { get; set; }

    public string Kuvaus { get; set; }

    public double Hinta { get; set; }

    public double Alv { get; set; }

    //Luotu erillinen muuttuja, jotta voidaan näyttää hinta, joka sisältää ALV:n
    public double HintaAlv => LaskeAlv();

    public virtual Alue Alue { get; set; } = null!;

    public virtual ICollection<VarauksenPalvelut> VarauksenPalveluts { get; set; } = new List<VarauksenPalvelut>();





    //Funktio ALV laskemiseen
    private double LaskeAlv()
    {
        return Hinta + Hinta * (Alv / 100);
    }
}
