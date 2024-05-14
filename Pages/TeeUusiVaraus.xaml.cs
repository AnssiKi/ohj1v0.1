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

            var mokitValitullaAlueella = await context.Mokkis.Where(m => m.AlueId == selectedAlue.AlueId).ToListAsync();//filtteroidaan m�kit alueella
            var palvelutValitullaAlueella = await context.Palvelus.Where(p => p.AlueId == selectedAlue.AlueId).ToListAsync();//filtteroidaan palvelut alueella

            if (!mokitValitullaAlueella.Any()) //Jos alueella ei ole m�kkej�
            {
                await DisplayAlert("Ilmoitus", "Alueella ei ole m�kkej�", "OK!");
            }
            else
            {
                mokki_lista.ItemsSource = mokitValitullaAlueella;

                if (!palvelutValitullaAlueella.Any()) //Jos alueella ei ole palveluita
                {
                    await DisplayAlert("Ilmoitus", "Alueella ei ole  tarjolla palveluita", "OK!");
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
                await DisplayAlert("Ilmoitus", "Aloitusp�iv�m��r� tulee olla aikaisintaan t�n��n", "OK!");
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
                await DisplayAlert("Virhe", "Aloitusp�iv�m��r� ei voi olla lopetusp�iv�m��r�n j�lkeen", "OK");

            }
            else if (alkupaiva.Value == loppupaiva.Value)
            {
                await DisplayAlert("Ilmoitus", "Minimi vuokrausaika 1vrk. Valitse uusi loppu p�iv�m��r�", "OK");

            }
            if ((Alue)alue_nimi.SelectedItem != null)
            {
                List<Mokki> vapaanaolevat;

                using var context = new VnContext();
                {

                    // Suodatetaan ensin m�kit alueen mukaan
                    vapaanaolevat = await context.Mokkis
                        .Where(m => m.AlueId == selectedAlue.AlueId)
                        .Include(m => m.Varaus)
                        .ToListAsync();

                    // Sen j�lkeen suodatetaan m�kit, jotka ovat vapaana annettuina p�ivin�
                    vapaanaolevat = vapaanaolevat
                        .Where(m => m.Varaus.All(v => v.VarattuLoppupvm < alkupaiva.Value || v.VarattuAlkupvm > loppupaiva.Value))
                        .ToList();
                }
                if (vapaanaolevat.Any())
                {//Asetetaan vapaanaolevat m�kit n�kyv�ksi
                   mokki_lista.ItemsSource = vapaanaolevat;
                }
                else 
                {//Jos ei oo m�kkej� vapaana,ei n�ytet� mit��n
                   mokki_lista.ItemsSource = null; 
                }
                mokki_lista.IsEnabled = true;
                palvelu_lista.IsEnabled = true;
            }
        }
        else
        {
            await DisplayAlert("Ilmoitus", "Valitse aloitusp�iv�m���r�, n�m� tiedot ovat pakollisia", "OK");
        }
    }
    private async void henkilomaara_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((Alue)alue_nimi.SelectedItem != null && alkupaiva.HasValue && loppupaiva.HasValue)
        {
            List<Mokki> filteredMokit;
            int henkilo = henkilomaara.SelectedIndex + 1;
            
            using var context = new VnContext();
            { //suodatetaan ensin m�kit alueen mukaan ja henkil�m��r�n mukaan
                 filteredMokit = await context.Mokkis
                .Where(m => m.Henkilomaara >= henkilo && m.AlueId == selectedAlue.AlueId)
                .Include(m => m.Varaus)
                .ToListAsync();

                //Sen j�lkeen suodatetaan m�kit jotka vapaana annettuina p�ivin�
                filteredMokit = filteredMokit
                    .Where(m => m.Varaus.All(v => v.VarattuLoppupvm < alkupaiva.Value || v.VarattuAlkupvm > loppupaiva.Value))
                    .ToList();
            }
            if (!filteredMokit.Any())
            {
                //Jos alueella ei ole vapaana m�kkej�, annetaan alert
                await DisplayAlert("Ilmoitus", "Valitettavasti alueella ei ole m�kkej� vapaana valittuna ajankohtana", "OK!");
                mokki_lista.ItemsSource = null;
            }

            else
            {//Asetetaan m�kkilistaan vapaana olevat m�kit
                mokki_lista.ItemsSource = filteredMokit;
            }
        }
        else
        {
            await DisplayAlert("Ilmoitus", "Valitse ensin alue ja p�iv�m��r�t,jolloin haluaisit vuokrata m�kin", "OK!");

        }

    }

    private async void mokki_lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        //Tarkistetaan ettei m�kill� oo samaan aikaan jo olemassa olevia varauksia
        using (var context = new VnContext())
        {
            var mokki = e.Item as Mokki;
            var mokkiId = mokki.MokkiId;
            //eka filtter�id��n m�kin kaikki varaukset ja tarkistetaan ettei ole valittuina p�ivin�
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
            {//Jos m�kill� on varauksia kyseisen� aikana ja vahingossa p��ssy n�kym��n yritet��n napata se t�ll�
                await DisplayAlert("Ilmoitus","M�kki on varattuna halutulla ajankohdalla, valitse eri m�kki","OK!");
                selectedMokki = null;
            }
            else
            {//jos m�kki on vapaana otetaan se olio selectedMokki muuttujaan
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
            await DisplayAlert("Ilmoitus","Valitse ensin palvelu, sen j�lkeen lukum��r�","OK!");
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
                // P�ivit� valittujen palveluiden lista
                listaViewModel.OnItemTapped(selectedPalvelu);

                       
            }
            else
            {
              await DisplayAlert("Tuli t�h�n","",""); //v�liaikanen alertti virheen seurantaan
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
            {//jos k�ytt�j� valitsee ett� haluaakin lis�t� palvelut ni estet��n siirtyminen uuelle sivulle
                await Navigation.PushAsync(new Uusi_asiakas(this, varauksenTiedot));
            }
        }
        else
        {
            //Jos ei valinnu m�kki� ni ei p��stet� etenem��n
            await DisplayAlert("Virhe", "M�kki� ei ole valittu", "OK!");
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
        {   //Tarkistetaan ett� on valinnut my�s m�kin ja sit siirryt��n
            // Navigointi uudelle sivulle ja annetaan mukaan varauksen tiedot
            VarauksenTiedot varauksenTiedot = await VarauksenTiedotAsync();

            if (varauksenTiedot != null) {//jos k�ytt�j� valitsee ett� haluaakin lis�t� palvelut ni estet��n siirtyminen uuelle sivulle
                await Navigation.PushAsync(new Vanha_asiakas(this, varauksenTiedot));
            }
        }
        else
        {   //Ei p��stet� jatkamaan jos ei valinnu m�kki�
            await DisplayAlert("Virhe", "M�kki� ei ole valittu", "OK!");
        }
    }

    private async Task<VarauksenTiedot> VarauksenTiedotAsync()
    {
        //Tarkistetaan ett� onko varaus alkamassa jo alle viikonp��st�
        var erotus= alkupvm.Date - DateTime.Now;
        var vahvistus = DateTime.Now;

        if (erotus.TotalDays <= 7) 
        {
            vahvistus = DateTime.Now;
        }
        else { vahvistus = alkupvm.Date - TimeSpan.FromDays(7); }

        VarauksenTiedot varauksenTiedot = new VarauksenTiedot
        {  // T�m� ottaa talteen varauksentiedot luokkaan omiin muuttujiin, jotta helpompi siirty� sivulta toiselle
            ValittuMokki = selectedMokki,
            ValittuAlue = selectedAlue,
            VarattuAlkupvm = alkupvm.Date,
            VarattuLoppupvm = loppupvm.Date,
            Varattupvm = DateTime.Now,
            Vahvistuspvm = vahvistus,
            VarauksenPalveluts = new List<VarauksenPalvelut>()
        };

        // Tarkista, onko palvelu valittu ja onko lukum��r� m��ritelty
        if (listaViewModel.valitutPalvelutIdLista==null)
        {
            bool jatkaIlmanPalveluita = await DisplayAlert("Ilmoitus", "Haluatko jatkaa ilman palveluita?", "Kyll�", "Ei");
            if (!jatkaIlmanPalveluita)
            {
                return null; // Jos k�ytt�j� ei halua jatkaa ilman palveluita, palauta null
               
            }
            // Jos k�ytt�j� haluaa jatkaa ilman palveluita, jatketaan ilman palveluiden lis�yst�
           
        }
        else
        {
            // Jos palveluita on valittu, lis�t��n ne varauksen tietoihin
            foreach (var palveluId in listaViewModel.valitutPalvelutIdLista)
            {
                int palvelunLukumaara = listaViewModel.PalveluidenLkm.ContainsKey(palveluId) ? listaViewModel.PalveluidenLkm[palveluId] : 0;

                if (selectedPalvelu != null && palvelunLukumaara <= 0)
                {
                    palvelunLukumaara += 1;

                    varauksenTiedot.VarauksenPalveluts.Add(new VarauksenPalvelut
                    {
                        PalveluId = palveluId, // K�ytet��n palvelun ID:t�
                        Lkm = palvelunLukumaara
                    });

                    
                    listaViewModel.EiValittu(selectedPalvelu, palvelunLukumaara);
                }
                if (palvelunLukumaara > 0)
                {
                    varauksenTiedot.VarauksenPalveluts.Add(new VarauksenPalvelut
                    {
                        PalveluId = palveluId, // K�ytet��n palvelun ID:t�
                        Lkm = palvelunLukumaara
                    });
                }
            }
        }
        return varauksenTiedot;
    }  
}