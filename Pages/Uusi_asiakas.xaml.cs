using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;
namespace ohj1v0._1;

public partial class Uusi_asiakas : ContentPage
{
    private VarauksenTiedot varauksenTiedot;

    public Uusi_asiakas(TeeUusiVaraus mp,VarauksenTiedot tiedot)
    {
        InitializeComponent();
        this.BindingContext = mp;
        varauksenTiedot = tiedot;
    }
    Funktiot funktiot = new Funktiot();
    AsiakasViewmodel asiakasviewmodel = new AsiakasViewmodel();
    VarausViewmodel varausViewmodel = new VarausViewmodel();


    private void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void etunimi_TextChanged(object sender, TextChangedEventArgs e)
    { // entryn pituus rajoitettu xaml.cs max 20 merkkiin
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
    {
        Entry entry = (Entry)sender;
        funktiot.CheckEntryInteger(entry, this); // funktiossa tarkistetaan ettei syote sisalla tekstia
    }

    private void puhelinnumero_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 15 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 15, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
        funktiot.CheckEntryInteger(entry, this); // funktiossa tarkistetaan ettei syote sisalla tekstia
    }

    private void email_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 50 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 50, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka

    }

    private async void tallenna_Clicked(object sender, EventArgs e)
    {
        Type luokka = typeof(ohj1v0._1.Models.Asiaka);
        string selite = "asiakas";
        Entry posti = postinumero;
        Entry puhelin = puhelinnumero;
        Entry sahkoposti = email;
        Grid grid = (Grid)entry_grid;

        // (!funktiot.CheckInput(this, grid)) // Tarkistetaan onko kaikissa entryissa ja pickereissa sisaltoa
        //{
        //   // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        //}
        //
        // if (!funktiot.CheckEntryInteger(posti, this)) // tarkistetaan onko postinumero int
        //{
        // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        //}

        //        else if (!funktiot.CheckEntryInteger(puhelin, this)) // tarkistetaan onko puhelinnumero int
        //      {
        // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        //    }

        // vertailut eivat toimi koska sivulle ei ole listausta - mista tieto?

        /*else if (!funktiot.CheckTupla(this, puhelin, lista, luokka, selite)) // varmistetaan ettei ole samaa puhelinnumeroa
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else if (!funktiot.CheckTupla(this, sahkoposti, lista, luokka, selite)) // varmistetaan ettei ole samaa sahkopostia
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }*/

        // Tarkistukset lapi voidaan tallentaa

        try
        { //Tallentaa uuden asiakkaan tietokantaan
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
                    // AsiakasId p‰ivittyy automaattisesti tietokannassa
                };


                dbContext.Asiakas.Add(asiakas);
                dbContext.SaveChanges();
                BindingContext = new AsiakasViewmodel();
                await asiakasviewmodel.LoadAsiakasFromDatabaseAsync();
            
                var varaus = new Varau()
                {
                    AsiakasId = asiakas.AsiakasId,
                    MokkiId = varauksenTiedot.ValittuMokki.MokkiId,
                    VarattuPvm = varauksenTiedot.Varattupvm,
                    VahvistusPvm = varauksenTiedot.Vahvistuspvm,
                    VarattuAlkupvm = varauksenTiedot.VarattuAlkupvm,
                    VarattuLoppupvm = varauksenTiedot.VarattuLoppupvm,
                   // Asiakas = asiakas,
                   // Mokki = varauksenTiedot.ValittuMokki,
                    //VarauksenPalveluts = varauksenTiedot.VarauksenPalveluts

                };

                await varausViewmodel.LoadVarausFromDatabaseAsync();
                dbContext.Varaus.Add(varaus);
                dbContext.SaveChanges();
                BindingContext = new VarausViewmodel();
                await varausViewmodel.LoadVarausFromDatabaseAsync();
            }
        
            await DisplayAlert("Asiakkaan ja varauksen tallennus onnistui!", "", "OK");
            grid = (Grid)entry_grid;

            foreach (var child in grid.Children)
            {
                if (child is Entry entry)
                {
                    entry.Text = ""; // Tyhjent‰‰ entryn
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
        }
         
        
    }
}