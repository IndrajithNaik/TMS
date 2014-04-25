using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesLayer.Entities
{
    //class JobCardForService: TractorSellDetail
    //{
    //    public int DealerId { get; set; }
    //    public DateTime DateOfCardIssue { get; set; }
    //    public DateTime DateTimeIn { get; set; }
    //    public bool WarrantyApplied { get; set; }
    //    public string ServiceCouponNo { get; set; }
    //    public bool PaidOrFree { get; set; }
    //    public bool isAccidental { get; set; }
    //    public DateTime ServiceStartDT { get; set;}
    //    public DateTime ServiceSEndDT { get; set; }
    //    public string PurposeOfBuying { get; set; }
    //    public float EstimatedCost { get; set; }
    //    public DateTime EstimatedDateOfDelivery { get; set; }
    //    public List<Complaint> _ComplaintList { get; set; }

    //    //JobCardForService(List<Complaint> complaints)
    //    //{
    //    //    _ComplaintList = complaints;
    //    //}

    //    public float TotalAmount { get; set; }
    //}

    public class Complaint
    {
        public string CustomerComplaint { get; set; }
        public string ActionTaken { get; set; }
        public float LabourCharges { get; set; }
    }
}
