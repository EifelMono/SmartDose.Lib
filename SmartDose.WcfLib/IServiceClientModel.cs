using System.Threading.Tasks;

namespace SmartDose.WcfLib
{
    public interface IServiceClientModel
    {
        Task<IServiceResult> ExecuteModelReadAsync(ReadBuilder readBuilder);

        Task<IServiceResult<bool>> ExecuteModelDeleteAsync(DeleteBuilder deleteBuilder);

        Task<IServiceResult<long>> ExecuteModelReadIdOverIdentifierAsync(ReadIdOverIdentifierBuilder readIdOverIdentifierBuilder);
    }
}
