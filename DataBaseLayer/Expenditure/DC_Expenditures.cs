using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntitiesLayer.Entities;

namespace DataBaseLayer
{
    public partial class DataLayerComponent : IDataLayerComponent
    {
        # region - Expenditure Module.
        public string AddAllExpenditureHeads(List<ExpenditureHead> eHL)
        {
            //throw new NotImplementedException();

            string stringExpenditureAdd = string.Empty;

            foreach (ExpenditureHead eH in eHL)
            {
                if (!CheckIfExpenditureHeadPresent(eH.ExpenditureType))
                {
                    dc.tblExpenditureHeads.InsertOnSubmit(GetExpenditureTblEquivalent(eH));
                    dc.SubmitChanges();
                }

                else
                {
                    stringExpenditureAdd += eH.ExpenditureType + "";
                }
            }

            //if (stringExpenditureAdd != string.Empty)
            //{
            //    MessageBox.Show(stringExpenditureAdd + "Error while generating Excel report");
            //}
            return stringExpenditureAdd;
        }

        private bool CheckIfExpenditureHeadPresent(string p)
        {
            //throw new NotImplementedException();

            var isExists = from eH in dc.tblExpenditureHeads
                           where eH.Expenditure_Type == p
                           select eH;

            if (isExists.Count() > 0)
            {
                return true;
            }
            else return false;
        }

        private tblExpenditureHead GetExpenditureTblEquivalent(ExpenditureHead eH)
        {
            if (null != eH)
            {
                return new tblExpenditureHead
                {
                    Expenditure_Type = eH.ExpenditureType
                };
            }
            else
            {
                return null;
            }
        }

        public bool AddAllExpenditureDetails(List<ExpenditureDetail> eDL)
        {
            //throw new NotImplementedException();

            foreach (ExpenditureDetail eD in eDL)
            {
                dc.tblExpenditureDetails.InsertOnSubmit(GetExpenditureDetailTblEquivalent(eD));
                dc.SubmitChanges();
            }

            return true;
        }

        private tblExpenditureDetail GetExpenditureDetailTblEquivalent(ExpenditureDetail eD)
        {
            if (null != eD)
            {
                return new tblExpenditureDetail
                {
                    Expenditure_Id = getExpenditureId(eD.ExpenditureType),
                    ExpenditureDate = eD.ExpenditureDate,
                    VochourNumber = eD.VoucherNumber,
                    Perticulars = eD.Perticulars,
                    Amount = eD.Amount,
                };
            }
            else
            {
                return null;
            }
        }

        private int? getExpenditureId(string p)
        {
            int E_Id = (from eh in dc.tblExpenditureHeads
                        where eh.Expenditure_Type == p
                        select eh.Expenditure_Id).FirstOrDefault();

            return E_Id;
        }

        public List<ExpenditureHead> GetAllExpenditureheads()
        {
            IEnumerable<tblExpenditureHead> lstExpenditureHeads = from eH in dc.tblExpenditureHeads
                                                                  select eH;

            List<ExpenditureHead> expenditureHeadL = GetEquivalentEH(lstExpenditureHeads);

            return expenditureHeadL.ToList();
        }

        private List<ExpenditureHead> GetEquivalentEH(IEnumerable<tblExpenditureHead> lstExpenditureHeads)
        {
            List<ExpenditureHead> expenditureHeadL = new List<ExpenditureHead>();

            if (lstExpenditureHeads.Count() > 0)
            {
                foreach (tblExpenditureHead expH in lstExpenditureHeads)
                {
                    ExpenditureHead exH = new ExpenditureHead();
                    exH.ExpenditureId = expH.Expenditure_Id;
                    exH.ExpenditureType = expH.Expenditure_Type;

                    expenditureHeadL.Add(exH);
                }
            }

            return expenditureHeadL;

        }
        public int GetLastestVoucherNumber()
        {
            //throw new NotImplementedException();
            if (dc.tblExpenditureDetails.Count() > 0)
            {
                //return (from v in dc.tblExpenditureDetails
                       // select v.ExpenditureDetail_Id).Last();

                //return (new <DataBaseLayer.tblExpenditureDetail>(dc.tblExpenditureDetails)).Items[dc.tblExpenditureDetails.Count() - 1]._ExpenditureDetail_Id;

                return dc.tblExpenditureDetails.Count();
            }

            return 1;
        }
        # endregion - Expenditure Module.
    }
}
