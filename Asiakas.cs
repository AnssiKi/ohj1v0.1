using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ohj1v0._1
{   // Muokkaushistoria:
    // Luokka luotu 10042024 AK
    class Asiakas
    {
        //Yksityiset muutttujat
        private int asiakasID;
        private int postinumero;
        private string etunimi;
        private string sukunimi;
        private string lahiosoite;
        private string email;
        private string puhelinnumero;

        // Muodostimet
        public Asiakas()
        {
        }
        public Asiakas(int asiakasID, int postinumero, string etunimi, string sukunimi, string lahiosoite, string email, string puhelinnumero)
        {
            this.AsiakasID = asiakasID;
            this.Postinumero = postinumero;
            this.Etunimi = etunimi;
            this.Sukunimi = sukunimi;
            this.Lahiosoite = lahiosoite;
            this.Email = email;
            this.Puhelinnumero = puhelinnumero;
        }

        // GET/SET -metodit
        public int AsiakasID { get => asiakasID; set => asiakasID = value; }
        public int Postinumero { get => postinumero; set => postinumero = value; }
        public string Etunimi { get => etunimi; set => etunimi = value; }
        public string Sukunimi { get => sukunimi; set => sukunimi = value; }
        public string Lahiosoite { get => lahiosoite; set => lahiosoite = value; }
        public string Email { get => email; set => email = value; }
        public string Puhelinnumero { get => puhelinnumero; set => puhelinnumero = value; }
    }
}
