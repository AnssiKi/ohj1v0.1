using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;
using Pomelo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace ohj1v0._1;

public partial class Alueet : ContentPage
{
    public Alueet()
	{
		InitializeComponent();
        //BindingContext = new AlueViewmodel();
        lista.BindingContext = alueViewmodel;
    }
    Funktiot funktiot = new Funktiot();
    AlueViewmodel alueViewmodel = new AlueViewmodel();
    Alue SelectedAlue;


    private async void tallenna_Clicked(object sender, EventArgs e)
    {
        Type luokka = typeof(Alue);
        string selite = "alue";
        Entry entry = alue_nimi;        
        Grid grid = (Grid)entry_grid;
        string vertailu = "Nimi";

        if (!funktiot.CheckInput(this, grid)) // Tarkistetaan onko kaikissa entryissa ja pickereissa sisaltoa
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else if(!funktiot.CheckEntryText(entry, this)) // tarkistetaan onko alueen nimessa numeroita
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else if (!funktiot.CheckTupla(this, entry, lista, luokka, selite, vertailu)) // varmistetaan ettei ole samannimista aluetta
        {
            // tahan esim entryn background varin vaihtamista tai focus suoraan kyseiseen entryyn
        }

        else // Tarkistukset lapi voidaan tallentaa
        {
            try
            {
                // Lis�t��n alue tietokantaan ja p�ivitet��n listan�kym�
                using (var dbContext = new VnContext()) 
                {
                    var alue = new Alue()
                    {
                        Nimi = alue_nimi.Text
                        // AlueId p�ivittyy automaattisesti tietokannassa
                    };

                    dbContext.Alues.Add(alue);  
                    dbContext.SaveChanges();
                    await alueViewmodel.LoadAluesFromDatabaseAsync();

                }
                    await DisplayAlert("", "Tallennettu", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", $"Tallennuksessa tapahtui virhe: {ex.Message}", "OK");
            }
        }
    }

    private async void tyhjenna_Clicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti tyhjent�� lomakkeen tiedot?", "Kyll�", "Ei");

        // Jos k�ytt�j� valitsee "Kyll�", toteutetaan peruutustoimet
        if (result)
        {
            Grid grid = (Grid)entry_grid;
            ListView list = (ListView)lista;
            funktiot.TyhjennaEntryt(grid, list);
        }
        else
        {
            // Jos k�ytt�j� valitsee "Ei", peruutetaan toiminto
            // T�h�n ei oo pakko laittaa mit�� kerta se ei haluakkaa.
        }

    }
    void lista_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        SelectedAlue = (Alue)e.SelectedItem;
    }
    private async void poista_Clicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti poistaa tiedon?", "Kyll�", "Ei");
        // Jos k�ytt�j� valitsee "Kyll�", toteutetaan peruutustoimet
        if (result)
        {
            try
            {
                // Poistetaan alue tietokannasta ja p�ivitet��n listan�kym�
                using (var dbContext = new VnContext())
                {
                    dbContext.Alues.Remove(SelectedAlue);
                    dbContext.SaveChanges();
                    await alueViewmodel.LoadAluesFromDatabaseAsync();
                }
                await DisplayAlert("", "Poisto onnistui", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", $"Poistossa tapahtui virhe: {ex.Message}", "OK");
            }

        }
        else
        {
            // Jos k�ytt�j� valitsee "Ei", peruutetaan toiminto
            // T�h�n ei oo pakko laittaa mit�� kerta se ei haluakkaa poistaa.
        }
    }

    private void lista_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item == null)
        {
            return;
        }

        var selectedAlue = (Alue)e.Item;
        alue_nimi.Text = selectedAlue.Nimi;

    }
    
    private void alue_nimi_TextChanged(object sender, TextChangedEventArgs e)
    {// entryn pituus rajoitettu xaml.cs max 40 merkkiin
        Entry entry = (Entry)sender; 
        funktiot.CheckEntryPituus(entry, 40, this); // funktiossa ilmoitetaan jos kayttajan syote liian pitka
        funktiot.CheckEntryText(entry, this); // tarkistetaan ettei syottessa ole numeroita
    }

    private void Hae_alue_nimi_TextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            lista.ItemsSource = alueViewmodel.Alues; // naytetaan kaikki alueet jos ei mitaan hakukentassa
        }
        else
        {
            // filtteroidaan listview sisalto hakukentan mukaan
            var filteredAlues = alueViewmodel.Alues.Where(m => m.Nimi.ToLower().Contains(searchText.ToLower())).ToList();
            lista.ItemsSource = filteredAlues;
        }
    }

 
}