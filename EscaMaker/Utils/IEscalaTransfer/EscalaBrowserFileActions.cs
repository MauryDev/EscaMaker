using EscaMaker.Services;
using Microsoft.AspNetCore.Components.Forms;

namespace EscaMaker.Utils.EscalaTransfer
{
    public class EscalaBrowserFileActions : IEscalaTransfer
    {
        public Func<IBrowserFile>? GetFile { get; set; }
        JsonExport? JsonExport { get; set; }
        public EscalaBrowserFileActions(JsonExport jsonExport)
        {
            JsonExport = jsonExport;
        }
        async Task<bool> IEscalaTransfer.Export(EscalaLocalData data)
        {
            try
            {
                if (JsonExport == null)
                    return false;

                var json = System.Text.Json.JsonSerializer.Serialize(data);

                await JsonExport.GenerateJSONFile(json, "escalas-data.json");
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
        }

        async Task<EscalaLocalData?> IEscalaTransfer.Import()
        {
            if (GetFile == null)
                return null;

            using var stream = GetFile().OpenReadStream(1024 * 10);
            using StreamReader reader = new(stream);
            var txt = await reader.ReadToEndAsync();
            return System.Text.Json.JsonSerializer.Deserialize<EscalaLocalData>(txt);
        }

        Task<bool> IEscalaTransfer.Delete()
        {
            throw new NotImplementedException();
        }
    }
}
