
using Blazored.LocalStorage;
using EscaMaker.View;

namespace EscaMaker.Services.ScheduleTransfer
{
    public class ScheduleLocalStorageActions : IScheduleTransfer
    {
        const string KeyStorage = "escalas";
        public ILocalStorageService StorageService { get; set; }
        public ScheduleLocalStorageActions(ILocalStorageService storageService)
        {
            StorageService = storageService;
        }

        bool IScheduleTransfer.CanDelete => true;

        async Task<bool> IScheduleTransfer.Export(string name, LocalScheduleData data)
        {
            SchedulesData escalas;
            if (await StorageService.ContainKeyAsync(KeyStorage))
            {
                escalas = await StorageService.GetItemAsync<SchedulesData>(KeyStorage) ?? new SchedulesData([]);
            }
            else
            {
                escalas = new SchedulesData([]);
            }
            escalas.Schedules[name] = data;
            await StorageService.SetItemAsync(KeyStorage, escalas);
            return true;
        }
        

        async Task<LocalScheduleData?> IScheduleTransfer.Import(string name)
        {

            if (await StorageService.ContainKeyAsync(KeyStorage))
            {
                var escalas = await StorageService.GetItemAsync<SchedulesData?>(KeyStorage);
                if (escalas?.Schedules.TryGetValue(name, out var escala) ?? false)
                {
                    return escala;
                }
            }
            return null;
        }
        async Task<IEnumerable<string>> IScheduleTransfer.GetNames()
        {
            if (await StorageService.ContainKeyAsync(KeyStorage))
            {
                var escalas = await StorageService.GetItemAsync<SchedulesData?>(KeyStorage);
                return escalas?.Schedules.Select(e => e.Key) ?? [];
            }
            return [];
        }
        async Task<bool> IScheduleTransfer.Delete(string name)
        {
            if (await StorageService.ContainKeyAsync(KeyStorage))
            {
                var escalas = await StorageService.GetItemAsync<SchedulesData?>(KeyStorage);
                var ret = escalas?.Schedules.Remove(name) ?? false;
                if (ret)
                {
                    await StorageService.SetItemAsync(KeyStorage, escalas);
                    return true;
                }
                return false;
            }
            return false;
        }

        
    }
}
