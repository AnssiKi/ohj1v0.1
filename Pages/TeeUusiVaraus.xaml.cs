using Microsoft.EntityFrameworkCore;
using ohj1v0._1.Luokat;
using ohj1v0._1.Models;
using ohj1v0._1.Viewmodels;

namespace ohj1v0._1;

public partial class TeeUusiVaraus : ContentPage
{
    Funktiot funktiot = new Funktiot();
    AlueViewmodel alueViewmodel = new AlueViewmodel();
    MokkiViewmodel mokkiViewmodel = new MokkiViewmodel();
    VarausViewmodel varausViewmodel = new VarausViewmodel();
    PalveluViewmodel palveluViewmodel = new PalveluViewmodel();
    ListaViewModel listaViewModel = new ListaViewModel();

    Alue selectedAlue = null;
    Mokki selectedMokki = null;
    Palvelu selectedPalvelu = null;
    private int lukumaara = 0;
    private DateTime? alkupaiva = DateTime.Today;
    private DateTime? loppupaiva = DateTime.Today.AddDays(1);


    public TeeUusiVaraus()
    {
        InitializeComponent();
        alue_nimi.BindingContext = alueViewmodel;
        varauspvm.Text = DateTime.Now.ToString("dd.MM.yyyy");
        mokki_lista.BindingContext = mokkiViewmodel.Mokkis;
        palvelu_lista.BindingContext = palveluViewmodel.Palvelus;
        InitializeDefaultValues();
    }

    private void InitializeDefaultValues()
    {
        alkupvm.Date = DateTime.Today;
        loppupvm.Date = DateTime.Today.AddDays(1);
        alkupvm.IsEnabled = false;
        loppupvm.IsEnabled = false;
        alue_nimi.SelectedIndex = -1;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        InitializeDefaultValues();
    }


    private async void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {

        if ((Alue)alue_nimi.SelectedItem != null)
        {
            selectedAlue = (Alue)alue_nimi.SelectedItem;

            using var context = new VnContext();

            var mokitValitullaAlueella = await context.Mokkis.Where(m => m.AlueId == selectedAlue.AlueId).ToListAsync();//filtteroidaan m�kit alueella
            var palvelutValitullaAlueella = await context.Palvelus.Where(p => p.AlueId == selectedAlue.AlueId).ToListAsync();//filtteroidaan palvelut alueella

            alkupvm.IsEnabled = true;
            loppupvm.IsEnabled = true;

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
        if (e.NewDate < DateTime.Today)
        {
            await DisplayAlert("Ilmoitus", "Aloitusp�iv�m��r� tulee olla aikaisintaan t�n��n", "OK!");
            alkupvm.Date = DateTime.Today;  // Resetoi alkup�iv�m��r�
        }
        else
        {
            alkupaiva = e.NewDate;
            // Varmistetaan, ett� loppup�iv�m��r� on v�hint��n yksi p�iv� alkup�iv�m��r�n j�lkeen
            if (loppupaiva.HasValue && loppupaiva.Value <= alkupaiva.Value)
            {
                loppupaiva = alkupaiva.Value.AddDays(1);
                loppupvm.Date = loppupaiva.Value;
            }
        }
    }

    private async void loppupvm_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (alkupaiva.HasValue && e.NewDate <= alkupaiva.Value)
        {
            await DisplayAlert("Ilmoitus", "Loppup�iv�m��r� tulee olla v�hint��n yksi p�iv� alkup�iv�m��r�n j�lkeen", "OK!");
            loppupaiva = alkupaiva.Value.AddDays(1);
            loppupvm.Date = loppupaiva.Value;
        }
        else
        {
            loppupaiva = e.NewDate;
        }

        if (alkupaiva.HasValue && loppupaiva.HasValue)
        {
            if (selectedAlue != null)
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
                {
                    // Asetetaan vapaanaolevat m�kit n�kyv�ksi
                    mokki_lista.ItemsSource = vapaanaolevat;
                }
                else
                {
                    // Jos ei ole m�kkej� vapaana, ei n�ytet� mit��n
                    mokki_lista.ItemsSource = null;
                }
                mokki_lista.IsEnabled = true;
                palvelu_lista.IsEnabled = true;
            }
        }
        else
        {
            await DisplayAlert("Ilmoitus", "Valitse aloitusp�iv�m��r�, n�m� tiedot ovat pakollisia", "OK");
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
                await DisplayAlert("Ilmoitus", "M�kki on varattuna halutulla ajankohdalla, valitse eri m�kki", "OK!");
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

    }
    private async void uusi_asiakas_Clicked(object sender, EventArgs e)
    {

        if (selectedMokki != null && selectedAlue != null && alkupaiva.HasValue && loppupaiva.HasValue) //T�st� poistettu funkiot-luokan k�ytt� tarkistuksista ja vaaditaan ett� kaikki n�m� on valittuna ennen kun voi siirty� tallentamaan varausta
        {
            //Hetaan varauksen tiedot omiin muuttujiin talteen annetaan mukaan uudelle sivulle ja siell� kysyt��n haluaako ilman palveluita
            VarauksenTiedot varauksenTiedot = await VarauksenTiedotAsync();

            if (varauksenTiedot != null)
            {// jos varauksen tiedot tallentui siirryt��n uudelle sivulle
                await Navigation.PushAsync(new Uusi_asiakas(this, varauksenTiedot));
            }
        }
        else
        {
            //Jos ei valinnu m�kki� tai aluetta tai p�iv�m��ri� ni ei p��stet� etenem��n
            await DisplayAlert("Virhe", "Kaikkia varaukseen tarvittavia tietoja ei ole valittu", "OK!");
        }
    }

    private async void vanha_asiakas_Clicked(object sender, EventArgs e)
    {

        if (selectedMokki != null && selectedAlue != null && alkupaiva.HasValue && loppupaiva.HasValue)//Tarkistetaan ett� on valinnut kaikki tarvittavat tiedot
        {
            // Haetaan varauksen tiedot omiin muuttujiin talteen. Siell� kysyt��n haluaako jatkaa ilman palveluita
            VarauksenTiedot varauksenTiedot = await VarauksenTiedotAsync();

            if (varauksenTiedot != null)
            {//Jos k�ytt�j� on joko valinnut palvelut tai siirtymisen ilman palveluita, p��st��n etenem��n
                await Navigation.PushAsync(new Vanha_asiakas(this, varauksenTiedot));
            }
        }
        else
        {   //Ei p��stet� jatkamaan jos ei valinnu kaikkia tarvittavia tietoja
            await DisplayAlert("Virhe", "Kaikkia varaukseen tarvittavia tietoja ei ole valittu", "OK!");
        }
    }

    private async Task<VarauksenTiedot> VarauksenTiedotAsync()
    {
        //Tarkistetaan ett� onko varaus alkamassa jo alle viikonp��st�
        var erotus = alkupvm.Date - DateTime.Now;
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
        if (!listaViewModel.valitutPalvelutIdLista.Any())
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
    public void TyhjennaVarausTiedot()
    {

        InitializeDefaultValues();

        selectedAlue = null; //nollataan kaikki valinnat
        selectedMokki = null;
        selectedPalvelu = null;
        lukumaara = 0; //Tyhjent�� varauksen palveluihin liittyv�n lukum��r�n

        mokki_lista.ItemsSource = null; //laitetaan listat tyhjiks
        palvelu_lista.ItemsSource = null;

        mokki_lista.IsEnabled = false;
        palvelu_lista.IsEnabled = false;


    }
}