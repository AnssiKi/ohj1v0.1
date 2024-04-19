namespace ohj1v0._1;

public partial class Asiakkaat : ContentPage
{
	public Asiakkaat()
	{
		InitializeComponent();
	}
    private void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void etunimi_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void sukunimi_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void lahiosoite_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void postinumero_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void email_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private async void tallenna_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Tallennettu!", "", "OK!"); //tässä vaiheessa pelkkä alertti!! Tarvii toiminnallisuuden vielä
    }

    private void tyhjenna_Clicked(object sender, EventArgs e)
    {

    }

    private async void poista_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Haluatko varmasti poistaa asiakkaan ?", "", "Kyllä, haluan poistaa asiakkaan!");// Tässä vaiheessa pelkkä alertti, toiminnallisuus puuttuu vielä !!
        //Tarviiko tähän jonku eri alertin käyttöön et jos haluaa perua painamisen? 
    }

    private void hae_sukunimella_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }
}