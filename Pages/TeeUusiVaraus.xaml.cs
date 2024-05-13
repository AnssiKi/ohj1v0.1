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

    Alue selectedAlue = null;
    Mokki selectedMokki = null;
    Palvelu selectedPalvelu = null;
    private int lukumaara = 0;
    private DateTime? alkupaiva = DateTime.Now;
    private DateTime? loppupaiva = null;







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

    private async void alkupvm_DateSelected(object sender, DateChangedEventArgs e)
    {
        alkupaiva = e.NewDate;

        
       
        if (alkupaiva.HasValue) 
        {
            if (alkupaiva.Value < DateTime.Now)
            {
                await DisplayAlert("Aloituspäivämäärä tulee olla aikaisintaan tänään", "Valitse uusi päivämäärä", "OK!");
            }
        }
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
            else if (alkupaiva.Value == loppupaiva.Value)
            {
                await DisplayAlert("Minimi vuokrausaika 1vrk", "Valitse uusi loppu päivämäärä", "OK!");

            }
            if ((Alue)alue_nimi.SelectedItem != null)
            {
                List<Mokki> vapaanaolevat;

                using var context = new VnContext();
                {

                    // Suodatetaan ensin mökit alueen mukaan
                    vapaanaolevat = await context.Mokkis
                        .Where(m => m.AlueId == selectedAlue.AlueId)
                        .Include(m => m.Varaus)
                        .ToListAsync();

                    // Sen jälkeen suodatetaan mökit, jotka ovat vapaana annettuina päivinä
                    vapaanaolevat = vapaanaolevat
                        .Where(m => m.Varaus.All(v => v.VarattuLoppupvm < alkupaiva.Value || v.VarattuAlkupvm > loppupaiva.Value))
                        .ToList();
                }
                if (vapaanaolevat.Any())
                {//Asetetaan vapaanaolevat mökit näkyväksi
                   mokki_lista.ItemsSource = vapaanaolevat;
                }
                else 
                {//Jos ei oo mökkejä vapaana,ei näytetä mitään
                   mokki_lista.ItemsSource = null; 
                }
            }
        }
        else
        {
            await DisplayAlert("Valitse myös aloituspäivä ", "nämä tiedot ovat pakollisia", "OK!");
        }
    }



    private async void henkilomaara_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((Alue)alue_nimi.SelectedItem != null && alkupaiva.HasValue && loppupaiva.HasValue)
        {
            List<Mokki> filteredMokit;
            int henkilo = henkilomaara.SelectedIndex + 1;
            
            using var context = new VnContext();
            { //suodatetaan ensin mökit alueen mukaan ja henkilömäärän mukaan
                 filteredMokit = await context.Mokkis
                .Where(m => m.Henkilomaara >= henkilo && m.AlueId == selectedAlue.AlueId)
                .Include(m => m.Varaus)
                .ToListAsync();

                //Sen jälkeen suodatetaan mökit jotka vapaana annettuina päivinä
                filteredMokit = filteredMokit
                    .Where(m => m.Varaus.All(v => v.VarattuLoppupvm < alkupaiva.Value || v.VarattuAlkupvm > loppupaiva.Value))
                    .ToList();
            }
            
            if (!filteredMokit.Any())
            {
                //Jos alueella ei ole vapaana mökkejä, annetaan alert
                await DisplayAlert("Valitettavasti alueella ei ole mökkejä vapaana valittuna ajankohtana", "vaihda päivämääriä", "OK!");
                mokki_lista.ItemsSource = null;
            }

            else
            {//Asetetaan mökkilistaan vapaana olevat mökit
                mokki_lista.ItemsSource = filteredMokit;
            }
        }
        else
        {
            await DisplayAlert("Valitse ensin alue ja päivämäärät,jolloin haluaisit vuokrata mökin", "", "OK!");

        }

    }

    private async void mokki_lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        //Tarkistetaan ettei mökillä oo samaan aikaan jo olemassa olevia varauksia
        using (var context = new VnContext())
        {
            var mokki = e.Item as Mokki;
            var mokkiId = mokki.MokkiId;
            //eka filtteröidään mökin kaikki varaukset ja tarkistetaan ettei ole valittuina päivinä
            var mokin_varaukset = context.Varaus

                .Where(v => v.MokkiId == mokkiId &&
                            ((v.VarattuAlkupvm <= alkupaiva && v.VarattuLoppupvm >= alkupaiva) ||
                             (v.VarattuAlkupvm <= loppupaiva && v.VarattuLoppupvm >= loppupaiva)))
                .Select(v => new
                {
                    VarausId = v.VarausId,
                    AlkuPvm = v.VarattuAlkupvm,
                    LoppuPvm = v.VarattuLoppupvm,
                    MokkiNimi = v.Mokki.Mokkinimi,
                    MokkiAlue = v.Mokki.Alue.Nimi,
                    PalveluNimi = v.VarauksenPalveluts.Select(vp => vp.Palvelu.Nimi).FirstOrDefault(),
                    PalveluAlue = v.VarauksenPalveluts.Select(vp => vp.Palvelu.Alue.Nimi).FirstOrDefault()
                })
                .OrderBy(v => v.VarausId)
                .ToList();
            if (mokin_varaukset.Any())
            {//Jos mökillä on varauksia kyseisenä aikana ja vahingossa päässy näkymään yritetään napata se tällä
                await DisplayAlert("Mökki varattuna valittuna ajankohtana","Vaihda mökkiä tai vuokrausaikaa","OK!");
                selectedMokki = null;
            }
            else
            {//jos mökki on vapaana otetaan se olio selectedMokki muuttujaan
                selectedMokki = (Mokki)mokki_lista.SelectedItem;
            }


        }
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

            if (varauksenTiedot != null)
            {//jos käyttäjä valitsee että haluaakin lisätä palvelut ni estetään siirtyminen uuelle sivulle
                await Navigation.PushAsync(new Uusi_asiakas(this, varauksenTiedot));
            }
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

            if (varauksenTiedot != null) {//jos käyttäjä valitsee että haluaakin lisätä palvelut ni estetään siirtyminen uuelle sivulle
                await Navigation.PushAsync(new Vanha_asiakas(this, varauksenTiedot));
            }
        }
        else
        {   //Ei päästetä jatkamaan jos ei valinnu mökkiä
            await DisplayAlert("No pittäähän se mökkiki valita jos meinasit mökille mennä", "", "OK!");
        }


    }

    private async Task<VarauksenTiedot> VarauksenTiedotAsync()
    {
        //Tarkistetaan että onko varaus alkamassa jo alle viikonpäästä
        var erotus= alkupvm.Date - DateTime.Now;
        var vahvistus = DateTime.Now;

        if (erotus.TotalDays <= 7) 
        {
            vahvistus = DateTime.Now;
        }
        else { vahvistus = alkupvm.Date - TimeSpan.FromDays(7); }

        VarauksenTiedot varauksenTiedot = new VarauksenTiedot
        {  // Tämä ottaa talteen varauksentiedot luokkaan omiin muuttujiin, jotta helpompi siirtyä sivulta toiselle
            ValittuMokki = selectedMokki,
            ValittuAlue = selectedAlue,
            VarattuAlkupvm = alkupvm.Date,
            VarattuLoppupvm = loppupvm.Date,
            Varattupvm = DateTime.Now,
            Vahvistuspvm = vahvistus,
            VarauksenPalveluts = new List<VarauksenPalvelut>()
        };

        // Tarkista, onko palvelu valittu ja onko lukumäärä määritelty
        if (listaViewModel.valitutPalvelutIdLista==null || listaViewModel.valitutPalvelutIdLista.Count == 0 || lukumaara <= 0)
        {
            bool jatkaIlmanPalveluita = await DisplayAlert("Palveluita ei valittu", "Haluatko jatkaa ilman palveluita?", "Kyllä", "Ei");
            if (!jatkaIlmanPalveluita)
            {
                return null; // Jos käyttäjä ei halua jatkaa ilman palveluita, palauta null
               
            }
            // Jos käyttäjä haluaa jatkaa ilman palveluita, jatketaan ilman palveluiden lisäystä
        }
        else
        {
            // Jos palveluita on valittu, lisätään ne varauksen tietoihin
            foreach (var palveluId in listaViewModel.valitutPalvelutIdLista)
            {
                int palvelunLukumaara = listaViewModel.PalveluidenLkm.ContainsKey(palveluId) ? listaViewModel.PalveluidenLkm[palveluId] : 0;
                if (palvelunLukumaara > 0)
                {
                    varauksenTiedot.VarauksenPalveluts.Add(new VarauksenPalvelut
                    {
                        PalveluId = palveluId, // Käytetään palvelun ID:tä
                        Lkm = palvelunLukumaara
                    });
                }
            }
        }

        return varauksenTiedot;
    }
  
}