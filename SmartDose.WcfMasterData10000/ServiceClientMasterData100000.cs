using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using RowaMore.Extensions;
using SmartDose.WcfLib;

namespace SmartDose.WcfMasterData10000
{
    public class ServiceClientMasterData100000 : ServiceClientModel, IMasterDataService, IMasterDataServiceCallback
    {
        public ServiceClientMasterData100000(string endpointAddress, SecurityMode securityMode = SecurityMode.None) : base(endpointAddress, securityMode)
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

        protected async Task<TResult> CatcherServiceResultAsync<TResult>(Func<Task<TResult>> func) where TResult : ServiceResult, new()
        {
            try
            {
                return await func().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ThrowOnConnectionError)
                    throw ex;
                return new TResult { Exception = ex, Status = ServiceResultStatus.ErrorConnection };
            }
        }

        #region IServiceClient
        // Done in SmartDose.WcfLib by Reflection
        #endregion

        #region ServiceClientModel

        #region Create

        public ServiceResultModelCreateResponse ModelCreate(ModelCreateRequest modelCreateRequest)
            => ModelCreateAsync(modelCreateRequest).Result;

        public async Task<ServiceResultModelCreateResponse> ModelCreateAsync(ModelCreateRequest modelCreateRequest)
            => await CatcherServiceResultAsync(() => Client.ModelCreateAsync(modelCreateRequest)).ConfigureAwait(false);

        public async override Task<WcfLib.IServiceResult> ExecuteModelCreateAsync(CreateBuilder createBuilder)
        {
            var buildValues = createBuilder.GetValues();
            var request = new ModelCreateRequest
            {
                ModelType = new ModelClassType
                {
                    Name = buildValues.ModelType?.Name ?? "",
                    Namespace = buildValues.ModelType?.Namespace ?? "",
                    AssemblyName = buildValues.ModelType?.Assembly?.GetName()?.Name?? ""
                },
                DebugInfoFlag = buildValues.DebugInfoFlag,
                TableOnlyFlag = buildValues.TableOnlyFlag
            };
            var serviceResult = await ModelCreateAsync(request).ConfigureAwait(false);
            var newServiceResult = serviceResult.CastByClone<ServiceResult<string>>();
            newServiceResult.Data = serviceResult?.Data?.ZippedJsonData;
            return newServiceResult;
        }
        #endregion

        #region Delete

        public ServiceResultBool ModelDelete(ModelDeleteRequest modelDeleteRequest)
            => ModelDeleteAsync(modelDeleteRequest).Result;

        public async Task<ServiceResultBool> ModelDeleteAsync(ModelDeleteRequest modelDeleteRequest)
            => await CatcherServiceResultAsync(() => Client.ModelDeleteAsync(modelDeleteRequest)).ConfigureAwait(false);

        public async override Task<IServiceResult<bool>> ExecuteModelDeleteAsync(DeleteBuilder deleteBuilder)
        {
            var buildValues = deleteBuilder.GetValues();
            var request = new ModelDeleteRequest
            {
                ModelType = new ModelClassType
                {
                    Name = buildValues.ModelType?.Name ?? "",
                    Namespace = buildValues.ModelType?.Namespace ?? "",
                    AssemblyName = buildValues.ModelType?.Assembly?.GetName()?.Name ?? ""
                },
                DebugInfoFlag = buildValues.DebugInfoFlag,
                TableOnlyFlag = buildValues.TableOnlyFlag,
                WhereAsJson = buildValues.WhereAsJson,
            };
            return (await ModelDeleteAsync(request).ConfigureAwait(false)).CastByClone<ServiceResult<bool>>();
        }
        #endregion

        #region Read
        public ServiceResultModelReadResponse ModelRead(ModelReadRequest modelReadRequest)
            => ModelReadAsync(modelReadRequest).Result;

        public async Task<ServiceResultModelReadResponse> ModelReadAsync(ModelReadRequest modelReadRequest)
            => await CatcherServiceResultAsync(() => Client.ModelReadAsync(modelReadRequest)).ConfigureAwait(false);

        public async override Task<IServiceResult> ExecuteModelReadAsync(ReadBuilder readBuilder)
        {
            var buildValues = readBuilder.GetValues();
            var request = new ModelReadRequest
            {
                ModelType = new ModelClassType
                {
                    Name = buildValues.ModelType?.Name ?? "",
                    Namespace = buildValues.ModelType?.Namespace ?? "",
                    AssemblyName = buildValues.ModelType?.Assembly?.GetName()?.Name ?? ""
                },
                DebugInfoFlag = buildValues.DebugInfoFlag,
                TableOnlyFlag = buildValues.TableOnlyFlag,
                WhereAsJson = buildValues.WhereAsJson,
                OrderByAsJson = buildValues.OrderByAsJson,
                OrderByType = new ModelClassType
                {
                    Name = buildValues.OrderByType?.Name ?? "",
                    Namespace = buildValues.OrderByType?.Namespace ?? "",
                    AssemblyName = buildValues.OrderByType?.Assembly?.GetName()?.Name ?? ""
                },
                OrderByAsc = buildValues.OrderByAsc,
                SelectAsJson = buildValues.SelectAsJson,
                SelectType = new ModelClassType
                {
                    Name = buildValues.SelectType?.Name ?? "",
                    Namespace = buildValues.SelectType?.Namespace ?? "",
                    AssemblyName = buildValues.SelectType?.Assembly?.GetName()?.Name ?? "",
                    TypeAsJson = buildValues.SelectType?.ToJson()
                },
                Page = buildValues.Page,
                PageSize = buildValues.PageSize,
                ResultType = new ModelClassType
                {
                    Name = buildValues.ResutType?.Name ?? "",
                    Namespace = buildValues.ResutType?.Namespace ?? "",
                    AssemblyName = buildValues.ResutType?.Assembly?.GetName()?.Name ?? ""
                },
            };
            var serviceResult = await ModelReadAsync(request).ConfigureAwait(false);
            var newServiceResult = serviceResult.CastByClone<ServiceResult<string>>();
            newServiceResult.Data = serviceResult?.Data?.ZippedJsonData;
            return newServiceResult;
        }
        #endregion

        #region ReadIdOverIdentifier
        public ServiceResultLong ModelReadIdOverIdentifier(ModelReadIdOverIdentifierRequest modelReadIdOverIdentifierRequest)
            => ModelReadIdOverIdentifierAsync(modelReadIdOverIdentifierRequest).Result;

        public async Task<ServiceResultLong> ModelReadIdOverIdentifierAsync(ModelReadIdOverIdentifierRequest modelReadIdOverIdentifierRequest)
               => await CatcherServiceResultAsync(() => Client.ModelReadIdOverIdentifierAsync(modelReadIdOverIdentifierRequest)).ConfigureAwait(false);

        public async override Task<WcfLib.IServiceResult<long>> ExecuteModelReadIdOverIdentifierAsync(ReadIdOverIdentifierBuilder readIdOverIdentifierBuilder)
        {
            var buildValues = readIdOverIdentifierBuilder.GetValues();
            var request = new ModelReadIdOverIdentifierRequest
            {
                ModelType = new ModelClassType
                {
                    Name = buildValues.ModelType?.Name ?? "",
                    Namespace = buildValues.ModelType?.Namespace ?? "",
                    AssemblyName = buildValues.ModelType?.Assembly?.GetName()?.Name ?? ""
                },
                DebugInfoFlag = buildValues.DebugInfoFlag,
                Identifier = buildValues.Identifier,
            };
            return (await ModelReadIdOverIdentifierAsync(request).ConfigureAwait(false)).CastByClone<ServiceResult<long>>();
        }
        #endregion

        #region Update

        public ServiceResultModelUpdateResponse ModelUpdate(ModelUpdateRequest modelUpdateRequest)
            => ModelUpdateAsync(modelUpdateRequest).Result;

        public async Task<ServiceResultModelUpdateResponse> ModelUpdateAsync(ModelUpdateRequest modelUpdateRequest)
            => await CatcherServiceResultAsync(() => Client.ModelUpdateAsync(modelUpdateRequest)).ConfigureAwait(false);

        public async override Task<WcfLib.IServiceResult> ExecuteModelUpdateAsync(UpdateBuilder updateBuilder)
        {
            var buildValues = updateBuilder.GetValues();
            var request = new ModelUpdateRequest
            {
                ModelType = new ModelClassType
                {
                    Name = buildValues.ModelType.Name,
                    Namespace = buildValues.ModelType.Namespace,
                    AssemblyName = buildValues.ModelType?.Assembly?.GetName()?.Name ?? ""
                },
                DebugInfoFlag = buildValues.DebugInfoFlag,
                TableOnlyFlag = buildValues.TableOnlyFlag
            };
            var serviceResult = await ModelUpdateAsync(request).ConfigureAwait(false);
            var newServiceResult = serviceResult.CastByClone<ServiceResult<string>>();
            newServiceResult.Data = serviceResult?.Data?.ZippedJsonData;
            return newServiceResult;
        }
        #endregion

        #endregion

        #region Client CallBacks
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

        #region Client Methods 

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
