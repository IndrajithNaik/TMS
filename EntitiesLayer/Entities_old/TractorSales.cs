using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class TractorSales
    {
        public int TractorSaleId { get; set; }
        public Bill ObjBill { get; set; }
        public int TractorId { get; set; }
        public double Discount { get; set; }
        public double TotalAmount { get; set; }
        public DateTime DateOfSale { get; set; }
        public DateTime WarrentyExpDate { get; set; }
    }
}
