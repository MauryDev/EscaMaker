using EscaMaker.Services;
using EscaMaker.View;
using Microsoft.AspNetCore.Components.Forms;

namespace EscaMaker.Utils.EscalaTransfer
{
    public class ScheduleBrowserFileActions : IScheduleTransfer
    {
        public Func<IBrowserFile>? GetFile { get; set; }
        JsonExport? JsonExport { get; set; }

        bool IScheduleTransfer.CanDelete => false;

        public ScheduleBrowserFileActions(JsonExport jsonExport)
        {
            JsonExport = jsonExport;
        }
 
        async Task<bool> IScheduleTransfer.Export(string name, LocalScheduleData data)
        {
            try
            {
                if (JsonExport == null)
                    return false;

                var escalasData = new SchedulesData(new() { { name, data } });
                
                var json = System.Text.Json.JsonSerializer.Serialize(escalasData);

                await JsonExport.GenerateJSONFile(json, "escalas-data.json");
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        async Task<LocalScheduleData?> IScheduleTransfer.Import(string nameId)
        {
            if (GetFile == null)
                return null;

            using var stream = GetFile().OpenReadStream(1024 * 10);
            using StreamReader reader = new(stream);
            var txt = await reader.ReadToEndAsync();
            var escalas = System.Text.Json.JsonSerializer.Deserialize<SchedulesData>(txt);
            if (escalas?.Schedules.TryGetValue(nameId, out var escala) ?? false)
                return escala;

            return null;
        }

        async Task<IEnumerable<string>> IScheduleTransfer.GetNames()
        {
            if (GetFile == null)
                return [];
            using var stream = GetFile().OpenReadStream(1024 * 10);
            using StreamReader reader = new(stream);
            var txt = await reader.ReadToEndAsync();
            var escalas = System.Text.Json.JsonSerializer.Deserialize<SchedulesData>(txt);
            return escalas?.Schedules.Select(e => e.Key) ?? [];
        }




        Task<bool> IScheduleTransfer.Delete(string name)
        {
            throw new NotImplementedException();
        }
    }
}
