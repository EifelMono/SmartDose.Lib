using System.Threading.Tasks;

namespace SmartDose.WcfLib
{
    public interface IServiceClientModel
    {
        Task<ServiceResult> ExecuteModelReadAsync(ReadBuilder readBuilder);

        Task<ServiceResult<bool>> ExecuteModelDeleteAsync(DeleteBuilder deleteBuilder);

        Task<ServiceResult<long>> ExecuteModelReadIdOverIdentifierAsync(ReadIdOverIdentifierBuilder readIdOverIdentifierBuilder);
    }
}
