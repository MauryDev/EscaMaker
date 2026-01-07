using EscaMaker.Services.Contracts;
using EscaMaker.Utils.EscalaTransfer;
using Microsoft.AspNetCore.Components.Forms;

namespace EscaMaker.Services
{
    public class ScheduleDataTransferService : IEscalaDataTransferService
    {
        
        IServiceProvider ServiceProvider { get; set; }
        public ScheduleDataTransferService(
           IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        IScheduleTransfer IEscalaDataTransferService.GetBrowserFileProvider(Func<IBrowserFile>? file)
        {
            var value = ActivatorUtilities.CreateInstance<ScheduleBrowserFileActions>(ServiceProvider);
            value.GetFile = file;
            return value;
        }

        IScheduleTransfer IEscalaDataTransferService.GetLocalStorageProvider()
        {
            return ActivatorUtilities.CreateInstance<ScheduleLocalStorageActions>(ServiceProvider);
        }

        IScheduleTransfer IEscalaDataTransferService.GetCloudProvider()
        {
            return ActivatorUtilities.CreateInstance<ScheduleCloudActions>(ServiceProvider);
        }
    }
}
