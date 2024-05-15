using CommunityToolkit.Maui.Storage;
using Microsoft.EntityFrameworkCore;
using ohj1v0._1.Luokat;
using ohj1v0._1.Models;
using ohj1v0._1.Viewmodels;
using iTextKernel = iText.Kernel.Pdf;
using iTextLayout = iText.Layout;
using iTextLOElement = iText.Layout.Element;
using iTextLOP = iText.Layout.Properties;

namespace ohj1v0._1;

public partial class Laskut : ContentPage
{
    LaskuViewmodel laskuViewmodel = new LaskuViewmodel();
    VnContext context = new VnContext();
    Lasku selectedLasku;
    
    Funktiot funktiot = new Funktiot();

    bool isUserCheckChange = true; //pit‰‰ kirjaa siit‰ onko checkboxiin tehty muutos k‰ytt‰j‰- vai ohjelmaper‰inen
    
    public Laskut()
	{
	    InitializeComponent();
        
        BindingContext = laskuViewmodel;
        maksettu.IsEnabled = false;
        this.Appearing += OnPageAppearing;
	}
    private void OnPageAppearing(object sender, EventArgs e)
    {
        BindingContext = new LaskuViewmodel();
    }

    private async void maksettu_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!isUserCheckChange) return;
        if (selectedLasku != null && selectedLasku.Maksettu == 0) 
        {
            selectedLasku.Maksettu = 1;
            context.Laskus.Update(selectedLasku);
            context.SaveChanges();
            OnPropertyChanged(nameof(selectedLasku.Maksettu));
            BindingContext = new LaskuViewmodel();  
            selectedLasku = null;
            await DisplayAlert("Tallennus", "Lasku merkitty maksetuksi", "OK");
        }
        else 
        {
            await DisplayAlert("Virhe", "Valitse ensin lasku", "OK");
            return; 
        }
        Grid grid = (Grid)entry_grid;
        ListView list = (ListView)lista;
        funktiot.TyhjennaEntryt(grid, list);
        Label_laskuID.Text = "";
        isUserCheckChange = false;
        maksettu.IsChecked = false;
    }

    private async void tulosta_Clicked(object sender, EventArgs e)
    {
        Asiaka laskunAsiakas = HaeLaskunAsiakas(selectedLasku);
        List<Palvelu> laskunPalvelut = HaeLaskunPalvelut(selectedLasku);
        string maksuninfo;
        if (selectedLasku == null)
        {
            DisplayAlert("Virhe", "Valitse ensin varaus josta haluat muodostaa laskun", "OK");
            return;
        }
        //Tehd‰‰n PDF:
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
            $"Asiakas: {laskunAsiakas.Etunimi} {laskunAsiakas.Sukunimi}\n"+
            $"Hinta: {selectedLasku.Summa}Ä\n" +
            $"Palvelut:")
            .SetTextAlignment(iTextLOP.TextAlignment.LEFT)
           .SetFontSize(12);
        document.Add(varausInfo);

        if (laskunPalvelut.Any()) 
        {
            foreach (var lp in laskunPalvelut) 
            {
                var palvelutInfo = new iTextLOElement.Paragraph($"{lp.Nimi}\n"+
                    $"Hinta sis. {lp.Alv}% alv: \n"+
                    $"{lp.HintaAlv}Ä\n"+
                    $"_______________")
                    .SetTextAlignment(iTextLOP.TextAlignment.LEFT)
                    .SetFontSize (12);
                document.Add(palvelutInfo);
            }
        }
        else 
        {
            var palvelutInfo = new iTextLOElement.Paragraph($"Muistattehan varata ensi kerralla myˆs lis‰palvelut!")
                    .SetTextAlignment(iTextLOP.TextAlignment.LEFT)
                    .SetFontSize(12);
            document.Add(palvelutInfo);

        }

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

        var loppusummaInfo = new iTextLOElement.Paragraph($"Laskun loppusumma: {selectedLasku.Summa}Ä\n" +
            $"Verot: {selectedLasku.Alv}Ä")
            .SetTextAlignment(iTextLOP.TextAlignment.LEFT)
            .SetFontSize(12);
        document.Add(loppusummaInfo);

        iTextLOElement.Paragraph maksuInfo = new iTextLOElement.Paragraph($"Saajan tilinumero:\n" +
        $"FI12 3456 7890 1234 56\n" +
        $"Pankkiyhteys: HVKVG\n" +
        $"Viite: \n" +
        selectedLasku.VarausId+"\n"+
        $"Laskun p‰iv‰m‰‰r‰: " + DateTime.Now.ToString("d") +
        $"\nLaskun er‰p‰iv‰: " + DateTime.Now.AddDays(14).ToString("d"))

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

    public Asiaka HaeLaskunAsiakas(Lasku selectedLasku) 
    {
        Varau varaus = context.Varaus.FirstOrDefault(v=>v.VarausId == selectedLasku.VarausId);
        if (varaus == null) 
        {
            return null;
        }
        Asiaka asiakas = context.Asiakas.FirstOrDefault(a => a.AsiakasId == varaus.AsiakasId);
        return asiakas;
    }
    public List<Palvelu> HaeLaskunPalvelut(Lasku selectedLasku)
    {
        Varau varau = context.Varaus.Include(v => v.VarauksenPalveluts)
                                .FirstOrDefault(v => v.VarausId == selectedLasku.VarausId);

        // If no Varau was found, return null
        if (varau == null)
        {
            return null;
        }

        // Get the list of Palvelu IDs associated with the Varau
        List<uint> palveluIds = varau.VarauksenPalveluts.Select(vp => vp.PalveluId).ToList();

        // Get the list of Palvelu associated with the Varau
        List<Palvelu> palvelut = context.Palvelus
            .Where(p => palveluIds.Contains(p.PalveluId))
            .ToList();

        return palvelut;
    }
}