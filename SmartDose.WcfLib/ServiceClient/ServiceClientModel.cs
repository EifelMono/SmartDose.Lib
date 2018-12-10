
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SmartDose.WcfLib
{
    public abstract class ServiceClientModel: ServiceClient, IServiceClientModel
    {
        public ServiceClientModel(string endpointAddress, SecurityMode securityMode = SecurityMode.None): 
            base(endpointAddress, securityMode)
        {
        }

        public ModelBuilder<T> Model<T>() where T : class
        {
            return new ModelBuilder<T>(this) { };
        }

        #region Create

        public abstract Task<IServiceResult> ExecuteModelCreateAsync(CreateBuilder createBuilder);

        public CreateBuilder<T> ModelCreate<T>() where T : class
        {
            return new CreateBuilder<T>(this) { };
        }

        #endregion

        #region Delete
        public abstract Task<IServiceResult<bool>> ExecuteModelDeleteAsync(DeleteBuilder deleteBuilder);

        public DeleteBuilder<T> ModelDelete<T>() where T : class
        {
            return new DeleteBuilder<T>(this) { };
        }
        #endregion

        #region Read
        public abstract Task<IServiceResult> ExecuteModelReadAsync(ReadBuilder readBuilder);

        public ReadBuilder<T> ModelRead<T>() where T : class
        {
            return new ReadBuilder<T>(this) { };
        }
        #endregion

        #region ReadIdOverIdentifier
        public abstract Task<IServiceResult<long>> ExecuteModelReadIdOverIdentifierAsync(ReadIdOverIdentifierBuilder readIdOverIdentifierBuilder);

        public ReadIdOverIdentifierBuilder<T> ModelReadIdOverIdentifier<T>() where T : class
        {
            return new ReadIdOverIdentifierBuilder<T>(this) { };
        }

        #endregion

        #region Update

        public abstract Task<IServiceResult> ExecuteModelUpdateAsync(UpdateBuilder UpdateBuilder);

        public UpdateBuilder<T> ModelUpdate<T>() where T : class
        {
            return new UpdateBuilder<T>(this) { };
        }

        #endregion
    }
}
