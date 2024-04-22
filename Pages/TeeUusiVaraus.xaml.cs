using ohj1v0._1;
using ohj1v0._1.Luokat;

namespace ohj1v0._1;

public partial class TeeUusiVaraus : ContentPage
{
	public TeeUusiVaraus()
	{
		InitializeComponent();
	}
    Funktiot funktiot = new Funktiot();

    private void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void alkupvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void loppupvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void henkilomaara_SelectedIndexChanged(object sender, EventArgs e)
    {

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
        Navigation.PushAsync(new Uusi_asiakas(this));
    }

    private void vanha_asiakas_Clicked(object sender, EventArgs e)
    { // Tässä pelkästään navigointi uudelle sivulle 
        // muut toiminnallisuudet puuttuvat vielä siirtyykö tieto mukana?
        Navigation.PushAsync(new Vanha_asiakas(this));

    }

}