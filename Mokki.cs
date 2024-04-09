using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ohj1v0._1
{
    class Mokki : Alue
    {
        /*Muokkaushistoria:
         * Luokka luotu 20240409 VH
         */

        //Yksityiset muuttujat
        private int mokki_id;
        private string mokki_postinumero; 
        private string mokki_nimi;
        private string mokki_katusosoite;
        private double mokki_hinta;
        private string mokki_kuvaus;
        private int mokki_henkilomaara;
        private string mokki_varustelu;



        //Muodostimet
        public Mokki() : base() // Kutsuu myos Alue-luokan oletusmuodostinta
        {
        }

        public Mokki(int mokki_id, string mokki_postinumero, string mokki_nimi, string mokki_katusosoite,
            double mokki_hinta, string mokki_kuvaus, int mokki_henkilomaara, string mokki_varustelu,
            int alue_id, string alue_nimi)
            : base(alue_id, alue_nimi) // Kutsuu myos Alue-luokan parametrillista muodostinta
        {
            this.mokki_id = mokki_id;
            this.mokki_postinumero = mokki_postinumero;
            this.mokki_nimi = mokki_nimi;
            this.mokki_katusosoite = mokki_katusosoite;
            this.mokki_hinta = mokki_hinta;
            this.mokki_kuvaus = mokki_kuvaus;
            this.mokki_henkilomaara = mokki_henkilomaara;
            this.mokki_varustelu = mokki_varustelu;
        }

        // Getterit ja setterit
        public int MokkiId
        {
            get { return mokki_id; }
            set { mokki_id = value; }
        }
        public string MokkiPostinumero
        {
            get { return mokki_postinumero; }
            set { mokki_postinumero = value; }
        }

        public string Mokkinimi
        {
            get { return mokki_nimi; }
            set { mokki_nimi = value; }
        }

        public string MokkiKatusosoite
        {
            get { return mokki_katusosoite; }
            set { mokki_katusosoite = value; }
        }

        public double MokkiHinta
        {
            get { return mokki_hinta; }
            set { mokki_hinta = value; }
        }

        public string MokkiKuvaus
        {
            get { return mokki_kuvaus; }
            set { mokki_kuvaus = value; }
        }

        public int MokkiHenkilomaara
        {
            get { return mokki_henkilomaara; }
            set { mokki_henkilomaara = value; }
        }

        public string MokkiVarustelu
        {
            get { return mokki_varustelu; }
            set { mokki_varustelu = value; }
        }
    }
}
