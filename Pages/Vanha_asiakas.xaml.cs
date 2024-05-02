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
            lista.BindingContext = asiakasviewmodel; // n‰ytet‰‰n kaikki asiakkaat, jos ei mit‰‰n hakukent‰ss‰
        }
        else
        {
            // filtterˆid‰‰n ListView sis‰ltˆ hakukent‰n mukaan
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
                        VarattuLoppupvm = varauksenTiedot.VarattuLoppupvm

                        //varauksen palvelut pit‰‰ viel‰ saaha kuntoon
                        //VarauksenPalveluts = varauksenTiedot.VarauksenPalveluts

                    };


                    await varausViewmodel.LoadVarausFromDatabaseAsync();
                    dbContext.Varaus.Add(varaus);
                    dbContext.SaveChanges();
                    BindingContext = new VarausViewmodel();
                    await varausViewmodel.LoadVarausFromDatabaseAsync();
                }

                await DisplayAlert("Tallennettu", "", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
            }
        }
    }
}