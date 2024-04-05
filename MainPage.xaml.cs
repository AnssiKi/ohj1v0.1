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
    }

}
