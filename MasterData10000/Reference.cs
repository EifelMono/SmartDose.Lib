using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData10000
{
    public partial class MasterDataServiceClientBase : System.ServiceModel.DuplexClientBase<MasterData10000.IMasterDataService>, MasterData10000.IMasterDataService
    {
        public virtual void SubscribeForCallbacks()
        {
            SubscribeForCallbacksAsync().Wait();
        }

        public virtual void UnsubscribeForCallbacks()
        {
            UnsubscribeForCallbacksAsync().Wait();
        }
    }
}

