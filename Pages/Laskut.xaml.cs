using ohj1v0._1.Luokat;
using ohj1v0._1.Models;
using ohj1v0._1.Viewmodels;

namespace ohj1v0._1;

public partial class Laskut : ContentPage
{
    readonly LaskuViewmodel laskuViewmodel = new LaskuViewmodel();

    Lasku selectedLasku;
    VnContext context;
    Funktiot funktiot;

    bool isUserCheckChange = true; //pit‰‰ kirjaa siit‰ onko checkboxiin tehty muutos k‰ytt‰j‰- vai ohjelmaper‰inen
    
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
    private async void maksettu_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!isUserCheckChange) return;
        if (selectedLasku != null && selectedLasku.Maksettu == 0) 
        {
            selectedLasku.Maksettu = 1;
            context.Laskus.Update(selectedLasku);
            context.SaveChanges();
            OnPropertyChanged(nameof(selectedLasku));
            await laskuViewmodel.LoadLaskutFromDatabaseAsync();
            selectedLasku = null;
        }
        else 
        {
            await DisplayAlert("Virhe", "Valitse ensin lasku", "OK");
            return; }

    }

    private void tulosta_Clicked(object sender, EventArgs e)
    {

    }

    private async void tyhjenna_Clicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti tyhjent‰‰ lomakkeen tiedot?", "Kyll‰", "Ei");

        // Jos k‰ytt‰j‰ valitsee "Kyll‰", toteutetaan peruutustoimet
        if (result)
        {
            Grid grid = (Grid)entry_grid;
            ListView list = (ListView)lista;
            funktiot.TyhjennaEntryt(grid, list);
            Label_laskuID.Text = "";
        }
        return;
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
        isUserCheckChange = false;
        if (selectedLasku.Maksettu == 0) 
        { 
            maksettu.IsChecked = false; 
        }
        else 
        { 
            maksettu.IsChecked = true;
        }
        isUserCheckChange = true;
    }
}