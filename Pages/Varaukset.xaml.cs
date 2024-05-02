using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
namespace ohj1v0._1;

public partial class Varaukset : ContentPage
{
    Funktiot funktiot = new Funktiot();
    AlueViewmodel alueViewmodel = new AlueViewmodel();
    VarausViewmodel varausViewmodel = new VarausViewmodel();
    MokkiViewmodel mokkiViewmodel = new MokkiViewmodel();
    Varau selectedVaraus;

    public Varaukset()
    {
        InitializeComponent();
        lista.BindingContext = varausViewmodel;
        alue_nimi.BindingContext = alueViewmodel;
        mokin_nimi.BindingContext = mokkiViewmodel;

    }

    private void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Tarkistetaan, ett‰ valittu alue on asetettu
        if (alue_nimi.SelectedItem != null)
        {
            // Haetaan valitun alueen ID
            uint valitunAlueenId = ((Alue)alue_nimi.SelectedItem).AlueId;

            // Luodaan tietokantayhteys
            using (var context = new VnContext())
            {
                // Haetaan kaikki ne mokit, jotka kuuluvat valittuun alueeseen
                var mokitValitultaAlueelta = context.Mokkis.Where(m => m.AlueId == valitunAlueenId).ToList();

                // Asetetaan mokit pickerin sis‰llˆksi
                mokin_nimi.ItemsSource = mokitValitultaAlueelta;
            }
        }
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

    private void mokin_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Tarkistetaan, ett‰ on valittu mˆkki
        if (mokin_nimi.SelectedItem != null)
        {
            // Haetaan valitun mˆkin tiedot
            Mokki valittuMokki = (Mokki)mokin_nimi.SelectedItem;

            // Haetaan mˆkin postinumero
            string postinro = valittuMokki.Postinro;

            // Haetaan paikkakunta postinumeron perusteella
            string paikka = "";

            using (var context = new VnContext())
            {
                Posti posti = context.Postis.FirstOrDefault(p => p.Postinro == postinro);
                if (posti != null)
                {
                    paikka = posti.Toimipaikka;
                }
            }

            // Asetetaan tiedot oikeisiin Entry-kenttiin
            postinumero.Text = postinro;
            paikkakunta.Text = paikka;
        }
        else
        {
            // Tyhjennet‰‰n tiedot, jos mˆkki‰ ei ole valittu
            postinumero.Text = "";
            paikkakunta.Text = "";
        }
    }

    private async void postinumero_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 5 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryInteger(entry, this); // funktiossa tarkistetaan ettei syote sisalla tekstia
        using (var dbContext = new VnContext())
        {
            var posti = await dbContext.Postis.FirstOrDefaultAsync(p => p.Postinro == entry.Text);
            if (posti != null)
            {
                paikkakunta.Text = posti.Toimipaikka;
            }
        }

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
                        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti muokata varauksen tietoja?", "Kyll‰", "Ei");

                        // Jos k‰ytt‰j‰ valitsee "Kyll‰", toteutetaan peruutustoimet
                        if (result)
                        {

                            // VarausId p‰ivittyy automaattisesti tietokannassa
                            selectedVaraus.Asiakas.Etunimi = etunimi.Text;
                            selectedVaraus.Asiakas.Sukunimi = sukunimi.Text;
                            selectedVaraus.Asiakas.Puhelinnro = puhelinnumero.Text;
                            selectedVaraus.Asiakas.Email = sahkoposti.Text;
                            selectedVaraus.Mokki.Mokkinimi = ((Mokki)mokin_nimi.SelectedItem).Mokkinimi;
                            selectedVaraus.Mokki.Postinro = postinumero.Text;

                            if (selectedVaraus.Mokki != null && mokin_nimi.SelectedItem != null)
                            {
                                selectedVaraus.Mokki.AlueId = ((Mokki)mokin_nimi.SelectedItem).AlueId;
                            }

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
                            TyhjennaFunktio();

                        }
                        else
                        {
                            await DisplayAlert("Muutoksia ei tallennettu", "Valitse varaus listalta jos haluat muokata mˆkki‰", "OK");
                            TyhjennaFunktio();

                        }

                        foreach (var child in grid)
                        { // Muuttaa entryt tallennuksen j‰lkeen vain lukumuotoon

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
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti tyhjent‰‰ lomakkeen tiedot?", "Kyll‰", "Ei");

        // Jos k‰ytt‰j‰ valitsee "Kyll‰", toteutetaan peruutustoimet
        if (result)
        {
            Grid grid = (Grid)entry_grid;
            TyhjennaFunktio();
            selectedVaraus = null;

            foreach (var child in grid)
            { // Muuttaa entryt tyhjennyksen j‰lkeen vain lukumuotoon

                if (child is Entry entry)
                {
                    entry.IsReadOnly = true;
                }
            }
        }
    }

    private async void poista_Clicked(object sender, EventArgs e)
    {
        Grid grid = (Grid)entry_grid;
        if (selectedVaraus != null)
        {
            bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti poistaa tiedon?", "Kyll‰", "Ei");

            // Jos k‰ytt‰j‰ valitsee "Kyll‰", toteutetaan peruutustoimet
            if (result)
            {
                try
                {
                    using (var dbContext = new VnContext())
                    {
                        dbContext.Varaus.Remove(selectedVaraus);
                        dbContext.SaveChanges();
                        await varausViewmodel.LoadVarausFromDatabaseAsync();
                        TyhjennaFunktio();
                    }
                    await DisplayAlert("Poisto onnistui", "", "OK");
                }
                catch 
                {
                    await DisplayAlert("Virhe", "Varausta ei voitu poistaa.", "OK");
                }
            }

            foreach (var child in grid)
            { // Muuttaa entryt tyhjennyksen j‰lkeen vain lukumuotoon

                if (child is Entry entry)
                {
                    entry.IsReadOnly = true;
                }
            }
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

        id.Text = selectedVaraus.VarausId.ToString();

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

        if (selectedVaraus.Mokki != null)
        {
            mokin_nimi.SelectedIndex = (int)selectedVaraus.Mokki.MokkiId - 1;
        }
        else
        {
            mokin_nimi.SelectedItem = null;
        }

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
        { // Muuttaa entry valinnan j‰lkeen isreadonly=false

            if (child is Entry entry)
            {
                entry.IsReadOnly = false;
            }
        }
    }

    private void TyhjennaFunktio()
    {
        Grid grid = (Grid)entry_grid;
        ListView list = (ListView)lista;
        funktiot.TyhjennaEntryt(grid, list);
    }

    private async void muodostalasku_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("H‰h‰‰", "Ei t‰m‰ viel‰ tee mit‰‰n", "OK");
    }
}