using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDose.WcfLib
{
    public class CreateBuilder : ModelBuilder
    {
        public CreateBuilder(IServiceClientModel client) : base(client)
        {
        }

        // Use deconstructor while protected properties 
        public (Type ModelType,
            bool DebugInfoFlag
            ) GetValues()
                => (ModelType, DebugInfoFlag);
    }

    public class CreateBuilder<TModel> : CreateBuilder where TModel : class
    {
        public CreateBuilder(IServiceClientModel client) : base(client)
        {
            ModelType = typeof(TModel);
        }
    }
}
