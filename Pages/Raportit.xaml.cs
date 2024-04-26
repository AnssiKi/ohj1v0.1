namespace ohj1v0._1;
using ohj1v0._1.Luokat;
using ohj1v0._1.Viewmodels;
using ohj1v0._1.Models;

public partial class Raportit : ContentPage
{
    public Raportit()
    {
        InitializeComponent();
        raportti = new List<Raportti>();
        alue_nimi.BindingContext = alueViewmodel;
    }

    AlueViewmodel alueViewmodel = new AlueViewmodel();
    private List<Raportti> raportti;

    private void hae_Clicked(object sender, EventArgs e)
    {
        string selectedRaportti = raportti_valinta.SelectedItem?.ToString();
        Alue selectedAlue = (Alue)alue_nimi.SelectedItem;
        DateTime startDate = alkupvm.Date;
        DateTime endDate = loppupvm.Date;
        string Raporttityyppi;

        if (selectedRaportti == "Majoitus")
        {
            Raporttityyppi = "Majoitus";
        }
        else if (selectedRaportti == "Palvelut")
        {
            Raporttityyppi = "Palvelut";
        }
        else
        {
            return; // ei tehda mitaan jos raportin tyyppia ei ole valittu
        }

        if (selectedAlue == null)
        {
            return; // ei tehda mitaan jos aluetta ei ole valittu
        }

        Raportti newRaportti = new Raportti
        {
            Raporttityyppi = Raporttityyppi,
            Alue = selectedAlue.Nimi,
            Alkupvm = startDate,
            Loppupvm = endDate
            // Yhteensa = CRUD haku tietokannasta valituilla rajauksilla            
        };

        raportti.Add(newRaportti);
        lista.ItemsSource = null;
        lista.ItemsSource = raportti;
    }

    
    private void alkupvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void loppupvm_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void alue_nimi_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}