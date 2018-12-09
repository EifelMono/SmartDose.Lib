using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartDose.WcfLib;

namespace SmartDose.WcfMasterData10000
{
    public class ServiceClientMasterData100000 : ServiceClientModel, IMasterDataService, IMasterDataServiceCallback
    {
        #region ServiceClient
        public override Task CloseAsync()
        {
            throw new NotImplementedException();
        }

        public override Task OpenAsync()
        {
            throw new NotImplementedException();
        }

        public override Task SubscribeForCallbacksAsync()
        {
            throw new NotImplementedException();
        }

        public override Task UnsubscribeForCallbacksAsync()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ServiceClientMode
        public override Task<WcfLib.ServiceResult<bool>> ExecuteModelDeleteAsync(DeleteBuilder deleteBuilder)
        {
            throw new NotImplementedException();
        }

        public override Task<WcfLib.ServiceResult> ExecuteModelReadAsync(ReadBuilder readBuilder)
        {
            throw new NotImplementedException();
        }

        public override Task<WcfLib.ServiceResult<long>> ExecuteModelReadIdOverIdentifierAsync(ReadIdOverIdentifierBuilder readIdOverIdentifierBuilder)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region CallBacks
        protected override void AssignClientCallbacks(bool on)
        {
            throw new NotImplementedException();
        }

        public void MedicinesChanged(List<Medicine> medicines)
        {
            throw new NotImplementedException();
        }

        public void MedicinesDeleted(List<string> medicineIdentifiers)
        {
            throw new NotImplementedException();
        }
        #endregion


        public Task<ServiceResultBool> CanistersDeleteByIdentifierAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultCanister> CanistersGetCanisterByIdentifierAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultBool> CustomersDeleteByIdentifierAsync(string customerIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultCustomer> CustomersGetCustomerByIdentifierAsync(string customerIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultBool> DestinationFacilitiesDeleteByIdentifierAsync(string destinationFacilityIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultDestinationFacility> DestinationFacilitiesGetDestinationFacilityByIdentifierAsync(string destinationFacilityIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<Medicine> GetMedicineByIdentifierAsync(string medicineIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultBool> ManufacturersDeleteByIdentifierAsync(string manufacturerIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultManufacturer> ManufacturersGetManufacturerByIdentifierAsync(string manufacturerIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultBool> MedicinesDeleteByIdentifierAsync(string medicineIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultMedicine> MedicinesGetMedcineByIdentifierAsync(string medicineIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultBool> ModelDeleteAsync(ModelDeleteRequest modelDeleteRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultModelReadResponse> ModelReadAsync(ModelReadRequest modelReadRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultLong> ModelReadIdOverIdentifierAsync(ModelReadIdOverIdentifierRequest modelReadIdOverIdentifierRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultBool> PatientsDeleteByIdentifierAsync(string patientIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultPatient> PatientsGetPatientByIdentifierAsync(string patientIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultBool> TraysDeleteByIdentifierAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultTray> TraysGetTrayByIdentifierAsync(string identifier)
        {
            throw new NotImplementedException();
        }
    }
}
