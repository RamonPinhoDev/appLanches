using AplicativoLanches.Pages;
using AplicativoLanches.Services;
using AplicativoLanches.Validator;
using Microsoft.WindowsAppSDK.Runtime.Packages;

namespace AplicativoLanches
{
    public partial class App : Application
    {
        
       private readonly ApiService _apiService;
       private readonly IValidator _validator;
        public App(ApiService apiService, IValidator validator)
        {
            _apiService = apiService;
            _validator = validator;
            InitializeComponent();

            //MainPage = new AppShell();
           // MainPage = new NavigationPage(new InscricaoPage(_apiService, _validator));

            SetMain();
        }

        private void SetMain()
        {
            var acesstoken = Preferences.Get("acesstoken", string.Empty);
            if (string.IsNullOrEmpty(acesstoken))
            {
                MainPage = new NavigationPage(new LoginPage(_apiService, _validator));
            }

            MainPage = new AppShell();
        }

    }
}
