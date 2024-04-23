using MySqlConnector;
using ohj1v0._1.Luokat;

namespace ohj1v0._1
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Vasta {count} mökki ostettu";
            else
                CounterBtn.Text = $"No nyt on jo {count} mökkiä ostettu";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
        
        //testaus että tietokantayhteys aukeaa MH
        private async void OnDatabaseClicked(object sender, EventArgs e)
        {
            /*DatabaseConnector dbc = new DatabaseConnector();
            try
            {
                var conn = dbc._getConnection();
                conn.Open();
                await DisplayAlert("Onnistui", "Tietokanta yhteys  aukesi", "OK");
            }
            catch (MySqlException ex)
            {
                await DisplayAlert("Failure", ex.Message, "OK");
            }*/
        }
    }

}
