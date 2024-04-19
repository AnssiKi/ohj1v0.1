namespace ohj1v0._1;

public partial class Varaukset : ContentPage
{
	public Varaukset()
	{
		InitializeComponent();
	}

    private void alkupvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void loppupvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void vahvistuspvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void varauspvm_DateSelected(object sender, DateChangedEventArgs e)
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

    private void hae_varaukset_DateSelected(object sender, DateChangedEventArgs e)
    {

    }
}