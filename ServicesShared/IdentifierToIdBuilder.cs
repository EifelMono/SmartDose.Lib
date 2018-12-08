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
    public class IdentifierToIdBuilder : ModelBuilder
    {
        public IdentifierToIdBuilder(IServiceClient client) : base(client)
        {
        }

        protected string Identifier { get; set; }
        // Use deconstructor while protected properties 
        public (Type ModelType,
            bool DebugInfoFlag,
            string Identifier
            ) GetValues()
                => (ModelType, DebugInfoFlag, Identifier);
    }

    public class IdentifierToIdBuilder<TModel> : IdentifierToIdBuilder where TModel : class
    {
        public IdentifierToIdBuilder(IServiceClient client) : base(client)
        {
            ModelType = typeof(TModel);
        }

        public IdentifierToIdBuilder<TModel> Where(string identifier)
        {
            if (identifier != null)
                Identifier = identifier;
            return this;
        }

        public ServiceResult<long> FirstOrDefault(string identifier= null)
            => FirstOrDefaultAsync(identifier).Result;

        public async Task<ServiceResult<long>> FirstOrDefaultAsync(string identifier = null)
        {
            Where(identifier);
            return await Client.ExecuteModelIdentifierToIdAsync(this).ConfigureAwait(false);
        }
    }
}
