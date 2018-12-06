using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData10000
{
    public interface IMasterDataCallbackEvents : IMasterDataServiceCallback
    {
        event Action<object, Medicine[]> OnMedicinesChanged;
        event Action<object, string[]> OnMedicinesDeleted;
    }
}
