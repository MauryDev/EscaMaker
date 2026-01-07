using System.Text.Json.Serialization;

namespace EscaMaker.View;

public record Login(string user, string password);

public record LoginResponse(string token);

public class EscaMakerInfo
{
    [JsonPropertyName("Nome")] public required string Nome { get; set; }
    [JsonPropertyName("JSONData")] public required string JSONData { get;set; }
}

public class EscaMakerInfoWithId: EscaMakerInfo
{
    public string _id { get; set; }
}

public record ApiResponse(EscaMakerInfoWithId? data);
public record DeleteResult(bool isSuccess);

public record DeleteBody(
    [property: JsonPropertyName("Nome")] string Nome
);

public record GetAllNamesResponseDTO(
    [property: JsonPropertyName("Nomes")] string[] Nomes
);