﻿using System;
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

        internal async override Task<ServiceResult> ExecuteQueryAsync(QueryBuilder queryBuilder)
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
                TableOnlyFlag = queryBuildValues.TableOnlyFlag,
                DebugInfoFlag = queryBuildValues.DebugInfoFlag,
            };
            var serviceResult = await QueryAsync(queryRequest).ConfigureAwait(false);
            var newServiceResult = serviceResult.CastByClone<ServiceResult<string>>();
            newServiceResult.Data = serviceResult?.Data?.ZippedJsonData;
            return newServiceResult;
        }
        #endregion

        #region IdentifierToId
        public ServiceResultLong IdentifierToId(IdentifierToIdRequest identifierToIdRequest)
            => IdentifierToIdAsync(identifierToIdRequest).Result;

        public async Task<ServiceResultLong> IdentifierToIdAsync(IdentifierToIdRequest identifierToIdRequest)
               => await CatcherServiceResultAsync(() => Client.IdentifierToIdAsync(identifierToIdRequest)).ConfigureAwait(false);

        internal async override Task<ServiceResult<long>> ExecuteIdentifierToIdAsync(string identifier, string modelName, string modelNamespace)
            => (await IdentifierToIdAsync(new IdentifierToIdRequest
            {
                ModelName = modelName,
                ModelNamespace = modelNamespace,
                Identifier = identifier
            }).ConfigureAwait(false)).CastByClone<ServiceResult<long>>();
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
        #endregion

        #region Canister
        public ServiceResultBool CanistersDeleteByIdentifier(string CanisterIdentifier)
            => CanistersDeleteByIdentifierAsync(CanisterIdentifier).Result;
        public async Task<ServiceResultBool> CanistersDeleteByIdentifierAsync(string CanisterIdentifier)
             => await CatcherServiceResultAsync(() => Client.CanistersDeleteByIdentifierAsync(CanisterIdentifier))
                    .ConfigureAwait(false);

        public ServiceResultCanister CanistersGetCanisterByIdentifier(string CanisterIdentifier)
            => CanistersGetCanisterByIdentifierAsync(CanisterIdentifier).Result;
        public async Task<ServiceResultCanister> CanistersGetCanisterByIdentifierAsync(string CanisterIdentifier)
            => await CatcherServiceResultAsync(() => Client.CanistersGetCanisterByIdentifierAsync(CanisterIdentifier))
                    .ConfigureAwait(false);
        #endregion

        #region Customer
        public ServiceResultBool CustomersDeleteByIdentifier(string CustomerIdentifier)
            => CustomersDeleteByIdentifierAsync(CustomerIdentifier).Result;
        public async Task<ServiceResultBool> CustomersDeleteByIdentifierAsync(string CustomerIdentifier)
             => await CatcherServiceResultAsync(() => Client.CustomersDeleteByIdentifierAsync(CustomerIdentifier))
                    .ConfigureAwait(false);

        public ServiceResultCustomer CustomersGetCustomerByIdentifier(string CustomerIdentifier)
            => CustomersGetCustomerByIdentifierAsync(CustomerIdentifier).Result;
        public async Task<ServiceResultCustomer> CustomersGetCustomerByIdentifierAsync(string CustomerIdentifier)
            => await CatcherServiceResultAsync(() => Client.CustomersGetCustomerByIdentifierAsync(CustomerIdentifier))
                    .ConfigureAwait(false);
        #endregion

        #region DestinationFacilities
        public ServiceResultBool DestinationFacilitiesDeleteByIdentifier(string DestinationFacilityIdentifier)
           => DestinationFacilitiesDeleteByIdentifierAsync(DestinationFacilityIdentifier).Result;
        public async Task<ServiceResultBool> DestinationFacilitiesDeleteByIdentifierAsync(string DestinationFacilityIdentifier)
            => await CatcherServiceResultAsync(() => Client.DestinationFacilitiesDeleteByIdentifierAsync(DestinationFacilityIdentifier))
                    .ConfigureAwait(false);

        public ServiceResultDestinationFacility DestinationFacilitiesGetDestinationFacilityByIdentifier(string DestinationFacilityIdentifier)
            => DestinationFacilitiesGetDestinationFacilityByIdentifierAsync(DestinationFacilityIdentifier).Result;
        public async Task<ServiceResultDestinationFacility> DestinationFacilitiesGetDestinationFacilityByIdentifierAsync(string DestinationFacilityIdentifier)
            => await CatcherServiceResultAsync(() => Client.DestinationFacilitiesGetDestinationFacilityByIdentifierAsync(DestinationFacilityIdentifier))
                    .ConfigureAwait(false);
        #endregion

        #region Manufacturers
        public ServiceResultBool ManufacturersDeleteByIdentifier(string ManufacturerIdentifier)
                 => ManufacturersDeleteByIdentifierAsync(ManufacturerIdentifier).Result;
        public async Task<ServiceResultBool> ManufacturersDeleteByIdentifierAsync(string ManufacturerIdentifier)
             => await CatcherServiceResultAsync(() => Client.ManufacturersDeleteByIdentifierAsync(ManufacturerIdentifier))
                    .ConfigureAwait(false);

        public ServiceResultManufacturer ManufacturersGetManufacturerByIdentifier(string ManufacturerIdentifier)
            => ManufacturersGetManufacturerByIdentifierAsync(ManufacturerIdentifier).Result;
        public async Task<ServiceResultManufacturer> ManufacturersGetManufacturerByIdentifierAsync(string ManufacturerIdentifier)
            => await CatcherServiceResultAsync(() => Client.ManufacturersGetManufacturerByIdentifierAsync(ManufacturerIdentifier))
                    .ConfigureAwait(false);
        #endregion

        #region Patients
        public ServiceResultBool PatientsDeleteByIdentifier(string PatientIdentifier)
              => PatientsDeleteByIdentifierAsync(PatientIdentifier).Result;
        public async Task<ServiceResultBool> PatientsDeleteByIdentifierAsync(string PatientIdentifier)
             => await CatcherServiceResultAsync(() => Client.PatientsDeleteByIdentifierAsync(PatientIdentifier))
                    .ConfigureAwait(false);

        public ServiceResultPatient PatientsGetPatientByIdentifier(string PatientIdentifier)
            => PatientsGetPatientByIdentifierAsync(PatientIdentifier).Result;
        public async Task<ServiceResultPatient> PatientsGetPatientByIdentifierAsync(string PatientIdentifier)
            => await CatcherServiceResultAsync(() => Client.PatientsGetPatientByIdentifierAsync(PatientIdentifier))
                    .ConfigureAwait(false);
        #endregion

        #region Trays
        public ServiceResultBool TraysDeleteByIdentifier(string TrayIdentifier)
              => TraysDeleteByIdentifierAsync(TrayIdentifier).Result;
        public async Task<ServiceResultBool> TraysDeleteByIdentifierAsync(string TrayIdentifier)
             => await CatcherServiceResultAsync(() => Client.TraysDeleteByIdentifierAsync(TrayIdentifier))
                    .ConfigureAwait(false);

        public ServiceResultTray TraysGetTrayByIdentifier(string TrayIdentifier)
            => TraysGetTrayByIdentifierAsync(TrayIdentifier).Result;
        public async Task<ServiceResultTray> TraysGetTrayByIdentifierAsync(string TrayIdentifier)
            => await CatcherServiceResultAsync(() => Client.TraysGetTrayByIdentifierAsync(TrayIdentifier))
                    .ConfigureAwait(false);
        #endregion

        #endregion
    }
}
