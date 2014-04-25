using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    public class District
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }

    }

    public class Taluk
    {
        public int TalukId { get; set; }
        public string TalukName { get; set; }
        public District ObjDistrict { get; set; }
    }

    public class Village
    {
        public int VillageId { get; set; }
        public string VillageName { get; set; }
        public Taluk ObjTaluk { get; set; }
    }

}
