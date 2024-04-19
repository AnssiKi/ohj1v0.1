namespace ohj1v0._1;

public partial class Palvelut : ContentPage
{
	public Palvelut()
	{
		InitializeComponent();
	}

    private void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void palvelu_nimi_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void palvelu_kuvaus_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void palvelu_hinta_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void palvelu_alv_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    private async void tallenna_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Tallennettu!", "", "OK!"); //tässä vaiheessa pelkkä alertti!! Tarvii toiminnallisuuden vielä
    }

    private async void tyhjenna_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Haluatko varmasti tyhjentää lomakkeen tiedot ?", "", "Kyllä!");// Tässä vaiheessa pelkkä alertti, toiminnallisuus puuttuu vielä !!
        //Tarviiko tähän jonku eri alertin käyttöön et jos haluaa perua painamisen? 
    }

    private async void poista_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Haluatko varmasti poistaa tiedon ?", "", "Kyllä, haluan poistaa tiedon!");// Tässä vaiheessa pelkkä alertti, toiminnallisuus puuttuu vielä !!
        //Tarviiko tähän jonku eri alertin käyttöön et jos haluaa perua painamisen? 

    }

    private void hae_nimella_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }
}