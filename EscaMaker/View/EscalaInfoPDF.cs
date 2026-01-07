namespace EscaMaker.View;

public record EscalaInfoPDF(EscalaInfo EscalaInfo, IEnumerable<IEnumerable<byte>> periodoData, IEnumerable<IEnumerable<string>> periodosNomes);
