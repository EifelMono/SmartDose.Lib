using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartDose.WcfLib
{
    public interface IServiceClient
    {
        void CreateClient();

        Task OpenAsync();

        Task CloseAsync();

        Task SubscribeForCallbacksAsync();

        Task UnsubscribeForCallbacksAsync();
    }
}
