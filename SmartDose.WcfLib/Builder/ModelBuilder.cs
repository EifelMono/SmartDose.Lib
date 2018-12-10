using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDose.WcfLib
{
    public class ModelBuilder
    {
        public ModelBuilder(IServiceClientModel client)
        {
            Client = client;
        }

        protected IServiceClientModel Client { get; set; }

        protected Type ModelType { get; set; }

        protected bool _DebugInfoFlag { get; set; } = false;
        protected bool DebugInfoFlag
        {
            get => DebugInfoAllFlag ? DebugInfoAllFlag : _DebugInfoFlag;
            set => _DebugInfoFlag = value;
        }

        private static bool DebugInfoAllFlag { get; set; } = false;

        public static void SwitchDebugInfoAll(bool flag)
            => DebugInfoAllFlag = flag;
    }

    public class ModelBuilder<TModel> : ModelBuilder where TModel : class
    {
        public ModelBuilder(IServiceClientModel client) : base(client)
        {
            ModelType = typeof(TModel);
        }

        public ReadBuilder<TModel> Read()
        {
            return new ReadBuilder<TModel>(Client);
        }
        public DeleteBuilder<TModel> Delete()
        {
            return new DeleteBuilder<TModel>(Client);
        }

        public ReadIdOverIdentifierBuilder<TModel> ReadIdOverIdentifier()
        {
            return new ReadIdOverIdentifierBuilder<TModel>(Client);
        }
    }
}
