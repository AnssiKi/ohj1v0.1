namespace ohj1v0._1;

public partial class TeeUusiVaraus : ContentPage
{
	public TeeUusiVaraus()
	{
		InitializeComponent();
	}

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

    }

    private void uusi_asiakas_Clicked(object sender, EventArgs e)
    { // Tässä pelkästään navigointi uudelle sivulle 
        // muut toiminnallisuudet puuttuvat vielä siirtyykö tieto mukana?
        Navigation.PushAsync(new Uusi_asiakas(this));
    }

    private void vanha_asiakas_Clicked(object sender, EventArgs e)
    {

    }

}