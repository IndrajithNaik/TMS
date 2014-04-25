using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntitiesLayer.Entities;

namespace DataBaseLayer
{
    public partial class DataLayerComponent : IDataLayerComponent
    {

        #region - Places Info.

        // [Start] - Places Info Form.
        public void AddDistrict(Village V)
        {

            bool isDPresent = checkIfDistrictAlreadyExists(V.District_Name);
            if (!isDPresent)
            {
                tblDistrict tD = new tblDistrict();
                tD.District_Name = V.District_Name;

                // Add the District
                dc.tblDistricts.InsertOnSubmit(tD);

                dc.SubmitChanges();
            }
            return;
        }

        private bool checkIfDistrictAlreadyExists(string p)
        {
            var isDPresent = from D in dc.tblDistricts
                             where p.ToUpper() == D.District_Name.ToUpper()
                             select D;
            if (null != isDPresent && isDPresent.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public void AddTaluk(Village V)
        {
            bool isTPresent = checkIfTalukAlreadyExists(V.Taluk_Name);
            if (!isTPresent)
            {
                tblTaluk tT = new tblTaluk();
                tT.Taluk_Name = V.Taluk_Name;
                tT.District_Id = getDistrictIdFromName(V.District_Name);

                // Add the Taluk
                dc.tblTaluks.InsertOnSubmit(tT);
                dc.SubmitChanges();
            }
            return;
        }

        private int getDistrictIdFromName(string p)
        {
            int D_Id = (from D in dc.tblDistricts
                        where D.District_Name.ToUpper() == p.ToUpper()
                        select D.District_Id).FirstOrDefault();
            return D_Id;
        }

        private bool checkIfTalukAlreadyExists(string p)
        {
            var isTPresent = from T in dc.tblTaluks
                             where p.ToUpper() == T.Taluk_Name.ToUpper()
                             select T;
            if (null != isTPresent && isTPresent.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public void AddVillage(Village V)
        {
            //throw new NotImplementedException();

            bool isVPresent = checkIfVillageAlreadyExists(V.Village_Name);
            if (!isVPresent)
            {
                tblVillage tV = new tblVillage();
                tV.Taluk_Id = getTalukIdFromName(V.Taluk_Name);
                tV.Village_Name = V.Village_Name;

                // Add the Village
                dc.tblVillages.InsertOnSubmit(tV);
                dc.SubmitChanges();
            }
            return;
        }

        private bool checkIfVillageAlreadyExists(string p)
        {
            //throw new NotImplementedException();
            var IsVPresent = from V in dc.tblVillages
                             where V.Village_Name.ToUpper() == p.ToUpper()
                             select V;

            if (null != IsVPresent && IsVPresent.Count() > 0)
            {
                return true;
            }
            return false;
        }

        private int getTalukIdFromName(string p)
        {
            //throw new NotImplementedException();
            int T_Id = (from T in dc.tblTaluks
                        where T.Taluk_Name.ToUpper() == p.ToUpper()
                        select T.Taluk_Id).FirstOrDefault();
            return T_Id;
        }


        public List<string> GetAllTaluksCorrespondingToDistrict(string p)
        {
            IEnumerable<string> taluks = from T in dc.tblTaluks
                                         where getDistrictIdFromName(p) == T.District_Id
                                         select T.Taluk_Name;
            return taluks.ToList();
        }

        public List<string> GetAllDistricts()
        {
            IEnumerable<string> districts = from D in dc.tblDistricts
                                            select D.District_Name;
            return districts.ToList();
        }

        public List<Village> GetAllVillages()
        {
            IEnumerable<tblVillage> villages = from V in dc.tblVillages
                                               select V;

            List<Village> villagesList = null;
            if (null != villages && villages.Count() > 0)
            {
                villagesList = new List<Village>();

                foreach (tblVillage tV in villages)
                {
                    Village V = new Village();
                    V.Village_Id = tV.Village_Id;
                    V.Village_Name = tV.Village_Name;
                    V.Taluk_Name = getTalukNameFromId(tV.Taluk_Id);
                    V.District_Name = getDistrictNameFromTalukId(tV.Taluk_Id);
                    villagesList.Add(V);
                }
            }
            return villagesList;
        }

        private string getDistrictNameFromTalukId(int p)
        {
            int? districtId = (from T in dc.tblTaluks
                               where p == T.Taluk_Id
                               select T.District_Id).FirstOrDefault();

            string districtName = (from D in dc.tblDistricts
                                   where D.District_Id == districtId
                                   select D.District_Name).FirstOrDefault();
            return districtName;
        }

        private string getTalukNameFromId(int p)
        {
            string talukName = (from T in dc.tblTaluks
                                where T.Taluk_Id == p
                                select T.Taluk_Name).FirstOrDefault();
            return talukName;
        }

        public System.Collections.IEnumerable GetAllVillagesCorrespondingToTaluk(string talukName)
        {
            int talukId = (from T in dc.tblTaluks
                           where T.Taluk_Name.ToUpper() == talukName.ToUpper()
                           select T.Taluk_Id).FirstOrDefault();

            IEnumerable<string> villages = from V in dc.tblVillages
                                           where V.Taluk_Id == talukId
                                           select V.Village_Name;
            return villages.ToList();
        }
        // [End] - Places Info Form.

        #endregion - Places Info.
    }
}
