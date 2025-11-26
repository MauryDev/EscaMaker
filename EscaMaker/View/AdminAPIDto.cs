using System.Text.Json.Serialization;

namespace EscaMaker.View;

public class Login
{
    public string user { get; set; }
    public string password { get; set; }
}

public class LoginResponse
{
    public string token { get; set; }
}

public class EscaMakerInfo
{
    [JsonPropertyName(nameof(Nome))]
    public string Nome { get; set; }
    [JsonPropertyName(nameof(JSONData))]
    public string JSONData { get; set; } // O conteúdo JSON a ser salvo
}
public class EscaMakerInfoWithId: EscaMakerInfo
{
    public string _id { get; set; }
}

public class ApiResponse
{
    public EscaMakerInfoWithId? data { get; set; }
}
public class DeleteResult
{
    public bool isSuccess { get; set; }
}

public class DeleteBody
{
    [JsonPropertyName(nameof(Nome))]
    public string Nome { get; set; }
}

public class GetAllNamesResponseDTO
{
    [JsonPropertyName(nameof(Nomes))]
    public string[] Nomes { get; set; }
}