using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;


namespace ohj1v0._1;

/* Luotu tiedostonluonnin yhteydess‰ ja lis‰tty painikkeiden luurankokoodit VH/MH toimesta jossain vaiheessa, ajankohta tuntematon 06052024 KA
 * Lis‰tty tarkastusfunktiot, ajankohta tuntematon 06052024 KA
 * Muokattu hakukentt‰ Picker-elementiksi, 25042024 KA
 * Toteutettu tiedon nouto listviewist‰, 26042024 KA
 * CRUD-toiminnallisuudet saatettu toimimaan, 06052024 KA
 */

public partial class Palvelut : ContentPage
{
	Funktiot funktiot = new Funktiot();
    PalveluViewmodel palveluViewmodel = new PalveluViewmodel();
    AlueViewmodel alueViewmodel = new AlueViewmodel();
    Palvelu selectedPalvelu;


    public Palvelut()
	{
		InitializeComponent();
        this.BindingContext = palveluViewmodel;
        lista.BindingContext = palveluViewmodel;
        alue_nimi.BindingContext = alueViewmodel;
        hae_alueella.BindingContext = alueViewmodel;
        palvelu_hinta.TextChanged += palvelu_hinta_TextChanged;
        palvelu_hinta.TextChanged += palvelu_hinta_laskealv;

	}

    private void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    //funktio, joka laskee arvolis‰verollisen hinnan alvhinta-kentt‰‰n
    private void palvelu_hinta_laskealv(object sender, EventArgs e)

    {   //Jos sek‰ palvelu_hinta kent‰ss‰ ja palvelu_alv pickeriss‰ on valinta
        if(!string.IsNullOrEmpty(palvelu_hinta.Text) && palvelu_alv.SelectedIndex != -1)
        {
            //tarkastetaan syˆtteet ja mik‰li muuttuvat doubleksi, suoritetaan laskutoimitus ja vied‰‰n tulos alvhinta kentt‰‰n
            if(double.TryParse(palvelu_hinta.Text, out double hinta) && double.TryParse(palvelu_alv.SelectedItem.ToString(), out double kerroin))
            {
                double hintaAlv = hinta * (1 + (kerroin / 100));
                alvhinta.Text = hintaAlv.ToString("0.##");
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }

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
        try
        {
            if (selectedPalvelu != null)
            {
                if (!funktiot.CheckInput(this, entry_grid)) // Tarkistetaan onko kaikissa entryissa ja pickereissa sisaltoa
                {
                    // Mahdollinen virheenk‰sittely
                }
                else if (!funktiot.CheckEntryDouble(palvelu_hinta, this)) // tarkistetaan hinta double
                {
                    // Mahdollinen virheenk‰sittely
                }
                else // Tarkistukset lapi voidaan tallentaa
                {
                    bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti muokata palvelun tietoja?", "Kyll‰", "Ei");

                    if (result)
                    {
                        if (alue_nimi.SelectedItem is Alue selectedAlue &&
                            double.TryParse(palvelu_hinta.Text, out double phinta) &&
                            double.TryParse(palvelu_alv.SelectedItem?.ToString(), out double alv))
                        {
                            using (var dbContext = new VnContext())
                            {
                                var palvelu = await dbContext.Palvelus.FindAsync(selectedPalvelu.PalveluId);

                                if (palvelu != null)
                                {
                                    // P‰ivitet‰‰n arvot
                                    palvelu.AlueId = selectedAlue.AlueId;
                                    palvelu.Nimi = palvelu_nimi.Text;
                                    palvelu.Kuvaus = palvelu_kuvaus.Text;
                                    palvelu.Hinta = phinta;
                                    palvelu.Alv = alv;

                                    dbContext.Palvelus.Update(palvelu);
                                    await dbContext.SaveChangesAsync();

                                    // Lataa p‰ivitetyt tiedot viewmodeliin
                                    await palveluViewmodel.LoadPalvelusFromDatabaseAsync();
                                    palveluViewmodel.OnPropertyChanged(nameof(palveluViewmodel.Palvelus));

                                    // N‰yt‰ tallennusilmoitus
                                    await DisplayAlert("Tallennus", "Muutokset tallennettu", "OK");

                                    // Tyhjenn‰ syˆtekent‰t
                                    TyhjennaTiedot();
                                }
                                else
                                {
                                    await DisplayAlert("Virhe", "Palvelua ei lˆytynyt tietokannasta", "OK");
                                    TyhjennaTiedot();
                                }
                            }
                        }
                        else
                        {
                            await DisplayAlert("Virhe", "Tarkista syˆtt‰m‰si tiedot", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Tallennus", "Muutoksia ei tallennettu", "OK");
                        TyhjennaTiedot();
                    }
                }
            }
            else
            {
                // Uuden palvelun tallennus
                if (!funktiot.CheckInput(this, entry_grid))
                {
                    // Mahdollinen virheenk‰sittely
                }
                else if (!funktiot.CheckTupla(this, palvelu_nimi, lista, typeof(Palvelu), "palvelu", "Nimi"))
                {
                    // Mahdollinen virheenk‰sittely
                }
                else if (!funktiot.CheckEntryDouble(palvelu_hinta, this))
                {
                    // Mahdollinen virheenk‰sittely
                }
                else
                {
                    using (var dbContext = new VnContext())
                    {
                        // Haetaan seuraava vapaa PalveluID
                        var maxID = dbContext.Palvelus.DefaultIfEmpty().Max(p => p == null ? 0 : p.PalveluId);
                        var newID = maxID + 1;

                        // Varmistetaan, ett‰ alue_nimi.SelectedItem on oikein
                        if (alue_nimi.SelectedItem is Alue selectedAlue)
                        {
                            var palvelu = new Palvelu()
                            {
                                PalveluId = newID,
                                AlueId = selectedAlue.AlueId,  // K‰ytet‰‰n valitun Alue-objektin AlueId:t‰
                                Nimi = palvelu_nimi.Text,
                                Kuvaus = palvelu_kuvaus.Text,
                                Hinta = double.Parse(palvelu_hinta.Text),
                                Alv = double.Parse(palvelu_alv.SelectedItem.ToString()),
                            };

                            dbContext.Palvelus.Add(palvelu);
                            await dbContext.SaveChangesAsync();

                            await palveluViewmodel.LoadPalvelusFromDatabaseAsync();
                            palveluViewmodel.OnPropertyChanged(nameof(palveluViewmodel.Palvelus));

                            await DisplayAlert("Tallennus", "Tiedot tallennettu onnistuneesti", "OK");
                            TyhjennaTiedot();
                        }
                        else
                        {
                            await DisplayAlert("Virhe", "Valitse alue", "OK");
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
        }
    }

    private async void tyhjenna_Clicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti tyhjent‰‰ lomakkeen tiedot?", "Kyll‰", "Ei");

        // Jos k‰ytt‰j‰ valitsee "Kyll‰", toteutetaan peruutustoimet
        if (result)
        {
            TyhjennaTiedot();
            selectedPalvelu = null;
        }
        return;
    }

    private async void poista_Clicked(object sender, EventArgs e)
    {      
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti poistaa tiedon?", "Kyll‰", "Ei");

        // Jos k‰ytt‰j‰ valitsee "Kyll‰", toteutetaan peruutustoimet
        if (result)
        {
            try
            {

                using (var dbContext = new VnContext())
                {
                    dbContext.Palvelus.Remove(selectedPalvelu);
                    dbContext.SaveChanges();
                    await palveluViewmodel.LoadPalvelusFromDatabaseAsync();
                    TyhjennaTiedot();
                }
                await DisplayAlert("", "Poisto Onnistui", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", $"Poistossa tapahtui virhe: {ex.Message}", "OK");
            }
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
        selectedPalvelu = (Palvelu)e.Item;

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

        //Haetaan tietokannasta ALV-kanta ja p‰ivitet‰‰n Picker-elementti sen mukaisesti.
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
            var filteredPalvelut = palveluViewmodel.Palvelus.Where(p => p.Alue.AlueId == selectedAlue.AlueId).ToList();
            lista.ItemsSource = filteredPalvelut;   
        
        }

    }

    //Painike, jolla tyhjennet‰‰n aluevalint ja p‰ivitet‰‰n lista vastaamaan tietokantaa
    private void hae_alueella_tyhjennaClicked(object sender, EventArgs e)
    {
        hae_alueella.SelectedIndex = -1;
        lista.ItemsSource = palveluViewmodel.Palvelus;
    }

    //Tyhjennys-funktio, joka tyhjent‰‰ entry-kent‰t sek‰ palvelu_id-labelin. 
    private void TyhjennaTiedot()
    {
        funktiot.TyhjennaEntryt(entry_grid, lista);
        palvelu_id.Text = null;
        alvhinta.Text = null;
    }
}