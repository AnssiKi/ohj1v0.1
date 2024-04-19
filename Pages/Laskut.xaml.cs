namespace ohj1v0._1;

public partial class Laskut : ContentPage
{
	public Laskut()
	{
		InitializeComponent();
	}

    private void maksettu_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {

    }

    private void tulosta_Clicked(object sender, EventArgs e)
    {

    }

    private async void tyhjenna_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Haluatko varmasti tyhjentää lomakkeen tiedot ?", "", "Kyllä!");// Tässä vaiheessa pelkkä alertti, toiminnallisuus puuttuu vielä !!
        //Tarviiko tähän jonku eri alertin käyttöön et jos haluaa perua painamisen? 
    }

    private void hakupvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }

}