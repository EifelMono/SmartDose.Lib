using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDose.WcfLib
{
    public class UpdateBuilder : ModelBuilder
    {
        public UpdateBuilder(IServiceClientModel client) : base(client)
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

    public class UpdateBuilder<TModel> : UpdateBuilder where TModel : class
    {
        public UpdateBuilder(IServiceClientModel client) : base(client)
        {
        }
    }
}
