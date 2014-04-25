using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntitiesLayer.Entities;

namespace DataBaseLayer
{
    interface IDataLayerComponent
    {
        void AddSupplier(Supplier s);
        void AddTractorModels(TractorPurchase t);
        List<Supplier> GetSuppliersMatchingFullName(string s);
        List<String> GetAllSuppliersNames();
        List<TractorPurchase> GetTractorssMatchingFullName(string str);
        void AddSpareDetails(Spare s);
        List<Spare> GetSparesMatchingFullName(string str);


        void AddTractorPurchaseDetails(TractorPurchaseDetail t);
        void AddSparesPurchaseDetails(SparePurchaseDetail s);

        List<string> GetAllTractorsName();
        List<string> GetAllSparesName();

        void AddCustomerDeatils(Customer customer);
        List<Customer> GetCustomersMatchingFullName(string customerName);


        string AddAllExpenditureHeads(List<ExpenditureHead> eHL);
        bool AddAllExpenditureDetails(List<ExpenditureDetail> eDL);



        // This is for Place Info Form:

        void AddDistrict(Village V);
        void AddTaluk(Village V);
        void AddVillage(Village V);

        List<string> GetAllTaluksCorrespondingToDistrict(string p);
        List<string> GetAllDistricts();
        List<Village> GetAllVillages();


        bool SavePDIReport(TractorPurchaseDetail tractorPurChaseDetail);

    }
}
