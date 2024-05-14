using CommunityToolkit.Maui.Storage;
using Microsoft.Maui.Controls;
using ohj1v0._1.Luokat;
using ohj1v0._1.Models;
using ohj1v0._1.Viewmodels;
using Org.BouncyCastle.Bcpg;
using iTextKernel = iText.Kernel.Pdf;
using iTextLayout = iText.Layout;
using iTextLOElement = iText.Layout.Element;
using iTextLOP = iText.Layout.Properties;

namespace ohj1v0._1;

public partial class Laskut : ContentPage
{
    readonly LaskuViewmodel laskuViewmodel = new LaskuViewmodel();

    Lasku selectedLasku;
    VnContext context = new VnContext();
    Funktiot funktiot;

    bool isUserCheckChange = true; //pit�� kirjaa siit� onko checkboxiin tehty muutos k�ytt�j�- vai ohjelmaper�inen
    
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

    private async void tulosta_Clicked(object sender, EventArgs e)
    {
        string maksuninfo;
        if (selectedLasku == null)
        {
            DisplayAlert("Virhe", "Valitse ensin varaus josta haluat muodostaa laskun", "OK");
            return;
        }
        //Tehd��n PDF:
        using var memoryStream = new MemoryStream();
        iTextKernel.PdfWriter writer = new iTextKernel.PdfWriter(memoryStream);
        iTextKernel.PdfDocument pdf = new iTextKernel.PdfDocument(writer);
        iTextLayout.Document document = new iTextLayout.Document(pdf);

        iTextLOElement.Paragraph header = new iTextLOElement.Paragraph("Village Newbies OY - Lasku")
            .SetTextAlignment(iTextLOP.TextAlignment.CENTER)
            .SetFontSize(22);
        document.Add(header);

        var varausInfo = new iTextLOElement.Paragraph($"Varaus ID: {selectedLasku.VarausId}\n" +
            $"Laskun numero: {selectedLasku.LaskuId}\n" +       
            $"Hinta: {selectedLasku.Summa}�\n" +
            $"Palvelut:")
            .SetTextAlignment(iTextLOP.TextAlignment.LEFT)
           .SetFontSize(12);
        document.Add(varausInfo);
        
        if (selectedLasku.Maksettu == 0)
        {
            maksuninfo = "Laskua ei ole maksettu.";
        }
        else 
        {
            maksuninfo = "Lasku on maksettu.";

        }
        var statusInfo = new iTextLOElement.Paragraph(maksuninfo)
            .SetTextAlignment(iTextLOP.TextAlignment.LEFT)
           .SetFontSize(12);
        document.Add(statusInfo);

        var loppusummaInfo = new iTextLOElement.Paragraph($"Laskun loppusumma: {selectedLasku.Summa}�\n" +
            $"Verot: {selectedLasku.Alv}�")
            .SetTextAlignment(iTextLOP.TextAlignment.LEFT)
            .SetFontSize(12);
        document.Add(loppusummaInfo);

        iTextLOElement.Paragraph maksuInfo = new iTextLOElement.Paragraph($"Saajan tilinumero:\n" +
        $"FI12 3456 7890 1234 56\n" +
        $"Pankkiyhteys: HVKVG\n" +
        $"Viite: " +
        selectedLasku.VarausId+"\n"+
        $"Lasku on tulostettu Lasku -taulusta")

        .SetTextAlignment(iTextLOP.TextAlignment.LEFT)
        .SetFontSize(12);
        document.Add(maksuInfo);
        document.Close();

        byte[] pdfData = memoryStream.ToArray();
        using var stream = new MemoryStream(pdfData);

        var fileSaveResult = await FileSaver.Default.SaveAsync("sample.pdf", "Lasku_varaus_" +
            selectedLasku.VarausId.ToString() + ".pdf", stream);

        if (fileSaveResult.IsSuccessful)
        {
            await DisplayAlert("Tallennettu", $"Tiedosto tallennettu sijaintiin: {fileSaveResult.FilePath}", "OK");
        }
        else
        {
            await DisplayAlert("Virhe", $"Tiedoston tallentaminen ei onnistunut: {fileSaveResult.Exception.Message}", "OK");
        }


    }

    async void tyhjenna_Clicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Vahvistus", "Haluatko varmasti tyhjent�� lomakkeen tiedot?", "Kyll�", "Ei");

        // Jos k�ytt�j� valitsee "Kyll�", toteutetaan peruutustoimet
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