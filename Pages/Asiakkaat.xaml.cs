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
    {// entryn pituus rajoitettu xaml.cs max 20 merkkiin

    }

    private void sukunimi_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 40 merkkiin

    }

    private void lahiosoite_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 40 merkkiin

    }

    private void postinumero_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 5 merkkiin

    }

    private void email_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 50 merkkiin

    }
    private void puhelinnumero_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 15 merkkiin

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
        await DisplayAlert("Haluatko varmasti poistaa asiakkaan ?", "", "Kyll�, haluan poistaa asiakkaan!");// T�ss� vaiheessa pelkk� alertti, toiminnallisuus puuttuu viel� !!
        //Tarviiko t�h�n jonku eri alertin k�ytt��n et jos haluaa perua painamisen? 
    }

    private void hae_sukunimella_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }


}