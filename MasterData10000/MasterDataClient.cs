using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rowa.Lib.Wcf;

namespace MasterData10000
{
   public class MasterDataClient :
          ClientWithCallbackEvents<MasterDataServiceClient, IMasterDataService, IMasterDataServiceCallback, IMasterDataCallbackEvents>, IMasterDataService
    {
        public MasterDataClient(string serviceUrl,
            BindingType bindingType = BindingType.NetTcp,
            uint communicationTimeoutSeconds = 1800,
            uint connectionCheckIntervalSeconds = 5) :
            base(serviceUrl, new MasterDataCallbacks(), bindingType, communicationTimeoutSeconds, connectionCheckIntervalSeconds)
        {
        }

        #region QueryBuilder
        public async Task<ServiceResult> ExecuteQueryBuilderAsync(QueryBuilder queryBuilder)
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
            };
            var serviceResult = await QueryAsync(queryRequest).ConfigureAwait(false);
            var newServiceResult = serviceResult.CastByClone<ServiceResult<string>>();
            newServiceResult.Data = serviceResult?.Data?.ZippedJsonData;
            return newServiceResult;
        }
        #endregion

        #region Client Methods
        public Task<ServiceResultBool> CanistersDeleteByIdentifierAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultCanister> CanistersGetCanisterByIdentifierAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultCanisterList> CanistersGetCanistersByIdentifiersAsync(string[] identifiers, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultLong> CanistersGetIdByIdentifierAsync(string identifier)
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

        public Task<ServiceResultCustomerList> CustomersGetCustomersByIdentifiersAsync(string[] customerIdentifiers, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultLong> CustomersGetIdByIdentifierAsync(string customerIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultBool> DestinationFacilitiesDeleteByIdentifierAsync(string destinationFacilityIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultDestinationFacilityList> DestinationFacilitiesGetDestinationFacilitiesByIdentifiersAsync(string[] destinationFacilityIdentifiers, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultDestinationFacility> DestinationFacilitiesGetDestinationFacilityByIdentifierAsync(string destinationFacilityIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultLong> DestinationFacilitiesGetIdByIdentifierAsync(string destinationFacilityIdentifier)
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

        public Task<ServiceResultLong> ManufacturersGetIdByIdentifierAsync(string manufacturerIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultManufacturer> ManufacturersGetManufacturerByIdentifierAsync(string manufacturerIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultManufacturerList> ManufacturersGetManufacturersByIdentifiersAsync(string[] manufacturerIdentifiers, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultBool> MedicinesDeleteByIdentifierAsync(string medicineIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultLong> MedicinesGetIdByIdentifierAsync(string medicineIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultMedicine> MedicinesGetMedcineByIdentifierAsync(string medicineIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultMedicineList> MedicinesGetMedcinesByIdentifiersAsync(string[] medicineIdentifiers, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultBool> PatientsDeleteByIdentifierAsync(string patientIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultLong> PatientsGetIdByIdentifierAsync(string patientIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultPatient> PatientsGetPatientByIdentifierAsync(string patientIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultPatientList> PatientsGetPatientsByIdentifiersAsync(string[] patientIdentifiers, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultQueryResponse> QueryAsync(QueryRequest queryRequest)
        {
            throw new NotImplementedException();
        }

        public Task SubscribeForCallbacksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultBool> TraysDeleteByIdentifierAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultLong> TraysGetIdByIdentifierAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultTray> TraysGetTrayByIdentifierAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultTrayList> TraysGetTraysByIdentifiersAsync(string[] identifiers, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task UnsubscribeForCallbacksAsync()
        {
            throw new NotImplementedException();
        }
        #endregion Client Methods
    }
}
