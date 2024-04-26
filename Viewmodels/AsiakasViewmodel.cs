using Microsoft.EntityFrameworkCore;
using ohj1v0._1.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

/*Luotu viewmodel asiakas-luokalle 26042024 MH 
 * 
 * 
 */

namespace ohj1v0._1.Viewmodels
{
    public class AsiakasViewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Asiaka> _asiakkaat;

        public ObservableCollection<Asiaka> Asiakas
        {
            get => _asiakkaat;
            set
            {
                if (_asiakkaat != value)
                {
                    _asiakkaat = value;
                    OnPropertyChanged(nameof(Asiakas));
                }
            }
        }

        public AsiakasViewmodel()
        {
            _asiakkaat = new ObservableCollection<Asiaka>();
            LoadAsiakasFromDatabaseAsync();
        }
        public async Task LoadAsiakasFromDatabaseAsync()
        {
            AsiakasLoad loader = new AsiakasLoad();
            var AsiakasFromDb = await loader.LoadAsiakasAsync();
            _asiakkaat.Clear();
            foreach (var asiakas in AsiakasFromDb)
            {
                _asiakkaat.Add(asiakas);
            }
        }
        public async Task PoistaAsiakasAsync(int asiakasId)
        {
            var asiakas = _asiakkaat.FirstOrDefault(a => a.AsiakasId == asiakasId);
            if (asiakas != null)
            {
                _asiakkaat.Remove(asiakas);
                using var context = new VnContext();
                context.Asiakas.Remove(asiakas);
                await context.SaveChangesAsync();
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }



    public class AsiakasLoad
    {
        public async Task<List<Asiaka>> LoadAsiakasAsync()
        {
            using var context = new VnContext();
            var asiakas = await context.Asiakas.Include(m => m.PostinroNavigation).Include(m => m.Varaus).OrderBy(a => a.AsiakasId).ToListAsync();
            return asiakas;
        }
    }


}


