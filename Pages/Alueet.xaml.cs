namespace ohj1v0._1;

public partial class Alueet : ContentPage
{
	public Alueet()
	{
		InitializeComponent();
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

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }
    
    private void alue_nimi_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 40 merkkiin

    }

    private void Hae_alue_nimi_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
}