using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;

namespace ohj1v0._1;

public partial class Mokit : ContentPage
{
    public Mokit()
    {
        InitializeComponent();
        lista.BindingContext = mokkiViewmodel;
        alue_nimi.BindingContext = alueViewmodel;
    }
    Funktiot funktiot = new Funktiot();
    MokkiViewmodel mokkiViewmodel = new MokkiViewmodel();
    AlueViewmodel alueViewmodel = new AlueViewmodel();

    private void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void mokki_nimi_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 45 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 45, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
    }

    private void mokki_nimi_Unfocused(object sender, FocusEventArgs e)
    {
        Type luokka = typeof(Mokki);
        String selite = "mökki";
        Entry entry = mokki_nimi;
        string vertailu = "Mokkinimi";

        if (!funktiot.CheckTupla(this, entry, lista, luokka, selite, vertailu)) // varmistetaan ettei ole samannimista aluetta
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

    }

    private void mokki_katuosoite_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 45 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 45, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
    }

    private void mokki_postinumero_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 5 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryInteger(entry, this); // funktiossa tarkistetaan ettei syote sisalla tekstia
    }

    private void mokki_hinta_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 8 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 8, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
        funktiot.CheckEntryDouble(entry, this); // tarkistetaan että syote on double
    }

    private void mokki_kuvaus_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 150 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 150, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
    }

    private void mokki_varustelu_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 100 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 100, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
    }

    private async void tallenna_Clicked(object sender, EventArgs e)
    {
        Type luokka = typeof(Mokki);
        string selite = "mökki";
        Entry nimi = mokki_nimi;
        Entry hinta = mokki_hinta;
        Entry postinumero = mokki_postinumero;
        Grid grid = (Grid)entry_grid;
        string vertailu = "Mokkinimi";

        if (!funktiot.CheckInput(this, grid)) // Tarkistetaan onko kaikissa entryissa ja pickereissa sisaltoa
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else if (!funktiot.CheckTupla(this, nimi, lista, luokka, selite, vertailu)) // varmistetaan ettei ole samannimista mokkia
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else if (!funktiot.CheckEntryDouble(hinta, this)) // tarkistetaan onko hinta double
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else if (!funktiot.CheckEntryInteger(postinumero, this)) // tarkistetaan onko postinumero int
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else // Tarkistukset lapi voidaan tallentaa
        {
            try
            {
                // CRUD - toiminnot
                await DisplayAlert("Tallennettu", "", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
            }

        }

    }

    private void mokki_henkilomaara_SelectedIndexChanged(object sender, EventArgs e)
    {

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
            //poistetaan tiedot tähän
        }
        else
        {
            // Jos käyttäjä valitsee "Ei", peruutetaan toiminto
            // Tähän ei oo pakko laittaa mitää kerta se ei haluakkaa poistaa.
        }

    }

    private void hae_nimella_TextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            lista.ItemsSource = mokkiViewmodel.Mokkis; // naytetaan kaikki mokit jos ei mitaan hakukentassa
        }
        else
        {
            // filtteroidaan listview sisalto hakukentan mukaan
            var filteredMokkis = mokkiViewmodel.Mokkis.Where(m => m.Mokkinimi.ToLower().Contains(searchText.ToLower())).ToList();
            lista.ItemsSource = filteredMokkis;
        }
    }


    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item == null)
        {
            return;
        }

        var selectedMokki = (Mokki)e.Item;

        //alueen haku ei toimi kunnolla 

        if (selectedMokki.Alue != null)
        {
            alue_nimi.SelectedIndex = (int)selectedMokki.Alue.AlueId -1;
        }
        else
        {
            alue_nimi.SelectedItem = null;
        }

        mokki_id.Text = selectedMokki.MokkiId.ToString();
        mokki_nimi.Text = selectedMokki.Mokkinimi;
        mokki_katuosoite.Text = selectedMokki.Katuosoite;
        mokki_postinumero.Text = selectedMokki.Postinro;
        mokki_paikkakunta.Text = selectedMokki.PostinroNavigation.Toimipaikka;

        mokki_hinta.Text = selectedMokki.Hinta.ToString();
        mokki_kuvaus.Text = selectedMokki.Kuvaus;
        mokki_varustelu.Text = selectedMokki.Varustelu;

        string henkilomaaraString = selectedMokki.Henkilomaara.ToString();
        if (mokki_henkilomaara.Items.Contains(henkilomaaraString))
        {
            mokki_henkilomaara.SelectedItem = henkilomaaraString;
        }

    }
}