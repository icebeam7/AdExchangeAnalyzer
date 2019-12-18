using Xamarin.Forms;

namespace AdExchangeAnalyzer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Views.AnomalyView();
        }
    }
}
