namespace EscaMaker.View
{
    public record EscalaInfoPDF(Utils.EscalaInfo EscalaInfo, IEnumerable<IEnumerable<byte>> periodoData, IEnumerable<IEnumerable<string>> periodosNomes);
}