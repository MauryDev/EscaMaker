namespace EscaMaker.Services;

using EscaMaker.View;
using iText.StyledXmlParser.Jsoup.Nodes;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net.Http;
using System.Net.Http.Json;

public class AdminApiService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    private string _authToken = string.Empty; 


    public async Task<(string?,string?)> LoginAsync(string user, string password)
    {
        var loginInfo = new Login { user = user, password = password };
        const string endpoint = "/api/admin/login";

        var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = JsonContent.Create(loginInfo),
            Headers =
            {
                {"X-Auth-Token", _authToken }
            }
        };

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            _authToken = result?.token ?? string.Empty;
            if (string.IsNullOrEmpty(_authToken))
            {
                return ("Login bem-sucedido, mas o token está vazio.", null);
            }

            
            return (null,_authToken);
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return ($"Fail in Login: {errorContent}", null);
        }
        else
        {
            response.EnsureSuccessStatusCode(); // Lança HttpRequestException para outros erros
            return ($"Undefined behaviour", null);
        }
    }

    public async Task<ApiResponse?> LoadAsync(string name)
    {
        var endpoint = $"/api/admin/load?name={Uri.EscapeDataString(name)}";

        EnsureAuthenticated();

        var request = new HttpRequestMessage(HttpMethod.Get, endpoint)
        {
            Headers =
            {
                {"X-Auth-Token", _authToken }
            }
        };

        var responseHttp = await _httpClient.SendAsync(request);

        var response = await responseHttp.Content.ReadFromJsonAsync<ApiResponse>();
       

        if (response == null || response.data == null)
        {
            return null;
        }

        return response;
    }
    public async Task<ApiResponse?> SaveAsync(EscaMakerInfo info)
    {
        try
        {
            const string endpoint = "/api/admin/save";

            EnsureAuthenticated();

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = JsonContent.Create(info),
                Headers =
                {
                    {"X-Auth-Token", _authToken }
                }
            };

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ApiResponse>();
        } catch(Exception ex)
        {
            Console.WriteLine(ex);

            return null;
        }
        
    }


    public async Task<bool> DeleteAsync(string nome)
    {
        const string endpoint = "/api/admin/delete";

        EnsureAuthenticated();


        var body = new DeleteBody { Nome = nome };
        
        var request = new HttpRequestMessage(HttpMethod.Delete, endpoint)
        {
            Content = JsonContent.Create(body),
            Headers =
            {
                {"X-Auth-Token", _authToken }
            }
        };

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<DeleteResult>();
        return result?.isSuccess ?? false;
    }

    public async Task<string[]?> GetAllNames()
    {
        var endpoint = "/api/admin/loadAllNames";

        EnsureAuthenticated();

        var request = new HttpRequestMessage(HttpMethod.Get, endpoint)
        {
            Headers =
            {
                {"X-Auth-Token", _authToken }
            }
        };

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GetAllNamesResponseDTO>();
        return result != null ? result.Nomes : null;
    }
    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(_authToken);

    void EnsureAuthenticated()
    {
        if (string.IsNullOrEmpty(_authToken))
        {
            throw new InvalidOperationException("Não autenticado. Você deve fazer login primeiro.");
        }
    }
    public void LoadAuthToken(string token) => _authToken = token;
}
