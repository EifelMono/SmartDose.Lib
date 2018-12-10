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

        protected bool TableOnlyFlag { get; set; } = false;

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
    }
}
