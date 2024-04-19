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
        await DisplayAlert("Tallennettu!", "", "OK!"); //t‰ss‰ vaiheessa pelkk‰ alertti!! Tarvii toiminnallisuuden viel‰
    }

    private async void tyhjenna_Clicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti tyhjent‰‰ lomakkeen tiedot?", "Kyll‰", "Ei");

        // Jos k‰ytt‰j‰ valitsee "Kyll‰", toteutetaan peruutustoimet
        if (result)
        {
            //TYHJENNETƒƒN tiedot t‰h‰n
        }
        else
        {
            // Jos k‰ytt‰j‰ valitsee "Ei", peruutetaan toiminto
            // T‰h‰n ei oo pakko laittaa mit‰‰ kerta se ei haluakkaa.
        }
    }

    private async void poista_Clicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti poistaa tiedon?", "Kyll‰", "Ei");

        // Jos k‰ytt‰j‰ valitsee "Kyll‰", toteutetaan peruutustoimet
        if (result)
        {
            //poistetaan tiedot t‰h‰n
        }
        else
        {
            // Jos k‰ytt‰j‰ valitsee "Ei", peruutetaan toiminto
            // T‰h‰n ei oo pakko laittaa mit‰‰ kerta se ei haluakkaa poistaa.
        }
    }

    private void hae_sukunimella_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }
}