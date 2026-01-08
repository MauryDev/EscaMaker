
using EscaMaker.Services;
using EscaMaker.View;
using System.Text.Json;

namespace EscaMaker.Services.ScheduleTransfer
{
    public class ScheduleCloudActions : IScheduleTransfer
    {
        AdminApiService AdminApiService { get; set; }

        bool IScheduleTransfer.CanDelete => true;

        public ScheduleCloudActions(AdminApiService adminApiService)
        {
            AdminApiService = adminApiService;
        }
        async Task<bool> IScheduleTransfer.Export(string name, LocalScheduleData data)
        {
            var Escamakerinfo = new View.EscaMakerInfo()
            {
                Nome = name,
                JSONData = JsonSerializer.Serialize(data)
            };
            try
            {
                var resultado = await AdminApiService.SaveAsync(Escamakerinfo);
                return resultado != null && resultado.success;
            } catch
            {
                return false;
            }
        }

        async Task<LocalScheduleData?> IScheduleTransfer.Import(string name)
        {
            try
            {
                var resultado = await AdminApiService.LoadAsync(name);
                if (resultado == null || resultado.data == null)
                    return null;

                return JsonSerializer.Deserialize<LocalScheduleData>(resultado.data.JSONData);
            }
            catch (Exception)
            {

                return null;
            }
        }

        async Task<bool> IScheduleTransfer.Delete(string Name)
        {
            return await AdminApiService.DeleteAsync(Name);

        }

        async Task<IEnumerable<string>> IScheduleTransfer.GetNames()
        {
            return await AdminApiService.GetAllNames() ?? [];
        }
    }
}
