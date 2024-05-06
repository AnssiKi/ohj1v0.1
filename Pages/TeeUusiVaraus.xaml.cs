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

            var mokitValitullaAlueella = await context.Mokkis.Where(m => m.AlueId == selectedAlue.AlueId).ToListAsync();//filtteroidaan m�kit alueella
            var palvelutValitullaAlueella = await context.Palvelus.Where(p => p.AlueId == selectedAlue.AlueId).ToListAsync();//filtteroidaan palvelut alueella

            if (!mokitValitullaAlueella.Any()) //Jos alueella ei ole m�kkej�
            {
                await DisplayAlert("T�ll� alueella ei ole viel� m�kkej�", "Valitse uusi alue", "OK!");
            }
            else
            {
                mokki_lista.ItemsSource = mokitValitullaAlueella;

                if (!palvelutValitullaAlueella.Any()) //Jos alueella ei ole palveluita
                {
                    await DisplayAlert("Valitulla alueella ei ole saatavilla palveluita.", "Voit kuitenkin vuokrata m�kin tai halutessasi palveluita vaihtaa aluetta", "OK!");
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
                await DisplayAlert("Virhe", "Aloitusp�iv�m��r� ei voi olla lopetusp�iv�m��r�n j�lkeen", "OK");

            }
            else if (alkupaiva.Value < DateTime.Now)
            {
                await DisplayAlert("Aloitusp�iv�m��r� tulee olla aikaisintaan t�n��n", "Valitse uusi p�iv�m��r�", "OK!");
            }
        }
        else
        {
            await DisplayAlert("Valitse my�s aloitusp�iv� ", "n�m� tiedot ovat pakollisia", "OK!");
        }
    }



    private async void henkilomaara_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((Alue)alue_nimi.SelectedItem != null)
        {
            int henkilo = henkilomaara.SelectedIndex + 1;

            using var context = new VnContext();
            //suodatetaan ensin m�kit alueen mukaan
            var filteredMokit = await context.Mokkis
            .Where(m => m.Henkilomaara >= henkilo && m.AlueId == selectedAlue.AlueId)
            .Include(m => m.Varaus)
            .ToListAsync();
            //Sen j�lkeen suodatetaan m�kit jotka vapaana annettuina p�ivin�
            filteredMokit = filteredMokit
                .Where(m => m.Varaus.All(v => v.VarattuLoppupvm < alkupaiva.Value || v.VarattuAlkupvm > loppupaiva.Value))
                .ToList();
            if (!filteredMokit.Any())
            {
                //Jos alueella ei ole vapaana m�kkej�, annetaan alert
                await DisplayAlert("Valitettavasti alueella ei ole m�kkej� vapaana valittuna ajankohtana", "vaihda p�iv�m��ri�", "OK!");
            }

            else
            {//Asetetaan m�kkilistaan vapaana olevat m�kit
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
            await DisplayAlert("Valitse ensin palvelu","Sen j�lkeen voit antaa lukum��r�n","OK!");
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

            await Navigation.PushAsync(new Uusi_asiakas(this, varauksenTiedot));
        }
        else
        {
            //Jos ei valinnu m�kki� ni ei p��stet� etenem��n
            await DisplayAlert("No pitt��h�n se m�kkiki valita jos meinasit m�kille menn�", "", "OK!");
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

            await Navigation.PushAsync(new Vanha_asiakas(this, varauksenTiedot));

        }
        else
        {   //Ei p��stet� jatkamaan jos ei valinnu m�kki�
            await DisplayAlert("No pitt��h�n se m�kkiki valita jos meinasit m�kille menn�", "", "OK!");
        }


    }

    private async Task<VarauksenTiedot> VarauksenTiedotAsync()
    {
        // T�m� ottaa talteen varauksentiedot luokkaan omiin muuttujiin, jotta helpompi siirty� sivulta toiselle
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

        // Tarkista, onko palvelu valittu ja onko lukum��r� m��ritelty
        if (listaViewModel.valitutPalvelutLista==null || listaViewModel.valitutPalvelutLista.Count == 0 || lukumaara <= 0)
        {
            bool jatkaIlmanPalveluita = await DisplayAlert("Palveluita ei valittu", "Haluatko jatkaa ilman palveluita?", "Kyll�", "Ei");
            if (!jatkaIlmanPalveluita)
            {
                // Jos k�ytt�j� ei halua jatkaa ilman palveluita, palauta null
               
            }
            // Jos k�ytt�j� haluaa jatkaa ilman palveluita, jatketaan ilman palveluiden lis�yst�
        }
        else
        {
            // Jos palveluita on valittu, lis�t��n ne varauksen tietoihin
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