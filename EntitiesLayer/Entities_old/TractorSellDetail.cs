using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class TractorSellDetail: Customer
    {
        public List<Tractor> _TractorsSold;

        TractorSellDetail(List<Tractor> tractorsSold)
        {
            _TractorsSold = tractorsSold;
        }

       
        public string ModeOfPurchase { get; set; }
        public bool BillStatus { get; set; }
        public float GrandTotal { get; set; }

        public void CalculateGrandTotal(List<Tractor> tractorsSold)
        {
            foreach (Tractor t in tractorsSold)
            {
                GrandTotal += t.TotalAmount;
            }
        }

        public string TractorsSold { get; set; }

        //public override string ToString(List<Tractor> tractorsSold)
        //{
        //    string tractorsSoldString = string.Empty;

        //    foreach (Tractor t in tractorsSold)
        //    {
        //        // This should be comma seperated values with each of the tractor fields with some delimter.
        //    }
        //    return tractorsSoldString;
        //}
    }

    //class TractorEntity:Tractor
    //{
    //    //public string TractorEngineNo { get; set; }
        //public string TractorChassisNo { get; set; }
        //public string TractorDescription { get; set; }
        //public int ServiceBookNo { get; set; }
        //public string BatteryNo { get; set; }
        //public float NetAmount { get; set; }
        //public int Quantity { get; set; }
        //public float VatInPercent { get; set; }
        //public float TotalAmount { get; set; }
    //}
}
