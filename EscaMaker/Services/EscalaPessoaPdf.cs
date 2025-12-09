using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Event;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace EscaMaker.Services;


public class EscalaPessoaPdf
{
    const float TitleFontSize = 35f;
    const float DayFontSize = 25f;
    const float EntryFontSize = 25f;
    const float LeftMargin = 80f;
    const float DayBottomSpacing = 80f;
    const float EntryBottomSpacing = 10f;
    const float PageMargin = 30f;
    PdfFont? FontStd { get; set; }
    Document? DocumentCurrent { get; set; }
    BackgroundEventHandler EventHandler { get; set; }

    public EscalaPessoaPdf(BackgroundEventHandler backgroundEventHandler)
    {
        this.EventHandler = backgroundEventHandler;
    }
    PdfFont GetPdfFont()
    {
        FontStd ??= PdfFontFactory.CreateFont(Resources.Resource.georgiaFont, "");
        return FontStd;
    }
    public Stream GeneratePDF(string NamePessoa, int Month, Dictionary<DateOnly, IEnumerable<string>> Funcoes)
    {
        var output = new MemoryStream();

        CreateSimplePdf(Resources.Resource.image, output, NamePessoa, Month, Funcoes);

        output.Seek(0, SeekOrigin.Begin);
        FontStd = null;
        DocumentCurrent = null;
        this.EventHandler.Reset();
        return output;
    }

    void CreateSimplePdf(byte[] imageBytes, Stream outputStream,
        string NamePessoa, int Month,
        Dictionary<DateOnly, IEnumerable<string>> Funcoes)
    {
        using var writer = new PdfWriter(outputStream);
        writer.SetCloseStream(false);

        using var pdf = new PdfDocument(writer);
        DocumentCurrent = new Document(pdf);

        DocumentCurrent.SetMargins(PageMargin, PageMargin, PageMargin, PageMargin);
        AddFullPageBackgroundImage(imageBytes);

        AddTitle($"Escala de {Utils.DateTimeUtil.GetMesNome(Month)}");
        AddNameInfo(NamePessoa);

        var firstPage = true;

        foreach (var funcoesDia in Funcoes)
        {
           
            AddDiaInfo(funcoesDia.Key);
            AddRoles(funcoesDia.Value);
            

            firstPage = false;

        }

        ((IDisposable)DocumentCurrent).Dispose();
    }
 

    void AddFullPageBackgroundImage(byte[] imageBytes)
    {
        var imageData = ImageDataFactory.Create(imageBytes);
        
        var pdf = DocumentCurrent.GetPdfDocument();
        this.EventHandler.Init(imageData);
        pdf.AddEventHandler(PdfDocumentEvent.INSERT_PAGE, this.EventHandler);
    }

    void AddTitle(string text)
    {
        var font = GetPdfFont();

        var p = new Paragraph()
            .Add(new Text(text).SetFontSize(TitleFontSize).SimulateBold())
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFont(font)
            .SetMarginLeft(LeftMargin);

        DocumentCurrent.Add(p);
    }
    void AddNameInfo(string Name)
    {
        var font = GetPdfFont();

        var formatted = $"Nome - {Name}";

        var p = new Paragraph()
            .Add(new Text(formatted).SetFontSize(DayFontSize))
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginLeft(LeftMargin)
            .SetFont(font)
            .SetMarginBottom(DayBottomSpacing);

        DocumentCurrent.Add(p);
    }
    void AddDiaInfo(DateOnly diaInfo)
    {
        var font = GetPdfFont();
        var diaSemanaNome = Utils.DateTimeUtil.GetDiaSemanaNome(diaInfo.DayOfWeek);
        var formatted = $"{diaInfo.Day:D2}/{diaInfo.Month:D2} - {diaSemanaNome}";

        var p = new Paragraph()
            .Add(new Text(formatted).SetFontSize(DayFontSize))
            .SetMarginLeft(LeftMargin)
            .SetFont(font)
            .SetMarginBottom(EntryBottomSpacing);


        DocumentCurrent.Add(p);
    }


    void AddRoles(IEnumerable<string> roles)
    {
        var font = GetPdfFont();

        List list = new List()
            .SetSymbolIndent(12)
            .SetMarginLeft(LeftMargin)
            .SetFont(font)
            .SetFontSize(EntryFontSize)
            .SetListSymbol("\u2022");

        
        foreach (string item in roles)
        {
            list.Add(item);
        }

        DocumentCurrent.Add(list);

     

    }
}
