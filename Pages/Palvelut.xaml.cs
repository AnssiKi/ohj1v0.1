using ohj1v0._1.Luokat;

namespace ohj1v0._1;

public partial class Palvelut : ContentPage
{
	public Palvelut()
	{
		InitializeComponent();
	}
    Funktiot funktiot = new Funktiot();

    private void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void palvelu_nimi_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 40 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 40, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka

    }

    private void palvelu_kuvaus_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 255 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 255, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
    }

    private void palvelu_hinta_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 8 merkkiin
        Entry entry = (Entry)sender;
        funktiot.CheckEntryPituus(entry, 8, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
        funktiot.CheckEntryDouble(entry, this); // tarkistetaan ett‰ syote on double
    }

    private void palvelu_alv_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    private async void tallenna_Clicked(object sender, EventArgs e)
    {
        Grid grid = (Grid)entry_grid;
        funktiot.Tallenna(this, grid);        
    }

    private async void tyhjenna_Clicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti tyhjent‰‰ lomakkeen tiedot?", "Kyll‰", "Ei");

        // Jos k‰ytt‰j‰ valitsee "Kyll‰", toteutetaan peruutustoimet
        if (result)
        {
            Grid grid = (Grid)entry_grid;
            funktiot.TyhjennaEntryt(grid);
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

    private void hae_nimella_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }
}