using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntitiesLayer.Entities;

namespace DataBaseLayer
{
    public partial class DataLayerComponent : IDataLayerComponent
    {
        # region - Spare Purchase Details.
        private void getTableInvoiceEquivalentFromSparePurchaseObj(ref tblPurchaseInvoice invoice, TractorPurchaseDetail t)
        {
            
            invoice.invoiceNumber = t.InvoiceNumber;// = txtInvoiceNo.Text;
            invoice.supplierId = getSuppplierIdFromName(t.supplierName);// = cmbSupplier.Text;
            invoice.invoiceDate = t.InvoiceDate; //= dateTimeInvoiceDate.SelectedDate.Value;
            invoice.recievedDate = t.RecievedDateTime;// = dateTimeInvoiceUpdate.SelectedDate.Value;
            invoice.purchaseType = t.PurchaseType;// = "SPARES";
            //invoice.expOnTotalAmount = t.ExpOnTotalAmount;// = float.Parse(txtExpOnTotalAmount.Text.ToString());
            //invoice.discountOnTotalAmount = t.DIscountOnTotalAmount;// = float.Parse(txtDiscount.Text.ToString());
            invoice.grandTotal = t.GrandTotal;// = float.Parse(txtGrandTotalOfInvoice.Text.ToString());
        }

        public bool _isInvoiceUpdateRequired = false;
        public bool _isPurchaseDetailsUpdateRequired = false;
        public void AddSparesPurchaseDetails(SparePurchaseDetail s)
        {
            // First Add the Invoice to Invoice Table.
            // Add the Spares to Spares Table.            

            // This is to Add Or Update the Invoice Details.            
            AddOrUpdateTheInvoiceDetails(s);

            // This is to Add/ Update the Spare Purchase Details.
            AddOrUpdateSparePurchaseDetails(s);

            // Add Or Update Spares Stock to Spare Stock Table.
            UpdateSparesStock(s);
        }

        public static List<tblSparePurchaseDetail> _oldSparesPurchaseDetails = null;
        private void AddOrUpdateSparePurchaseDetails(SparePurchaseDetail s)
        {
            List<tblSparePurchaseDetail> tblSparesPurchaseDetails = new List<tblSparePurchaseDetail>();


            getTableSpareDetailsEquivalentFromSparePurchaseObj(ref tblSparesPurchaseDetails, s);

            dc.tblSparePurchaseDetails.InsertAllOnSubmit(tblSparesPurchaseDetails);

            dc.SubmitChanges();

            # region - Commented Code.
            //if (null != _OldSpareDetailsOnEditing)
            //{
            //    _oldSparesPurchaseDetails = new List<tblSparePurchaseDetail>();
            //    SparePurchaseDetail s1 = s;
            //    s1._listOfSpares = _OldSpareDetailsOnEditing;
            //    getTableSpareDetailsEquivalentFromSparePurchaseObj(ref _oldSparesPurchaseDetails, s1);
            //}

            //int count = 0;
            //foreach (tblSparePurchaseDetail tblPur in tblSparesPurchaseDetails)
            //{
            //    tblSparePurchaseDetail alreadyPresent = CheckIfSparePurchaseDetailsAlreadyPresent(tblPur, s, count);


            //    if (!_isPurchaseDetailsUpdateRequired && null == alreadyPresent)
            //    {
            //        dc.tblSparePurchaseDetails.InsertOnSubmit(tblPur);                    
            //    }
            //    else
            //    {
            //        //Reduce the Stock from stock table.
            //        RemoveWrongEnteredStock(alreadyPresent);
            //        _oldSparesPurchaseDetails[count] = tblPur;
            //    }
            //    dc.SubmitChanges();
            //    count++;
            //}   
            # endregion - Commented Code.
        }

        private void RemoveWrongEnteredStock(tblSparePurchaseDetail alreadyPresent)
        {
            tblSparesStock t = dc.tblSparesStocks.Where(p => p.Spare_Id == alreadyPresent.spareId).First(); ;
            t.Spare_Stock -= alreadyPresent.quantity;
            dc.SubmitChanges();
        }

        private void AddOrUpdateTheInvoiceDetails(SparePurchaseDetail s)
        {

            tblPurchaseInvoice invoice = new tblPurchaseInvoice();
            getTableInvoiceEquivalentFromSparePurchaseObj(ref invoice, s);
            dc.tblPurchaseInvoices.InsertOnSubmit(invoice);
            dc.SubmitChanges();

            # region - Commented Code.
            //CheckIfInvoiceIsAlreadyPresent(s.InvoiceNumber);

            //if (_isInvoiceUpdateRequired)
            //{
            //dc.tblPurchaseInvoices.InsertOnSubmit(invoice);
            //}
            //else
            //{
            //    invoice = dc.tblPurchaseInvoices.Where(i => i.invoiceNumber == s.InvoiceNumber).First();
            //}

            # endregion - Commented Code.
        }

        private tblSparePurchaseDetail CheckIfSparePurchaseDetailsAlreadyPresent(tblSparePurchaseDetail tblPur, SparePurchaseDetail s, int count)
        {
            //TractorPurchaseDetail 
            int invoiceId = getInvoiceId(s.InvoiceNumber, s.supplierName);
            int spareId = getSpareIdFromSparePartCode(s._listOfSpares[0].SparePartCode);
            DateTime dtInvoice = s.InvoiceDate;

            tblSparePurchaseDetail alreadyPresent = (from tpD in dc.tblSparePurchaseDetails
                                                     where tpD.invoiceId == invoiceId && tpD.spareId == spareId
                                                     select tpD).FirstOrDefault();

            return alreadyPresent;
        }

        private void CheckIfInvoiceIsAlreadyPresent(string p)
        {
            var IsInvoicePresent = from inv in dc.tblPurchaseInvoices
                                   where inv.invoiceNumber.ToString().ToUpper() == p.ToString().ToUpper()
                                   select inv;

            if (IsInvoicePresent.Count() > 0)
            {
                _isInvoiceUpdateRequired = false;
            }
            else
            {
                _isInvoiceUpdateRequired = true;
            }
        }

        private void UpdateSparesStock(SparePurchaseDetail s)
        {
            List<Spare> spares = s._listOfSpares;

            foreach (Spare sp in s._listOfSpares)
            {
                // Find if Spare is already there in the table. If there please add it.
                //bool isExists = false;

                UpdateEachSpareStock(sp);
            }
        }

        private void UpdateEachSpareStock(Spare sp)
        {
            int spareId = getSpareIdFromSparePartCode(sp.SparePartCode);

            var ExistingSpare = from spItem in dc.tblSparesStocks
                                where spItem.Spare_Id == spareId
                                select spItem;
            if (null != ExistingSpare && ExistingSpare.Count() <= 0)
            {
                // Add the spare compenent to the Spare Quantity table.
                AddNewSpareToStockTable(sp.Quantity, spareId);
            }
            else
            {
                // Update the Quantity with the New Quantity adding the present quantity.
                UpdateTheExistingSparesStock(sp.Quantity, spareId);
            }
        }

        private void UpdateTheExistingSparesStock(int quantity, int spareId)
        {
            tblSparesStock t = dc.tblSparesStocks.Where(p => p.Spare_Id == spareId).First(); ;
            t.Spare_Stock += quantity;
            dc.SubmitChanges();
        }

        private void AddNewSpareToStockTable(int Quantity, int spareId)
        {
            tblSparesStock spStock = new tblSparesStock();
            spStock.Spare_Id = spareId;
            spStock.Spare_Stock = Quantity;
            dc.tblSparesStocks.InsertOnSubmit(spStock);
            dc.SubmitChanges();
        }

        private void getTableSpareDetailsEquivalentFromSparePurchaseObj(ref List<tblSparePurchaseDetail> tblSparesPurchaseDetails, SparePurchaseDetail s)
        {
            List<Spare> sparesInPurchaseDetail = null;
            if (s._listOfSpares.Count() > 0)
            {
                sparesInPurchaseDetail = new List<Spare>();

                foreach (Spare sp in s._listOfSpares)
                {

                    tblSparePurchaseDetail tsp = new tblSparePurchaseDetail();
                    tsp.invoiceId = getInvoiceId(s.InvoiceNumber, s.supplierName);
                    tsp.spareId = getSpareIdFromSparePartCode(sp.SparePartCode);
                    tsp.quantity = sp.Quantity;
                    tsp.unitRateOfPurchase = sp.SpareUnitPrice;
                    tsp.unitRateofSell = sp.SpareUnitPriceForSale;
                    tsp.subTotalAmount = sp.SubTotal;
                    tsp.vatOnSubTotal = sp.VatOnSubTotal;
                    tsp.grandTotalAmount = sp.GrandTotal;

                    tblSparesPurchaseDetails.Add(tsp);
                }
            }
        }

        private int getSpareIdFromSparePartCode(string p)
        {
            int spareId = (from sp in dc.tblSpares
                           where sp.sparePartNo == p
                           select sp.spareId).FirstOrDefault();
            return spareId;
        }

        private int getInvoiceId(string p, string p_2)
        {
            int invoiceId = (from i in dc.tblPurchaseInvoices
                             where i.invoiceNumber == p && i.supplierId == getSuppplierIdFromName(p_2)
                             select i.invoiceId).FirstOrDefault();
            return invoiceId;
        }

        private void getTableInvoiceEquivalentFromSparePurchaseObj(ref tblPurchaseInvoice invoice, SparePurchaseDetail s)
        {
            invoice.invoiceNumber = s.InvoiceNumber;// = txtInvoiceNo.Text;
            invoice.supplierId = getSuppplierIdFromName(s.supplierName);// = cmbSupplier.Text;
            invoice.invoiceDate = s.InvoiceDate; //= dateTimeInvoiceDate.SelectedDate.Value;
            invoice.recievedDate = s.RecievedDateTime;// = dateTimeInvoiceUpdate.SelectedDate.Value;
            invoice.purchaseType = s.PurchaseType;// = "SPARES";
            invoice.expOnTotalAmount = s.ExpOnTotalAmount;// = float.Parse(txtExpOnTotalAmount.Text.ToString());
            invoice.discountOnTotalAmount = s.DIscountOnTotalAmount;// = float.Parse(txtDiscount.Text.ToString());
            invoice.grandTotal = s.GrandTotal;// = float.Parse(txtGrandTotalOfInvoice.Text.ToString());
        }
        public List<string> GetAllSparesCodesCorrespondingToName(string str)
        {
            var allSparesCodes = from s in dc.tblSpares
                                 where s.spareName == str
                                 select s.sparePartNo;
            return allSparesCodes.ToList();
        }

        public string GetCorresponsingPartDecriptionForPartCode(string str)
        {
            var SparesCodeDescription = (from s in dc.tblSpares
                                         where s.sparePartNo == str
                                         select s.spareDescription + "," + s.spareUnitPrice.ToString()).FirstOrDefault();
            return SparesCodeDescription;
        }

        public Invoice GetLatestAddedInvoiceNumber(string invoiceNumber)
        {
            var invoiceLatest = (from invoice in dc.tblPurchaseInvoices
                                 where invoiceNumber == invoice.invoiceNumber
                                 select invoice).FirstOrDefault();

            Invoice inv = null;

            if (null != invoiceLatest)
            {
                inv = new Invoice
                {
                    InvoiceId = invoiceLatest.invoiceId,
                    InvoiceNumber = invoiceLatest.invoiceNumber,
                    InvoiceDate = DateTime.Parse(invoiceLatest.invoiceDate.ToString()),
                    GrandTotal = float.Parse(invoiceLatest.grandTotal.ToString()),
                    ExpOnTotalAmount = float.Parse(invoiceLatest.expOnTotalAmount.ToString()),
                    PurchaseType = invoiceLatest.purchaseType,
                    RecievedDateTime = DateTime.Parse(invoiceLatest.recievedDate.ToString()),


                };
            }

            return inv;
        }

        List<Spare> _OldSpareDetailsOnEditing = null;
        public List<Spare> GetSparesCorrespondingToInvoiceNumber(string invoiceNo)
        {
            int inVID = getInvoiceId(invoiceNo);
            var spares = from sp in dc.tblSparePurchaseDetails
                         where sp.invoiceId == inVID
                         select sp;

            List<Spare> _spares = null;

            if (spares.Count() > 0)
            {
                _spares = new List<Spare>();
                foreach (tblSparePurchaseDetail sPD in spares)
                {
                    Spare s = new Spare();

                    s.SparePuchaseId = sPD.sparePurchaseId;
                    s.SpareId = sPD.spareId;
                    //s.SpareUnitPrice = float.Parse(sPD.unitRateofSell.ToString());
                    s.Quantity = int.Parse(sPD.quantity.ToString());
                    s.SubTotal = float.Parse(sPD.subTotalAmount.ToString());

                    GetSpareDetailsFromSpareId(sPD.spareId, ref s);
                    _spares.Add(s);
                }
            }
            _OldSpareDetailsOnEditing = _spares;
            return _spares;
        }

        private void GetSpareDetailsFromSpareId(int p, ref Spare s)
        {
            var spare = (from sp in dc.tblSpares
                         where sp.spareId == p
                         select sp).FirstOrDefault();

            s.SparePartCode = spare.sparePartNo;
            s.SparePartDescription = spare.spareDescription;
            s.SpareUnitPriceForSale = float.Parse(spare.spareUnitPrice.ToString());
            s.SpareName = spare.spareName;
            s.supplierName = spare.tblSupplier.supplierName;
        }

        public int getInvoiceId(string invoiceNo)
        {
            int iN = (from i in dc.tblPurchaseInvoices
                      where invoiceNo == i.invoiceNumber
                      select i.invoiceId).FirstOrDefault();
            return iN;
        }

        public List<Invoice> GetInvoicesWithRespectToDates(DateTime dateTime)
        {
            var invoiceLatest = from invoice in dc.tblPurchaseInvoices
                                where dateTime.DayOfYear == invoice.invoiceDate.Value.DayOfYear
                                select invoice;

            List<Invoice> invoiceList = null;
            if (null != invoiceLatest && invoiceLatest.Count() > 0)
            {
                invoiceList = new List<Invoice>();

                foreach (tblPurchaseInvoice tblPI in invoiceLatest)
                {
                    Invoice inv = new Invoice
                    {
                        InvoiceNumber = tblPI.invoiceNumber,
                        InvoiceDate = DateTime.Parse(tblPI.invoiceDate.ToString()),
                        GrandTotal = float.Parse(tblPI.grandTotal.ToString())
                    };

                    invoiceList.Add(inv);
                }
            }

            return invoiceList;
        }

        public List<Spare> GetSparesStocks()
        {
            List<Spare> _spares = null;

            var stocks = from stock in dc.tblSparesStocks
                         select stock;

            if (null != stocks && stocks.Count() > 0)
            {
                _spares = new List<Spare>();
                foreach (tblSparesStock tS in stocks)
                {
                    Spare s = new Spare();

                    GetSpareDetailsFromSpareId(tS.Spare_Id, ref s);
                    s.supplierId = tS.Spare_Id;
                    s.SpareStock = int.Parse(tS.Spare_Stock.ToString());
                    _spares.Add(s);
                }
            }
            return _spares;
        }

        public int GetPurchaseDetailId(string _invoiceId, string supplierPartCode)
        {
            int i = 0;

            var purchaseId = (from pd in dc.tblSparePurchaseDetails
                              where pd.spareId == getSpareIdFromSparePartCode(supplierPartCode) && int.Parse(_invoiceId) == pd.invoiceId
                              select pd.sparePurchaseId).FirstOrDefault();
            if (null != purchaseId && purchaseId.ToString() != string.Empty)
            {
                i = purchaseId;
            }
            return i;
        }

        public Spare GetSparePurchaseDetailsById(int sparePurchaseDetailId)
        {
            tblSparePurchaseDetail tblSparePurchaseDetail = (from spD in dc.tblSparePurchaseDetails
                                                             where spD.sparePurchaseId == sparePurchaseDetailId
                                                             select spD).FirstOrDefault();
            return new Spare
            {
                SparePuchaseId = tblSparePurchaseDetail.sparePurchaseId,
                SpareId = tblSparePurchaseDetail.spareId,
                Quantity = (int)tblSparePurchaseDetail.quantity,
                SpareUnitPriceForSale = (int)tblSparePurchaseDetail.unitRateofSell,
                SubTotal = (double)tblSparePurchaseDetail.subTotalAmount
            };
        }

        public void UpdateSparePurchaseInvoice(Invoice _selectedInvoiceForEdit)
        {
            //throw new NotImplementedException();

            tblPurchaseInvoice tblPurInv = (from i in dc.tblPurchaseInvoices
                                            where i.invoiceId == _selectedInvoiceForEdit.InvoiceId
                                            select i).FirstOrDefault();

            GetUpdatedEquivalentInvoice(_selectedInvoiceForEdit, tblPurInv);

            dc.SubmitChanges();
        }

        private void GetUpdatedEquivalentInvoice(Invoice _selectedInvoiceForEdit, tblPurchaseInvoice tblPurInv)
        {
            tblPurInv.invoiceNumber = _selectedInvoiceForEdit.InvoiceNumber;// = txtInvoiceNo.Text;
            tblPurInv.supplierId = getSuppplierIdFromName(_selectedInvoiceForEdit.supplierName);// = cmbSupplier.Text;
            tblPurInv.invoiceDate = _selectedInvoiceForEdit.InvoiceDate; //= dateTimeInvoiceDate.SelectedDate.Value;
            tblPurInv.recievedDate = _selectedInvoiceForEdit.RecievedDateTime;// = dateTimeInvoiceUpdate.SelectedDate.Value;
            tblPurInv.purchaseType = _selectedInvoiceForEdit.PurchaseType;// = "SPARES";
            tblPurInv.expOnTotalAmount = _selectedInvoiceForEdit.ExpOnTotalAmount;// = float.Parse(txtExpOnTotalAmount.Text.ToString());
            tblPurInv.discountOnTotalAmount = _selectedInvoiceForEdit.DIscountOnTotalAmount;// = float.Parse(txtDiscount.Text.ToString());
            tblPurInv.grandTotal = _selectedInvoiceForEdit.GrandTotal;// = float.Parse(txtGrandTotalOfInvoice.Text.ToString());

        }

        public void UpdateSelectedPurchaseDetail(Spare _selectedSparePurchaseDetailForEdit)
        {
            //throw new NotImplementedException();
            tblSparePurchaseDetail tblSPD = (from spD in dc.tblSparePurchaseDetails
                                             where spD.sparePurchaseId == _selectedSparePurchaseDetailForEdit.SparePuchaseId
                                             select spD).FirstOrDefault();

            Dictionary<int, int> oldSpareIdStock = new Dictionary<int, int>();
            int oldSpareId = tblSPD.spareId;
            int oldSpareStockContent = (int)tblSPD.quantity * -1;

            UpdateTheExistingSparesStock(oldSpareStockContent, oldSpareId);

            // Delete this much from the Spares Stock.         

            UpdateInvoiceWithLatestSpare(_selectedSparePurchaseDetailForEdit, tblSPD);

            dc.SubmitChanges();

            UpdateEachSpareStock(_selectedSparePurchaseDetailForEdit);


        }

        private void UpdateInvoiceWithLatestSpare(Spare _selectedSparePurchaseDetailForEdit, tblSparePurchaseDetail tblSPD)
        {
            tblSPD.spareId = getSpareIdFromSparePartCode(_selectedSparePurchaseDetailForEdit.SparePartCode);
            tblSPD.quantity = _selectedSparePurchaseDetailForEdit.Quantity;
            tblSPD.unitRateOfPurchase = _selectedSparePurchaseDetailForEdit.SpareUnitPrice;
            tblSPD.unitRateofSell = _selectedSparePurchaseDetailForEdit.SpareUnitPriceForSale;
            tblSPD.subTotalAmount = _selectedSparePurchaseDetailForEdit.SubTotal;
            tblSPD.vatOnSubTotal = _selectedSparePurchaseDetailForEdit.VatOnSubTotal;
            tblSPD.grandTotalAmount = _selectedSparePurchaseDetailForEdit.GrandTotal;
        }
        # endregion - Spare Purchase Details.
    }
}
