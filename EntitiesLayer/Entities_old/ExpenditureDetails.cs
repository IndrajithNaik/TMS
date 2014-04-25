using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class ExpenditureDetail: ExpenditureHead
    {
        //List<ExpenditureHead> _expenditureHeads { get; set; }
        public DateTime ExpenditureDate { get; set; }
        public int VoucherNumber { get; set; }
        public double Amount { get; set; }
        public string Perticulars { get; set; }
    }
}
