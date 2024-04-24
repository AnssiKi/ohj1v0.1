using Microsoft.Maui.ApplicationModel.Communication;
using ohj1v0._1.Luokat;

namespace ohj1v0._1;

public partial class Vanha_asiakas : ContentPage
{
	public Vanha_asiakas(TeeUusiVaraus tuv)
	{
		InitializeComponent();
	}
    Funktiot funktiot = new Funktiot();

    private void hae_sukunimella_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private async void tallenna_Clicked(object sender, EventArgs e)
    { // tarvitsee tarkistuksen onko listviesta valittu jotain tallennettavaksi

        if (true)
        {

        }

        else // Tarkistukset lapi voidaan tallentaa
        {
            try
            {
                // CRUD - toiminnot
                await DisplayAlert("Tallennettu", "", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
            }
        }
    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }

}