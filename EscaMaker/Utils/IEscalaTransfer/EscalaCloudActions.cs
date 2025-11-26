
using EscaMaker.Services;
using System.Text.Json;

namespace EscaMaker.Utils.EscalaTransfer
{
    public class EscalaCloudActions : IEscalaTransfer
    {
        AdminApiService AdminApiService { get; set; }
        Func<string> GetName { get; set; }
        string Name => GetName();
        public EscalaCloudActions(AdminApiService adminApiService, Func<string> GetName)
        {
            AdminApiService = adminApiService;
            this.GetName = GetName;
        }
        async Task<bool> IEscalaTransfer.Export(EscalaLocalData data)
        {
            var Escamakerinfo = new View.EscaMakerInfo()
            {
                Nome = Name,
                JSONData = JsonSerializer.Serialize(data)
            };
            try
            {
                var resultado = await AdminApiService.SaveAsync(Escamakerinfo);
                return resultado != null;
            } catch
            {
                return false;
            }
        }

        async Task<EscalaLocalData?> IEscalaTransfer.Import()
        {
            try
            {
                var resultado = await AdminApiService.LoadAsync(Name);
                if (resultado == null || resultado.data == null)
                    return null;

                return JsonSerializer.Deserialize<EscalaLocalData>(resultado.data.JSONData);
            }
            catch (Exception)
            {

                return null;
            }
        }

        async Task<bool> IEscalaTransfer.Delete()
        {
            return await AdminApiService.DeleteAsync(Name);

        }
    }
}
