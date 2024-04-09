using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ohj1v0._1
{
    class Palvelut : Alue
    {

        /*Muokkaushistoria:
         * Luokka luotu 20240409 VH
         */

        //Yksityiset muuttujat
        private int palvelu_id;
        private string palvelu_nimi;
        private string palvelu_kuvaus;
        private double palvelu_hinta;
        private double palvelu_alv;

        //Muodostimet
        public Palvelut() :base() // Kutsuu myos Alue-luokan oletusmuodostinta
        {
        }

        public Palvelut(int palvelu_id, string palvelu_nimi, string palvelu_kuvaus, 
            double palvelu_hinta, double palvelu_alv, int alue_id, string alue_nimi) 
            : base(alue_id, alue_nimi) // Kutsuu myos Alue-luokan parametrillista muodostinta 
        {
            this.palvelu_id = palvelu_id;
            this.palvelu_nimi = palvelu_nimi;
            this.palvelu_kuvaus = palvelu_kuvaus;
            this.palvelu_hinta = palvelu_hinta;
            this.palvelu_alv = palvelu_alv;

        }

        // Getterit ja setterit
        public int PalveluId
        {
            get { return palvelu_id; }
            set { palvelu_id = value; }
        }

        public string PalveluNimi
        {
            get { return palvelu_nimi; }
            set { palvelu_nimi = value; }
        }

        public string PalveluKuvaus
        {
            get { return palvelu_kuvaus; }
            set { palvelu_kuvaus = value; }
        }

        public double PalveluHinta
        {
            get { return palvelu_hinta; }
            set { palvelu_hinta = value; }
        }

        public double PalveluAlv
        {
            get { return palvelu_alv; }
            set { palvelu_alv = value; }
        }


    }
}
