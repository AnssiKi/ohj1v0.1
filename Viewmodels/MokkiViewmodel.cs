using Microsoft.EntityFrameworkCore;
using ohj1v0._1.Models;
using System.ComponentModel;

/*Luotu viewmodel mokkiluokalle 24042024 VH 
 * 
 * 
 */

namespace ohj1v0._1.Viewmodels
{
    public class MokkiViewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<Mokki> _mokkis;

        public List<Mokki> Mokkis
        {
            get => _mokkis;
            set
            {
                if (_mokkis != value)
                {
                    _mokkis = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mokkis)));
                }
            }
        }

        public MokkiViewmodel()
        {
            LoadMokkisFromDatabaseAsync();
        }

        public async Task LoadMokkisFromDatabaseAsync()
        {
            MokkiLoad loader = new MokkiLoad();
            Mokkis = await loader.LoadMokkisAsync();

        }
    }



    public class MokkiLoad
    {
        public async Task<List<Mokki>> LoadMokkisAsync()
        {
            using var context = new VnContext();
            var mokkis = await context.Mokkis.Include(m => m.PostinroNavigation).Include(m => m.Alue).OrderBy(a => a.MokkiId).ToListAsync(); //lajitellaan taulun tiedot mokkiID mukaisesti
            return mokkis;
        }
    }
}
