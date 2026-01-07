using EscaMaker.Services.Contracts;
using EscaMaker.Utils.EscalaTransfer;
using Microsoft.AspNetCore.Components.Forms;

namespace EscaMaker.Services
{
    public class EscalaDataTransferService : IEscalaDataTransferService
    {
        
        IServiceProvider ServiceProvider { get; set; }
        public EscalaDataTransferService(
           IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        IEscalaTransfer IEscalaDataTransferService.GetBrowserFileProvider(Func<IBrowserFile>? file = null)
        {
            var value = ActivatorUtilities.CreateInstance<EscalaBrowserFileActions>(ServiceProvider);
            value.GetFile = file;
            return value;
        }

        IEscalaTransfer IEscalaDataTransferService.GetLocalStorageProvider()
        {
            return ActivatorUtilities.CreateInstance<EscalaLocalStorageActions>(ServiceProvider);
        }

        IEscalaTransfer IEscalaDataTransferService.GetCloudProvider()
        {
            return ActivatorUtilities.CreateInstance<EscalaCloudActions>(ServiceProvider);
        }
    }
}
