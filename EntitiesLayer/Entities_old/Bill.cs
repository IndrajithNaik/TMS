using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
   public class Bill: Customer
    {
        public int BillNumber { get; set; }
        public DateTime DateOfBill { get; set; }
        public float GrandTotal { get; set; }
        public string BillStatus { get; set; }
        public string ModeOfPayment { get; set; }
        public string ModeOfPurchase { get; set; }
        public float Balance { get; set; }
        public string BillType { get; set; }
        public string ServiceBookNumber { get; set; }

    }
}
