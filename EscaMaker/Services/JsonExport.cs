using Microsoft.JSInterop;

namespace EscaMaker.Services;

public class JsonExport
{
    private readonly IJSRuntime _jsRuntime;
    IJSObjectReference? modulePDF;
    public JsonExport(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    public async ValueTask<IJSObjectReference> GetModule()
    {
        modulePDF ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/exportJson.js");
        return modulePDF;
    }
    public async Task GeneratePDF(string data,string filename)
    {
        // Call the JavaScript function to generate the PDF
        var module = await GetModule();
        using var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(data));
        using var stream = new DotNetStreamReference(ms);
        await module.InvokeVoidAsync("default.GenerateJSONFile", stream, filename);
    }
    
}
