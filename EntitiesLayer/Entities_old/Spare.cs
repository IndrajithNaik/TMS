using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class Spare : Supplier
    {
        public int SpareId { get; set; }
        public string SparePartCode { get; set; }
        public string SpareName { get; set; }
        public string SparePartDescription { get; set; }
        public double SpareUnitPrice { get; set; }
        public string MinMaxAndRolevel { get; set; }
        public int Quantity { get; set; }
        public double SpareUnitPriceForSale { get; set; }
        public double SubTotal { get; set; }
        public float VatOnSubTotal { get; set; }

        public string SpareBinNumber { get; set; }

        public double GrandTotal { get; set; }


        public DateTime WarrantyPeriodUpto { get; set; }

        public int SpareStock { get; set; }

        public string SpareStatus { get; set; }

        public int SparePuchaseId { get; set; }
    }
}
