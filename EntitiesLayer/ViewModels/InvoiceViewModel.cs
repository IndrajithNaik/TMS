using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.ViewModels
{
    public class InvoiceViewModel
    {
        public string InvoiceNumber { get; set; }

        public string InvoiceDate { get; set; }

        public decimal GrandTotal { get; set; }

    }
}
