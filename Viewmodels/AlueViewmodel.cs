using Microsoft.EntityFrameworkCore;
using ohj1v0._1.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

/*Luotu viewmodel alueluokalle 23042024 KA 
 *Päivitetty LoadAluesFromDatabaseAsync-funktiota Villen toimesta jossain vaiheessa viikkoa 28042024 KA
 * 
 */

namespace ohj1v0._1.Viewmodels
{
    public class AlueViewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Alue> _alues;

        public ObservableCollection<Alue> Alues
        {
            get => _alues;
            set
            {
                if (_alues != value)
                {
                    _alues = value;
                    OnPropertyChanged(nameof(Alues));
                }
            }
        }

        public AlueViewmodel()
        {
            _alues = new ObservableCollection<Alue>();
            LoadAluesFromDatabaseAsync();
        }

        public async Task LoadAluesFromDatabaseAsync()
        {
            AlueLoad loader = new AlueLoad();
            var aluesFromDb = await loader.LoadAluesAsync();
            _alues.Clear();
            foreach (var alue in aluesFromDb)
            {
                _alues.Add(alue);
            }
        }

        public void OnPropertyChanged(string propertyName)//Funktio jota kutsutaan kun tehään alueeseen muutoksia, päivittää pickerit ja listat missä se on
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class AlueLoad
    {
        public async Task<List<Alue>> LoadAluesAsync()
        {
            using var context = new VnContext();
            var alues = await context.Alues.OrderBy(a => a.AlueId).ToListAsync(); //lajitellaan taulun tiedot alueID mukaisesti
            return alues;
        }
    }
}