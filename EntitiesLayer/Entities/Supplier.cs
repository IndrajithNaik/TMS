using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierContactNumber { get; set; }
        public string SupplierAddress { get; set; }
        public TYPE SupplierType { get; set; }
        public STATUS SupplierStatus { get; set; }
    }
}
