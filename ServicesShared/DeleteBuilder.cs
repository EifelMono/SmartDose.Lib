using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serialize.Linq.Extensions;
using RowaMore.Extensions;

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
    public class DeleteBuilder: ModelBuilder
    {
        public DeleteBuilder(IServiceClient client): base(client)
        {
        }

        protected bool TableOnlyFlag { get; set; } = false;

        protected string WhereAsJson { get; set; } = string.Empty;

        // Use deconstructor while protected properties 
        public (Type ModelType,
            bool DebugInfoFlag,
            bool TableOnlyFlag,
            string WhereAsJson
            ) GetValues()
                => (ModelType, DebugInfoFlag, TableOnlyFlag, WhereAsJson);
    }

    public class DeleteBuilder<TModel> : DeleteBuilder where TModel : class
    {
        public DeleteBuilder(IServiceClient client) : base(client)
        {
            ModelType = typeof(TModel);
        }

        public DeleteBuilder<TModel> Where(Expression<Func<TModel, bool>> whereExpression)
        {
            if (whereExpression != null)
                WhereAsJson = whereExpression.ToJson();
            return this;
        }

        public DeleteBuilder<TModel> UseTableOnly(bool tableOnlyFlag = true)
        {
            TableOnlyFlag = tableOnlyFlag;
            return this;
        }

        public DeleteBuilder<TModel> UseDebugInfo(bool debugInfoFlag = true)
        {
            DebugInfoFlag = debugInfoFlag;
            return this;
        }

        public DeleteBuilder<TModel> UseDebugInfoAll(bool debugInfoAllFlag)
        {
            SwitchDebugInfoAll(debugInfoAllFlag);
            return this;
        }

        public ServiceResult<bool> Execute(Expression<Func<TModel, bool>> whereExpression = null) 
            => ExecuteAsync(whereExpression).Result;


        public async Task<ServiceResult<bool>> ExecuteAsync(Expression<Func<TModel, bool>> whereExpression = null)
        {
            Where(whereExpression);
            return await Client.ExecuteModelDeleteAsync(this).ConfigureAwait(false);
        }
    }
}
