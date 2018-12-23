using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartDose.WcfLib
{
    public class CreateBuilder : ModelBuilder
    {
        public CreateBuilder(IServiceClientModel client) : base(client)
        {
        }

        // Use deconstructor while protected properties 
        public (Type ModelType,
            bool DebugInfoFlag,
            bool TableOnlyFlag
            ) GetValues()
                => (ModelType, DebugInfoFlag, TableOnlyFlag);
    }

    public class CreateBuilder<TModel> : CreateBuilder where TModel : class
    {
        public CreateBuilder(IServiceClientModel client) : base(client)
        {
            ModelType = typeof(TModel);
        }

        #region Model
        public CreateBuilder<TModel> DebugInfoAll(bool debugInfoFlagAll)
        {
            ModelBuilder.DebugInfoAll(debugInfoFlagAll);
            return this;
        }
        public CreateBuilder<TModel> DebugInfo(bool debugInfoFlag)
        {
            DebugInfoFlag = debugInfoFlag;
            return this;
        }

        public CreateBuilder<TModel> TableOnly(bool tableOnlyFlag=true)
        {
            TableOnlyFlag = tableOnlyFlag;
            return this;
        }
        #endregion

        #region Execute

        public IServiceResult Execute()
            => ExecuteAsync().Result;

        public async Task<IServiceResult> ExecuteAsync()
        {
            return await Client.ExecModelCreateAsync(this).ConfigureAwait(false);
        }

        #endregion
    }
}
