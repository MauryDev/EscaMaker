
using EscaMaker.Services;
using EscaMaker.View;
using System.Text.Json;

namespace EscaMaker.Utils.EscalaTransfer
{
    public class EscalaCloudActions : IEscalaTransfer
    {
        AdminApiService AdminApiService { get; set; }

        bool IEscalaTransfer.CanDelete => true;

        public EscalaCloudActions(AdminApiService adminApiService)
        {
            AdminApiService = adminApiService;
        }
        async Task<bool> IEscalaTransfer.Export(string name, EscalaLocalData data)
        {
            var Escamakerinfo = new View.EscaMakerInfo()
            {
                Nome = name,
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

        async Task<EscalaLocalData?> IEscalaTransfer.Import(string name)
        {
            try
            {
                var resultado = await AdminApiService.LoadAsync(name);
                if (resultado == null || resultado.data == null)
                    return null;

                return JsonSerializer.Deserialize<EscalaLocalData>(resultado.data.JSONData);
            }
            catch (Exception)
            {

                return null;
            }
        }

        async Task<bool> IEscalaTransfer.Delete(string Name)
        {
            return await AdminApiService.DeleteAsync(Name);

        }

        async Task<IEnumerable<string>> IEscalaTransfer.GetNames()
        {
            return await AdminApiService.GetAllNames() ?? [];
        }
    }
}
