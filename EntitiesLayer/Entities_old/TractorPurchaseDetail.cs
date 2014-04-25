using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class TractorPurchaseDetail: Invoice
    {
        public List<TractorPurchase> TractorsPurchased;

        public TractorPurchase tractorPurchased;

        public int TractorPurchaseId { get; set; }
        //public TractorPurchaseDetail(List<Tractor> tractorsPurchased)
        //{
        //    TractorsPurchased = tractorsPurchased;
        //}

        //public string PurchaseInvoiceNo { get; set; }
        //public DateTime DateOfInvoice { get; set; }
        //public DateTime DateOfUpdate { get; set; }
        //public float TradeDiscout { get; set; }
        //public float SubTotalAmount { get; set; }
        //public float SalesTaxAndVat { get; set; }
        //public float InsuranceAndOthers { get; set; }
        //public float GrandTotal { get; set; }

    }
}
