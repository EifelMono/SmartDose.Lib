namespace SmartDose.WcfLib
{
    public class ModelBuilderStart<TModel> : ModelBuilder where TModel : class
    {
        public ModelBuilderStart(IServiceClientModel client) : base(client)
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
