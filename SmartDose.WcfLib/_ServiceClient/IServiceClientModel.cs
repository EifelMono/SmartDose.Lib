using System.Threading.Tasks;

namespace SmartDose.WcfLib
{
    public interface IServiceClientModel
    {
        Task<IServiceResult> ExecModelCreateAsync(CreateBuilder createBuilder);

        Task<IServiceResult<bool>> ExecModelDeleteAsync(DeleteBuilder deleteBuilder);

        Task<IServiceResult> ExecModelReadAsync(ReadBuilder readBuilder);

        Task<IServiceResult<long>> ExecModelReadIdOverIdentifierAsync(ReadIdOverIdentifierBuilder readIdOverIdentifierBuilder);

        Task<IServiceResult> ExecModelUpdateAsync(UpdateBuilder updateBuilder);
    }
}
