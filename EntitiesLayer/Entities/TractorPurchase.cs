using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntitiesLayer.Entities;

namespace EntitiesLayer.Entities
{
    public class TractorPurchase
    {
        public int TractorId { get; set; }

        public int InvoiceId { get; set; }

        public int TractorModelId { get; set; }

        public string TractorEngineNo { get; set; }

        public string TractorChassisNo { get; set; }

        public string TractorSpecification { get; set; }

        public string TractorFIPNumber { get; set; }

        public string TractorAlternateMaker { get; set; }

        public string TractorSelfStarterMaker { get; set; }

        public int PDIHours { get; set; }

        public int ServiceBookNo { get; set; }

        public Invoice ObjInvoice { get; set; }

        public TractorModel ObjTractorModel { get; set; }
    }
}
