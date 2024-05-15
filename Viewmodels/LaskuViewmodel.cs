using Microsoft.EntityFrameworkCore;
using ohj1v0._1.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ohj1v0._1.Viewmodels
{
    public class LaskuViewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Lasku> _laskut;
        public ObservableCollection<Lasku> Laskut
        {
            get => _laskut;
            set
            {
                if (_laskut != value)
                {
                    _laskut = value;
                    OnPropertyChanged(nameof(Laskut));

                }
            }
        }
        public LaskuViewmodel()
        {
            _laskut = new ObservableCollection<Lasku>();
            LoadLaskutFromDatabaseAsync();
        }

        public async Task LoadLaskutFromDatabaseAsync()
        {
            LaskuLoad loader = new LaskuLoad();
            var LaskuFromDb = await loader.LoadLaskuAsync();
            _laskut.Clear();
            foreach (var lasku in LaskuFromDb)
            {
                _laskut.Add(lasku);
            }
        }

        public async Task PoistaLaskuAsync(int laskuID)
        {
            var lasku = _laskut.FirstOrDefault(a => a.LaskuId == laskuID);
            if (lasku != null)
            {
                _laskut.Remove(lasku);
                using var context = new VnContext();
                context.Laskus.Remove(lasku);
                await context.SaveChangesAsync();
            }
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async Task LoadUnPaidLaskutFromDatabaseAsync()
        {
            LaskuLoad loader = new LaskuLoad();
            var unpaidLaskutFromDb = await loader.LoadUnpaidLaskutAsync();
            _laskut.Clear();
            foreach (var lasku in unpaidLaskutFromDb)
            {
                _laskut.Add(lasku);
            }
        }
    }
    public class LaskuLoad
    {
        public async Task<List<Lasku>> LoadLaskuAsync()
        {
            using var context = new VnContext();
            var lasku = await context.Laskus.Include(m => m.Varaus).OrderBy(a => a.LaskuId).ToListAsync();
            return lasku;
        }
        public async Task<List<Lasku>> LoadUnpaidLaskutAsync()
        {
            using var context = new VnContext();
            var unpaidLaskut = await context.Laskus
                .Where(lasku => lasku.Maksettu == 0)
                .ToListAsync();
            return unpaidLaskut;
        }
    }
}

