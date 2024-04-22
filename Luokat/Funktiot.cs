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



    }
}
