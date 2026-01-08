using Microsoft.AspNetCore.Components.Forms;

namespace EscaMaker.Services.ScheduleTransfer;

public interface IScheduleBrowserConfig
{
    IBrowserFile? BrowserFile { set; }
}
