using Microsoft.Maui.ApplicationModel.Communication;
using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;

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

    private void hae_sukunimella_TextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            lista.BindingContext = asiakasviewmodel; // näytetään kaikki asiakkaat, jos ei mitään hakukentässä
        }
        else
        {
            // filtteröidään ListView sisältö hakukentän mukaan
            var filteredAsiakkaat = asiakasviewmodel.Asiakas.Where(a => a.Sukunimi.ToLower().Contains(searchText.ToLower())).ToList();
            lista.ItemsSource = filteredAsiakkaat;
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
                        VarattuLoppupvm = varauksenTiedot.VarattuLoppupvm,
                        VarauksenPalveluts = varauksenTiedot.VarauksenPalveluts

                    };


                    await varausViewmodel.LoadVarausFromDatabaseAsync();
                    dbContext.Varaus.Add(varaus);
                    dbContext.SaveChanges();
                    BindingContext = new VarausViewmodel();
                    await varausViewmodel.LoadVarausFromDatabaseAsync();
                }

                await DisplayAlert("Varaus tallennettu", "", "OK");
                listaViewModel.NollaaValitutPalvelut();
                await Navigation.PushAsync(new TeeUusiVaraus());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
            }
        }
    }
}