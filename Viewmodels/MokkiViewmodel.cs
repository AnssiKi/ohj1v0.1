﻿using Microsoft.EntityFrameworkCore;
using ohj1v0._1.Models;
using System.Collections.ObjectModel;
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

        private ObservableCollection<Mokki> _mokkis;

        public ObservableCollection<Mokki> Mokkis
        {
            get => _mokkis;
            set
            {
                if (_mokkis != value)
                {
                    _mokkis = value;
                    OnPropertyChanged(nameof(Mokkis));
                }
            }
        }

        public MokkiViewmodel()
        {
            _mokkis = new ObservableCollection<Mokki>();
            LoadMokkisFromDatabaseAsync();
        }
        public async Task LoadMokkisFromDatabaseAsync()
        {
            MokkiLoad loader = new MokkiLoad();
            var mokkisFromDb = await loader.LoadMokkisAsync();
            _mokkis.Clear();
            foreach (var mokki in mokkisFromDb)
            {
                _mokkis.Add(mokki);
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }



    public class MokkiLoad
    {
        public async Task<List<Mokki>> LoadMokkisAsync()
        {
            using var context = new VnContext();
            var mokkis = await context.Mokkis.Include(m => m.PostinroNavigation).Include(m => m.Alue).OrderBy(a => a.MokkiId).ToListAsync();
            return mokkis;
        }
    }
}