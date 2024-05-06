using ohj1v0._1;
using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ohj1v0._1;

public partial class TeeUusiVaraus : ContentPage
{
    Funktiot funktiot = new Funktiot();
    AlueViewmodel alueViewmodel = new AlueViewmodel();
    MokkiViewmodel mokkiViewmodel = new MokkiViewmodel();
    VarausViewmodel varausViewmodel = new VarausViewmodel();
    PalveluViewmodel palveluViewmodel = new PalveluViewmodel();
    ListaViewModel listaViewModel = new ListaViewModel();

    public TeeUusiVaraus()
    {
        InitializeComponent();
        alue_nimi.BindingContext = alueViewmodel;
        varauspvm.Text = DateTime.Now.ToString("dd.MM.yyyy");
        mokki_lista.BindingContext = mokkiViewmodel.Mokkis;
        palvelu_lista.BindingContext = palveluViewmodel.Palvelus;

    }

    Alue selectedAlue;
    Mokki selectedMokki;
    Palvelu selectedPalvelu;
    private int lukumaara;
    private DateTime? alkupaiva;
    private DateTime? loppupaiva;







    private async void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {

        if ((Alue)alue_nimi.SelectedItem != null)
        {
            selectedAlue = (Alue)alue_nimi.SelectedItem;

            using var context = new VnContext();

            var mokitValitullaAlueella = await context.Mokkis.Where(m => m.AlueId == selectedAlue.AlueId).ToListAsync();//filtteroidaan mökit alueella
            var palvelutValitullaAlueella = await context.Palvelus.Where(p => p.AlueId == selectedAlue.AlueId).ToListAsync();//filtteroidaan palvelut alueella

            if (!mokitValitullaAlueella.Any()) //Jos alueella ei ole mökkejä
            {
                await DisplayAlert("Tällä alueella ei ole vielä mökkejä", "Valitse uusi alue", "OK!");
            }
            else
            {
                mokki_lista.ItemsSource = mokitValitullaAlueella;

                if (!palvelutValitullaAlueella.Any()) //Jos alueella ei ole palveluita
                {
                    await DisplayAlert("Valitulla alueella ei ole saatavilla palveluita.", "Voit kuitenkin vuokrata mökin tai halutessasi palveluita vaihtaa aluetta", "OK!");
                }
                else
                {
                    palvelu_lista.ItemsSource = palvelutValitullaAlueella;
                }
            }
        }


    }

    private void alkupvm_DateSelected(object sender, DateChangedEventArgs e)
    {
        alkupaiva = e.NewDate;
        if (alkupaiva != null) { }
    }

    private async void loppupvm_DateSelected(object sender, DateChangedEventArgs e)
    {
        loppupaiva = e.NewDate;

        if (alkupaiva.HasValue && loppupaiva.HasValue)
        {

            if (alkupaiva.Value > loppupaiva.Value)
            {
                await DisplayAlert("Virhe", "Aloituspäivämäärä ei voi olla lopetuspäivämäärän jälkeen", "OK");

            }
            else if (alkupaiva.Value < DateTime.Now)
            {
                await DisplayAlert("Aloituspäivämäärä tulee olla aikaisintaan tänään", "Valitse uusi päivämäärä", "OK!");
            }
        }
        else
        {
            await DisplayAlert("Valitse myös aloituspäivä ", "nämä tiedot ovat pakollisia", "OK!");
        }
    }



    private async void henkilomaara_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((Alue)alue_nimi.SelectedItem != null)
        {
            int henkilo = henkilomaara.SelectedIndex + 1;

            using var context = new VnContext();
            //suodatetaan ensin mökit alueen mukaan
            var filteredMokit = await context.Mokkis
            .Where(m => m.Henkilomaara >= henkilo && m.AlueId == selectedAlue.AlueId)
            .Include(m => m.Varaus)
            .ToListAsync();
            //Sen jälkeen suodatetaan mökit jotka vapaana annettuina päivinä
            filteredMokit = filteredMokit
                .Where(m => m.Varaus.All(v => v.VarattuLoppupvm < alkupaiva.Value || v.VarattuAlkupvm > loppupaiva.Value))
                .ToList();
            if (!filteredMokit.Any())
            {
                //Jos alueella ei ole vapaana mökkejä, annetaan alert
                await DisplayAlert("Valitettavasti alueella ei ole mökkejä vapaana valittuna ajankohtana", "vaihda päivämääriä", "OK!");
            }

            else
            {//Asetetaan mökkilistaan vapaana olevat mökit
                mokki_lista.ItemsSource = filteredMokit;
            }
        }

    }

    private void mokki_lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        selectedMokki = (Mokki)mokki_lista.SelectedItem;


    }
    private async void palvelu_lkm_TextChanged(object sender, TextChangedEventArgs e)
    {
        Entry entry = (Entry)sender;
        
        
        if (!funktiot.CheckEntryInteger(entry, this))
        {
            // funktiossa tarkistetaan ettei syote sisalla tekstia
        }
        else if (selectedPalvelu != null) 
        {
            lukumaara = int.Parse(entry.Text);
            entry.BindingContext = selectedPalvelu;
            listaViewModel.PalveluLkm_TextChanged(sender, e);
        }
        else 
        {
            await DisplayAlert("Valitse ensin palvelu","Sen jälkeen voit antaa lukumäärän","OK!");
        }
        entry.TextChanged += (sender, e) =>
        {
            listaViewModel.PalveluLkm_TextChanged(sender, e);
        };
    }
    private async void palvelu_lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {
      

        selectedPalvelu = (Palvelu)palvelu_lista.SelectedItem;


            if (selectedPalvelu != null)
            {
                // Päivitä valittujen palveluiden lista
                listaViewModel.OnItemTapped(selectedPalvelu);
                
            }
            else
            {
              await DisplayAlert("Tuli tähän","",""); //väliaikanen alertti virheen seurantaan
            }



    }




    private async void uusi_asiakas_Clicked(object sender, EventArgs e)
    {
        Grid grid = (Grid)entry_grid;
        Grid grid2 = (Grid)entry_grid2;

        if (!funktiot.CheckInput(this, entry_grid))
        {
            //Tarkistetaan onko kaikki tarvittavat tiedot annettu
        }
        else if (!funktiot.CheckInput(this, entry_grid2))
        {
            //Tarkistetaan onko kaikki tarvittavat tiedot annettu
        }

        else if (selectedMokki != null)
        {
            //navigointi uudelle sivulle ja annetaan mukaan varauksen tiedot
            VarauksenTiedot varauksenTiedot = await VarauksenTiedotAsync();

            await Navigation.PushAsync(new Uusi_asiakas(this, varauksenTiedot));
        }
        else
        {
            //Jos ei valinnu mökkiä ni ei päästetä etenemään
            await DisplayAlert("No pittäähän se mökkiki valita jos meinasit mökille mennä", "", "OK!");
        }
    }

    private async void vanha_asiakas_Clicked(object sender, EventArgs e)
    {
        if (!funktiot.CheckInput(this, entry_grid))
        {
            //Tarkistetaan onko kaikki tarvittavat tiedot annettu
        }
        else if (!funktiot.CheckInput(this, entry_grid2))
        {
            //Tarkistetaan onko kaikki tarvittavat tiedot annettu
        }
        else if (selectedMokki != null)
        {   //Tarkistetaan että on valinnut myös mökin ja sit siirrytään
            // Navigointi uudelle sivulle ja annetaan mukaan varauksen tiedot
            VarauksenTiedot varauksenTiedot = await VarauksenTiedotAsync();

            await Navigation.PushAsync(new Vanha_asiakas(this, varauksenTiedot));

        }
        else
        {   //Ei päästetä jatkamaan jos ei valinnu mökkiä
            await DisplayAlert("No pittäähän se mökkiki valita jos meinasit mökille mennä", "", "OK!");
        }


    }

    private async Task<VarauksenTiedot> VarauksenTiedotAsync()
    {
        // Tämä ottaa talteen varauksentiedot luokkaan omiin muuttujiin, jotta helpompi siirtyä sivulta toiselle
        VarauksenTiedot varauksenTiedot = new VarauksenTiedot
        {
            ValittuMokki = selectedMokki,
            ValittuAlue = selectedAlue,
            VarattuAlkupvm = alkupvm.Date,
            VarattuLoppupvm = loppupvm.Date,
            Varattupvm = DateTime.Now,
            Vahvistuspvm = alkupvm.Date - TimeSpan.FromDays(7),
            VarauksenPalveluts = new List<VarauksenPalvelut>()
        };

        // Tarkista, onko palvelu valittu ja onko lukumäärä määritelty
        if (listaViewModel.valitutPalvelutLista==null || listaViewModel.valitutPalvelutLista.Count == 0 || lukumaara <= 0)
        {
            bool jatkaIlmanPalveluita = await DisplayAlert("Palveluita ei valittu", "Haluatko jatkaa ilman palveluita?", "Kyllä", "Ei");
            if (!jatkaIlmanPalveluita)
            {
                // Jos käyttäjä ei halua jatkaa ilman palveluita, palauta null
               
            }
            // Jos käyttäjä haluaa jatkaa ilman palveluita, jatketaan ilman palveluiden lisäystä
        }
        else
        {
            // Jos palveluita on valittu, lisätään ne varauksen tietoihin
            foreach (var palvelu in listaViewModel.valitutPalvelutLista)
            {
                int palvelunLukumaara = listaViewModel.PalveluidenLkm.ContainsKey(palvelu) ? listaViewModel.PalveluidenLkm[palvelu] : 0;
                if (palvelunLukumaara > 0)
                {
                    varauksenTiedot.VarauksenPalveluts.Add(new VarauksenPalvelut
                    {
                        Palvelu = palvelu,
                        Lkm = palvelunLukumaara
                    });
                }
            }
        }

        return varauksenTiedot;
    }
}