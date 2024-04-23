using Microsoft.EntityFrameworkCore;
using ohj1v0._1.Models;
using System.ComponentModel;

/*Luotu viewmodel alueluokalle 23042024 KA
 * 
 * 
 */

namespace ohj1v0._1.Viewmodels
{   
    public class AlueViewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<Alue> _alues;

        public List<Alue> Alues
        {
            get => _alues;
            set
            {
                if(_alues != value)
                {
                    _alues = value;
                   PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Alues))); 
                }
                


            }
        }

        public AlueViewmodel()
        {
            LoadAluesFromDatabaseAsync();
        }

        public async Task LoadAluesFromDatabaseAsync()
        {
            AlueLoad loader = new AlueLoad();
            Alues = await loader.LoadAluesAsync();
           
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
