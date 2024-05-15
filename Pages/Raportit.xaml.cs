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
        int yhteensa;

        if (selectedRaportti == "Majoitus")
        {
            Raporttityyppi = "Majoitus";

            using (var context = new VnContext()) // haetaan majoitukset tietokannasta 
            {
                yhteensa = context.Varaus
                            .Where(v => v.Mokki.AlueId == selectedAlue.AlueId &&
                                        v.VarattuAlkupvm >= startDate &&
                                        v.VarattuLoppupvm <= endDate)
                            .Count();
            }
        }
        else if (selectedRaportti == "Palvelut")
        {
            Raporttityyppi = "Palvelut";

            using (var context = new VnContext())
            {
                yhteensa = context.VarauksenPalveluts
                            .Where(vp => vp.Palvelu.AlueId == selectedAlue.AlueId &&
                                          vp.Varaus.VarattuAlkupvm >= startDate &&
                                          vp.Varaus.VarattuLoppupvm <= endDate)
                            .Sum(vp => vp.Lkm);
            }
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
            Loppupvm = endDate,
            Yhteensa = yhteensa,       
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

    private void tyhjenna_Clicked(object sender, EventArgs e)
    {
        raportti.Clear();
        lista.ItemsSource = null;
        lista.ItemsSource = raportti;
    }
}