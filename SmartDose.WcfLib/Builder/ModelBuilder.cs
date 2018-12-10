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
            get => DebugInfoFlagAll ? DebugInfoFlagAll : _DebugInfoFlag;
            set => _DebugInfoFlag = value;
        }

        private static bool DebugInfoFlagAll { get; set; } = false;

        public static void SwitchDebugInfoFlagAll(bool on)
            => DebugInfoFlagAll = on;
    }

    public class ModelBuilder<TModel> : ModelBuilder where TModel : class
    {
        public ModelBuilder(IServiceClientModel client) : base(client)
        {
            ModelType = typeof(TModel);
        }

        public CreateBuilder<TModel> Create()
            => new CreateBuilder<TModel>(Client);

        public DeleteBuilder<TModel> Delete()
            => new DeleteBuilder<TModel>(Client);

        public ReadBuilder<TModel> Read()
            => new ReadBuilder<TModel>(Client);

        public ReadIdOverIdentifierBuilder<TModel> ReadIdOverIdentifier()
            => new ReadIdOverIdentifierBuilder<TModel>(Client);

        public UpdateBuilder<TModel> Update()
            => new UpdateBuilder<TModel>(Client);
    }
}
