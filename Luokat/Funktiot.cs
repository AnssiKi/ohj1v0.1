using iText.Layout.Element;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ohj1v0._1.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ohj1v0._1.Luokat
{
    internal class Funktiot
    {
        private async void DisplayAlertOnPage(ContentPage page, string title, string message, string buttonLabel)
        {
            await page.DisplayAlert(title, message, buttonLabel);
        }

        public void CheckEntryPituus(Entry entry, int maxLength, ContentPage currentPage)
        { // tarkistetaan ettei entryn syote ole liian pitka
            try
            {
                if (entry.Text.Length >= maxLength)
                {
                    DisplayAlertOnPage(currentPage, "Virhe", $"Syöte voi olla enintään {maxLength} merkkiä pitkä.", "OK");
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        public bool CheckEntryDouble(Entry entry, ContentPage currentPage)
        { // Tarkistetaan, että entryn syöte on double-muotoista ja vahintaan 0

            if (!string.IsNullOrWhiteSpace(entry.Text))
            {
                if (!IsDouble(entry.Text))
                {
                    DisplayAlertOnPage(currentPage, "Virhe", "Syöteessä ei voi olla tekstiä.", "OK");
                    return false; // syote ei ole double
                }
                else
                {
                    double value = Convert.ToDouble(entry.Text);
                    if (value < 0)
                    {
                        DisplayAlertOnPage(currentPage, "Virhe", "Syötteen on oltava ei-negatiivinen.", "OK");
                        return false; // syote negatiivinen luku
                    }
                }
                return true; // kaikki oikein
            }
            return false; // syote tyhja
        }

        private bool IsDouble(string input)
        {
            return double.TryParse(input, out _);
        }


        public bool CheckEntryInteger(Entry entry, ContentPage currentPage)
        { // Tarkistetaan, että entryn syöte on int-muotoista ja vahintaan 0

            if (!string.IsNullOrWhiteSpace(entry.Text))
            {
                if (!IsInteger(entry.Text))
                {
                    DisplayAlertOnPage(currentPage, "Virhe", "Syöteessä ei voi olla tekstiä.", "OK");
                    return false; // syote ei ole int
                }
                else
                {
                    int value = Convert.ToInt32(entry.Text);
                    if (value < 0)
                    {
                        DisplayAlertOnPage(currentPage, "Virhe", "Syötteen on positiivinen kokonaisluku.", "OK");
                        return false; // syote on negatiivinen
                    }
                    return true; // kaikki oikein
                }
            }
            return false; // syote tyhja
        }

        private bool IsInteger(string input)
        {
            return int.TryParse(input, out _);
        }

        public bool CheckEntryText(Entry entry, ContentPage currentPage)
        { // tarkistetaan etta syotteessa ei ole numeroita
            if (ContainsNumbers(entry.Text))
            {
                DisplayAlertOnPage(currentPage, "Virhe", $"Syötteessä ei voi olla numeroita.", "OK");
                return false;
            }
            return true;
        }

        private bool ContainsNumbers(string input)
        {
            return input.Any(char.IsDigit);
        }

        public bool CheckTupla(ContentPage currentPage, Entry entry, ListView lista, Type luokka, string selite, string propertyName)
        { // tarkistetaan onko saman nimisia tietoja jo tietokannassa esim samanniminen alue

            foreach (var item in lista.ItemsSource)
            {
                if (item.GetType() == luokka)
                {
                    var vertailu = (dynamic)item;
                    var vertailtavaArvo = vertailu.GetType().GetProperty(propertyName).GetValue(vertailu, null);
                    if (vertailtavaArvo.ToString() == entry.Text)
                    {
                        DisplayAlertOnPage(currentPage, "Virhe", string.Format("Saman niminen {0} jo olemassa", selite), "OK");
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CheckInput(ContentPage currentPage, Grid grid)
        {
            DateTime? alkuPvm = null;
            DateTime? loppuPvm = null;

            foreach (var child in grid.Children)
            {
                if (child is Entry entry && string.IsNullOrWhiteSpace(entry.Text))
                {
                    DisplayAlertOnPage(currentPage, "Virhe", "Kaikki kentät eivät ole täytetty", "OK");
                    return false; // Palautetaan false, jos löytyy tyhja entry                            
                }

                if (child is Picker picker && picker.SelectedIndex == -1)
                {
                    DisplayAlertOnPage(currentPage, "Virhe", "Kaikkiin valintatyökaluihin täytyy valita", "OK");
                    return false; // Palautetaan false, jos Pickerissä ei ole valittu mitaan
                }

                if (child is DatePicker datePicker)
                {
                    if (datePicker.FindByName("alkupvm") != null)
                    {
                        var alkuDatePicker = datePicker.FindByName("alkupvm") as DatePicker;
                        if (alkuDatePicker != null)
                        {
                            alkuPvm = alkuDatePicker.Date;
                        }
                    }
                    if (datePicker.FindByName("loppupvm") != null)
                    {
                        var loppuDatePicker = datePicker.FindByName("loppupvm") as DatePicker;
                        if (loppuDatePicker != null)
                        {
                            loppuPvm = loppuDatePicker.Date;
                        }
                    }
                    if (alkuPvm.Value > loppuPvm.Value)
                    {
                        DisplayAlertOnPage(currentPage, "Virhe", "Aloituspäivämäärä ei voi olla lopetuspäivämäärän jälkeen", "OK");
                        return false; // Palautetaan false, jos alkuPvm on suurempi kuin loppuPvm
                    }
                }
            }
            return true; // Palautetaan true, jos testit lapi
        }

        public void TyhjennaEntryt(Grid grid, ListView lista)
        {
            foreach (var child in grid.Children)
            {
                if (child is Entry entry)
                {
                    entry.Text = ""; // Tyhjentää entryn
                }

                else if (child is Picker picker)
                {
                    picker.SelectedIndex = -1; // Tyhjentää pickerin valinnan
                }
                else if (child is DatePicker datePicker)
                {
                    datePicker.Date = DateTime.Today; // Asettaa datepickerin valinnan kuluvaksi paivaksi
                }
                else if (child is Label label && label == label.FindByName<Label>("id"))
                {
                    label.Text = ""; // Tyhjentää labelin, jos sen x:Name on "id"
                }
            }

            if (lista.SelectedItem != null)
            {
                lista.SelectedItem = null; // poistaa listview valinnan
            }
        }
        public async Task TyhjennaVarauksenTiedotAsync(VarauksenTiedot varauksenTiedot)
        {
            // Alustettaan VarauksenTiedot-olio käytettäväksi uudelleen
            varauksenTiedot = new VarauksenTiedot
            {
                ValittuMokki = null,
                ValittuAlue = null,
                VarattuAlkupvm = DateTime.MinValue,
                VarattuLoppupvm = DateTime.MinValue,
                Varattupvm = DateTime.MinValue,
                Vahvistuspvm = DateTime.MinValue,
                VarauksenPalveluts = new List<VarauksenPalvelut>()
            };
        }

       

    }
}
