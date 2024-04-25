using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;
namespace ohj1v0._1;

public partial class Varaukset : ContentPage
{
    public Varaukset()
    {
        InitializeComponent();
        lista.BindingContext = varausViewmodel;
        alue_nimi.BindingContext = alueViewmodel;

    }
    Funktiot funktiot = new Funktiot();
    AlueViewmodel alueViewmodel = new AlueViewmodel();
    VarausViewmodel varausViewmodel = new VarausViewmodel();

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
    private void puhelinnumero_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 15 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 15, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
        funktiot.CheckEntryInteger(entry, this); // funktiossa tarkistetaan ettei syote sisalla tekstia
    }

    private void sahkoposti_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 50 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 50, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
    }


    private void mokin_nimi_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 45 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 45, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka

    }

    private void postinumero_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 5 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryInteger(entry, this); // funktiossa tarkistetaan ettei syote sisalla tekstia

    }

    private void paikkakunta_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void alkupvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void loppupvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void vahvistuspvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }



    private async void tallenna_Clicked(object sender, EventArgs e)
    {
        Grid grid = (Grid)entry_grid;

        if (!funktiot.CheckInput(this, grid)) // Tarkistetaan onko kaikissa entryissa ja pickereissa sisaltoa
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

    private void hae_varaukset_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item == null)
        {
            return;
        }

        var selectedVaraus = (Varau)e.Item;
        varaus_id.Text = selectedVaraus.VarausId.ToString();

        //tiedot eivat kulje luokasta toiseen kunnolla

        /*tahan alue pickeriin tieto
        etunimi.Text = selectedVaraus.Asiakas.Etunimi;
        sukunimi.Text = selectedVaraus.Asiakas.Sukunimi;
        puhelinnumero.Text = selectedVaraus.Asiakas.Puhelinnro;
        sahkoposti.Text = selectedVaraus.Asiakas.Email;
        mokin_nimi.Text = selectedVaraus.Mokki.Mokkinimi;
        postinumero.Text= selectedVaraus.Asiakas.Postinro;
        paikkakunta.Text*/

        if (selectedVaraus.VarattuAlkupvm != null)
        {
            alkupvm.Date = selectedVaraus.VarattuPvm.Value;
        }
        if (selectedVaraus.VarattuLoppupvm != null)
        {
            loppupvm.Date = selectedVaraus.VarattuLoppupvm.Value;
        }
        if (selectedVaraus.VahvistusPvm != null)
        {
            vahvistuspvm.Date = selectedVaraus.VahvistusPvm.Value;
        }

        if (selectedVaraus.VarattuPvm.HasValue)
        {
            varauspvm.Text = selectedVaraus.VarattuPvm.Value.ToString("dd.MM.yyyy");
        }

        foreach (var child in VerticalStack.Children)
        { // Muuttaa entry valinnan j‰lkeen isreadonly=false

            if (child is Entry entry)
            {
                entry.IsReadOnly = false;
            }


        }


    }
}