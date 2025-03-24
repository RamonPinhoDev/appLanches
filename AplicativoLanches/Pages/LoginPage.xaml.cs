using AplicativoLanches.Services;

namespace AplicativoLanches.Pages;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService;
    public LoginPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void BtnSingnup_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(EntEmail.Text))
        {
            await DisplayAlert("Error","Informe o seu email","Cancelar");
        }
        if (string.IsNullOrEmpty(EntPassword.Text)) { await DisplayAlert("Error","Informe a sua senha","Cancelar");
        
        }

        var response = await _apiService.Login(EntEmail.Text, EntPassword.Text);
        if (!response.HasError) 
        {
            Application.Current!.MainPage = new AppShell();
        }
    }



    private async void TapRegister_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new InscricaoPage(_apiService));

    }
}