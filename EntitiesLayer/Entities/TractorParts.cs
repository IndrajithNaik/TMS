using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class TractorParts
    {
        public int TractorPartId { get; set; }

        public int TractorId { get; set; }

        public int BatteryId { get; set; }

        public string PartMaker { get; set; }
        
        public string PartSize { get; set; }
        
        public string PartSerialNo { get; set; }
        
        public string PartRemarks { get; set; }

        public PARTTYPE PartType { get; set; }
    }
}
