using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;
using Microsoft.EntityFrameworkCore;
namespace ohj1v0._1;

public partial class Uusi_asiakas : ContentPage
{
    private VarauksenTiedot varauksenTiedot;

    public Uusi_asiakas(TeeUusiVaraus mp, VarauksenTiedot tiedot)
    {
        InitializeComponent();
        this.BindingContext = mp;
        varauksenTiedot = tiedot;
        tuv = mp;
    }
    //Luodaan funktioluokalle oma olio
    Funktiot funktiot = new Funktiot();

    //T‰ss‰ luodaan viewmodeleiden oliot
    AsiakasViewmodel asiakasviewmodel = new AsiakasViewmodel();
    VarausViewmodel varausViewmodel = new VarausViewmodel();
    ListaViewModel listaViewModel = new ListaViewModel();

    TeeUusiVaraus tuv = new TeeUusiVaraus(); //T‰m‰ sit‰ varten ett‰ voi tyhjent‰‰ sivun tiedot kutsumalla sen sivun funktiota
    

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
    private async void postinumero_TextChanged(object sender, TextChangedEventArgs e)
    {
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

        if (!funktiot.CheckInput(this, grid)) // Tarkistetaan onko kaikissa entryissa ja pickereissa sisaltoa
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else if (!funktiot.CheckEntryInteger(posti, this)) // tarkistetaan onko postinumero int
        {
            //tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else if (!funktiot.CheckEntryInteger(puhelin, this)) // tarkistetaan onko puhelinnumero int
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }
        else
        {
            bool onKaytossa = await asiakasviewmodel.OnkoPuhelinTaiSahkopostiKaytossa(puhelinnumero.Text, email.Text);//Tarkistetaan ettei asiakas olekin jo tietokannassa jos puhelinnumero tai s‰hkˆposti on sama

            if (!onKaytossa)
            {

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

                        dbContext.Asiakas.Add(asiakas); //Lis‰s‰‰n asiakas tietokantaan ja listaan
                        dbContext.SaveChanges();
                        BindingContext = new AsiakasViewmodel();

                        asiakasviewmodel.OnPropertyChanged(nameof(asiakasviewmodel.Asiakas));//Kutsutaan funktioo, joka ilmoittaa observacollectionille ett‰ asiakaslista on muuttunut ja p‰ivitt‰‰ asiakaslistan n‰kym‰n

                        await asiakasviewmodel.LoadAsiakasFromDatabaseAsync();
                
                            var varaus = new Varau()//Lis‰t‰‰n uuden asiakkaan varaus
                            {
                                AsiakasId = asiakas.AsiakasId,
                                MokkiId = varauksenTiedot.ValittuMokki.MokkiId,
                                VarattuPvm = varauksenTiedot.Varattupvm,
                                VahvistusPvm = varauksenTiedot.Vahvistuspvm,
                                VarattuAlkupvm = varauksenTiedot.VarattuAlkupvm,
                                VarattuLoppupvm = varauksenTiedot.VarattuLoppupvm

                            };

                            await varausViewmodel.LoadVarausFromDatabaseAsync();
                            dbContext.Varaus.Add(varaus);//Lis‰t‰‰n varaus tietokantaan ja listaan
                            dbContext.SaveChanges();
                            varausViewmodel.OnPropertyChanged(nameof(varausViewmodel.Varaukset)); //kutsutaan funktioo joka kertoo observacollectionille listan muuttuneen ja p‰ivitt‰‰ listan myˆs varaus-sivulla

                        if (varauksenTiedot.VarauksenPalveluts.Any()) //Jos varauksella on palveluita lis‰t‰‰n ne varaukseen ja tietokantaan
                        {
                            var varausId = varaus.VarausId;

                            // Lis‰t‰‰n varauksen ID jokaiseen VarauksenPalvelut-olioon,jos useampi palvelu ni kaikki saa varauksen id,ett‰ voi yhdist‰‰ palvelut oikeaan varaukseen
                            foreach (var palvelu in varauksenTiedot.VarauksenPalveluts)
                            {
                                palvelu.VarausId = varausId;
                                dbContext.VarauksenPalveluts.Add(palvelu);
                            }
                            //T‰h‰n pit‰‰ lis‰t‰ mill‰ p‰ivitet‰‰n varauksen palvelut lista, jos ne lis‰t‰‰n n‰kym‰‰n
                            dbContext.SaveChanges();
                            await varausViewmodel.LoadVarausFromDatabaseAsync();
                        }
                    }
                    await DisplayAlert("Asiakkaan ja varauksen tallennus onnistui!", "", "OK");

                    //nollataan listviewin lista
                    listaViewModel.NollaaValitutPalvelut();

                    //nollataan varauksenTiedot
                    await funktiot.TyhjennaVarauksenTiedotAsync(varauksenTiedot);

                    tuv.TyhjennaVarausTiedot(); //nollataan teeuusivaraus-sivun tiedot
                    
                    grid = (Grid)entry_grid;

                    foreach (var child in grid.Children)//Nollataan t‰m‰n sivun entryt
                    {
                        if (child is Entry entry)
                        {
                            entry.Text = ""; // Tyhjent‰‰ entryn
                        }
                    }
                    await Navigation.PopAsync(); //kokeillaan nyt nollauksen j‰lkeen vaan palata takas sinne sivulle et toimiiko.
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
                }

            }
            else
            {
                await DisplayAlert("Asiakas kyseisell‰ puhelinnumerolla ","tai s‰hkˆpostilla on jo tietokannassa","OK!");
            }
        }
    }
}