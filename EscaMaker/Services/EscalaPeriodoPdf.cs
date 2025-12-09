using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Event;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace EscaMaker.Services;

public class EscalaPeriodoPdf
{
    const float TitleFontSize = 35f;
    const float DayFontSize = 25f;
    const float EntryFontSize = 25f;
    const float LeftMargin = 80f;
    const float DayBottomSpacing = 60f;
    const float EntryBottomSpacing = 20f;
    const float PageMargin = 30f;
    PdfFont? FontStd { get; set; }
    Document? DocumentCurrent { get; set; }
    BackgroundEventHandler EventHandler { get; set; }
    public EscalaPeriodoPdf(BackgroundEventHandler backgroundEventHandler)
    {
        this.EventHandler = backgroundEventHandler;
    }
    PdfFont GetPdfFont()
    {
        FontStd ??= PdfFontFactory.CreateFont(Resources.Resource.georgiaFont, "");
        return FontStd;
    }
    public Stream GeneratePDF(IEnumerable<(DateOnly Date, IEnumerable<(string Name, string Role)> Items)> schedule)
    {
        var output = new MemoryStream();
        CreateSimplePdf(Resources.Resource.image, output, schedule);
        output.Seek(0, SeekOrigin.Begin);
        FontStd = null;
        DocumentCurrent = null;
        EventHandler.Reset();
        return output;
    }

    void CreateSimplePdf(byte[] imageBytes, Stream outputStream, IEnumerable<(DateOnly Date, IEnumerable<(string Name, string Role)> Items)> schedule)
    {
        using var writer = new PdfWriter(outputStream);
        writer.SetCloseStream(false);

        using var pdf = new PdfDocument(writer);
        DocumentCurrent = new Document(pdf);
        DocumentCurrent.SetMargins(PageMargin, PageMargin, PageMargin, PageMargin);
        AddFullPageBackgroundImage(imageBytes);

        var firstPage = true;
        foreach (var day in schedule)
        {
            if (!firstPage)
                DocumentCurrent.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            firstPage = false;

            AddTitle("PROGRAMAÇÃO DE CULTO");
            AddDayInfo(day.Date);

            foreach (var (name, role) in day.Items)
                AddEntry(name, role);
        }
        ((IDisposable)DocumentCurrent).Dispose();

    }

    void AddFullPageBackgroundImage(byte[] imageBytes)
    {
        var imageData = ImageDataFactory.Create(imageBytes);

        var pdf = DocumentCurrent.GetPdfDocument();
        EventHandler.Init(imageData);
        pdf.AddEventHandler(PdfDocumentEvent.INSERT_PAGE, EventHandler);
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

    void AddDayInfo(DateOnly date)
    {
        var font = GetPdfFont();

        var weekday = Utils.DateTimeUtil.GetDiaSemanaNome(date.DayOfWeek);
        var formatted = $"{date:dd/MM} - {weekday}";

        var p = new Paragraph()
            .Add(new Text(formatted).SetFontSize(DayFontSize))
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginLeft(LeftMargin)
            .SetFont(font)
            .SetMarginBottom(DayBottomSpacing);

        DocumentCurrent.Add(p);
    }

    void AddEntry(string name, string role)
    {
        var font = GetPdfFont();

        var text = $"{name} - {role}";
        var p = new Paragraph()
            .Add(new Text(text).SetFontSize(EntryFontSize))
            .SetMarginLeft(LeftMargin)
            .SetFont(font)
            .SetMarginBottom(EntryBottomSpacing);

        DocumentCurrent.Add(p);
    }
}
