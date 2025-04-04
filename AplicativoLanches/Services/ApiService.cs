﻿using AplicativoLanches.Models;
using AplicativoLanches.Services;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AplicativoLanches.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://ntkxp91r-7292.brs.devtunnels.ms/";

    private readonly ILogger<ApiService> _logger;

    JsonSerializerOptions _serializerOptions;
    public ApiService(HttpClient httpClient,
                      ILogger<ApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<ApiResponse<bool>> RegistrarUsuario(string nome, string email,
                                                          string telefone, string password)
    {
        try
        {
            var register = new Register()
            {
                Nome = nome,
                Email = email,
                Telefone = telefone,
                Senha = password
            };

            var json = JsonSerializer.Serialize(register, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/Usuarios/Register", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
                };
            }

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao registrar o usuário: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }
    public async Task<ApiResponse<bool>> Login(string email, string password)
    {
        try
        {
            var login = new Login()
            {
                Email = email,
                Senha = password
            };

            var json = JsonSerializer.Serialize(login, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/Usuarios/Login", content);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao enviar requisição HTTP : {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao enviar requisição HTTP : {response.StatusCode}"
                };
            }

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Token>(jsonResult, _serializerOptions);

            Preferences.Set("accesstoken", result!.AccessToken);
            Preferences.Set("usuarioid", (int)result.UsuarioId!);
            Preferences.Set("usuarionome", result.UsuarioNome);

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro no login : {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }
    private async Task<HttpResponseMessage> PostRequest(string uri, HttpContent content)
    {
        var enderecoUrl = _baseUrl + uri;
        try
        {
            var result = await _httpClient.PostAsync(enderecoUrl, content);
            return result;
        }
        catch (Exception ex)
        {
            // Log o erro ou trate conforme necessário
            _logger.LogError($"Erro ao enviar requisição POST para {uri}: {ex.Message}");
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }


    public async Task<(List<Categoria>?Categoria, string? ErrorMessage)> GetCategorias()
    {
        return await GetAsync<List<Categoria>>("api/categorias");
    }

    public async Task<(List<Produto>? Produtos, string? ErroMessage)> GetProdutos(string tipoProduto, string? categoriaid)
    {
        string endpoint = $"api/Produtos?tipoProduto={tipoProduto}&categoriaId={categoriaid}";

        return await GetAsync<List<Produto>>(endpoint);

    }
        private async Task<(T? Data, string? ErrorMessage)> GetAsync<T>(string endpoint)

    {
        try
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync(AppConfig.BaseUrl + endpoint);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<T>(responseString, _serializerOptions);
                return (data ?? Activator.CreateInstance<T>(), null);
            }
            else
            { if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    string erroMesage = "Unauthorized";
                    _logger.LogInformation(erroMesage);
                    return (default, erroMesage);
                }

                string generalErroMessage = $"Erro na requisão :{response.ReasonPhrase}";
                _logger.LogInformation(generalErroMessage);
                return (default, generalErroMessage);
            }
        }
        catch (HttpRequestException ex)
        {
            string errorMessage = $"Erro de requisao Http: {ex.Message}";
            _logger.LogInformation(ex, errorMessage);
            return (default, errorMessage);
        }
        catch (JsonException ex) 
        {string errorMessage =$"Erro de serialização JSON: {ex.Message}";
            _logger.LogInformation(ex, errorMessage); return (default, errorMessage);

        }
        catch (Exception ex)
        { string errorMessage = $"Erro insperado: {ex.Message}";
            _logger.LogInformation(ex, errorMessage);
            return (default, errorMessage);
        
        }

    }

    private void AddAuthorizationHeader()
    {
        var token = Preferences.Get("accesstoken", string.Empty);
        if (string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }



}
