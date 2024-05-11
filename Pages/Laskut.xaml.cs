using ohj1v0._1.Viewmodels;

namespace ohj1v0._1;

public partial class Laskut : ContentPage
{
	public Laskut()
	{
	    InitializeComponent();
        BindingContext = new LaskuViewmodel();
	}

    private void maksettu_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {

    }

    private void tulosta_Clicked(object sender, EventArgs e)
    {

    }

    private async void tyhjenna_Clicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti tyhjentää lomakkeen tiedot?", "Kyllä", "Ei");

        // Jos käyttäjä valitsee "Kyllä", toteutetaan peruutustoimet
        if (result)
        {
            //TYHJENNETÄÄN tiedot tähän
        }
        else
        {
            // Jos käyttäjä valitsee "Ei", peruutetaan toiminto
            // Tähän ei oo pakko laittaa mitää kerta se ei haluakkaa.
        }
    }

    private void hakupvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }

}