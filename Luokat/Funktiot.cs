using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (entry.Text.Length >= maxLength)
            {
                DisplayAlertOnPage(currentPage, "Virhe", $"Syöte voi olla enintään {maxLength} merkkiä pitkä.", "OK");

            }
        }

        public void CheckEntryDouble(Entry entry, ContentPage currentPage)
        { // Tarkistetaan, että entryn syöte on double-muotoista ja vahintaan 0

            if (!string.IsNullOrWhiteSpace(entry.Text))
            {
                if (!IsDouble(entry.Text))
                {
                    DisplayAlertOnPage(currentPage, "Virhe", "Syöteessä ei voi olla tekstiä.", "OK");
                }
                else
                {
                    double value = Convert.ToDouble(entry.Text);
                    if (value < 0)
                    {
                        DisplayAlertOnPage(currentPage, "Virhe", "Syötteen on oltava ei-negatiivinen.", "OK");
                    }
                }
            }
        }

        private bool IsDouble(string input)
        {
            return double.TryParse(input, out _);
        }


        public void CheckEntryInteger(Entry entry, ContentPage currentPage)
        { // Tarkistetaan, että entryn syöte on int-muotoista ja vahintaan 0

            if (!string.IsNullOrWhiteSpace(entry.Text))
            {
                if (!IsInteger(entry.Text))
                {
                    DisplayAlertOnPage(currentPage, "Virhe", "Syöteessä ei voi olla tekstiä.", "OK");
                }
                else
                {
                    int value = Convert.ToInt32(entry.Text);
                    if (value < 0)
                    {
                        DisplayAlertOnPage(currentPage, "Virhe", "Syötteen on oltava ei-negatiivinen.", "OK");
                    }
                }
            }
        }

        private bool IsInteger(string input)
        {
            return int.TryParse(input, out _);
        }

        public void CheckEntryText(Entry entry, ContentPage currentPage)
        { // tarkistetaan etta syotteessa ei ole numeroita
            if (ContainsNumbers(entry.Text))
            {
                DisplayAlertOnPage(currentPage, "Virhe", $"Syötteessä ei voi olla numeroita.", "OK");
            }
        }

        private bool ContainsNumbers(string input)
        {
            return input.Any(char.IsDigit);
        }
    }
}
