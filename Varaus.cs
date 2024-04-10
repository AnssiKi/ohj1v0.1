using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ohj1v0._1
{   // Muokkaushistoria:
    // Luokka luotu 10042024 AK
    class Varaus
    {
        //Yksityiset muuttujat
        private int varausID;
        private DateTime varauspvm;
        private DateTime vahvistuspvm;
        private DateTime alkupvm;
        private DateTime loppupvm;

        //Muodostimet
        public Varaus()
        {
        }
        public Varaus(int varausID, DateTime varauspvm, DateTime vahvistuspvm, DateTime alkupvm, DateTime loppupvm)
        {
            this.VarausID = varausID;
            this.Varauspvm = varauspvm;
            this.Vahvistuspvm = vahvistuspvm;
            this.Alkupvm = alkupvm;
            this.Loppupvm = loppupvm;
        }

        // GET/SET -metodit
        public int VarausID { get => varausID; set => varausID = value; }
        public DateTime Varauspvm { get => varauspvm; set => varauspvm = value; }
        public DateTime Vahvistuspvm { get => vahvistuspvm; set => vahvistuspvm = value; }
        public DateTime Alkupvm { get => alkupvm; set => alkupvm = value; }
        public DateTime Loppupvm { get => loppupvm; set => loppupvm = value; }
    }
}
