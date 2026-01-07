namespace EscaMaker.View;

public class EscalaLocalData
{
    public required int Ano { get; set; }
    public required byte Mes { get; set; }
    public required List<List<string>>[] Nomes { get; set; }
    public required EscalaInfo[] EscalasInfo { get; set; }
}
