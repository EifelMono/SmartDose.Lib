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
            return ((IMasterDataService)Client).GetCustomersAsync(searchFilter, sortFilter, pageFilter);
        }

        public Task<Customer> AddOrUpdateCustomerAsync(Customer customer)
        {
            return ((IMasterDataService)Client).AddOrUpdateCustomerAsync(customer);
        }

        public Task DeleteCustomerAsync(long customerId)
        {
            return ((IMasterDataService)Client).DeleteCustomerAsync(customerId);
        }

        public Task<int> GetCustomerCountAsync(List<SearchFilter> searchFilter)
        {
            return ((IMasterDataService)Client).GetCustomerCountAsync(searchFilter);
        }

        public Task<List<Medicine>> GetMedicinesAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            return ((IMasterDataService)Client).GetMedicinesAsync(searchFilter, sortFilter, pageFilter);
        }

        public Task<Medicine> GetMedicineAsync(string identifier)
        {
            return ((IMasterDataService)Client).GetMedicineAsync(identifier);
        }

        public Task<List<MedicineOverview>> GetMedicineOverviewAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            return ((IMasterDataService)Client).GetMedicineOverviewAsync(searchFilter, sortFilter, pageFilter);
        }

        public Task<List<string>> GetMedicinesMissingIdentifiersAsync(List<string> requestedMedicineIdentifiers, bool withSynonyms)
        {
            return ((IMasterDataService)Client).GetMedicinesMissingIdentifiersAsync(requestedMedicineIdentifiers, withSynonyms);
        }

        public Task<int> GetMedicineCountAsync(List<SearchFilter> searchFilter)
        {
            return ((IMasterDataService)Client).GetMedicineCountAsync(searchFilter);
        }

        public Task<Medicine> AddOrUpdateMedicineAsync(Medicine medicine)
        {
            return ((IMasterDataService)Client).AddOrUpdateMedicineAsync(medicine);
        }

        public Task DeleteMedicineAsync(long medicineId)
        {
            return ((IMasterDataService)Client).DeleteMedicineAsync(medicineId);
        }

        public Task<List<Medicine>> GetCanisterAllowedMedicinesAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            return ((IMasterDataService)Client).GetCanisterAllowedMedicinesAsync(searchFilter, sortFilter, pageFilter);
        }

        public Task<Medicine> ReleaseMedicineAsync(string identifier)
        {
            return ((IMasterDataService)Client).ReleaseMedicineAsync(identifier);
        }

        public Task<MedicineUseInformation> CheckIfMedicineCanBeDeletedAsync(long medId)
        {
            return ((IMasterDataService)Client).CheckIfMedicineCanBeDeletedAsync(medId);
        }

        public Task<bool> CheckIfMedicineExistsAsync(string identifier)
        {
            return ((IMasterDataService)Client).CheckIfMedicineExistsAsync(identifier);
        }

        public Task<List<Manufacturer>> GetManufacturersAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            return ((IMasterDataService)Client).GetManufacturersAsync(searchFilter, sortFilter, pageFilter);
        }

        public Task<Manufacturer> AddOrUpdateManufacturerAsync(Manufacturer manufacturer)
        {
            return ((IMasterDataService)Client).AddOrUpdateManufacturerAsync(manufacturer);
        }

        public Task DeleteManufacturerAsync(long manufacturerId)
        {
            return ((IMasterDataService)Client).DeleteManufacturerAsync(manufacturerId);
        }

        public Task<List<DestinationFacility>> GetDestinationFacilitiesAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            return ((IMasterDataService)Client).GetDestinationFacilitiesAsync(searchFilter, sortFilter, pageFilter);
        }

        public Task<DestinationFacility> AddOrUpdateDestinationFacilityAsync(DestinationFacility destinationFacility)
        {
            return ((IMasterDataService)Client).AddOrUpdateDestinationFacilityAsync(destinationFacility);
        }

        public Task DeleteDestinationFacilityAsync(long destinationFacilityId)
        {
            return ((IMasterDataService)Client).DeleteDestinationFacilityAsync(destinationFacilityId);
        }

        public Task<List<Canister>> GetCanistersAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            return ((IMasterDataService)Client).GetCanistersAsync(searchFilter, sortFilter, pageFilter);
        }

        public Task<int> GetCanisterCountAsync(List<SearchFilter> searchFilter)
        {
            return ((IMasterDataService)Client).GetCanisterCountAsync(searchFilter);
        }

        public Task<Canister> AddOrUpdateCanisterAsync(Canister canister)
        {
            return ((IMasterDataService)Client).AddOrUpdateCanisterAsync(canister);
        }

        public Task DeleteCanisterAsync(long canisterId)
        {
            return ((IMasterDataService)Client).DeleteCanisterAsync(canisterId);
        }

        public Task<Canister> ReleaseCanisterAsync(long id)
        {
            return ((IMasterDataService)Client).ReleaseCanisterAsync(id);
        }

        public Task<List<Tray>> GetTraysAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            return ((IMasterDataService)Client).GetTraysAsync(searchFilter, sortFilter, pageFilter);
        }

        public Task<Tray> AddOrUpdateTrayAsync(Tray tray)
        {
            return ((IMasterDataService)Client).AddOrUpdateTrayAsync(tray);
        }

        public Task DeleteTrayAsync(long trayId)
        {
            return ((IMasterDataService)Client).DeleteTrayAsync(trayId);
        }

        public Task<Tray> ReleaseTrayAsync(string identifier)
        {
            return ((IMasterDataService)Client).ReleaseTrayAsync(identifier);
        }

        public Task<List<LabelDesign>> GetLabelDesignsAsync(List<SearchFilter> searchFilter, SortFilter sortFilter, PageFilter pageFilter)
        {
            return ((IMasterDataService)Client).GetLabelDesignsAsync(searchFilter, sortFilter, pageFilter);
        }

        public Task<LabelDesign> AddOrUpdateLabelDesignAsync(LabelDesign labelDesign)
        {
            return ((IMasterDataService)Client).AddOrUpdateLabelDesignAsync(labelDesign);
        }

        public Task DeleteLabelDesignAsync(long labelDesignId)
        {
            return ((IMasterDataService)Client).DeleteLabelDesignAsync(labelDesignId);
        }

        public Task PrintLabelAsync(PrintDataRequest printRequest)
        {
            return ((IMasterDataService)Client).PrintLabelAsync(printRequest);
        }

        public Task<bool> IsRfidUniqueAsync(string rfid)
        {
            return ((IMasterDataService)Client).IsRfidUniqueAsync(rfid);
        }

        public Task<bool> IsTrayFreeAsync(long id)
        {
            return ((IMasterDataService)Client).IsTrayFreeAsync(id);
        }

        public Task<List<GlobalConfiguration>> GetGlobalConfigurationAsync(List<GlobalSettingType> globalSettings)
        {
            return ((IMasterDataService)Client).GetGlobalConfigurationAsync(globalSettings);
        }

        #endregion
    }
}
