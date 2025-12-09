using Microsoft.JSInterop;

namespace EscaMaker.Services;

public class PDFEscala
{
    private readonly IJSRuntime _jsRuntime;
    IJSObjectReference? modulePDF;
    public PDFEscala(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    public async ValueTask<IJSObjectReference> GetModule()
    {
        modulePDF ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/pdfEscala.js");
        return modulePDF;
    }
    public async Task GeneratePDF(Stream ms,string filename)
    {
        // Call the JavaScript function to generate the PDF
        var module = await GetModule();
        using var stream = new DotNetStreamReference(ms);
        await module.InvokeVoidAsync("default.GeneratePDF", stream, filename);
    }
    public async Task<string> GeneratePreview(MemoryStream ms)
    {
        // Call the JavaScript function to generate the PDF
        var module = await GetModule();
        using var stream = new DotNetStreamReference(ms);
        return await module.InvokeAsync<string>("default.GeneratePreview", stream);
    }
    public async Task DeletePreview(string filename)
    {
        // Call the JavaScript function to delete the preview
        var module = await GetModule();
        await module.InvokeVoidAsync("default.DeletePreview", filename);
    }
}
