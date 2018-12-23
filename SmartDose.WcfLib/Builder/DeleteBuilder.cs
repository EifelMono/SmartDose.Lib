using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serialize.Linq.Extensions;
using RowaMore.Extensions;

namespace SmartDose.WcfLib
{
    public class DeleteBuilder : ModelBuilder
    {
        public DeleteBuilder(IServiceClientModel client) : base(client)
        {
        }

        public string WhereAsJson { get; set; }

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
        public DeleteBuilder(IServiceClientModel client) : base(client)
        {
            ModelType = typeof(TModel);
        }

        #region Model
        public DeleteBuilder<TModel> DebugInfoAll(bool debugInfoFlagAll)
        {
            ModelBuilder.DebugInfoAll(debugInfoFlagAll);
            return this;
        }
        public DeleteBuilder<TModel> DebugInfo(bool debugInfoFlag)
        {
            DebugInfoFlag = debugInfoFlag;
            return this;
        }

        public DeleteBuilder<TModel> TableOnly(bool tableOnlyFlag=true)
        {
            TableOnlyFlag = tableOnlyFlag;
            return this;
        }
        #endregion

        #region Where

        public DeleteBuilder<TModel> Where(Expression<Func<TModel, bool>> whereExpression)
        {
            if (whereExpression != null)
                WhereAsJson = whereExpression.ToJson();
            return this;
        }

        #endregion

        #region Execute

        public IServiceResult<bool> Execute(Expression<Func<TModel, bool>> whereExpression = null)
            => ExecuteAsync(whereExpression).Result;

        public async Task<IServiceResult<bool>> ExecuteAsync(Expression<Func<TModel, bool>> whereExpression = null)
        {
            Where(whereExpression);
            return await Client.ExecModelDeleteAsync(this).ConfigureAwait(false);
        }

        #endregion
    }
}
