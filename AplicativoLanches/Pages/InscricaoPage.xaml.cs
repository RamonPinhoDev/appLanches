using AplicativoLanches.Models;
using AplicativoLanches.Services;

namespace AplicativoLanches.Pages;

public partial class InscricaoPage : ContentPage
{
    
    private readonly ApiService _apiService;
    public InscricaoPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void BtnSingnup_Clicked(object sender, EventArgs e)
    {
        var response = await _apiService.RegistrarUsuario(EntName.Text, EntEmail.Text, EntPhone.Text,EntPassword.Text);
        if (response.HasError)
        {
            await DisplayAlert("Aviso", "Sus conta foi criada com sucesso", "Ok");
            await Navigation.PushAsync(new LoginPage(_apiService));
        }
        else 
        {
            await DisplayAlert("Erro","Algo de errado!!!", "Cancelar");
        }

    }

    private async void TapLogin_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new LoginPage(_apiService));
    }
}