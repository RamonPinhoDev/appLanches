using AplicativoLanches.Pages;
using AplicativoLanches.Services;

namespace AplicativoLanches
{
    public partial class App : Application
    {private readonly ApiService _apiService;
        public App(ApiService apiService)
        {
            _apiService = apiService;
            InitializeComponent();

            MainPage = new AppShell();
            //MainPage = new NavigationPage(new InscricaoPage(_apiService));
        }
    }
}
