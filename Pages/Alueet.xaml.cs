namespace ohj1v0._1;

public partial class Alueet : ContentPage
{
	public Alueet()
	{
		InitializeComponent();
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
        await DisplayAlert("Haluatko varmasti poistaa tiedon ?", "", "Kyllä, haluan poistaa tiedon!");// Tässä vaiheessa pelkkä alertti, toiminnallisuus puuttuu vielä !!
        //Tarviiko tähän jonku eri alertin käyttöön et jos haluaa perua painamisen? 
    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }



    private void alue_nimi_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void Hae_alue_nimi_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
}