using Microsoft.AspNetCore.Components.Forms;

namespace EscaMaker.Services.Contracts
{
    public interface IEscalaDataTransferService
    {
        Utils.EscalaTransfer.IEscalaTransfer GetBrowserFileProvider(Func<IBrowserFile>? file = null);
        Utils.EscalaTransfer.IEscalaTransfer GetLocalStorageProvider();
        Utils.EscalaTransfer.IEscalaTransfer GetCloudProvider();
    }
}
