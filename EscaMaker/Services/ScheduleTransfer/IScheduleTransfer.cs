using EscaMaker.View;

namespace EscaMaker.Services.ScheduleTransfer
{
    public interface IScheduleTransfer
    {
        Task<bool> Export(string name, LocalScheduleData data);
        Task<LocalScheduleData?> Import(string nameId);

        Task<bool> Delete(string name);

        bool CanDelete { get; }

        Task<IEnumerable<string>> GetNames();
    }
}
