using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class TractorModel
    {
        public int TractorModelId { get; set; }

        public int SupplierId { get; set; }

        public string TractorModelName { get; set; }

        public decimal TractorShowRoomRate { get; set; }

        public byte[] TractorImage { get; set; }

        public STATUS TractorStatus { get; set; }

        public Supplier ObjSupplier { get; set; }

    }
}
