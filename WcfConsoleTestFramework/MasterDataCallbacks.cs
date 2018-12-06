using System;
using WcfConsoleTestFramework.ServiceReference1;

namespace WcfConsoleTestFramework
{
    /// <summary>
    ///     Implementation of the SmartDose server callbacks interface.
    ///     This class receives all the callbacks from SmartDose and dispatches them accordingly.
    /// </summary>
    /// <seealso cref="IMasterDataServiceCallback" />
    internal class MasterDataCallbacks : IMasterDataCallbackEvents
    {
        #region Implementation IMasterDataNgCallbackEvents

        public event Action<object, Medicine[]> OnMedicinesChanged;
        public event Action<object, string[]> OnMedicinesDeleted;

        #endregion Implementation IMasterDataCallbackEvents

        #region Medicine

        void IMasterDataServiceCallback.MedicinesChanged(Medicine[] medicines)
        {
            OnMedicinesChanged?.Invoke(this, medicines);
        }

        void IMasterDataServiceCallback.MedicinesDeleted(string[] medicineIdentifiers)
        {
            OnMedicinesDeleted?.Invoke(this, medicineIdentifiers);
        }

        #endregion
    }
}
