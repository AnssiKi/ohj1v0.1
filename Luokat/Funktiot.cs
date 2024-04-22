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
        {
            if (entry.Text.Length >= maxLength)
            {
                DisplayAlertOnPage(currentPage, "Virhe", $"Syöte voi olla enintään {maxLength} merkkiä pitkä.", "OK");
            }
        }

        public async void CheckEntryDouble(Entry entry, ContentPage currentPage)
        {
            if (!string.IsNullOrWhiteSpace(entry.Text) && !IsDouble(entry.Text))
            {
                DisplayAlertOnPage(currentPage, "Virhe", $"Syöteessä ei voi olla tekstiä.", "OK");
            }
        }

        private bool IsDouble(string input)
        {
            return double.TryParse(input, out _);
        }



    }
}
