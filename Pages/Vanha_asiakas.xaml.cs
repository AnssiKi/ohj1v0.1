using ohj1v0._1.Luokat;
using ohj1v0._1.Models;
using ohj1v0._1.Viewmodels;

namespace ohj1v0._1;

public partial class Vanha_asiakas : ContentPage
{
    private VarauksenTiedot varauksenTiedot; //T‰t‰ k‰ytet‰‰n varauksen tiedoille
    Funktiot funktiot = new Funktiot(); // Luodaan funktiot-luokan k‰ytˆlle olio

    AsiakasViewmodel asiakasviewmodel = new AsiakasViewmodel(); //luodaan viewmodeleille oliot
    VarausViewmodel varausViewmodel = new VarausViewmodel();
    ListaViewModel listaViewModel = new ListaViewModel();

    TeeUusiVaraus teeUV = new TeeUusiVaraus(); //T‰m‰ luodaan jotta voidaan kutsua sen sivun tyhjenn‰ funktiota
    Asiaka selectedAsiakas = null;
    bool valittu = false;
 
    public Vanha_asiakas(TeeUusiVaraus tuv, VarauksenTiedot tiedot)
    {
        InitializeComponent();
        varauksenTiedot = tiedot;
        lista.BindingContext = asiakasviewmodel;
        BindingContext = tuv;
        teeUV = tuv;
    }

    private void hae_sukunimella_TextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            lista.ItemsSource = asiakasviewmodel.Asiakas; // n‰ytet‰‰n kaikki asiakkaat, jos ei mit‰‰n hakukent‰ss‰
        }
        else
        {
            // filtterˆid‰‰n ListView sis‰ltˆ hakukent‰n mukaan
            var filteredAsiakkaat = asiakasviewmodel.Asiakas
                .Where(a => a.Sukunimi.StartsWith(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
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

                    };
                    await varausViewmodel.LoadVarausFromDatabaseAsync();
                    dbContext.Varaus.Add(varaus);
                    dbContext.SaveChanges();

                    //Kutsutaan funktiota, joka ilmottaa listalle et se on muuttunut ja p‰ivitt‰is listan varaukset sivulla
                    varausViewmodel.OnPropertyChanged(nameof(varausViewmodel.Varaukset));
                    if (varauksenTiedot.VarauksenPalveluts.Any()) { //jos varauksentiedoilla on palvelut, lis‰t‰‰n ne ‰sken luotuun varaukseen

                      var varausId = varaus.VarausId;

                    // Lis‰t‰‰n varauksen ID jokaiseen VarauksenPalvelut-olioon, jos useampi palvelu valittu,voidaan n‰in yhdist‰‰ kaikki palvelut oikeaan varaukseen
                    foreach (var palvelu in varauksenTiedot.VarauksenPalveluts)
                    {
                        palvelu.VarausId = varausId;
                        dbContext.VarauksenPalveluts.Add(palvelu);
                    }

                        dbContext.SaveChanges(); //Tallennetaan muutokset
                     //T‰h‰n v‰liin pit‰‰ laittaa jotain mill‰ p‰ivitet‰‰n varauksen palvelut lista jos ne laitetaan n‰kyville.
                         await varausViewmodel.LoadVarausFromDatabaseAsync(); //Ladataan varauslista uusiksi
                    }
                }

                await DisplayAlert("Ilmoitus", "Varaus tallennettu", "OK");

                listaViewModel.NollaaValitutPalvelut(); //Nollataan valitut palvelut Listaviewmodelista
                await funktiot.TyhjennaVarauksenTiedotAsync(varauksenTiedot); // Nollataan varauksentiedot muuttujat uuestaan k‰ytett‰v‰ksi
                teeUV.TyhjennaVarausTiedot(); //Tyhjennet‰‰n edellinen sivu ennen kuin sinne palataan
                selectedAsiakas = null; //nollataan asiakasvalinta

                await Navigation.PopAsync(); //palataan edelliselle sivulle
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
            }
        }
    }
}