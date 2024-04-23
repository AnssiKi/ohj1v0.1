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
    {
        Grid grid = (Grid)entry_grid;
        funktiot.Tallenna(this, grid);
    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }

}