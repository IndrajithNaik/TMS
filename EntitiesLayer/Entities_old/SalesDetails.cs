using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    class SalesDetails: Bill
    {
        List<Spare> _sparesPurchased;
        List<TractorPurchase> _tractorsPurchased;
    }
}
