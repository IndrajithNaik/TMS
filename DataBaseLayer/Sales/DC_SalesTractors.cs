using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntitiesLayer.Entities;

namespace DataBaseLayer
{
    public partial class DataLayerComponent : IDataLayerComponent
    {
        #region Tractor Sell Details

        public void AddTractorSellDetails(TractorSales tractorSales)
        {
            int billId = AddOrUpdateTractorBill(tractorSales.ObjBill);
            tblSalesTractor tractorSale = new tblSalesTractor()
            {
                Bill_Id = billId,
                Date_Of_Sale = tractorSales.DateOfSale,
                Discount = tractorSales.Discount,
                Total_Amount = tractorSales.TotalAmount,
                Tractor_Id = tractorSales.TractorId,
                //WarrantyExpDate = tractorSales.WarrentyExpDate
            };
            dc.tblSalesTractors.InsertOnSubmit(tractorSale);
            dc.SubmitChanges();
        }

        private int AddOrUpdateTractorBill(Bill tractorBill)
        {
            tblBill bill = new tblBill()
            {
                Customer_Id = tractorBill.CustomerId,
                Bill_GrandTotal = tractorBill.GrandTotal,
                Bill_Date = tractorBill.DateOfBill,
                Bill_Type = "T"
            };
            dc.tblBills.InsertOnSubmit(bill);
            dc.SubmitChanges();
            return dc.tblBills.Max(s => s.Bill_Id);
        }

        public List<Bill> SearchBill(object searchCriteria)
        {
            if (searchCriteria is DateTime)
            {
                return dc.tblBills.Where(s => s.Bill_Date.Equals(searchCriteria)).Select(s => new Bill()
                   {
                       DateOfBill = s.Bill_Date.Value,
                       GrandTotal = float.Parse(s.Bill_GrandTotal.Value.ToString()),
                       BillNumber = s.Bill_Id
                   }).ToList();
            }
            else
            {
                return dc.tblBills.Where(s => s.Bill_Id.Equals(searchCriteria)).Select(s => new Bill()
                {
                    DateOfBill = s.Bill_Date.Value,
                    GrandTotal = float.Parse(s.Bill_GrandTotal.Value.ToString()),
                    BillNumber = s.Bill_Id
                }).ToList();
            }
        }

        //public Tractor GetTractorDetails(int billNo)
        //{
        //    dc.tblTractors.Join(dc.tblSalesTractors, t => t.tractorId, s => s.Tractor_Id, (t, s) => new Tractor()
        //    {

        //        TractorId = t.tractorId
        //    });
        //}

        #endregion
    }
}
