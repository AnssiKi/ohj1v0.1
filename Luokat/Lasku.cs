using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ohj1v0._1.Luokat
{   // Muokkaushistoria:
    // Luokka luotu 10042024 AK
    class Lasku
    {
        //Yksityiset muuttujat
        private int laskuID;
        private double summa;
        private double alv;
        private bool maksettu;

        //Muodostimet
        public Lasku()
        {
        }
        public Lasku(int laskuID, double summa, double alv, bool maksettu)
        {
            this.laskuID = laskuID;
            this.summa = summa;
            this.alv = alv;
            this.maksettu = maksettu;
        }

        //GET/SET -metodit
        public int LaskuID { get => laskuID; set => laskuID = value; }
        public double Summa { get => summa; set => summa = value; }
        public double Alv { get => alv; set => alv = value; }
        public bool Maksettu { get => maksettu; set => maksettu = value; }
    }
}
