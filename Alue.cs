using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ohj1v0._1
{
    class Alue
    {
        /*Muokkaushistoria:
         * Luokka luotu 20240409 VH
         */

        //Yksityiset muuttujat
        private int alue_id;
        private string alue_nimi;


        //Muodostimet
        public Alue() //oletusmuodostin
        {
        }

        public Alue(int alue_id, string alue_nimi)
        {
            this.alue_id = alue_id;
            this.alue_nimi = alue_nimi;
        }

        //Getterit ja setterit

        public int AlueId
        {
            get { return alue_id; }
            set { alue_id = value; }
        }

        public string AlueNimi
        {
            get { return alue_nimi; }
            set { alue_nimi = value; }
        }
    }
}
