using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntitiesLayer.Entities;
using System.Data;


namespace DataBaseLayer
{
    public partial class DataLayerComponent : IDataLayerComponent
    {
        DataTractorShowRoomDataContext dc = new DataTractorShowRoomDataContext();

        private static DataLayerComponent _dataLayerComponent = null;
        public static DataLayerComponent GetInstance()
        {
            if (null != _dataLayerComponent)
            {
                return new DataLayerComponent();
            }
            return _dataLayerComponent;
        }

        public List<tblExpenditureDetail> GetAllExpenditureDetails()
        {
            //throw new NotImplementedException();

            return (from eD in dc.tblExpenditureDetails
                    select eD).ToList();
        }

        public void AddPDIInfomrationForTractor(TractorPurchaseDetail _tractorPurchaseDetail)
        {
            // Vishwa : [Start] - COmmented Code.
            foreach (TractorPurchase tra in _tractorPurchaseDetail.TractorsPurchased)
            {
                int tractorPurchaseId = _tractorPurchaseDetail.TractorPurchaseId;// getTractorPurchaseId(tra.TractorEngineNo);

                // Add the Battery Details to Berrty Table.
                tblTractorBattery tba = new tblTractorBattery();
                tba.tractorPurchaseId = tractorPurchaseId;
                tba.batteryMaker = tra.TractorBattery.BatteryMaker;
                tba.batteryRemarks = tra.TractorBattery.Remarks;
                tba.batterySerialNo = tra.TractorBattery.BatterySerialNo;
                tba.batterySize = tra.TractorBattery.BatterySize;

                dc.tblTractorBatteries.InsertOnSubmit(tba);
                dc.SubmitChanges();

                // Add the Tyres Details to Tyre Table.
                List<tblTractorTyre> listofTyres = new List<tblTractorTyre>();
                foreach (Tyre ty in tra.TractorTyres.ToList())
                {
                    tblTractorTyre tty = new tblTractorTyre();
                    tty.tractorPurchaseId = tractorPurchaseId;
                    tty.tyreMaker = ty.tyreMaker;
                    tty.tyreRemarks = ty.tyreMaker;
                    tty.tyreSerialNo = ty.tyreSerialNumber;
                    tty.tyreSize = ty.tyreSize;
                    listofTyres.Add(tty);
                }

                dc.tblTractorTyres.InsertAllOnSubmit(listofTyres);
                dc.SubmitChanges();

                // Vishwa : [Start] - COmmented Code.

            }
        }

        public List<tblPurchaseInvoice> GetAllTractorInvoices()
        {
            var invoiceList = from invoice in dc.tblPurchaseInvoices
                                 where invoice.purchaseType.ToUpper() == "T"
                                 select invoice;

            return invoiceList.ToList();
        }


        public bool SavePDIReport(TractorPurchaseDetail tractorPurChaseDetail)
        {
            throw new NotImplementedException();
        }

        public TractorPurchaseDetail GetTractorPurchaseDetailFromInvoiceNo(int _invoiceNumber)
        {
            tblTractorPurchaseDetail traPurDetail = (from tPD in dc.tblTractorPurchaseDetails
                                                     where tPD.invoiceId == getInvoiceId(_invoiceNumber.ToString())
                                                    select tPD).FirstOrDefault();

            //return traPurDetail;

            return new TractorPurchaseDetail
            {
                TractorPurchaseId = traPurDetail.tractorPurchaseId,
                InvoiceNumber = traPurDetail.tblPurchaseInvoice.invoiceNumber,
                InvoiceDate = (DateTime)traPurDetail.tblPurchaseInvoice.invoiceDate,
                RecievedDateTime = (DateTime)traPurDetail.tblPurchaseInvoice.recievedDate,                
                //tractorPurchased = GetTractorFromId(traPurDetail.tblTractor.tractorId),
                
            };
        }
    }
}