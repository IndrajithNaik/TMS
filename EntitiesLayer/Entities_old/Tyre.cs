using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class Tyre
    {
        public int TyreId { get; set; }
        public string tyreMaker { get; set; }
        public string tyreSize { get; set; }
        public string tyreSerialNumber { get; set; }
        public string tyreRemarks { get; set; }
    }
}
