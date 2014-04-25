using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class SpareRates
    {

        public int SpareRateId { get; set; }

        public int SparePartId { get; set; }

        public int SupplierId { get; set; }

        public decimal SpareRateValue { get; set; }

        public SpareParts ObjSpareParts { get; set; }

        public Supplier ObjSupplier { get; set; }

    }
}
