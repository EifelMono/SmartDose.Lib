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
    public class DeleteBuilder
    {
        public DeleteBuilder(ServiceClientBase client)
        {
            Client = client;
        }

        protected ServiceClientBase Client { get; set; }

        protected string WhereAsJson { get; set; } = string.Empty;

        protected Type ModelType { get; set; }

        protected bool TableOnlyFlag { get; set; } = false;
        protected bool _DebugInfoFlag { get; set; } = false;
        protected bool DebugInfoFlag
        {
            get => DebugInfoAllFlag ? DebugInfoAllFlag : _DebugInfoFlag;
            set => _DebugInfoFlag = value;
        }

        private static bool DebugInfoAllFlag { get; set; } = false;

        public static void SwitchDebugInfoAll(bool flag)
            => DebugInfoAllFlag = flag;

        // Use deconstructor while protected properties 
        public (string WhereAsJson,
            Type ModelType,
            bool TableOnlyFlag,
            bool DebugInfoFlag) GetValues()
                => (WhereAsJson, ModelType, TableOnlyFlag, DebugInfoFlag);
    }

    public class DeleteBuilder<TModel> : DeleteBuilder where TModel : class
    {
        public DeleteBuilder(ServiceClientBase client) : base(client)
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
            return await Client.ExecuteDeleteAsync(this).ConfigureAwait(false);
        }
    }
}
