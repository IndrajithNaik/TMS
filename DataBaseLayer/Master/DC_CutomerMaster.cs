using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntitiesLayer.Entities;

namespace DataBaseLayer
{
    public partial class DataLayerComponent : IDataLayerComponent
    {
        # region - Customer - Master.
        public List<Spare> GetAllSparesCodesIdsWithScarecity()
        {
            //throw new NotImplementedException();

            var sparesScarcity = from spSt in dc.tblSparesStocks
                                 join spT in dc.tblSpares on spSt.Spare_Id equals spT.spareId
                                 where spSt.Spare_Stock > 3
                                 select new
                                 {
                                     SpareId = spT.spareId,
                                     SpareCode = spT.sparePartNo,
                                     Description = spT.spareDescription,
                                     Quantity = spSt.Spare_Stock
                                 };
            List<Spare> spares = null;
            if (sparesScarcity.Count() > 0)
            {
                spares = new List<Spare>();
                foreach (var spr in sparesScarcity)
                {
                    spares.Add(new Spare
                    {
                        SpareId = spr.SpareId,
                        SparePartCode = spr.SpareCode,
                        SparePartDescription = spr.Description,
                        Quantity = (int)spr.Quantity
                    });
                }
            }

            return spares;
        }

        public void AddCustomerDeatils(Customer customer)
        {
            tblCustomer tCus = new tblCustomer();

            GetCorrespondingtblCustomerFromCustomer(customer, tCus);

            dc.tblCustomers.InsertOnSubmit(tCus);
            dc.SubmitChanges();
        }

        private static void GetCorrespondingtblCustomerFromCustomer(Customer customer, tblCustomer tCus)
        {
            tCus.Customer_Name = customer.CustomerName;
            tCus.Customer_Contact_Nos = customer.CustomerContactNos;
            tCus.Customer_Address = customer.CustomerAdress;
            tCus.Customer_Village = customer.CustomerVillage;
            tCus.Customer_Taluk = customer.CustomerTaluk;
            tCus.Customer_District = customer.CustomerDistrict;
            tCus.Customer_Status = customer.CustomerStatus;
        }



        public List<Customer> GetCustomersMatchingFullName(string customerName)
        {
            try
            {
                List<Customer> customerList = null;

                var customers = from c in dc.tblCustomers
                                where c.Customer_Name.StartsWith(customerName)
                                select c;

                if (customers.Count() > 0)
                {
                    customerList = new List<Customer>();
                    foreach (tblCustomer c in customers)
                    {
                        Customer cus = new Customer();
                        cus.CustomerId = c.Customer_Id;
                        cus.CustomerName = c.Customer_Name;
                        cus.CustomerContactNos = c.Customer_Contact_Nos.Split(',')[0];
                        cus.CustomerAdress = c.Customer_Address;
                        cus.CustomerTaluk = c.Customer_Taluk;
                        cus.CustomerDistrict = c.Customer_District;
                        cus.CustomerVillage = c.Customer_Village;
                        customerList.Add(cus);
                    }
                }

                return customerList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Customer> GetAllCustomers()
        {
            var customers = from C in dc.tblCustomers
                            select C;

            List<Customer> custList = null;

            if (customers.Count() > 0)
            {
                custList = new List<Customer>();

                foreach (tblCustomer C in customers)
                {
                    Customer CU = new Customer();

                    CU.CustomerId = C.Customer_Id;
                    CU.CustomerName = C.Customer_Name;
                    CU.CustomerContactNos = C.Customer_Contact_Nos;
                    CU.CustomerAdress = C.Customer_Address;

                    custList.Add(CU);
                }
            }

            return custList;
        }

        public Customer GetCustomerWithId(int cusId)
        {
            tblCustomer tCus = (from C in dc.tblCustomers
                                where C.Customer_Id == cusId
                                select C).FirstOrDefault();

            return GetCorrespondingCustomersFromTblCustomer(tCus);
        }

        private Customer GetCorrespondingCustomersFromTblCustomer(tblCustomer tCus)
        {
            return new Customer
            {
                CustomerId = tCus.Customer_Id,
                CustomerName = tCus.Customer_Name,
                CustomerContactNos = tCus.Customer_Contact_Nos,
                CustomerAdress = tCus.Customer_Address,
                CustomerVillage = tCus.Customer_Village,
                CustomerTaluk = tCus.Customer_Taluk,
                CustomerDistrict = tCus.Customer_District,
                CustomerStatus = (bool)tCus.Customer_Status

            };
        }

        public void UpdateCustomerDetails(Customer _customerToBeUpdated)
        {
            tblCustomer tbCus = (from c in dc.tblCustomers
                                 where c.Customer_Id == _customerToBeUpdated.CustomerId
                                 select c).FirstOrDefault();

            GetCorrespondingtblCustomerFromCustomer(_customerToBeUpdated, tbCus);

            dc.SubmitChanges();
            // 
        }
        # endregion - Customer - Master.
    }
}
