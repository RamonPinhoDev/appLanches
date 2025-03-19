using AplicativoLanches.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AplicativoLanches.Services
{
    internal class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseurl = "";
        private ILogger<ApiService> _logger;
        JsonSerializerOptions _serializerOptions;

        public ApiService(HttpClient httpClient, string baseurl, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _baseurl = baseurl;
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        }


        public async Task<ApiResponse<bool>> RegistrarUsuario(string nome, string email, string telefone, string password)
        {
            try
            {
                var register = new Register()
                {
                    Email = email,
                    Nome = nome,
                    Telefone = telefone,
                    Senha = password
                };

                var json = JsonSerializer.Serialize(register, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await PostRequest("api/Usuarios/Register", content);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Erro ao enviar requisição HTPP{response.StatusCode}");
                    return new ApiResponse<bool> { ErrorMessage = $"Erro ao enviar requisição HTPP{response.StatusCode}" };

                }
                return new ApiResponse<bool> { Data = true };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao registrar ao usuário {ex.Message}");
                return new ApiResponse<bool> { ErrorMessage= ex.Message };
            }

        }


        public async Task<ApiResponse<bool>> Login (string email, string senha)
        {

            try
            {
                var login = new Login() { Email = email, Senha = senha };
                var json = JsonSerializer.Serialize(login, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await PostRequest("api/Usuarios/Register", content);
                if (!response.IsSuccessStatusCode) {
                    _logger.LogError($"Erro ao enviar HTTP: {response.StatusCode}");
                    return new ApiResponse<bool> { ErrorMessage = $"Erro ao enviar HTTP: {response.StatusCode}" };
                
                }

                var JsonResult = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Token>(JsonResult, _serializerOptions);
                Preferences.Set("acesstoken",result!.AcessToken);
                Preferences.Set("usuarioId", (int)result.UsuarioId);
                Preferences.Set("usuarionome", result.UsuarioName);
                return new ApiResponse<bool> {Data = true };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error no login : {ex.Message}");
                return new ApiResponse<bool> {ErrorMessage = ex.Message };

            }
        }


        private async Task<HttpResponseMessage> PostRequest(string uri, HttpContent httpContent)
        {
            var enderecoUrl = AppConfig.BaseUrl + uri;
            try 
            {
                var result = await _httpClient.PostAsync(enderecoUrl, httpContent);
                return result;
            } catch (Exception ex) 
            {
                _logger.LogError($"Erro ao enviar requisição POST para {uri} : {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            }
        }

    }
}
