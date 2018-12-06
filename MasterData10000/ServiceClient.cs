using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace MasterData10000
{
    public class ServiceClient : ServiceClientBase, IMasterDataService, IMasterDataServiceCallback
    {
        public ServiceClient(string endpointAddress, SecurityMode securityMode = SecurityMode.None) : base(endpointAddress, securityMode)
        {
        }

        public new MasterDataServiceClient Client { get => (MasterDataServiceClient)base.Client; set => base.Client = value; }

        public override void CreateClient()
        {
            Binding binding = new NetTcpBinding(SecurityMode)
            {
                OpenTimeout = TimeSpan.FromSeconds(1),
                ReceiveTimeout = TimeSpan.FromSeconds(30),
                SendTimeout = TimeSpan.FromSeconds(30),
                CloseTimeout = TimeSpan.FromSeconds(1)
            };
            if (EndpointAddress.ToLower().StartsWith("http"))
                binding = new NetHttpBinding();

            Client = new MasterDataServiceClient(binding, new EndpointAddress(EndpointAddress));
        }

        #region Query

        public ServiceResultQueryResponse Query(QueryRequest queryRequest)
            => QueryAsync(queryRequest).Result;

        public async Task<ServiceResultQueryResponse> QueryAsync(QueryRequest queryRequest)
            => await CatcherServiceResultAsync(() => Client.QueryAsync(queryRequest)).ConfigureAwait(false);

        public async override Task<ServiceResult> ExecuteQueryBuilderAsync(QueryBuilder queryBuilder)
        {
            var queryBuildValues = queryBuilder.GetValues();
            var queryRequest = new QueryRequest
            {
                ModelName = queryBuildValues.ModelType.Name,
                ModelNamespace = queryBuildValues.ModelType.Namespace,
                WhereAsJson = queryBuildValues.WhereAsJson,
                OrderByAsJson = queryBuildValues.OrderByAsJson,
                OrderByAsc = queryBuildValues.OrderByAsc,
                OrderByAs = (QueryRequestOrderByAs)queryBuildValues.OrderByAs,
                Page = queryBuildValues.Page,
                PageSize = queryBuildValues.PageSize,
                ResultAs = (QueryRequestResultAs)queryBuildValues.ResultAs,
                WithDebugInfo = queryBuildValues.WithDebugInfo,
            };
            var serviceResult = await QueryAsync(queryRequest).ConfigureAwait(false);
            var newServiceResult = serviceResult.CastByClone<ServiceResult<string>>();
            newServiceResult.Data = serviceResult?.Data?.ZippedJsonData;
            return newServiceResult;
        }
        #endregion

        #region Wrapped abstract Client Calls

        public async override Task OpenAsync()
        {
            await CatcherAsyncIgnore(() => Client.OpenAsync()).ConfigureAwait(false);
        }

        public async override Task CloseAsync()
        {
            await CatcherAsyncIgnore(() => Client.CloseAsync()).ConfigureAwait(false);
        }

        public async override Task SubscribeForCallbacksAsync()
        {
            await CatcherAsyncIgnore(() => Client.SubscribeForCallbacksAsync()).ConfigureAwait(false);
        }

        public async override Task UnsubscribeForCallbacksAsync()
        {
            await CatcherAsyncIgnore(() => Client.UnsubscribeForCallbacksAsync()).ConfigureAwait(false);
        }

        protected override void AssignClientCallbacks(bool on)
        {
            switch (on)
            {
                case true:
                    Client.MedicinesChangedReceived += MedicinesChanged;
                    Client.MedicinesDeletedReceived += MedicinesDeleted;

                    break;
                case false:
                    Client.MedicinesChangedReceived -= MedicinesChanged;
                    Client.MedicinesDeletedReceived -= MedicinesDeleted;
                    break;
            }
        }
        #endregion

        #region Wrapped Client Callbacks

        public event Action<List<Medicine>> OnMedicinesChanged;
        public void MedicinesChanged(List<Medicine> medicines)
            => OnMedicinesChanged?.Invoke(medicines);
        protected void MedicinesChanged(object sender, MedicinesChangedReceivedEventArgs args)
            => MedicinesChanged(args.medicines);


        public event Action<List<string>> OnMedicinesDeleted;
        public void MedicinesDeleted(List<string> medicineIdentifiers)
        {
            OnMedicinesDeleted?.Invoke(medicineIdentifiers);
        }
        protected void MedicinesDeleted(object sender, MedicinesDeletedReceivedEventArgs args)
            => MedicinesDeleted(args.medicineIdentifiers);
        #endregion

        #region Wrapped Client Methods 

        #region Old Stuff
        public Medicine GetMedicineByIdentifierA(string medicineIdentifier)
            => GetMedicineByIdentifierAsync(medicineIdentifier).Result;
        public async Task<Medicine> GetMedicineByIdentifierAsync(string medicineIdentifier)
            => await CatcherAsync(() => Client.GetMedicineByIdentifierAsync(medicineIdentifier))
                    .ConfigureAwait(false);
        #endregion

        #region Medicine
        public ServiceResultLong MedicinesGetIdByIdentifier(string medicineIdentifier)
            => MedicinesGetIdByIdentifierAsync(medicineIdentifier).Result;

        public async Task<ServiceResultLong> MedicinesGetIdByIdentifierAsync(string medicineIdentifier)
            => await CatcherServiceResultAsync(() => Client.MedicinesGetIdByIdentifierAsync(medicineIdentifier))
                    .ConfigureAwait(false);

        public ServiceResultBool MedicinesDeleteByIdentifier(string medicineIdentifier)
             => MedicinesDeleteByIdentifierAsync(medicineIdentifier).Result;
        public async Task<ServiceResultBool> MedicinesDeleteByIdentifierAsync(string medicineIdentifier)
             => await CatcherServiceResultAsync(() => Client.MedicinesDeleteByIdentifierAsync(medicineIdentifier))
                    .ConfigureAwait(false);

        public ServiceResultMedicine MedicinesGetMedcineByIdentifier(string medicineIdentifier)
            => MedicinesGetMedcineByIdentifierAsync(medicineIdentifier).Result;
        public async Task<ServiceResultMedicine> MedicinesGetMedcineByIdentifierAsync(string medicineIdentifier)
            => await CatcherServiceResultAsync(() => Client.MedicinesGetMedcineByIdentifierAsync(medicineIdentifier))
                    .ConfigureAwait(false);

        public ServiceResultMedicineList MedicinesGetMedcinesByIdentifiers(List<string> medicineIdentifiers, int page, int pageSize)
            => MedicinesGetMedcinesByIdentifiersAsync(medicineIdentifiers, page, pageSize).Result;
        public async Task<ServiceResultMedicineList> MedicinesGetMedcinesByIdentifiersAsync(List<string> medicineIdentifiers, int page, int pageSize)
            => await CatcherServiceResultAsync(() => Client.MedicinesGetMedcinesByIdentifiersAsync(medicineIdentifiers, page, pageSize))
                    .ConfigureAwait(false);
        #endregion

        #region Canister
        public Task<ServiceResultLong> CanistersGetIdByIdentifierAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultBool> CanistersDeleteByIdentifierAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultCanister> CanistersGetCanisterByIdentifierAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultCanisterList> CanistersGetCanistersByIdentifiersAsync(List<string> identifiers, int page, int pageSize)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Customer
        public Task<ServiceResultLong> CustomersGetIdByIdentifierAsync(string customerIdentifier)
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

        public Task<ServiceResultCustomerList> CustomersGetCustomersByIdentifiersAsync(List<string> customerIdentifiers, int page, int pageSize)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region DestinationFacilities
        public Task<ServiceResultLong> DestinationFacilitiesGetIdByIdentifierAsync(string destinationFacilityIdentifier)
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

        public Task<ServiceResultDestinationFacilityList> DestinationFacilitiesGetDestinationFacilitiesByIdentifiersAsync(List<string> destinationFacilityIdentifiers, int page, int pageSize)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Manufacturers

        public Task<ServiceResultLong> ManufacturersGetIdByIdentifierAsync(string manufacturerIdentifier)
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

        public Task<ServiceResultManufacturerList> ManufacturersGetManufacturersByIdentifiersAsync(List<string> manufacturerIdentifiers, int page, int pageSize)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Patients
        public Task<ServiceResultLong> PatientsGetIdByIdentifierAsync(string patientIdentifier)
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

        public Task<ServiceResultPatientList> PatientsGetPatientsByIdentifiersAsync(List<string> patientIdentifiers, int page, int pageSize)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Trays

        public Task<ServiceResultLong> TraysGetIdByIdentifierAsync(string identifier)
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

        public Task<ServiceResultTrayList> TraysGetTraysByIdentifiersAsync(List<string> identifiers, int page, int pageSize)
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion


    }
}
