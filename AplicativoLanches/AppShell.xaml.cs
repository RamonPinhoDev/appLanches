//using Android.Service.Autofill;

using AplicativoLanches.Pages;
using AplicativoLanches.Services;
using AplicativoLanches.Validator;

namespace AplicativoLanches
{
    public partial class AppShell : Shell
    {
        private readonly IValidator _validator;
        private readonly ApiService _apiService /*?? throw new ArgumentNullException(nameof(apiService))*/;
        public AppShell(IValidator validator, ApiService apiService)
        {
            InitializeComponent();
            _validator = validator;
            _apiService = apiService;
            ConfigureShell();
        }

        private void ConfigureShell()
        {
            var homePage = new HomePage( _validator, _apiService);
            var carrinhoPage = new CarrinhoPage();
            var favoritoPage = new FavoritosPage();
            var perfilPage = new PerfilPage();  

            Items.Add(new TabBar
            {
                Items = {


                    new ShellContent { Title= "Home", Icon ="home", Content = homePage},
                    new ShellContent {Title = "Carrinho", Icon ="cart", Content= carrinhoPage},
                    new ShellContent {Title = "Favoritos", Icon = "heart", Content = favoritoPage},
                    new ShellContent {Title = "Perfil", Icon ="profile", Content = perfilPage}

                    }
            });

        }

       
    }
}
