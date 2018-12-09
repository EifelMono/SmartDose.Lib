
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

        #region Read
        public abstract Task<ServiceResult> ExecuteModelReadAsync(ReadBuilder readBuilder);

        public ReadBuilder<T> ModelRead<T>() where T : class
        {
            return new ReadBuilder<T>(this) { };
        }
        #endregion

        #region Delete
        public abstract Task<ServiceResult<bool>> ExecuteModelDeleteAsync(DeleteBuilder deleteBuilder);

        public DeleteBuilder<T> ModelDelete<T>() where T : class
        {
            return new DeleteBuilder<T>(this) { };
        }
        #endregion

        #region ReadIdOverIdentifier

        public abstract Task<ServiceResult<long>> ExecuteModelReadIdOverIdentifierAsync(ReadIdOverIdentifierBuilder readIdOverIdentifierBuilder);

        public ReadIdOverIdentifierBuilder<T> ModelReadIdOverIdentifier<T>() where T : class
        {
            return new ReadIdOverIdentifierBuilder<T>(this) { };
        }

        #endregion
    }
}
