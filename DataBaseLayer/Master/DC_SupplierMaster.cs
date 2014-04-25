using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntitiesLayer.Entities;

namespace DataBaseLayer
{
    public partial class DataLayerComponent : IDataLayerComponent
    {
        #region - Supplier Master.
        public void AddSupplier(Supplier s)
        {
            try
            {
                tblSupplier tblSupplierObj = new tblSupplier();
                getTableSupplierEquivalentFromSupplierEntity(ref tblSupplierObj, s);
                dc.tblSuppliers.InsertOnSubmit(tblSupplierObj);
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        private void getTableSupplierEquivalentFromSupplierEntity(ref tblSupplier tblSupplierObj, Supplier s)
        {
            try
            {
                tblSupplierObj.supplierName = s.supplierName;
                tblSupplierObj.supplierStatus = s.supplierStatus;
                tblSupplierObj.supplierContactNo = s.supplierContactNumber;
                tblSupplierObj.supplierAddress = s.supplierAddress;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getSuppplierIdFromName(string p)
        {
            try
            {
                var supplierId = (from s in dc.tblSuppliers
                                  where s.supplierName.ToUpper() == p.ToUpper()
                                  select s.supplierId).FirstOrDefault();
                return supplierId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Supplier> GetSuppliersMatchingFullName(string str)
        {
            try
            {
                List<Supplier> supplierList = null;

                var supppliers = from s in dc.tblSuppliers
                                 where s.supplierName.StartsWith(str) && s.supplierStatus.ToUpper() != "BLOCK"
                                 select s;


                if (null != supppliers && supppliers.Count() > 0)
                {
                    supplierList = new List<Supplier>();
                    foreach (tblSupplier s in supppliers)
                    {
                        Supplier sup = new Supplier();
                        sup.supplierId = s.supplierId;
                        sup.supplierName = s.supplierName;
                        sup.supplierStatus = s.supplierStatus;
                        sup.supplierContactNumber = s.supplierContactNo;
                        sup.supplierAddress = s.supplierAddress;
                        supplierList.Add(sup);
                    }
                }

                return supplierList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetAllSuppliersNames()
        {
            try
            {
                var suppliers = (from s in dc.tblSuppliers
                                 where s.supplierStatus.ToUpper() != "BLOCK"
                                select s.supplierName.ToUpper()).Distinct();
                if (null != suppliers && suppliers.Count() > 0)
                {
                    return suppliers.ToList();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<string> GetAllSuppliersNames(string supplierName)
        {
            try
            {
                var supplier = (from s in dc.tblSuppliers
                                where s.supplierName.ToUpper() == supplierName
                                 select s.supplierName.ToUpper()).Distinct();
                if (null != supplier)
                {
                    return supplier.ToList();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Supplier GetSupplierFromId(int suplierId)
        {
            tblSupplier sup = (from s in dc.tblSuppliers
                               where s.supplierId == suplierId
                               select s).FirstOrDefault();

            Supplier supplier = GetCorrespondingSupplierFromtblSupplier(sup);

            return supplier;
        }

        private Supplier GetCorrespondingSupplierFromtblSupplier(tblSupplier sup)
        {
            return new Supplier
            {
                supplierId = sup.supplierId,
                supplierName = sup.supplierName,
                supplierStatus = sup.supplierStatus,
                supplierContactNumber = sup.supplierContactNo,
                supplierAddress = sup.supplierAddress
            };
        }

        public void UpdateSupplierDetails(Supplier _SupplierTobeEdited)
        {
            //tblSupplier updated = GetCorrespondingtblSupplierFromSupplier(_SupplierTobeEdited);

            tblSupplier toBeUpdated = (from s in dc.tblSuppliers
                                       where s.supplierId == _SupplierTobeEdited.supplierId
                                       select s).FirstOrDefault();

            GetCorrespondingtblSupplierFromSupplier(ref toBeUpdated, _SupplierTobeEdited);

            dc.SubmitChanges();
        }

        private void GetCorrespondingtblSupplierFromSupplier(ref tblSupplier sup, Supplier _SupplierTobeEdited)
        {
            //return new tblSupplier
            //{
            sup.supplierId = (int)_SupplierTobeEdited.supplierId;
            sup.supplierName = _SupplierTobeEdited.supplierName;
            sup.supplierStatus = _SupplierTobeEdited.supplierStatus;
            sup.supplierContactNo = _SupplierTobeEdited.supplierContactNumber;
            sup.supplierAddress = _SupplierTobeEdited.supplierAddress;
            //};
        }

        public List<Supplier> GetAllSuppliers()
        {
            return dc.tblSuppliers.Where(s => !s.supplierStatus.ToUpper().Equals("BLOCK")).Select(s => new Supplier()
            {
                supplierId = s.supplierId,
                supplierName = s.supplierName,
                supplierStatus = s.supplierStatus,
                supplierContactNumber = s.supplierContactNo,
                supplierAddress = s.supplierAddress

            }).ToList();
        }

        #endregion - Supplier Master.
    }
}
