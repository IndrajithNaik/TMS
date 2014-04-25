using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntitiesLayer.Entities;

namespace DataBaseLayer
{
    public partial class DataLayerComponent : IDataLayerComponent
    {
        #region - Tractor Purchasse
        public void AddTractorPurchaseDetails(TractorPurchaseDetail t)
        {
            // First Add the Invoice in it to Invoice Table.
            tblPurchaseInvoice invoice = new tblPurchaseInvoice();
            getTableInvoiceEquivalentFromSparePurchaseObj(ref invoice, t);
            dc.tblPurchaseInvoices.InsertOnSubmit(invoice);
            dc.SubmitChanges();

            // Add the Tractors Details inside to TractorPurchase Table. 
            List<tblTractorPurchaseDetail> traPurchaseDetail = null;
            getTableTractorPurchaseDetailEquivalentFromTractorPurchaseObj(ref traPurchaseDetail, t);
            dc.tblTractorPurchaseDetails.InsertAllOnSubmit(traPurchaseDetail);
            dc.SubmitChanges();

            // Add the Tractor to Stock Table.

            


        }

        private void getTableTractorPurchaseDetailEquivalentFromTractorPurchaseObj(ref List<tblTractorPurchaseDetail> traPurchaseDetail, TractorPurchaseDetail t)
        {
            //throw new NotImplementedException();
            if (t.TractorsPurchased.Count() > 0)
            {
                traPurchaseDetail = new List<tblTractorPurchaseDetail>();
                foreach (TractorPurchase tra in t.TractorsPurchased)
                {
                    tblTractorPurchaseDetail traPurDetail = new tblTractorPurchaseDetail();


                    traPurDetail.invoiceId = getInvoiceId(t.InvoiceNumber, t.supplierName);
                    traPurDetail.tractorSpecification = tra.TractorSpecification;
                    traPurDetail.tractorId = getTractorId(tra.TractorModel);
                    traPurDetail.engineNumber = tra.TractorEngineNo;
                    traPurDetail.chassisNumber = tra.TractorChassisNo;
                    traPurDetail.FIPNumber = tra.TractorFIPNumber;
                    traPurDetail.alternateMake = tra.TractorAlternateNumber;
                    traPurDetail.starterMotorMake = tra.TractorStarterMoterMake;
                    traPurDetail.PDIHours = tra.PDIHours;
                    traPurDetail.subTotalUnitPurchaseRate = tra.SubTotalPurchaseRate;
                    traPurDetail.vatOnTractor = tra.VATOnsubTotalPurchaseRate;
                    traPurDetail.insuranceAndOthers = tra.InsuranceAndOthers;
                    traPurDetail.grandTotal = tra.GrandTotal;


                    traPurchaseDetail.Add(traPurDetail);
                }
            }

        }

        private int getTractorId(string p)
        {
            int tid = (from t in dc.tblTractors
                       where t.tractorModel == p
                       select t.tractorId).FirstOrDefault();
            return tid;
        }

        private int getTractorPurchaseId(string p)
        {
            int tractorPurchaseDetailId = (from tpd in dc.tblTractorPurchaseDetails
                                           where tpd.engineNumber.ToUpper() == p.ToUpper()
                                           select tpd.tractorPurchaseId).FirstOrDefault();

            return tractorPurchaseDetailId;
        }

        public List<string> GetAllTractorsName()
        {
            var allTractors = from t in dc.tblTractors
                              select t.tractorModel;
            return allTractors.ToList();
        }

        #endregion - Tractor Purchasse
    }
}
