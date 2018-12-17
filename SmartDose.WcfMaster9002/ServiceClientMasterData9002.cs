using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using RowaMore.Extensions;
using SmartDose.WcfLib;

namespace SmartDose.WcfMasterData9002
{
    public class ServiceClientMasterData9002 : ServiceClient, IMasterDataService, IMasterDataServiceCallback
    {
        public ServiceClientMasterData9002(string endpointAddress, SecurityMode securityMode = SecurityMode.None) : base(endpointAddress, securityMode)
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

        #region IServiceClient
        // Done in SmartDose.WcfLib by Reflection
        #endregion

        #region Client CallBacks
        protected override void AssignClientCallbacks(bool on)
        {
            //switch (on)
            //{
            //    case true:
            //        Client.MedicinesChangedReceived += MedicinesChanged;
            //        Client.MedicinesDeletedReceived += MedicinesDeleted;

            //        break;
            //    case false:
            //        Client.MedicinesChangedReceived -= MedicinesChanged;
            //        Client.MedicinesDeletedReceived -= MedicinesDeleted;
            //        break;
            //}
        }

        public void SetCustomers(List<Customer> customers, bool initialSyncronization)
        {
            throw new NotImplementedException();
        }

        public void DeleteCustomers(List<long> customerIds)
        {
            throw new NotImplementedException();
        }

        public void SetCanisters(List<Canister> canisters, bool initialSyncronization)
        {
            throw new NotImplementedException();
        }

        public void DeleteCanisters(List<long> canisterIds)
        {
            throw new NotImplementedException();
        }

        public void CanisterLocationChanged(string canisterRfid, Location location)
        {
            throw new NotImplementedException();
        }

        public void SetManufacturers(List<Manufacturer> manufacturers, bool initialSyncronization)
        {
            throw new NotImplementedException();
        }

        public void DeleteManufacturers(List<long> manufacturerIds)
        {
            throw new NotImplementedException();
        }

        public void SetDestinationFacilities(List<DestinationFacility> destinationFacilitys, bool initialSyncronization)
        {
            throw new NotImplementedException();
        }

        public void DeleteDestinationFacilities(List<long> destinationFacilityIds)
        {
            throw new NotImplementedException();
        }

        public void SetMedicines(List<Medicine> medicines, bool initialSyncronization)
        {
            throw new NotImplementedException();
        }

        public void DeleteMedicines(List<long> medicineIds)
        {
            throw new NotImplementedException();
        }

        public void SetTrays(List<Tray> trays, bool initialSyncronization)
        {
            throw new NotImplementedException();
        }

        public void DeleteTrays(List<long> trayIds)
        {
            throw new NotImplementedException();
        }

        public void SetLabelDesigns(List<LabelDesign> labelDesigns, bool initialSyncronization)
        {
            throw new NotImplementedException();
        }

        public void DeleteLabelDesigns(List<long> labelDesignIds)
        {
            throw new NotImplementedException();
        }

        public void SetGlobalSettings(List<GlobalConfiguration> masterDataHandlingSettings)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region Client Methods 

        public Task<List<Customer>> GetCustomersAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> AddOrUpdateCustomerAsync(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCustomerAsync(long customerId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCustomerCountAsync(List<SearchFilter> searchFilter)
        {
            throw new NotImplementedException();
        }

        public Task<List<Medicine>> GetMedicinesAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            throw new NotImplementedException();
        }

        public Task<List<MedicineOverview>> GetMedicineOverviewAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetMedicinesMissingIdentifiersAsync(List<string> requestedMedicineIdentifiers, bool withSynonyms)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetMedicineCountAsync(List<SearchFilter> searchFilter)
        {
            throw new NotImplementedException();
        }

        public Task<Medicine> AddOrUpdateMedicineAsync(Medicine medicine)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMedicineAsync(long medicineId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Medicine>> GetCanisterAllowedMedicinesAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            throw new NotImplementedException();
        }

        public Task<Medicine> ReleaseMedicineAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<MedicineUseInformation> CheckIfMedicineCanBeDeletedAsync(long medId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Manufacturer>> GetManufacturersAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            throw new NotImplementedException();
        }

        public Task<Manufacturer> AddOrUpdateManufacturerAsync(Manufacturer manufacturer)
        {
            throw new NotImplementedException();
        }

        public Task DeleteManufacturerAsync(long manufacturerId)
        {
            throw new NotImplementedException();
        }

        public Task<List<DestinationFacility>> GetDestinationFacilitiesAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            throw new NotImplementedException();
        }

        public Task<DestinationFacility> AddOrUpdateDestinationFacilityAsync(DestinationFacility destinationFacility)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDestinationFacilityAsync(long destinationFacilityId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Canister>> GetCanistersAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCanisterCountAsync(List<SearchFilter> searchFilter)
        {
            throw new NotImplementedException();
        }

        public Task<Canister> AddOrUpdateCanisterAsync(Canister canister)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCanisterAsync(long canisterId)
        {
            throw new NotImplementedException();
        }

        public Task<Canister> ReleaseCanisterAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tray>> GetTraysAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            throw new NotImplementedException();
        }

        public Task<Tray> AddOrUpdateTrayAsync(Tray tray)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTrayAsync(long trayId)
        {
            throw new NotImplementedException();
        }

        public Task<Tray> ReleaseTrayAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<List<LabelDesign>> GetLabelDesignsAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            throw new NotImplementedException();
        }

        public Task<LabelDesign> AddOrUpdateLabelDesignAsync(LabelDesign labelDesign)
        {
            throw new NotImplementedException();
        }

        public Task DeleteLabelDesignAsync(long labelDesignId)
        {
            throw new NotImplementedException();
        }

        public Task PrintLabelAsync(PrintDataRequest printRequest)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsRfidUniqueAsync(string rfid)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsTrayFreeAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<GlobalConfiguration>> GetGlobalConfigurationAsync(List<GlobalSettingType> globalSettings)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
