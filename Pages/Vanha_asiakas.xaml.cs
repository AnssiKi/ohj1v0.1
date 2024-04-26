using Microsoft.Maui.ApplicationModel.Communication;
using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;

namespace ohj1v0._1;

public partial class Vanha_asiakas : ContentPage
{
	public Vanha_asiakas(TeeUusiVaraus tuv)
	{
		InitializeComponent();
        BindingContext = asiakasviewmodel;
    }
    Funktiot funktiot = new Funktiot();
    AsiakasViewmodel asiakasviewmodel = new AsiakasViewmodel();
    bool valittu = false;

    private void hae_sukunimella_TextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            lista.ItemsSource = asiakasviewmodel.Asiakas; // näytetään kaikki asiakkaat, jos ei mitään hakukentässä
        }
        else
        {
            // filtteröidään ListView sisältö hakukentän mukaan
            var filteredAsiakkaat = asiakasviewmodel.Asiakas.Where(a => a.Sukunimi.ToLower().Contains(searchText.ToLower())).ToList();
            lista.ItemsSource = filteredAsiakkaat;
        }
    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item == null)
        {
            return;
        }
        else
        {
            valittu = true;
        }

    }

    private async void tallenna_Clicked(object sender, EventArgs e)
    { 
        if (valittu) // listasta on valittu asiakas
        {
            try
            {
                // CRUD - toiminnot
                await DisplayAlert("Tallennettu", "", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
            }
        }
    }
}