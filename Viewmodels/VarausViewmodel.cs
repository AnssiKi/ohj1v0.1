using Microsoft.EntityFrameworkCore;
using ohj1v0._1.Models;
using System.ComponentModel;

/*Luotu viewmodel alueluokalle 25042024 VH 
 * 
 * 
 */

namespace ohj1v0._1.Viewmodels
{
    public class VarausViewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private List<Varau> _varaus;
        public List<Varau> Varaukset
        {
            get => _varaus;
            set
            {
                if (_varaus != value)
                {
                    _varaus = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Varaukset)));
                }
            }
        }

        public VarausViewmodel()
        {
            LoadVarausFromDatabaseAsync();
        }
        public async Task LoadVarausFromDatabaseAsync()
        {
            VarausLoad loader = new VarausLoad();
            Varaukset = await loader.LoadVarauksetAsync();
        }
    }

    public class VarausLoad
    {
        public async Task<List<Varau>> LoadVarauksetAsync()
        {
            using var context = new VnContext();
            var varaus = await context.Varaus.Include(v => v.Asiakas).Include(v => v.Mokki).ThenInclude(m => m.PostinroNavigation).Include(v => v.Mokki).ThenInclude(m => m.Alue).OrderBy(v => v.VarausId).ToListAsync();
            return varaus;
        }
    }
}
