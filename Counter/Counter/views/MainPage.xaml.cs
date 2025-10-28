using System.Threading.Tasks;

namespace Counter
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }


        private void PlusOne(object sender, EventArgs e)
        {
            count++;

                CounterBtn.Text = $"{count}";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void MinusOne(object sender, EventArgs e)
        {
            count--;

            
                CounterBtn.Text = $"{count}";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        
    }
}
