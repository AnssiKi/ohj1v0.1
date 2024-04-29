using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;
using System.Globalization;
namespace ohj1v0._1;

public partial class Varaukset : ContentPage
{
    Funktiot funktiot = new Funktiot();
    AlueViewmodel alueViewmodel = new AlueViewmodel();
    VarausViewmodel varausViewmodel = new VarausViewmodel();
    Varau selectedVaraus;

    public Varaukset()
    {
        InitializeComponent();
        lista.BindingContext = varausViewmodel;
        alue_nimi.BindingContext = alueViewmodel;

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
                using (var dbContext = new VnContext())

                    if (selectedVaraus != null) // voidaan vain korjata tietoja 
                    {
                        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti muokata varauksen tietoja?", "Kyllä", "Ei");

                        // Jos käyttäjä valitsee "Kyllä", toteutetaan peruutustoimet
                        if (result)
                        {
                            // VarausId päivittyy automaattisesti tietokannassa
                            selectedVaraus.Mokki.Alue.AlueId = (uint)alue_nimi.SelectedIndex + 1;
                            selectedVaraus.Asiakas.Etunimi = etunimi.Text;
                            selectedVaraus.Asiakas.Sukunimi = sukunimi.Text;
                            selectedVaraus.Asiakas.Puhelinnro = puhelinnumero.Text;
                            selectedVaraus.Asiakas.Email = sahkoposti.Text;
                            selectedVaraus.Mokki.Mokkinimi = mokin_nimi.Text;
                            selectedVaraus.Mokki.Postinro = postinumero.Text;
                            // CRUD paikkakunta haku postinumeron perusteella

                            if (DateTime.TryParseExact(varauspvm.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                            {
                                selectedVaraus.VarattuPvm = parsedDate;
                            }
                            
                            if (alkupvm != null)
                            {
                                selectedVaraus.VarattuAlkupvm = alkupvm.Date;
                            }
                            if (loppupvm != null)
                            {
                                selectedVaraus.VarattuLoppupvm = loppupvm.Date;
                            }

                            if (vahvistuspvm != null)
                            {
                                selectedVaraus.VahvistusPvm = vahvistuspvm.Date;
                            }

                            dbContext.Varaus.Update(selectedVaraus);
                            dbContext.SaveChanges();
                            await varausViewmodel.LoadVarausFromDatabaseAsync();
                            await DisplayAlert("", "Muutokset tallennettu", "OK");
                            grid = (Grid)entry_grid;
                            ListView list = (ListView)lista;
                            funktiot.TyhjennaEntryt(grid, list);
                            //tarvitseeko jotain N/A
                        }
                        else
                        {
                            await DisplayAlert("Muutoksia ei tallennettu", "Valitse varaus listalta jos haluat muokata mökkiä", "OK");
                            ListView list = (ListView)lista;
                            funktiot.TyhjennaEntryt(grid, list);
                            //tarvitseeko jotain N/A
                        }

                        foreach (var child in grid)
                        { // Muuttaa entryt tallennuksen jälkeen vain lukumuotoon

                            if (child is Entry entry)
                            {
                                entry.IsReadOnly = true;
                            }
                        }
                    }               

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

            foreach (var child in grid)
            { // Muuttaa entryt tyhjennyksen jälkeen vain lukumuotoon

                if (child is Entry entry)
                {
                    entry.IsReadOnly = true;
                }
            }
        }
        else
        {
            // Jos käyttäjä valitsee "Ei", peruutetaan toiminto
            // Tähän ei oo pakko laittaa mitää kerta se ei haluakkaa.
        }
    }

    private async void poista_Clicked(object sender, EventArgs e)
    {
        Grid grid = (Grid)entry_grid;
        if (selectedVaraus != null)
        {
            bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti poistaa tiedon?", "Kyllä", "Ei");

            // Jos käyttäjä valitsee "Kyllä", toteutetaan peruutustoimet
            if (result)
            {
                int varausId = int.Parse(varaus_id.Text);
                await varausViewmodel.PoistaVarausAsync(varausId);
                await varausViewmodel.LoadVarausFromDatabaseAsync();
                ListView list = (ListView)lista;
                funktiot.TyhjennaEntryt(grid, list);
            }
            else
            {
                await DisplayAlert("Poistaminen peruttu", "", "OK");
            }

            foreach (var child in grid)
            { // Muuttaa entryt tyhjennyksen jälkeen vain lukumuotoon

                if (child is Entry entry)
                {
                    entry.IsReadOnly = true;
                }
            }
        }
        else
        {
            await DisplayAlert("Valitse listalta poistettava mökki", "", "OK");
        }
    }

    private void hae_varaukset_DateSelected(object sender, DateChangedEventArgs e)
    {
        DateTime? selectedDateNullable = e.NewDate;

        if (selectedDateNullable.HasValue)
        {
            DateTime selectedDate = selectedDateNullable.Value;

            var filteredVaraukset = varausViewmodel.Varaukset.Where(v => v.VarattuAlkupvm == selectedDate.Date).ToList();
            lista.ItemsSource = filteredVaraukset;
        }
    }
    private void Hae_varaukset_tyhjenna_Clicked(object sender, EventArgs e)
    {
        hae_varaukset.Date = DateTime.Today;
        lista.ItemsSource = varausViewmodel.Varaukset;
    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        Grid grid = (Grid)entry_grid;

        if (e.Item == null)
        {
            return;
        }

        selectedVaraus = (Varau)e.Item;

        varaus_id.Text = selectedVaraus.VarausId.ToString();

        if (selectedVaraus.Mokki.Alue != null)
        {
            alue_nimi.SelectedIndex = (int)selectedVaraus.Mokki.Alue.AlueId - 1;
        }
        else
        {
            alue_nimi.SelectedItem = null;
        }

        etunimi.Text = selectedVaraus.Asiakas.Etunimi;
        sukunimi.Text = selectedVaraus.Asiakas.Sukunimi;
        puhelinnumero.Text = selectedVaraus.Asiakas.Puhelinnro;
        sahkoposti.Text = selectedVaraus.Asiakas.Email;
        mokin_nimi.Text = selectedVaraus.Mokki.Mokkinimi;
        postinumero.Text = selectedVaraus.Mokki.Postinro;
        paikkakunta.Text = selectedVaraus.Mokki.PostinroNavigation.Toimipaikka;

        if (selectedVaraus.VarattuAlkupvm != null)
        {
            alkupvm.Date = selectedVaraus.VarattuAlkupvm.Value;
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

        foreach (var child in grid)
        { // Muuttaa entry valinnan jälkeen isreadonly=false

            if (child is Entry entry)
            {
                entry.IsReadOnly = false;
            }
        }
    }
}