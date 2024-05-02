using ohj1v0._1;
using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;
using Microsoft.EntityFrameworkCore;

namespace ohj1v0._1;

public partial class TeeUusiVaraus : ContentPage
{
    Funktiot funktiot = new Funktiot();
    AlueViewmodel alueViewmodel = new AlueViewmodel();
    MokkiViewmodel mokkiViewmodel = new MokkiViewmodel();
    VarausViewmodel varausViewmodel = new VarausViewmodel();

    public TeeUusiVaraus()
	{
		InitializeComponent();
        alue_nimi.BindingContext = alueViewmodel;
        varauspvm.Text = DateTime.Now.ToString("dd.MM.yyyy");
        mokki_lista.BindingContext = mokkiViewmodel.Mokkis;
        
    }
    
    Alue selectedAlue;
    Mokki selectedMokki;
    private DateTime? alkupaiva;
    private DateTime? loppupaiva;




    private async void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {       

        if ((Alue)alue_nimi.SelectedItem != null)
        {
            selectedAlue = (Alue)alue_nimi.SelectedItem;
        }
        
    }

    private void alkupvm_DateSelected(object sender, DateChangedEventArgs e)
    {
        alkupaiva = e.NewDate;
    }

    private async void loppupvm_DateSelected(object sender, DateChangedEventArgs e)
    {
        loppupaiva = e.NewDate;

        if (alkupaiva.HasValue && loppupaiva.HasValue) {

            if (alkupaiva.Value > loppupaiva.Value)
            {
                DisplayAlert("Virhe", "Aloituspäivämäärä ei voi olla lopetuspäivämäärän jälkeen", "OK");

            }
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
            //Asetetaan mökkilistaan vapaana olevat mökit
            mokki_lista.ItemsSource = filteredMokit;
        }

    }

    private void mokki_lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        selectedMokki = (Mokki)mokki_lista.SelectedItem;
    }
    private void palvelu_lkm_TextChanged(object sender, TextChangedEventArgs e)
    {
        Entry entry = (Entry)sender;
        funktiot.CheckEntryInteger(entry, this); // funktiossa tarkistetaan ettei syote sisalla tekstia
    }

   


    private void uusi_asiakas_Clicked(object sender, EventArgs e)
    {
        // Tässä navigointi uudelle sivulle ja annetaan mukaan varauksen tiedot

        VarauksenTiedot varauksenTiedot = VarauksenTiedot();

        Navigation.PushAsync(new Uusi_asiakas(this,varauksenTiedot)); // tarvitsee tarkistukset tietojen oikeellisuudelle
    }

    private void vanha_asiakas_Clicked(object sender, EventArgs e)
    {
        // Navigointi uudelle sivulle ja annetaan mukaan varauksen tiedot
        VarauksenTiedot varauksenTiedot = VarauksenTiedot();

        Navigation.PushAsync(new Vanha_asiakas(this,varauksenTiedot)); // tarvitsee tarkistukset tietojen oikeellisuudelle

    }

    private VarauksenTiedot VarauksenTiedot() {
        //Tämä ottaa talteen varauksentiedot luokkaan omiin muuttujiin,jotta helpompi siirtyä sivulta toiselle

        VarauksenTiedot varauksenTiedot = new VarauksenTiedot
        {
            ValittuMokki = selectedMokki,
            ValittuAlue = selectedAlue,
            VarattuAlkupvm = alkupvm.Date,
            VarattuLoppupvm = loppupvm.Date,
            Varattupvm = DateTime.Now,
            Vahvistuspvm = alkupvm.Date - TimeSpan.FromDays(7)
        };

        return varauksenTiedot;
    }

}