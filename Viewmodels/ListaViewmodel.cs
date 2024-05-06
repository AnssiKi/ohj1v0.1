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
        public List<Palvelu> valitutPalvelutLista { get; set; }
        public Dictionary<Palvelu, int> PalveluidenLkm { get; set; } //Tällä saahaan palveluiden lukumäärät mukaan

        // Komento, jota kutsutaan, kun listan itemiä napautetaan
        public Command<Palvelu> OnItemTappedCommand { get; }

        public ListaViewModel()
        {
            Items = new ObservableCollection<Palvelu>();

            valitutPalvelutLista = new List<Palvelu>();

            OnItemTappedCommand = new Command<Palvelu>(OnItemTapped);

            PalveluidenLkm = new Dictionary<Palvelu, int>();

            // Lisää kohteita Items-kokoelmaan
        }

        public void OnItemTapped(Palvelu item)
        {
            if (valitutPalvelutLista.Contains(item))
            {
                valitutPalvelutLista.Remove(item);
                // Vähennä lukumäärää
                if (PalveluidenLkm[item] > 1)
                {
                    PalveluidenLkm[item]--;
                }
                else
                {
                    PalveluidenLkm.Remove(item);
                }
            }


            else
            {
                valitutPalvelutLista.Add(item);
                // Lisää lukumäärä tai aseta se yhdeksi, jos ei ole vielä listalla
                if (PalveluidenLkm.ContainsKey(item))
                {
                    PalveluidenLkm[item]++;
                }
                else
                {
                    PalveluidenLkm[item] = 1;
                }
            } 
        }

        public void PalveluLkm_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = (Entry)sender;

            Palvelu palvelu = entry.BindingContext as Palvelu;

                if (palvelu != null && int.TryParse(entry.Text, out int lukumaara))
                {
                   
                    PalveluidenLkm[palvelu] = lukumaara;
                }
            
        }
        public void NollaaValitutPalvelut()
        {
            valitutPalvelutLista.Clear();
            PalveluidenLkm.Clear();
        }
    }

}

