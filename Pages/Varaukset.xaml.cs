using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using iTextKernel = iText.Kernel.Pdf;
using iTextLayout = iText.Layout;
using iTextLOElement = iText.Layout.Element;
using iTextLOP = iText.Layout.Properties;
using CommunityToolkit.Maui.Storage;

namespace ohj1v0._1;

public partial class Varaukset : ContentPage
{
    Funktiot funktiot = new Funktiot();
    LaskuViewmodel laskuViewmodel = new LaskuViewmodel();
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
    public async void muodostalasku_Clicked(object sender, EventArgs e)
    { 
        //Tallennus tietokantaan
        if (selectedVaraus != null)
        {
            double loppusumma = 0;
            double verot = 0;
            var dbContext = new VnContext();
            var varauksenPalvelut = dbContext.VarauksenPalveluts.Where(m => m.VarausId == selectedVaraus.VarausId).Select(n => n.Palvelu).ToList();          
            // Iteroidaan varauksen palvelut ja lasketaan niiden hinta sek‰ eritell‰‰n verot
            if (varauksenPalvelut.Any()) 
            {
                foreach (var palvelu in varauksenPalvelut)
                {
                    loppusumma += palvelu.HintaAlv;
                    verot += (palvelu.HintaAlv - palvelu.Hinta);
                }
            }
            loppusumma += selectedVaraus.Mokki.Hinta;
            //Lasketaan loppusumma veroineen eriteltyn‰
            Lasku l = new Lasku();
            {
                l.VarausId = selectedVaraus.VarausId;
                l.Summa = loppusumma;
                l.Alv = verot;
                l.Maksettu = 0;
            };

            dbContext.Add(l);
            await dbContext.SaveChangesAsync();
            laskuViewmodel.LoadLaskutFromDatabaseAsync();
            TulostaPDF(selectedVaraus, varauksenPalvelut, loppusumma, verot);
        }
        else 
        {
            DisplayAlert("Virhe", "Valitse ensin varaus", "OK");
            return;
        }         
    }
    public async void TulostaPDF(Varau selectedVaraus, List<Palvelu> varauksenPalvelut, double loppusumma, double verot)//Tarkistaa onko laskua valittuna ja muodostaa PDF:n
    {
        if (selectedVaraus == null)
        {
            DisplayAlert("Virhe", "Valitse ensin varaus josta haluat muodostaa laskun", "OK");
            return;
        }
        //Tehd‰‰n PDF:
        using var memoryStream = new MemoryStream();
        iTextKernel.PdfWriter writer = new iTextKernel.PdfWriter(memoryStream);
        iTextKernel.PdfDocument pdf = new iTextKernel.PdfDocument(writer);
        iTextLayout.Document document = new iTextLayout.Document(pdf);

        iTextLOElement.Paragraph header = new iTextLOElement.Paragraph("Village Newbies OY - Lasku")
            .SetTextAlignment(iTextLOP.TextAlignment.CENTER)
            .SetFontSize(22);
        document.Add(header);

        var varausInfo = new iTextLOElement.Paragraph($"Varaus ID: {selectedVaraus.VarausId}\n" +
            $"Asiakas: {selectedVaraus.Asiakas.Etunimi} {selectedVaraus.Asiakas.Sukunimi}\n" +
            $"Mokki: {selectedVaraus.Mokki.Mokkinimi}\n" +
            $"Mˆkin varausp‰iv‰: {selectedVaraus.VarattuPvm}\n" +
            $"Varauksen vahvistusp‰iv‰: {selectedVaraus.VahvistusPvm}\n" +
            $"Majoittumisen alkamisp‰iv‰: {selectedVaraus.VarattuAlkupvm}\n" +
            $"Majoituksen loppumisp‰iv‰: {selectedVaraus.VarattuLoppupvm}\n" +
            $"Hinta: {selectedVaraus.Mokki.Hinta}Ä\n" +
            $"Palvelut:")
            .SetTextAlignment(iTextLOP.TextAlignment.LEFT)
           .SetFontSize(12);
        document.Add(varausInfo);

        if (varauksenPalvelut.Any())
        {
            foreach (var p in varauksenPalvelut)
            {
                var palvelu = p.Nimi;
                var palvelutInfo = new iTextLOElement.Paragraph($"{p.Nimi}\n+" +
                $"Hinta sis {p.Alv}% alv: {p.HintaAlv}Ä\n"+
                $"_______________\n")
                .SetTextAlignment(iTextLOP.TextAlignment.LEFT)
                .SetFontSize(12);
                document.Add(palvelutInfo);
            }
        }
        else
        {
            var palvelutInfo = new iTextLOElement.Paragraph($"Muistathan varata ensi kerralla myˆs lis‰palvelut!")
                .SetTextAlignment(iTextLOP.TextAlignment.LEFT)
                .SetFontSize(12);
            document.Add(palvelutInfo);

        }
        var loppusummaInfo = new iTextLOElement.Paragraph($"Laskun loppusumma: {loppusumma}Ä\n" +
            $"Verot: {verot}Ä")
            .SetTextAlignment(iTextLOP.TextAlignment.LEFT)
            .SetFontSize(12);
            document.Add(loppusummaInfo);

        iTextLOElement.Paragraph maksuInfo = new iTextLOElement.Paragraph($"Saajan tilinumero:\n" +
        $"FI12 3456 7890 1234 56\n" +
        $"Pankkiyhteys: HVKVG\n" +
        $"Viite:\n" +
        selectedVaraus.VarausId +
        $"Laskun p‰iv‰m‰‰r‰: " + DateTime.Now.ToString("d")+
        $"\nLaskun er‰p‰iv‰: " + DateTime.Now.AddDays(14).ToString("d"))

        .SetTextAlignment(iTextLOP.TextAlignment.LEFT)
        .SetFontSize(12);
        document.Add(maksuInfo);
        document.Close();

        byte[] pdfData = memoryStream.ToArray();
        using var stream = new MemoryStream(pdfData);

        var fileSaveResult = await FileSaver.Default.SaveAsync("sample.pdf", "Lasku_varaus_0" +
            selectedVaraus.VarausId.ToString() + ".pdf", stream);

        if (fileSaveResult.IsSuccessful)
        {
            await DisplayAlert("Tallennettu", $"Tiedosto tallennettu sijaintiin: {fileSaveResult.FilePath}", "OK");
        }
        else
        {
            await DisplayAlert("Virhe", $"Tiedoston tallentaminen ei onnistunut: {fileSaveResult.Exception.Message}", "OK");
        }
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
                {
                    var varaus = await dbContext.Varaus.FindAsync(selectedVaraus.VarausId);

                    if (varaus != null) // voidaan vain korjata tietoja 
                    {
                        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti muokata varauksen tietoja?", "Kyll‰", "Ei");

                        // Jos k‰ytt‰j‰ valitsee "Kyll‰", toteutetaan peruutustoimet
                        if (result)
                        {
                            // P‰ivitet‰‰n varauksen tiedot
                            varaus.Mokki = (Mokki)mokin_nimi.SelectedItem;
                            varaus.Mokki.Postinro = postinumero.Text;

                            if (selectedVaraus.Mokki != null && mokin_nimi.SelectedItem != null)
                            {
                                varaus.Mokki.AlueId = ((Mokki)mokin_nimi.SelectedItem).AlueId;
                            }

                            if (DateTime.TryParseExact(varauspvm.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                            {
                                varaus.VarattuPvm = parsedDate;
                            }

                            varaus.VarattuAlkupvm = alkupvm?.Date;
                            varaus.VarattuLoppupvm = loppupvm?.Date;
                            varaus.VahvistusPvm = vahvistuspvm?.Date;

                            dbContext.Varaus.Update(varaus);
                            await dbContext.SaveChangesAsync();
                            await varausViewmodel.LoadVarausFromDatabaseAsync();
                            varausViewmodel.OnPropertyChanged(nameof(varausViewmodel.Varaukset));
                            await DisplayAlert("", "Muutokset tallennettu", "OK");
                            TyhjennaFunktio();
                        }
                        else
                        {
                            await DisplayAlert("Muutoksia ei tallennettu", "Valitse varaus listalta jos haluat muokata mˆkki‰", "OK");
                            TyhjennaFunktio();
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
            TyhjennaFunktio();
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
                        dbContext.VarauksenPalveluts.RemoveRange(dbContext.VarauksenPalveluts.Where(vp => vp.VarausId == selectedVaraus.VarausId));
                        dbContext.Varaus.Remove(selectedVaraus);
                        dbContext.SaveChanges();
                        await varausViewmodel.LoadVarausFromDatabaseAsync();
                        TyhjennaFunktio();
                    }
                    varausViewmodel.OnPropertyChanged(nameof(Varaukset));
                    await DisplayAlert("Poisto onnistui", "", "OK");
                }
                catch
                {
                    await DisplayAlert("Virhe", "Varausta ei voitu poistaa.", "OK");
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
        alue_nimi.IsEnabled = true;
        mokin_nimi.IsEnabled = true;

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
    }

    private void TyhjennaFunktio()
    {
        Grid grid = (Grid)entry_grid;
        ListView list = (ListView)lista;
        funktiot.TyhjennaEntryt(grid, list);
        selectedVaraus = null;
        alue_nimi.IsEnabled = false;
        mokin_nimi.IsEnabled = false;

    }
}