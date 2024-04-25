using Microsoft.EntityFrameworkCore;
using ohj1v0._1.Models;
using System.ComponentModel;

/*Luotu Viewmodel palveluille 25042024 KA
 * 
 * 
 */

namespace ohj1v0._1.Viewmodels
{
    //Luodaan viewmodel, joka käyttää INotifyPropertyChanged-rajapintaa
    public class PalveluViewmodel : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged; 

        private List<Palvelu> _palvelus;

        public List<Palvelu> Palvelus
        {
            get => _palvelus;
            set
            {
                if (_palvelus != value)
                {
                    _palvelus = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Palvelus)));
                }

            }
        }

        //Konstruktori, jossa kutsutaan LoadPalvelusFromDatabaseAsync-metodia, kun viewmodel luodaan
        public PalveluViewmodel()
        {
            LoadPalvelusFromDatabaseAsync();
        }

        //metodi, jossa kutsutaan PalveluLoad luokan LoadPalveluAsync-funktiota.
        public async Task LoadPalvelusFromDatabaseAsync()
        {
            PalveluLoad loader = new PalveluLoad();
            Palvelus = await loader.LoadPalvelusAsync();
            

        }

    }   


        //Metodi, jossa luodaan EFCore-instanssi, jolla noudetaan tiedot tietokannasta.
        public class PalveluLoad
        {
                public async Task<List<Palvelu>> LoadPalvelusAsync()
                {
                    using var context = new VnContext();
                    var palvelus = await context.Palvelus.OrderBy(p => p.PalveluId).ToListAsync(); 
                    return palvelus;
                }
        }

    
    
}
