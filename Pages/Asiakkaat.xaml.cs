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
        await DisplayAlert("Tallennettu!", "", "OK!");
    }

    private void tyhjenna_Clicked(object sender, EventArgs e)
    {

    }

    private void poista_Clicked(object sender, EventArgs e)
    {

    }

    private void hae_sukunimella_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }
}