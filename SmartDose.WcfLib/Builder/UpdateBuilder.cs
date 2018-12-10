using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RowaMore.Extensions;

namespace SmartDose.WcfLib
{
    public class UpdateBuilder : ModelBuilder
    {
        public UpdateBuilder(IServiceClientModel client) : base(client)
        {
        }

        // Use deconstructor while protected properties 
        public string WhereAsJson { get; set; }

        // Use deconstructor while protected properties 
        public (Type ModelType,
            bool DebugInfoFlag,
            bool TableOnlyFlag,
            string WhereAsJson
            ) GetValues()
                => (ModelType, DebugInfoFlag, TableOnlyFlag, WhereAsJson);
    }

    public class UpdateBuilder<TModel> : UpdateBuilder where TModel : class
    {
        public UpdateBuilder(IServiceClientModel client) : base(client)
        {
        }

        #region Modelbuilder
        public UpdateBuilder<TModel> SetDebugInfoFlagAll(bool debugInfoFlagAll)
        {
            SetDebugInfoFlagAll(debugInfoFlagAll);
            return this;
        }
        public UpdateBuilder<TModel> SetDebugInfoFlag(bool debugInfoFlag)
        {
            DebugInfoFlag = debugInfoFlag;
            return this;
        }

        public UpdateBuilder<TModel> SetTableOnlyFlag(bool tableOnlyFlag)
        {
            TableOnlyFlag = tableOnlyFlag;
            return this;
        }
        #endregion

        #region WhereBuilder

        public UpdateBuilder<TModel> Where(Expression<Func<TModel, bool>> whereExpression)
        {
            if (whereExpression != null)
                WhereAsJson = whereExpression.ToJson();
            return this;
        }

        #endregion

        #region ExecuteBuilder

        public IServiceResult Execute(Expression<Func<TModel, bool>> whereExpression = null)
            => ExecuteAsync(whereExpression).Result;

        public async Task<IServiceResult> ExecuteAsync(Expression<Func<TModel, bool>> whereExpression = null)
        {
            Where(whereExpression);
            return await Client.ExecuteModelUpdateAsync(this).ConfigureAwait(false);
        }

        #endregion
    }
}
