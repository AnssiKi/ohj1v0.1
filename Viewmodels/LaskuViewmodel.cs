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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LaskuViewmodel()
        {
            LoadLaskut();
        }

        public void LoadLaskut()
        {
            using (var dbContext = new VnContext())
            {
                Laskut = new ObservableCollection<Lasku>(dbContext.Laskus.ToList());
            }
        }
    }
}
