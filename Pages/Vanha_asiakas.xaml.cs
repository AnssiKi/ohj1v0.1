using Microsoft.Maui.ApplicationModel.Communication;
using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;
using System.ComponentModel;

namespace ohj1v0._1;

public partial class Vanha_asiakas : ContentPage
{
    private VarauksenTiedot varauksenTiedot;
    AsiakasViewmodel asiakasviewmodel = new AsiakasViewmodel();
    VarausViewmodel varausViewmodel = new VarausViewmodel();
    ListaViewModel listaViewModel = new ListaViewModel();
    public Vanha_asiakas(TeeUusiVaraus tuv,VarauksenTiedot tiedot)
	{
		InitializeComponent();
        BindingContext = asiakasviewmodel;
        varauksenTiedot = tiedot;
        this.BindingContext = tuv;
    }
    Funktiot funktiot = new Funktiot();
    
    Asiaka selectedAsiakas;
    
    bool valittu = false;

    private CancellationTokenSource _cts;

    private async void hae_sukunimella_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts = null;
        }

        string searchText = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            lista.BindingContext = asiakasviewmodel; // n‰ytet‰‰n kaikki asiakkaat, jos ei mit‰‰n hakukent‰ss‰
        }
        else
        {
            _cts = new CancellationTokenSource();
            try
            {
                // Odota 500 millisekuntia ennen haun suorittamista
                await Task.Delay(500, _cts.Token);

                // filtterˆid‰‰n ListView sis‰ltˆ hakukent‰n mukaan
                var filteredAsiakkaat = asiakasviewmodel.Asiakas
                    .Where(a => a.Sukunimi.ToLower().Contains(searchText.ToLower()))
                    .ToList();

                lista.ItemsSource = filteredAsiakkaat;
            }
            catch (TaskCanceledException)
            {
                // Ei tehd‰ mit‰‰n, jos haku peruutetaan
            }
        }
    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item == null)
        {
            return;
        }
        else
        {
            valittu = true;
            selectedAsiakas = (Asiaka)lista.SelectedItem;
        }

    }

    private async void tallenna_Clicked(object sender, EventArgs e)
    { 
        if (valittu) // listasta on valittu asiakas
        {
            try
            {
                using (var dbContext = new VnContext())
                {
                    var varaus = new Varau()
                    {
                        AsiakasId = selectedAsiakas.AsiakasId,
                        MokkiId = varauksenTiedot.ValittuMokki.MokkiId,
                        VarattuPvm = varauksenTiedot.Varattupvm,
                        VahvistusPvm = varauksenTiedot.Vahvistuspvm,
                        VarattuAlkupvm = varauksenTiedot.VarattuAlkupvm,
                        VarattuLoppupvm = varauksenTiedot.VarattuLoppupvm

                    };


                    await varausViewmodel.LoadVarausFromDatabaseAsync();
                    dbContext.Varaus.Add(varaus);
                    dbContext.SaveChanges();

                    varausViewmodel.OnPropertyChanged(nameof(varausViewmodel.Varaukset));
                    //lis‰t‰‰n varaukselle varatut palvelut
                    var varausId = varaus.VarausId;

                    // Lis‰t‰‰n varauksen ID jokaiseen VarauksenPalvelut-olioon
                    foreach (var palvelu in varauksenTiedot.VarauksenPalveluts)
                    {
                        palvelu.VarausId = varausId;
                        dbContext.VarauksenPalveluts.Add(palvelu);
                    }

                    dbContext.SaveChanges(); //Tallennetaan muutokset
                    varausViewmodel.OnPropertyChanged(nameof(varausViewmodel.Varaukset));
                    await varausViewmodel.LoadVarausFromDatabaseAsync(); //Ladataan varauslista uusiksi
                }

                await DisplayAlert("Varaus tallennettu", "", "OK");
                listaViewModel.NollaaValitutPalvelut(); //Nollataan valitut palvelut Listaviewmodelista
                await funktiot.TyhjennaVarauksenTiedotAsync(varauksenTiedot); // Nollataan varauksentiedot muuttujat uuestaan k‰ytett‰v‰ksi

                await Navigation.PushAsync(new TeeUusiVaraus());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
            }
        }

       
    }
   
}