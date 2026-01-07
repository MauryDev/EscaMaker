using EscaMaker.View;

namespace EscaMaker.Utils.EscalaTransfer
{
    public interface IEscalaTransfer
    {
        Task<bool> Export(string name, EscalaLocalData data);
        Task<EscalaLocalData?> Import(string nameId);

        Task<bool> Delete(string name);

        bool CanDelete { get; }

        Task<IEnumerable<string>> GetNames();
    }
}
