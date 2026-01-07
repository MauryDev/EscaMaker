using EscaMaker.Services;
using EscaMaker.View;
using Microsoft.AspNetCore.Components.Forms;

namespace EscaMaker.Utils.EscalaTransfer
{
    public class EscalaBrowserFileActions : IEscalaTransfer
    {
        public Func<IBrowserFile>? GetFile { get; set; }
        JsonExport? JsonExport { get; set; }

        bool IEscalaTransfer.CanDelete => false;

        public EscalaBrowserFileActions(JsonExport jsonExport)
        {
            JsonExport = jsonExport;
        }
 
        async Task<bool> IEscalaTransfer.Export(string name, EscalaLocalData data)
        {
            try
            {
                if (JsonExport == null)
                    return false;

                var escalasData = new EscalasData(new() { { name, data } });
                
                var json = System.Text.Json.JsonSerializer.Serialize(escalasData);

                await JsonExport.GenerateJSONFile(json, "escalas-data.json");
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        async Task<EscalaLocalData?> IEscalaTransfer.Import(string nameId)
        {
            if (GetFile == null)
                return null;

            using var stream = GetFile().OpenReadStream(1024 * 10);
            using StreamReader reader = new(stream);
            var txt = await reader.ReadToEndAsync();
            var escalas = System.Text.Json.JsonSerializer.Deserialize<EscalasData>(txt);
            if (escalas?.Escalas.TryGetValue(nameId, out var escala) ?? false)
                return escala;

            return null;
        }

        async Task<IEnumerable<string>> IEscalaTransfer.GetNames()
        {
            if (GetFile == null)
                return [];
            using var stream = GetFile().OpenReadStream(1024 * 10);
            using StreamReader reader = new(stream);
            var txt = await reader.ReadToEndAsync();
            var escalas = System.Text.Json.JsonSerializer.Deserialize<EscalasData>(txt);
            return escalas?.Escalas.Select(e => e.Key) ?? [];
        }




        Task<bool> IEscalaTransfer.Delete(string name)
        {
            throw new NotImplementedException();
        }
    }
}
