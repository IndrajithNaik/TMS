using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class SpareParts
    {

        public int SparePartId { get; set; }

        public int SparePartCode { get; set; }

        public string SparePartDescription { get; set; }

        public int SpareMinLevel { get; set; }

        public int SpareBinNo { get; set; }

        public STATUS SpareStatus { get; set; }

    }
}
