using AplicativoLanches.Services;
using AplicativoLanches.Validator;

namespace AplicativoLanches.Pages;

public partial class HomePage : ContentPage
{
	private readonly IValidator _validator;
	private readonly ApiService _apiService;
    private bool _loginPageDisplayed = false;
    public HomePage(IValidator validator, ApiService apiService)
    {
        InitializeComponent();
        _validator = validator;
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetListaCategorias();
        await GetMaisVendida();
        await GetPopulares();
    }

    private async Task<IEnumerable<Categoria>> GetListaCategorias()
    {
        try
        {
            var (categorias, erroMessage) = await _apiService.GetCategorias();

            if erroMessage == "Unauthorized" && !_loginPageDisplayed)
            {
                await DisplayLoginPage();
                return Enumerable.Empty<Categoria>();
            }

            if (categorias == null)
            {
                await DisplayAlert("Erro", erroMessage ?? "Não foi possivel obter as categorias.", "Ok");
                return Enumerable.Empty<Categoria>();
            }

            CvCategorias.ItemsSource = categorias;
            return categorias;
        } catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro inesperado: {ex.Message}", "Ok");
            return Enumerable.Empty<Categoria>();
        }
    }
}