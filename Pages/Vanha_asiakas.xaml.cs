namespace ohj1v0._1;

public partial class Vanha_asiakas : ContentPage
{
	public Vanha_asiakas(TeeUusiVaraus tuv)
	{
		InitializeComponent();
	}

    private void hae_sukunimella_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private async void tallenna_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Tallennettu!", "", "OK!"); //tässä vaiheessa pelkkä alertti!! Tarvii toiminnallisuuden vielä

    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }

}