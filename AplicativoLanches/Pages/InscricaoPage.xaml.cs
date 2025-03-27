using AplicativoLanches.Models;
using AplicativoLanches.Services;
using AplicativoLanches.Validator;

namespace AplicativoLanches.Pages;

public partial class InscricaoPage : ContentPage
{
    private readonly IValidator _validator;
    private readonly ApiService _apiService;
    public InscricaoPage(ApiService apiService, IValidator validator)
    {
        InitializeComponent();
        _apiService = apiService;
        _validator = validator;
    }

    private async void BtnSingnup_Clicked(object sender, EventArgs e)
    {
        if (await _validator.Validar(EntName.Text, EntEmail.Text, EntPhone.Text, EntPassword.Text))
        {
            var response = await _apiService.RegistrarUsuario(EntName.Text, EntEmail.Text, EntPhone.Text, EntPassword.Text);
            if (response.HasError)
            {
                await DisplayAlert("Aviso", "Sus conta foi criada com sucesso", "Ok");
                await Navigation.PushAsync(new LoginPage(_apiService, _validator));
            }
            else
            {
                await DisplayAlert("Erro", "Algo de errado!!!", "Cancelar");
            }
        }else 
        {
            string MensagemError = "";

            MensagemError += _validator.NomeErro != null ? $"\n-{_validator.NomeErro}" : "";
            MensagemError += _validator.EmailErro != null ? $"\n-{_validator.EmailErro}" : "";
            MensagemError += _validator.TelefoneErro != null ? $"\n-{_validator.TelefoneErro}" : "";
            MensagemError += _validator.SenhaErro != null ?  $"\n-{_validator.SenhaErro}" : "";

            await DisplayAlert("Erro",MensagemError, "Cancelar");

        }
    }

    private async void TapLogin_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }
}