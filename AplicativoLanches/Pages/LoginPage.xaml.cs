using AplicativoLanches.Services;
using AplicativoLanches.Validator;

namespace AplicativoLanches.Pages;

public partial class LoginPage : ContentPage
{
    private IValidator _validator;
    private readonly ApiService _apiService;
    public LoginPage(ApiService apiService, IValidator validator)
    {
        InitializeComponent();
        _apiService = apiService;
        _validator = validator;
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
            Application.Current!.MainPage = new AppShell(_validator, _apiService);
        }
    }



    private async void TapRegister_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new InscricaoPage(_apiService, _validator));

    }
}