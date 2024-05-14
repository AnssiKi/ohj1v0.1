using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ohj1v0._1.Models;
using System.ComponentModel;
using ohj1v0._1.Luokat;

namespace ohj1v0._1.Viewmodels
{
    public class ListaViewModel
    {
        public ObservableCollection<Palvelu> Items { get; set; }
        public List<uint> valitutPalvelutIdLista { get; set; }
        public Dictionary<uint, int> PalveluidenLkm { get; set; } //Tällä saahaan palveluiden lukumäärät mukaan

        // Komento, jota kutsutaan, kun listan itemiä napautetaan
        public Command<Palvelu> OnItemTappedCommand { get; }

        public ListaViewModel()
        {
            Items = new ObservableCollection<Palvelu>();

            valitutPalvelutIdLista = new List<uint>();

            OnItemTappedCommand = new Command<Palvelu>(OnItemTapped);

            PalveluidenLkm = new Dictionary<uint, int>();

            
        }

        public void OnItemTapped(Palvelu item)// Lisää kohteita Items-kokoelmaan
        {
            uint palveluId = item.PalveluId;

            if (valitutPalvelutIdLista.Contains(palveluId))
            {
                valitutPalvelutIdLista.Remove(palveluId);//tarkistetaanko onko tupla
                // Vähennä lukumäärää
                if (PalveluidenLkm[palveluId] > 1)
                {
                    PalveluidenLkm[palveluId]--;
                }
                else
                {
                    PalveluidenLkm.Remove(palveluId);
                }
            }


            else
            {
                valitutPalvelutIdLista.Add(palveluId);
                // Lisää lukumäärä tai aseta se yhdeksi, jos ei ole vielä listalla
                if (PalveluidenLkm.ContainsKey(palveluId))
                {
                    PalveluidenLkm[palveluId]++;
                }
                else
                {
                    PalveluidenLkm[palveluId] = 1;
                }
            } 
        }

        public void PalveluLkm_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = (Entry)sender;

            Palvelu palvelu = entry.BindingContext as Palvelu;
            uint palveluId = palvelu.PalveluId;

            if (int.TryParse(entry.Text, out int lukumaara))
            {
                PalveluidenLkm[palveluId] = lukumaara;
            }

        }
        public void NollaaValitutPalvelut()
        {
            valitutPalvelutIdLista.Clear();
            PalveluidenLkm.Clear();
        }

        public void EiValittu(Palvelu selectedPalvelu,int luku)
        {
            Palvelu palvelu = selectedPalvelu;
            uint palveluId = palvelu.PalveluId;
            int lukumaara = luku;

            if (lukumaara!=0)
            {
                PalveluidenLkm[palveluId] = lukumaara;
            }
        }
    }

}

