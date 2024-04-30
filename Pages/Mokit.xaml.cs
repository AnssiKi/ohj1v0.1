using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;
using Microsoft.EntityFrameworkCore;



namespace ohj1v0._1;

public partial class Mokit : ContentPage
{
    Funktiot funktiot = new Funktiot();
    MokkiViewmodel mokkiViewmodel = new MokkiViewmodel();
    AlueViewmodel alueViewmodel = new AlueViewmodel();
    Mokki selectedMokki;

    public Mokit()
    {
        InitializeComponent();
        lista.BindingContext = mokkiViewmodel;
        alue_nimi.BindingContext = alueViewmodel;
    }

    private void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void mokki_nimi_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 45 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 45, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
    }

    private async void mokki_nimi_Unfocused(object sender, FocusEventArgs e)
    {
        Type luokka = typeof(Mokki);
        Entry entry = mokki_nimi;

        if (CheckTuplaTietokanta(mokki_nimi.Text, (uint)alue_nimi.SelectedIndex + 1)) // varmistetaan ettei ole samannimista mokkia
        {
            await DisplayAlert("Virhe", "Saman niminen mökki on jo alueella", "OK");
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }
    }

    private void mokki_katuosoite_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 45 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 45, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
    }

    private async void mokki_postinumero_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 5 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryInteger(entry, this); // funktiossa tarkistetaan ettei syote sisalla tekstia
        using (var dbContext = new VnContext())
        {
            var posti = await dbContext.Postis.FirstOrDefaultAsync(p => p.Postinro == entry.Text);
            if (posti != null)
            {
                mokki_paikkakunta.Text = posti.Toimipaikka;
            }
        }
    }

    private void mokki_postinumero_Unfocused(object sender, FocusEventArgs e)
    {

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
        Entry nimi = mokki_nimi;
        Entry hinta = mokki_hinta;
        Entry postinumero = mokki_postinumero;
        Grid grid = (Grid)entry_grid;

        if (selectedMokki != null) // paivitetaan jo olemassa olevaa mokkia
        {
            if (!funktiot.CheckInput(this, grid)) // Tarkistetaan onko kaikissa entryissa ja pickereissa sisaltoa
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
                    using (var dbContext = new VnContext())
                    {
                        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti muokata mökin tietoja?", "Kyllä", "Ei");

                        // Jos käyttäjä valitsee "Kyllä", toteutetaan peruutustoimet
                        if (result)
                        {
                            selectedMokki.AlueId = (uint)alue_nimi.SelectedIndex + 1;
                            selectedMokki.Mokkinimi = mokki_nimi.Text;
                            selectedMokki.Katuosoite = mokki_katuosoite.Text;
                            selectedMokki.Postinro = mokki_postinumero.Text;
                            selectedMokki.Hinta = double.Parse(mokki_hinta.Text);
                            selectedMokki.Kuvaus = mokki_kuvaus.Text;
                            selectedMokki.Varustelu = mokki_varustelu.Text;
                            selectedMokki.Henkilomaara = int.Parse(mokki_henkilomaara.SelectedItem.ToString());
                            dbContext.Mokkis.Update(selectedMokki);
                            dbContext.SaveChanges();
                            await mokkiViewmodel.LoadMokkisFromDatabaseAsync();
                            OnPropertyChanged(nameof(selectedMokki));
                            await DisplayAlert("", "Muutokset tallennettu", "OK");
                            TyhjennaFunktio();
                        }
                        else //jos ei haluakaan tallentaa, tyhjennetään tiedot
                        {
                            await DisplayAlert("Muutoksia ei tallennettu", "Valitse mökki listalta jos haluat muokata mökkiä", "OK");
                            TyhjennaFunktio();
                        }
                    }
                }

                catch (Exception ex)
                {
                    await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
                }
            }
        }

        else // tallennetaan uusi mokki
        {

            if (!funktiot.CheckInput(this, grid)) // Tarkistetaan onko kaikissa entryissa ja pickereissa sisaltoa
                   {
                       // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
                   }

            else if (CheckTuplaTietokanta(mokki_nimi.Text, (uint)alue_nimi.SelectedIndex + 1)) // varmistetaan ettei ole samannimista mokkia
            {
                await DisplayAlert("Virhe tallentaessa", "Saman niminen mökki on jo alueella", "OK");
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
                    using (var dbContext = new VnContext())
                    {
                        var mokki = new Mokki()
                        {
                            // MokkiId päivittyy automaattisesti tietokannassa
                            AlueId = (uint)alue_nimi.SelectedIndex + 1,
                            Mokkinimi = mokki_nimi.Text,
                            Katuosoite = mokki_katuosoite.Text,
                            Postinro = mokki_postinumero.Text,
                            Hinta = double.Parse(mokki_hinta.Text),
                            Kuvaus = mokki_kuvaus.Text,
                            Varustelu = mokki_kuvaus.Text,
                            Henkilomaara = int.Parse(mokki_henkilomaara.SelectedItem.ToString()),

                        };

                        dbContext.Mokkis.Add(mokki);
                        dbContext.SaveChanges();
                        BindingContext = new MokkiViewmodel();
                        await mokkiViewmodel.LoadMokkisFromDatabaseAsync();
                        await DisplayAlert("Tallennettu", "", "OK");
                        TyhjennaFunktio();

                    }                        

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
                }

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
            TyhjennaFunktio();
            selectedMokki = null;
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
            try
            {
                using (var dbContext = new VnContext())
                {
                    dbContext.Mokkis.Remove(selectedMokki);
                    dbContext.SaveChanges();
                    await mokkiViewmodel.LoadMokkisFromDatabaseAsync();
                    TyhjennaFunktio();

                }
                await DisplayAlert("", "Poisto onnistui", "OK");

            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", "Mökillä on jo varauksia, sitä ei voi poistaa.", "OK");
            }
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

        selectedMokki = (Mokki)e.Item;

        if (selectedMokki.Alue != null)
        {
            alue_nimi.SelectedIndex = (int)selectedMokki.Alue.AlueId - 1;
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

    private void TyhjennaFunktio()
    {
        Grid grid = (Grid)entry_grid;
        ListView list = (ListView)lista;
        funktiot.TyhjennaEntryt(grid, list);
        mokki_kuvaus.Text = "N/A";
        mokki_varustelu.Text = "N/A";
    }

    private bool CheckTuplaTietokanta(string mokkiNimi, uint alueId)
    {
        // Tarkistetaan, onko samalla alueella jo saman nimistä mökkiä
        return mokkiViewmodel.Mokkis.Any(m => m.Mokkinimi == mokkiNimi && m.AlueId == alueId);
    }


}