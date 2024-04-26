using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ohj1v0._1.Luokat
{
    internal class Raportti
    {
        public string Raporttityyppi { get; set; }
        public string Alue { get; set; }
        public DateTime Alkupvm { get; set; }
        public DateTime Loppupvm { get; set; }
        public double Yhteensa { get; set; }
    }
}
