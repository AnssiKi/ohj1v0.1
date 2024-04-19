namespace ohj1v0._1;

public partial class Alueet : ContentPage
{
	public Alueet()
	{
		InitializeComponent();
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