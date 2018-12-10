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
    }

    public class UpdateBuilder<TModel> : UpdateBuilder where TModel : class
    {
        public UpdateBuilder(IServiceClientModel client) : base(client)
        {
        }
    }
}
