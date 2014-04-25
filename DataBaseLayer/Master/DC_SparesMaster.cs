using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntitiesLayer.Entities;

namespace DataBaseLayer
{
    public partial class DataLayerComponent : IDataLayerComponent
    {
        # region - Spare Master.
        public void AddSpareDetails(Spare s)
        {
            try
            {
                tblSpare tblSpareObj = new tblSpare();

                getTableSpareEquivalentFromTractorEntity(ref tblSpareObj, s);
                dc.tblSpares.InsertOnSubmit(tblSpareObj);
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void getTableSpareEquivalentFromTractorEntity(ref tblSpare tblSpareObj, Spare s)
        {
            try
            {
                tblSpareObj.supplierId = getSuppplierIdFromName(s.supplierName);
                tblSpareObj.spareName = s.SpareName;
                tblSpareObj.sparePartNo = s.SparePartCode;
                tblSpareObj.spareDescription = s.SparePartDescription;
                tblSpareObj.spareUnitPrice = s.SpareUnitPrice;
                tblSpareObj.spareMinMaxLevel = s.MinMaxAndRolevel;
                tblSpareObj.spareStatus = s.SpareStatus;
                tblSpareObj.spareBinNumber = s.SpareBinNumber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<Spare> GetSparesMatchingFullName(string str)
        {
            try
            {
                List<Spare> spareList = null;

                var spares = from s in dc.tblSpares
                             where s.spareName.StartsWith(str)
                             select s;


                if (spares.Count() > 0)
                {
                    spareList = new List<Spare>();
                    foreach (tblSpare s in spares)
                    {
                        Spare spa = new Spare();
                        spa.SpareId = s.spareId;
                        spa.supplierName = s.tblSupplier.supplierName;
                        spa.SparePartCode = s.sparePartNo;
                        spa.SparePartDescription = s.spareDescription;
                        spa.SpareUnitPrice = Double.Parse(s.spareUnitPrice.ToString());
                        spareList.Add(spa);
                    }
                }

                return spareList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetAllSparesName()
        {
            var allSpares = from s in dc.tblSpares
                            select s.spareName;
            return allSpares.ToList();
        }

        public List<string> GetAllSparesName(string str)
        {
            var allSpares = (from s in dc.tblSpares
                            where s.tblSupplier.supplierName == str
                            select s.spareName.ToUpper()).Distinct();
            return allSpares.ToList();
        }
        public tblSpare GetSpareWithId(int spareId)
        {
            return (from p in dc.tblSpares
                    where p.spareId == spareId
                    select p).FirstOrDefault();
        }

        public void UpdateSparesDetails(tblSpare s)
        {
            dc.SubmitChanges();
        }
        # endregion - Spare Master.
    }
}
