using ohj1v0._1.Models;
using ohj1v0._1.Viewmodels;

namespace ohj1v0._1;

public partial class Laskut : ContentPage
{
    readonly LaskuViewmodel laskuViewmodel = new LaskuViewmodel();
    Lasku selectedLasku;
    VnContext context = new VnContext();
    public Laskut()
	{
	    InitializeComponent();
        BindingContext = laskuViewmodel;
        maksettu.IsEnabled = false;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await laskuViewmodel.LoadLaskutFromDatabaseAsync();
    }
    private void maksettu_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (selectedLasku != null && selectedLasku.Maksettu == 0) 
        {
            selectedLasku.Maksettu = 1;
            context.Laskus.Update(selectedLasku);
            context.SaveChanges();
            OnPropertyChanged(nameof(selectedLasku));
            laskuViewmodel.LoadLaskutFromDatabaseAsync();
            selectedLasku = null;
        }
        else 
        {
            DisplayAlert("Virhe", "Valitse ensin lasku", "OK");
            return; }

    }

    private void tulosta_Clicked(object sender, EventArgs e)
    {

    }

    private async void tyhjenna_Clicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti tyhjent�� lomakkeen tiedot?", "Kyll�", "Ei");

        // Jos k�ytt�j� valitsee "Kyll�", toteutetaan peruutustoimet
        if (result)
        {
            //TYHJENNET��N tiedot t�h�n
        }
        else
        {
            // Jos k�ytt�j� valitsee "Ei", peruutetaan toiminto
            // T�h�n ei oo pakko laittaa mit�� kerta se ei haluakkaa.
        }
    }

    private void hakupvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        selectedLasku = (Lasku)e.Item;
        maksettu.IsEnabled = true;
        laskuID.Text = selectedLasku.LaskuId.ToString();
        varausID.Text = selectedLasku.VarausId.ToString();  
        summa.Text = selectedLasku.Summa.ToString();
        alv.Text = selectedLasku.Alv.ToString();    
        if (selectedLasku.Maksettu == 0) 
        { 
            maksettu.IsChecked = false; 
        }
        else 
        { 
            maksettu.IsChecked = true;
        }
    }
}