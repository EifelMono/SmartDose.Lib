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

        // Use deconstructor while protected properties 
        public (Type ModelType,
            bool DebugInfoFlag
            ) GetValues()
                => (ModelType, DebugInfoFlag);
    }

    public class UpdateBuilder<TModel> : UpdateBuilder where TModel : class
    {
        public UpdateBuilder(IServiceClientModel client) : base(client)
        {
        }
    }
}
