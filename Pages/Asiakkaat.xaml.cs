using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;

namespace ohj1v0._1;

public partial class Asiakkaat : ContentPage
{
    Funktiot funktiot = new Funktiot();
    AsiakasViewmodel asiakasviewmodel = new AsiakasViewmodel();

    public Asiakkaat()
	{
		InitializeComponent();
        BindingContext = asiakasviewmodel;
	}
  


    private void etunimi_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 20 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 20, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
        funktiot.CheckEntryText(entry, this); // tarkistetaan ettei syottessa ole numeroita
    }

    private void sukunimi_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 40 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 40, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
        funktiot.CheckEntryText(entry, this); // tarkistetaan ettei syottessa ole numeroita
    }

    private void lahiosoite_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 40 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 40, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
    }

    private void postinumero_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 5 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryInteger(entry, this); // funktiossa tarkistetaan ettei syote sisalla tekstia
    }

    private void email_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 50 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 50, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka

    }
    private void puhelinnumero_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 15 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 15, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
        funktiot.CheckEntryInteger(entry, this); // funktiossa tarkistetaan ettei syote sisalla tekstia
    }

    private async void tallenna_Clicked(object sender, EventArgs e)
    {
        Type luokka = typeof(ohj1v0._1.Models.Asiaka);
        string selite = "asiakas";
        Entry posti = postinumero;
        Entry puhelin = puhelinnumero;
        Entry sahkoposti = email;
        Grid grid = (Grid)entry_grid;
        string vertailu = "Puhelinnro";
        string vertailu2 = "Email";

        // if (!funktiot.CheckInput(this, grid)) // Tarkistetaan onko kaikissa entryissa ja pickereissa sisaltoa
        // {
        // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        // }
        
        if (!funktiot.CheckEntryInteger(posti, this)) // tarkistetaan onko postinumero int
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else if (!funktiot.CheckEntryInteger(puhelin, this)) // tarkistetaan onko puhelinnumero int
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else if (!funktiot.CheckTupla(this, puhelin, lista, luokka, selite, vertailu)) // varmistetaan ettei ole samaa puhelinnumeroa
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else if (!funktiot.CheckTupla(this, sahkoposti, lista, luokka, selite, vertailu2)) // varmistetaan ettei ole samaa sahkopostia
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else // Tarkistukset lapi voidaan tallentaa
        {
            try
            {
                using (var dbContext = new VnContext())
                {
                    var asiakas = new Asiaka()
                    {
                        Etunimi = etunimi.Text,
                        Sukunimi = sukunimi.Text,
                        Lahiosoite = lahiosoite.Text,
                        Postinro = postinumero.Text,
                        Email = email.Text,
                        Puhelinnro = puhelinnumero.Text


                        // AlueId päivittyy automaattisesti tietokannassa
                    };

                    dbContext.Asiakas.Add(asiakas);
                    dbContext.SaveChanges();
                    BindingContext = new AsiakasViewmodel();
                    await asiakasviewmodel.LoadAsiakasFromDatabaseAsync();
                }
                await DisplayAlert("Tallennettu", "", "OK");
                grid = (Grid)entry_grid;
                ListView list = (ListView)lista;
                funktiot.TyhjennaEntryt(grid, list);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
            }

        }

    }

    private async void tyhjenna_Clicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti tyhjentää lomakkeen tiedot?", "Kyllä", "Ei");

        // Jos käyttäjä valitsee "Kyllä", toteutetaan peruutustoimet
        if (result)
        {
            Grid grid = (Grid)entry_grid;
            ListView list = (ListView)lista;
            funktiot.TyhjennaEntryt(grid, list);
        }
        else
        {
            // Jos käyttäjä valitsee "Ei", peruutetaan toiminto
            // Tähän ei oo pakko laittaa mitää kerta se ei haluakkaa.
        }
    }

    private async void poista_Clicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti poistaa tiedon?", "Kyllä", "Ei");

        // Jos käyttäjä valitsee "Kyllä", toteutetaan peruutustoimet
        if (result)
        {
            int asiakasId = int.Parse(asiakas_id.Text);

            await asiakasviewmodel.PoistaAsiakasAsync(asiakasId);
            await asiakasviewmodel.LoadAsiakasFromDatabaseAsync();
            Grid grid = (Grid)entry_grid;
            ListView list = (ListView)lista;
            funktiot.TyhjennaEntryt(grid, list);
        }
        else
        {
            await DisplayAlert("Poistaminen peruttu","","OK");
        }
    }

    private void hae_sukunimella_TextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            lista.ItemsSource = asiakasviewmodel.Asiakas; // näytetään kaikki asiakkaat, jos ei mitään hakukentässä
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

        var selectedAsiakas = (Asiaka)e.Item;



        asiakas_id.Text = selectedAsiakas.AsiakasId.ToString();
        etunimi.Text = selectedAsiakas.Etunimi;
        sukunimi.Text = selectedAsiakas.Sukunimi;
        puhelinnumero.Text = selectedAsiakas.Puhelinnro.ToString();
        lahiosoite.Text = selectedAsiakas.Lahiosoite;
        postinumero.Text = selectedAsiakas.Postinro;
        paikkakunta.Text = selectedAsiakas.PostinroNavigation.Toimipaikka;
        email.Text = selectedAsiakas.Email;
    }


}