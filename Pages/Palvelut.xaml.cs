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
        await DisplayAlert("Tallennettu!", "", "OK!");
    }

    private void tyhjenna_Clicked(object sender, EventArgs e)
    {

    }

    private void poista_Clicked(object sender, EventArgs e)
    {

    }

    private void hae_nimella_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }
}