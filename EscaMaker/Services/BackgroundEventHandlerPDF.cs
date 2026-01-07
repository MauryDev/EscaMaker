using iText.Commons.Actions;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Event;
using iText.Kernel.Pdf.Xobject;

namespace EscaMaker.Services;

public class BackgroundEventHandlerPDF : AbstractPdfDocumentEventHandler
{
    protected ImageData? Image;

    public void Init(ImageData imagedata)
    {
        Image = imagedata;

    }
    public void Reset()
    {
        Image = null;
    }

    public void HandleEventOnEvent(IEvent @event)
    {

        base.OnEvent(@event);
    }

    protected override void OnAcceptedEvent(AbstractPdfDocumentEvent @event)
    {
        var docEvent = (PdfDocumentEvent)@event;
        var pdfDoc = docEvent.GetDocument();
        var page = docEvent.GetPage();

        var pageSize = page.GetPageSize();

        PdfCanvas canvas = new(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

        canvas.AddXObjectFittedIntoRectangle(new PdfImageXObject(Image), new(0, 0, pageSize.GetWidth(), pageSize.GetHeight()));

        canvas.Release();
    }
}
