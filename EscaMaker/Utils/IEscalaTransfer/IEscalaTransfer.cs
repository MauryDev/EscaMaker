namespace EscaMaker.Utils.EscalaTransfer
{
    public interface IEscalaTransfer
    {
        Task<bool> Export(Utils.EscalaLocalData data);
        Task<Utils.EscalaLocalData?> Import();

        Task<bool> Delete();
    }
}
