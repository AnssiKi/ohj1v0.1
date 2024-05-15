using Microsoft.EntityFrameworkCore;
using ohj1v0._1.Luokat;
using ohj1v0._1.Models;
using ohj1v0._1.Viewmodels;
using System.Linq;

namespace ohj1v0._1;

public partial class TeeUusiVaraus : ContentPage
{
    Funktiot funktiot = new Funktiot();
    AlueViewmodel alueViewmodel = new AlueViewmodel();
    MokkiViewmodel mokkiViewmodel = new MokkiViewmodel();
    VarausViewmodel varausViewmodel = new VarausViewmodel();
    PalveluViewmodel palveluViewmodel = new PalveluViewmodel();
    ListaViewModel listaViewModel = new ListaViewModel();

    Alue selectedAlue = null;
    Mokki selectedMokki = null;
    Palvelu selectedPalvelu = null;
    private int lukumaara = 0;
    private DateTime? alkupaiva = DateTime.Today;
    private DateTime? loppupaiva = DateTime.Today.AddDays(1);

    public TeeUusiVaraus()
    {
        InitializeComponent();
        alue_nimi.BindingContext = alueViewmodel;
        varauspvm.Text = DateTime.Now.ToString("dd.MM.yyyy");
        mokki_lista.BindingContext = mokkiViewmodel.Mokkis;
        TyhjennaVarausTiedot();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        TyhjennaVarausTiedot();
    }

    private void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (alue_nimi.SelectedItem is Alue selected)
        {
            selectedAlue = selected;
            alkupvm.IsEnabled = true;
            loppupvm.IsEnabled = true;
            HakuBtn.IsEnabled = true;
            LoadPalvelut();  // Kutsu LoadPalvelut-metodia alueen valinnan jälkeen
        }
    }

    private async void alkupvm_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (e.NewDate < DateTime.Today)
        {
            await DisplayAlert("Ilmoitus", "Aloituspäivämäärä tulee olla aikaisintaan tänään", "OK");
            alkupvm.Date = DateTime.Today;  // Resetoi alkupäivämäärä
        }
        else
        {
            alkupaiva = e.NewDate;
            // Varmistetaan, että loppupäivämäärä on vähintään yksi päivä alkupäivämäärän jälkeen
            if (loppupaiva.HasValue && loppupaiva.Value <= alkupaiva.Value)
            {
                loppupaiva = alkupaiva.Value.AddDays(1);
                loppupvm.Date = loppupaiva.Value;
            }
        }
    }

    private async void loppupvm_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (alkupaiva.HasValue && e.NewDate <= alkupaiva.Value)
        {
            await DisplayAlert("Ilmoitus", "Loppupäivämäärä tulee olla vähintään yksi päivä alkupäivämäärän jälkeen", "OK");
            loppupaiva = alkupaiva.Value.AddDays(1);
            loppupvm.Date = loppupaiva.Value;
        }
        else
        {
            loppupaiva = e.NewDate;
        }
    }

    private async void HakuBtn_Clicked(object sender, EventArgs e)
    {
        if (alkupaiva.HasValue && loppupaiva.HasValue)
        {
            if (selectedAlue != null)
            {
                List<Mokki> kaikkiMokit;
                List<Mokki> vapaanaolevat;

                using var context = new VnContext();
                {
                    kaikkiMokit = await context.Mokkis
                        .Where(m => m.AlueId == selectedAlue.AlueId)
                        .Include(m => m.Varaus)
                        .ToListAsync();

                    vapaanaolevat = kaikkiMokit
                        .Where(m => m.Varaus.All(v => v.VarattuLoppupvm < alkupaiva.Value || v.VarattuAlkupvm > loppupaiva.Value))
                        .ToList();
                }

                var mokkiViews = kaikkiMokit.Select(m => new MokkiView
                {
                    Mokki = m,
                    Status = vapaanaolevat.Contains(m) ? "Vapaa" : "Varattu"
                }).ToList();

                mokki_lista.ItemsSource = mokkiViews;
                mokki_lista.IsEnabled = true;
            }
        }
        else
        {
            await DisplayAlert("Ilmoitus", "Valitse aloituspäivämäärä, nämä tiedot ovat pakollisia", "OK");
        }
    }

    private async void mokki_lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var selectedMokkiView = e.Item as MokkiView;
        var selectedMokki = selectedMokkiView?.Mokki;

        if (selectedMokkiView != null && selectedMokkiView.Status == "Vapaa")
        {
            using var context = new VnContext();
            var mokkiId = selectedMokki.MokkiId;

            var mokin_varaukset = context.Varaus
                .Where(v => v.MokkiId == mokkiId &&
                            ((v.VarattuAlkupvm <= alkupaiva && v.VarattuLoppupvm >= alkupaiva) ||
                             (v.VarattuAlkupvm <= loppupaiva && v.VarattuLoppupvm >= loppupaiva)))
                .Select(v => new
                {
                    VarausId = v.VarausId,
                    AlkuPvm = v.VarattuAlkupvm,
                    LoppuPvm = v.VarattuLoppupvm,
                    MokkiNimi = v.Mokki.Mokkinimi,
                    MokkiAlue = v.Mokki.Alue.Nimi,
                    PalveluNimi = v.VarauksenPalveluts.Select(vp => vp.Palvelu.Nimi).FirstOrDefault(),
                    PalveluAlue = v.VarauksenPalveluts.Select(vp => vp.Palvelu.Alue.Nimi).FirstOrDefault()
                })
                .OrderBy(v => v.VarausId)
                .ToList();

            if (mokin_varaukset.Any())
            {
                await DisplayAlert("Ilmoitus", "Mökki on varattuna halutulla ajankohdalla, valitse eri mökki", "OK!");
                selectedMokki = null;
            }
            else
            {
                this.selectedMokki = selectedMokki;
            }
        }
        else
        {
            await DisplayAlert("Ilmoitus", "Voit valita vain vapaana olevia mökkejä", "OK!");
        }
    }


    private async void uusi_asiakas_Clicked(object sender, EventArgs e)
    {
        if (selectedMokki != null && selectedAlue != null && alkupaiva.HasValue && loppupaiva.HasValue)
        {
            VarauksenTiedot varauksenTiedot = await VarauksenTiedotAsync();

            if (varauksenTiedot != null)
            {
                await Navigation.PushAsync(new Uusi_asiakas(this, varauksenTiedot));
            }
        }
        else
        {
            await DisplayAlert("Virhe", "Kaikkia varaukseen tarvittavia tietoja ei ole valittu", "OK");
        }
    }

    private async void vanha_asiakas_Clicked(object sender, EventArgs e)
    {
        if (selectedMokki != null && selectedAlue != null && alkupaiva.HasValue && loppupaiva.HasValue)
        {
            VarauksenTiedot varauksenTiedot = await VarauksenTiedotAsync();

            if (varauksenTiedot != null)
            {
                await Navigation.PushAsync(new Vanha_asiakas(this, varauksenTiedot));
            }
        }
        else
        {
            await DisplayAlert("Virhe", "Kaikkia varaukseen tarvittavia tietoja ei ole valittu", "OK");
        }
    }
    private async Task<VarauksenTiedot> VarauksenTiedotAsync()
    {
        var erotus = alkupvm.Date - DateTime.Now;
        var vahvistus = DateTime.Now;

        if (erotus.TotalDays <= 7)
        {
            vahvistus = DateTime.Now;
        }
        else
        {
            vahvistus = alkupvm.Date - TimeSpan.FromDays(7);
        }

        VarauksenTiedot varauksenTiedot = new VarauksenTiedot
        {
            ValittuMokki = selectedMokki,
            ValittuAlue = selectedAlue,
            VarattuAlkupvm = alkupvm.Date,
            VarattuLoppupvm = loppupvm.Date,
            Varattupvm = DateTime.Now,
            Vahvistuspvm = vahvistus,
            VarauksenPalveluts = new List<VarauksenPalvelut>()
        };

        foreach (var child in palveluContainer.Children)
        {
            if (child is Picker picker && picker.BindingContext is uint palveluId && picker.SelectedItem is int palvelunLukumaara)
            {
                if (palvelunLukumaara > 0)
                {
                    varauksenTiedot.VarauksenPalveluts.Add(new VarauksenPalvelut
                    {
                        PalveluId = palveluId,
                        Lkm = palvelunLukumaara
                    });
                }
            }
        }

        return varauksenTiedot;
    }


    public void TyhjennaVarausTiedot()
    {
        alkupvm.Date = DateTime.Today;
        loppupvm.Date = DateTime.Today.AddDays(1);
        alkupvm.IsEnabled = false;
        loppupvm.IsEnabled = false;
        alue_nimi.SelectedIndex = -1;
        HakuBtn.IsEnabled = false;

        selectedAlue = null;
        selectedMokki = null;
        selectedPalvelu = null;
        lukumaara = 0;

        mokki_lista.ItemsSource = null;
        mokki_lista.IsEnabled = false;

        palveluContainer.Children.Clear();
    }

    private async void LoadPalvelut()
    {
        if (selectedAlue == null) return;

        using var context = new VnContext();
        var palvelut = await context.Palvelus
            .Where(p => p.AlueId == selectedAlue.AlueId)
            .ToListAsync();

        palveluContainer.Children.Clear();
        palveluContainer.RowDefinitions.Clear();

        for (int i = 0; i < palvelut.Count; i++)
        {
            var palvelu = palvelut[i];

            // Lisää rivi Grid:iin
            palveluContainer.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var palveluLabel = new Label
            {
                Text = palvelu.Nimi,
                VerticalOptions = LayoutOptions.Center
            };

            var picker = new Picker
            {
                Title = "Määrä",
                WidthRequest = 100,
                ItemsSource = Enumerable.Range(0, 6).ToList(),
                BindingContext = palvelu.PalveluId, // Aseta BindingContext palvelun ID:lle
                SelectedIndex = 0 // Aseta oletusvalinnaksi ensimmäinen arvo (0)
            };

            picker.SelectedIndexChanged += (s, e) =>
            {
                if (picker.SelectedIndex != -1)
                {
                    var selectedAmount = (int)picker.SelectedItem;
                    listaViewModel.PalveluidenLkm[palvelu.PalveluId] = selectedAmount;
                }
            };

            // Aseta Label ja Picker Grid:iin
            Grid.SetRow(palveluLabel, i);
            Grid.SetColumn(palveluLabel, 0);

            Grid.SetRow(picker, i);
            Grid.SetColumn(picker, 1);

            palveluContainer.Children.Add(palveluLabel);
            palveluContainer.Children.Add(picker);
        }
    }

}
