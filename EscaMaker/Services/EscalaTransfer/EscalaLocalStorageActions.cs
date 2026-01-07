
using Blazored.LocalStorage;
using EscaMaker.View;

namespace EscaMaker.Utils.EscalaTransfer
{
    public class EscalaLocalStorageActions : IEscalaTransfer
    {
        const string KeyStorage = "escalas";
        public ILocalStorageService StorageService { get; set; }
        public EscalaLocalStorageActions(ILocalStorageService storageService)
        {
            StorageService = storageService;
        }

        bool IEscalaTransfer.CanDelete => true;

        async Task<bool> IEscalaTransfer.Export(string name, EscalaLocalData data)
        {
            EscalasData escalas;
            if (await StorageService.ContainKeyAsync(KeyStorage))
            {
                escalas = await StorageService.GetItemAsync<EscalasData>(KeyStorage) ?? new EscalasData([]);
            }
            else
            {
                escalas = new EscalasData([]);
            }
            escalas.Escalas[name] = data;
            await StorageService.SetItemAsync(KeyStorage, escalas);
            return true;
        }
        

        async Task<EscalaLocalData?> IEscalaTransfer.Import(string name)
        {

            if (await StorageService.ContainKeyAsync(KeyStorage))
            {
                var escalas = await StorageService.GetItemAsync<EscalasData?>(KeyStorage);
                if (escalas?.Escalas.TryGetValue(name, out var escala) ?? false)
                {
                    return escala;
                }
            }
            return null;
        }
        async Task<IEnumerable<string>> IEscalaTransfer.GetNames()
        {
            if (await StorageService.ContainKeyAsync(KeyStorage))
            {
                var escalas = await StorageService.GetItemAsync<EscalasData?>(KeyStorage);
                return escalas?.Escalas.Select(e => e.Key) ?? [];
            }
            return [];
        }
        async Task<bool> IEscalaTransfer.Delete(string name)
        {
            if (await StorageService.ContainKeyAsync(KeyStorage))
            {
                var escalas = await StorageService.GetItemAsync<EscalasData?>(KeyStorage);
                var ret = escalas?.Escalas.Remove(name) ?? false;
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
