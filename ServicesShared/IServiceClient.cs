#define UseModel
#if MasterData9002
#undef UseModel
#endif
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#if MasterData10000
namespace MasterData10000
#elif Settings10000
namespace Settings10000
#elif MasterData9002
namespace MasterData9002
#else
namespace ServicesShared
#endif
{
    public interface IServiceClient
    {
#if UseModel
        Task<ServiceResult> ExecuteModelQueryAsync(QueryBuilder queryBuilder);

        Task<ServiceResult<bool>> ExecuteModelDeleteAsync(DeleteBuilder deleteBuilder);

        Task<ServiceResult<long>> ExecuteModelIdentifierToIdAsync(string identifier, string modelName, string modelNamespace);
#endif
    }
}
