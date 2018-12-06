using System;
using WcfConsoleTestFramework.ServiceReference1;

namespace WcfConsoleTestFramework
{
    /// <summary>
    ///     Defines the master data service callback events.
    /// </summary>
    public interface IMasterDataCallbackEvents : IMasterDataServiceCallback
    {
        event Action<object, Medicine[]> OnMedicinesChanged;
        event Action<object, string[]> OnMedicinesDeleted;
    }
}
