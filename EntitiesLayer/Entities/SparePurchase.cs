using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class SparePurchase
    {
        public int SpareId { get; set; }

        public int SparePartId { get; set; }

        public int InvoiceId { get; set; }

        public int Quantity { get; set; }

        public int SupplierId { get; set; }

        public SpareParts ObjSpareParts { get; set; }

        public Invoice ObjInvoice { get; set; }

        public Supplier ObjSupplier { get; set; }
    }
}
