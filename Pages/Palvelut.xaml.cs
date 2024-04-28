using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;

namespace ohj1v0._1;

public partial class Palvelut : ContentPage
{
	Funktiot funktiot = new Funktiot();
    PalveluViewmodel pVm = new PalveluViewmodel();
    AlueViewmodel aVm = new AlueViewmodel();
   
   
    public Palvelut()
	{
		InitializeComponent();
        lista.BindingContext = pVm;
        alue_nimi.BindingContext = aVm;
        hae_alueella.BindingContext = aVm; 

        
	}

    
    
    private void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    

    private void palvelu_nimi_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 40 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 40, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka

    }

    private void palvelu_kuvaus_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 255 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 255, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
    }

    private void palvelu_hinta_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 8 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 8, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
        funktiot.CheckEntryDouble(entry, this); // tarkistetaan ett‰ syote on double
    }

    private void palvelu_alv_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    private async void tallenna_Clicked(object sender, EventArgs e)
    {
        Type luokka = typeof(ohj1v0._1.Models.Palvelu);
        string selite = "palvelu";
        Entry nimi = palvelu_nimi;
        Entry hinta = palvelu_hinta;
        Grid grid = (Grid)entry_grid;
        string vertailu = "Nimi";

        if (!funktiot.CheckInput(this, grid)) // Tarkistetaan onko kaikissa entryissa ja pickereissa sisaltoa
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else if (!funktiot.CheckTupla(this, nimi, lista, luokka, selite, vertailu)) // varmistetaan ettei ole samannimista palvelua
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else if (!funktiot.CheckEntryDouble(hinta, this)) // tarkistetaan hinta double
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

    private async void tyhjenna_Clicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti tyhjent‰‰ lomakkeen tiedot?", "Kyll‰", "Ei");

        // Jos k‰ytt‰j‰ valitsee "Kyll‰", toteutetaan peruutustoimet
        if (result)
        {
            Grid grid = (Grid)entry_grid;
            ListView list = (ListView)lista;
            funktiot.TyhjennaEntryt(grid, list);
        }
        else
        {
            // Jos k‰ytt‰j‰ valitsee "Ei", peruutetaan toiminto
            // T‰h‰n ei oo pakko laittaa mit‰‰ kerta se ei haluakkaa.
        }
    }

    private async void poista_Clicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti poistaa tiedon?", "Kyll‰", "Ei");

        // Jos k‰ytt‰j‰ valitsee "Kyll‰", toteutetaan peruutustoimet
        if (result)
        {
            //poistetaan tiedot t‰h‰n
        }
        else
        {
            // Jos k‰ytt‰j‰ valitsee "Ei", peruutetaan toiminto
            // T‰h‰n ei oo pakko laittaa mit‰‰ kerta se ei haluakkaa poistaa.
        }

    }

    //Eventhandler, joka noutaa tiedot listviewist‰ entrykenttiin.
    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item == null)
        {
            return;
        }

        var selectedPalvelu = (Palvelu)e.Item;

        if (selectedPalvelu.Alue != null)
        {
            alue_nimi.SelectedIndex = (int)selectedPalvelu.Alue.AlueId - 1;
        }
        else
        {
            alue_nimi.SelectedItem = null;
        }

        palvelu_id.Text = selectedPalvelu.PalveluId.ToString();
        palvelu_nimi.Text = selectedPalvelu.Nimi;
        palvelu_kuvaus.Text = selectedPalvelu.Kuvaus;
        palvelu_hinta.Text = selectedPalvelu.Hinta.ToString();

        if (selectedPalvelu.Alv == 10 || selectedPalvelu.Alv == 14 || selectedPalvelu.Alv == 24)
        {
            switch (selectedPalvelu.Alv)
            {
                case 10:
                    palvelu_alv.SelectedIndex = 0; // Indeksi 10 % arvolle
                    break;
                case 14:
                    palvelu_alv.SelectedIndex = 1; // Indeksi 14 % arvolle
                    break;
                case 24:
                    palvelu_alv.SelectedIndex = 2; // Indeksi 24 % arvolle
                    break;
            }
        }
        else
        {
            palvelu_alv.SelectedIndex = -1; // Ei valittu mit‰‰n
        }

    }


    //Eventhandler, joka valitsee n‰ytett‰v‰t tiedot Picker-elementin valinnan mukaan.
    private void hae_alueella_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedAlue = (Alue)picker.SelectedItem;

        if (selectedAlue != null) 
        {
            var filteredPalvelut = pVm.Palvelus.Where(p => p.Alue.AlueId == selectedAlue.AlueId).ToList();
            lista.ItemsSource = filteredPalvelut;   
        
        }

    }

    //Painike, jolla tyhjennet‰‰n aluevalint ja p‰ivitet‰‰n lista vastaamaan tietokantaa
    private void hae_alueella_tyhjennaClicked(object sender, EventArgs e)
    {
        hae_alueella.SelectedIndex = -1;
        lista.ItemsSource = pVm.Palvelus;

    }
}