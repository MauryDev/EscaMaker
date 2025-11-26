
using Blazored.LocalStorage;

namespace EscaMaker.Utils.EscalaTransfer
{
    public class EscalaLocalStorageActions : IEscalaTransfer
    {
        public ILocalStorageService StorageService { get; set; }
        public EscalaLocalStorageActions(ILocalStorageService storageService)
        {
            StorageService = storageService;
        }

        public async Task<bool> Export(EscalaLocalData data)
        {
            await StorageService.SetItemAsync("escala", data);
            return true;
        }

        public async Task<EscalaLocalData?> Import()
        {

            if (await StorageService.ContainKeyAsync("escala"))
            {
                return await StorageService.GetItemAsync<EscalaLocalData?>("escala");
            }
            return null;
        }

        
        async Task<bool> IEscalaTransfer.Delete()
        {
            if (await StorageService.ContainKeyAsync("escala"))
            {
                await StorageService.RemoveItemAsync("escala");
                return true;
            }
            return false;
        }
    }
}
