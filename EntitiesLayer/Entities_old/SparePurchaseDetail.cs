using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class SparePurchaseDetail : Invoice
    {
        public List<Spare> _listOfSpares { get; set; }

        public SparePurchaseDetail(List<Spare> items)
        {
            _listOfSpares = items;
        }
    }
}
