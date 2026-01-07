using Microsoft.AspNetCore.Components.Forms;

namespace EscaMaker.Services.Contracts
{
    public interface IEscalaDataTransferService
    {
        Utils.EscalaTransfer.IScheduleTransfer GetBrowserFileProvider(Func<IBrowserFile>? file = null);
        Utils.EscalaTransfer.IScheduleTransfer GetLocalStorageProvider();
        Utils.EscalaTransfer.IScheduleTransfer GetCloudProvider();
    }
}
