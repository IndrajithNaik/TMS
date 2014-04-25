using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal InvoiceSubTotal { get; set; }

        public decimal InvoiceDiscount { get; set; }

        public decimal InvoiceVATPrecent { get; set; }

        public decimal GrandTotal { get; set; }

        public decimal ExpOnTotalAmount { get; set; }

        public TYPE PurchaseType { get; set; }

    }
}
