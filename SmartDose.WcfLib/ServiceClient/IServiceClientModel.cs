using System.Threading.Tasks;

namespace SmartDose.WcfLib
{
    public interface IServiceClientModel
    {
        Task<IServiceResult> ExecuteModelCreateAsync(CreateBuilder createBuilder);

        Task<IServiceResult<bool>> ExecuteModelDeleteAsync(DeleteBuilder deleteBuilder);

        Task<IServiceResult> ExecuteModelReadAsync(ReadBuilder readBuilder);

        Task<IServiceResult<long>> ExecuteModelReadIdOverIdentifierAsync(ReadIdOverIdentifierBuilder readIdOverIdentifierBuilder);

        Task<IServiceResult> ExecuteModelUpdateAsync(UpdateBuilder updateBuilder);
    }
}
