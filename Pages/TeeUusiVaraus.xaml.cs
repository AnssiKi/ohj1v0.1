using ohj1v0._1;
using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;

namespace ohj1v0._1;

public partial class TeeUusiVaraus : ContentPage
{
	public TeeUusiVaraus()
	{
		InitializeComponent();
        alue_nimi.BindingContext = alueViewmodel;
        varauspvm.Text = DateTime.Now.ToString("dd.MM.yyyy");
        mokki_lista.BindingContext = mokkiViewmodel;
        mokki_lista.ItemsSource = null;
        
    }
    Funktiot funktiot = new Funktiot();
    AlueViewmodel alueViewmodel = new AlueViewmodel();
    MokkiViewmodel mokkiViewmodel = new MokkiViewmodel();
    Alue selectedAlue;


    private void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {       

        if ((Alue)alue_nimi.SelectedItem != null)
        {
            selectedAlue = (Alue)alue_nimi.SelectedItem;
        }

    }

    private void alkupvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void loppupvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void henkilomaara_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((Alue)alue_nimi.SelectedItem != null)
        {            
            int henkilo = henkilomaara.SelectedIndex + 1;
            var filteredMokit = mokkiViewmodel.Mokkis.Where(v => v.Henkilomaara == henkilo && v.Alue.AlueId == selectedAlue.AlueId).ToList();
            mokki_lista.ItemsSource = filteredMokit;
        }

    }

    private void mokki_lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }
    private void palvelu_lkm_TextChanged(object sender, TextChangedEventArgs e)
    {
        Entry entry = (Entry)sender;
        funktiot.CheckEntryInteger(entry, this); // funktiossa tarkistetaan ettei syote sisalla tekstia
    }

    private void uusi_asiakas_Clicked(object sender, EventArgs e)
    { // Tässä pelkästään navigointi uudelle sivulle 
        // muut toiminnallisuudet puuttuvat vielä siirtyykö tieto mukana?
        Navigation.PushAsync(new Uusi_asiakas(this)); // tarvitsee tarkistukset tietojen oikeellisuudelle
    }

    private void vanha_asiakas_Clicked(object sender, EventArgs e)
    { // Tässä pelkästään navigointi uudelle sivulle 
        // muut toiminnallisuudet puuttuvat vielä siirtyykö tieto mukana?
        Navigation.PushAsync(new Vanha_asiakas(this)); // tarvitsee tarkistukset tietojen oikeellisuudelle

    }

}