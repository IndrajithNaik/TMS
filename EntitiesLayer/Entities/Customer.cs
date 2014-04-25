using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public int VillageId { get; set; }

        public string CustomerName { get; set; }

        public Int64 CustomerContactNo { get; set; }

        public Int64 CustomerContactNo1 { get; set; }

        public string CustomerAdress { get; set; }

        public string CustomerEmail { get; set; }

        public STATUS CustomerStatus { get; set; }

        public Village ObjVillage { get; set; }

    }
}
