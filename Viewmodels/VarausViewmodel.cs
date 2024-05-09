using Microsoft.EntityFrameworkCore;
using ohj1v0._1.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

/*Luotu viewmodel varauksille 25042024 VH 
 * 
 * 
 */

namespace ohj1v0._1.Viewmodels
{
    public class VarausViewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Varau> _varaukset;

        public ObservableCollection<Varau> Varaukset
        {
            get => _varaukset;
            set
            {
                if (_varaukset != value)
                {
                    _varaukset = value;
                    OnPropertyChanged(nameof(Varaukset));
                }
            }
        }

        public VarausViewmodel()
        {
            _varaukset = new ObservableCollection<Varau>();
            LoadVarausFromDatabaseAsync();
        }
        public async Task LoadVarausFromDatabaseAsync()
        {
            VarausLoad loader = new VarausLoad();
            var varauksetFromDb = await loader.LoadVarauksetAsync();
            _varaukset.Clear();
            foreach (var varaus in varauksetFromDb)
            {
                _varaukset.Add(varaus);
            }
        }

        public async Task PoistaVarausAsync(int varausId)
        {
            var varaus = _varaukset.FirstOrDefault(a => a.VarausId == varausId);

            if (varaus != null)
            {
                _varaukset.Remove(varaus);
                using var context = new VnContext();
                context.Varaus.Remove(varaus);
                await context.SaveChangesAsync();
            }
        }

        public void OnPropertyChanged(string propertyName) //Funktio jota kutsutaan kun varaustiedot muuttuu. päivittää listat
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

public class VarausLoad
    {
    public async Task<List<Varau>> LoadVarauksetAsync()
    {
        using var context = new VnContext();
        var varaukset = await context.Varaus.Include(v => v.Asiakas).Include(v => v.Mokki).ThenInclude(m => m.PostinroNavigation).Include(v => v.Mokki).ThenInclude(m => m.Alue).OrderBy(v => v.VarausId).ToListAsync();
        return varaukset;
    }
}

