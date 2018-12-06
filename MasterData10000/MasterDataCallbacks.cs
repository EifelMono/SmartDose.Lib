using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData10000
{
    public class MasterDataCallbacks : IMasterDataCallbackEvents
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
