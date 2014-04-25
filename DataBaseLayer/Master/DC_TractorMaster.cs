using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntitiesLayer.Entities;

namespace DataBaseLayer
{
    public partial class DataLayerComponent : IDataLayerComponent
    {
        # region - Tractor Master.
        public void AddTractorModels(TractorPurchase t)
        {
            try
            {
                tblTractor tblTractorObj = new tblTractor();

                getTableTractorEquivalentFromTractorEntity(ref tblTractorObj, t);
                dc.tblTractors.InsertOnSubmit(tblTractorObj);
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void getTableTractorEquivalentFromTractorEntity(ref tblTractor tblTractorObj, TractorPurchase t)
        {
            try
            {
                tblTractorObj.supplierId = Convert.ToInt32((from s in dc.tblSuppliers
                                                            where s.supplierName == t.supplierName
                                                            select s.supplierId).FirstOrDefault().ToString());
                tblTractorObj.supplierId = getSuppplierIdFromName(t.supplierName);
                tblTractorObj.tractorName = t.TractorName;
                tblTractorObj.tractorModel = t.TractorModel;
                tblTractorObj.tractorShowroomPrice = t.TractorShowRoomPrice;
                tblTractorObj.tractorImagePath = t.imagePath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //ktblTractorObj.tractorImage = t.Tr
        }

        public List<TractorPurchase> GetTractorssMatchingFullName(string str)
        {
            try
            {
                List<TractorPurchase> tractorList = null;

                var tractors = from t in dc.tblTractors
                               where t.tractorName.StartsWith(str)
                               select t;


                if (null != tractors && tractors.Count() > 0)
                {
                    tractorList = new List<TractorPurchase>();
                    foreach (tblTractor t in tractors)
                    {
                        TractorPurchase tra = new TractorPurchase();
                        tra.TractorId = t.tractorId;
                        tra.supplierName = t.tblSupplier.supplierName;
                        tra.TractorName = t.tractorName;
                        tra.TractorModel = t.tractorModel;
                        tra.TractorShowRoomPrice = Double.Parse(t.tractorShowroomPrice.ToString());
                        tractorList.Add(tra);
                    }
                }

                return tractorList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public tblTractor GetTractorFromId(int tractorId)
        {
            return (from t in dc.tblTractors
                    where t.tractorId == tractorId
                    select t).FirstOrDefault();
        }

        public void UpdateTractorDetails(tblTractor _tractoBeUpdated)
        {
            dc.SubmitChanges();
        }

        public List<TractorPurchase> GetTractorBySupplierId(int supplierId)
        {
            return dc.tblTractors.Where(s => s.supplierId == supplierId).Select(s => new TractorPurchase()
       {
           TractorId = s.tractorId,
           TractorModel = s.tractorModel,
           TractorName = s.tractorName,
           TractorEngineNo = dc.tblTractorPurchaseDetails.Where(p => p.tractorId == s.tractorId).Select(p => p.engineNumber).First(),
           TractorChassisNo = dc.tblTractorPurchaseDetails.Where(p => p.tractorId == s.tractorId).Select(p => p.chassisNumber).First(),
           TractorSpecification = dc.tblTractorPurchaseDetails.Where(p => p.tractorId == s.tractorId).Select(p => p.tractorSpecification).First(),
           TractorShowRoomPrice = dc.tblTractorPurchaseDetails.Where(p => p.tractorId == s.tractorId).Select(p => p.subTotalUnitPurchaseRate).First(p => p.HasValue) ?? 0.0,
           InsuranceAndOthers = dc.tblTractorPurchaseDetails.Where(p => p.tractorId == s.tractorId).Select(p => (float)p.insuranceAndOthers).First(),
           GrandTotal = dc.tblTractorPurchaseDetails.Where(p => p.tractorId == s.tractorId).Select(p => (float)p.grandTotal).First(),
       }).ToList();
        }
        # endregion - Tractor Master.
    }
}
