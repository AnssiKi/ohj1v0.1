namespace ohj1v0._1;

public partial class Mokit : ContentPage
{
	public Mokit()
	{
		InitializeComponent();
	}

    private void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void mokki_nimi_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void mokki_katuosoite_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void mokki_postinumero_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void mokki_hinta_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void mokki_kuvaus_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void mokki_varustelu_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void mokki_henkilomaara_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private async void tallenna_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Tallennettu!", "", "OK!"); //t�ss� vaiheessa pelkk� alertti!! Tarvii toiminnallisuuden viel�
    }

    private async void tyhjenna_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Haluatko varmasti tyhjent�� lomakkeen tiedot ?", "", "Kyll�!");// T�ss� vaiheessa pelkk� alertti, toiminnallisuus puuttuu viel� !!
        //Tarviiko t�h�n jonku eri alertin k�ytt��n et jos haluaa perua painamisen? 

    }

    private async void poista_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Haluatko varmasti poistaa tiedon ?", "", "Kyll�, haluan poistaa tiedon!");// T�ss� vaiheessa pelkk� alertti, toiminnallisuus puuttuu viel� !!
        //Tarviiko t�h�n jonku eri alertin k�ytt��n et jos haluaa perua painamisen? 

    }

    private void hae_nimella_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }
}