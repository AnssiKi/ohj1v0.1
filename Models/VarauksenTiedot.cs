using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ohj1v0._1.Models
{//Luotu luokka varauksen tiedoille siirtymiseen eri sivujen välillä MH
    public class VarauksenTiedot
    {
        public Mokki ValittuMokki { get; set; }
        public Alue ValittuAlue { get; set; }
        public DateTime VarattuAlkupvm { get; set; }
        public DateTime VarattuLoppupvm { get; set; }
        public DateTime Varattupvm { get; set; }
        public DateTime Vahvistuspvm { get; set; }

        public ICollection<VarauksenPalvelut> VarauksenPalveluts { get; set; } = new List<VarauksenPalvelut>();

    }
}
