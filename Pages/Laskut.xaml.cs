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
        await DisplayAlert("Haluatko varmasti tyhjent�� lomakkeen tiedot ?", "", "Kyll�!");// T�ss� vaiheessa pelkk� alertti, toiminnallisuus puuttuu viel� !!
        //Tarviiko t�h�n jonku eri alertin k�ytt��n et jos haluaa perua painamisen? 
    }

    private void hakupvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {

    }

}