using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using CommonLayer;
using System.Transactions;

namespace DataBaseLayer
{
    public class DataLayer : IDataLayer
    {
        DataTractorShowroomNewDataContext dataContext = new DataTractorShowroomNewDataContext();

        private static DataLayer _instance;

        public static DataLayer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataLayer();
                }
                return _instance;
            }


        }

        public List<T> GetAll<T>() where T : class
        {
            return dataContext.GetTable<T>().ToList<T>();
        }

        public void Insert<T>(T obj) where T : class
        {
            dataContext.GetTable<T>().InsertOnSubmit(obj);
            dataContext.SubmitChanges();
        }

        public void Update<T>() where T : class
        {
            dataContext.SubmitChanges();
        }

        public T GetById<T>(Expression<Func<T, bool>> condition) where T : class
        {
            return dataContext.GetTable<T>().Single(condition);
        }

        public List<T> GetAll<T>(Expression<Func<T, bool>> condition) where T : class
        {
            return dataContext.GetTable<T>().Where(condition).ToList<T>();
        }

        public void Insert<T>(List<T> obj) where T : class
        {
            dataContext.GetTable<T>().InsertAllOnSubmit(obj);
            dataContext.SubmitChanges();
        }

        public int GetMasterId(string value)
        {
            return dataContext.MASTERs.Where(s => s.MASTER_VALUE.Equals(value)).Select(s => s.MASTER_ID).FirstOrDefault();
        }

        public List<DDBinding> GetStatus()
        {
            return GetAll<MASTER>().Where(s => s.MASTER_VALUE.Equals(MASTERENUM.STATUS.ToString()))
                 .Join(GetAll<MASTER>(), T1 => T1.MASTER_ID, T2 => T2.MASTER_PARENT_ID, (a, b) =>
                     new DDBinding
                     {
                         Id = b.MASTER_ID,
                         Name = b.MASTER_VALUE
                     }).ToList();
        }

        public List<DDBinding> GetSupplierType()
        {
            return GetAll<MASTER>().Where(s => s.MASTER_VALUE.Equals(MASTERENUM.TYPE.ToString()))
                .Join(GetAll<MASTER>(), T1 => T1.MASTER_ID, T2 => T2.MASTER_PARENT_ID, (a, b) =>
                    new DDBinding
                    {
                        Id = b.MASTER_ID,
                        Name = b.MASTER_VALUE
                    }).ToList();
        }

        public List<DDBinding> GetMasterValues(MASTERENUM valuesOf)
        {
            return GetAll<MASTER>().Where(s => s.MASTER_VALUE.Equals(valuesOf.ToString()))
                .Join(GetAll<MASTER>(), T1 => T1.MASTER_ID, T2 => T2.MASTER_PARENT_ID, (a, b) =>
                    new DDBinding
                    {
                        Id = b.MASTER_ID,
                        Name = b.MASTER_VALUE
                    }).ToList();
        }

        public List<DDBinding> GetAllSuppliers(TYPE supplierType, TYPE otherType = TYPE.BOTH)
        {
            return GetAll<MASTER>().Where(x => x.MASTER_VALUE.Equals(supplierType.ToString()) || x.MASTER_VALUE.Equals(otherType.ToString()))
                 .Join(GetAll<SUPPLIER>(), m => m.MASTER_ID, s => s.SUPPLIER_TYPE, (a, b) =>
                     new DDBinding
                     {
                         Id = b.SUPPLIER_ID,
                         Name = b.SUPPLIER_NAME
                     }).ToList();
        }

        public List<DDBinding> GetAllTractorModels(int supplierId, STATUS tractorStatus = STATUS.ACTIVE)
        {
            return GetAll<MASTER>().Where(x => x.MASTER_VALUE.Equals(tractorStatus.ToString()))
                 .Join(GetAll<TRACTOR_MODEL>(), m => m.MASTER_ID, s => s.TRACTOR_STATUS, (a, b) => b).Where(d => d.SUPPLIER_ID == supplierId)
                 .Select(s =>
                     new DDBinding
                     {
                         Id = s.TRACTOR_MODEL_ID,
                         Name = s.TRACTOR_MODEL_NAME
                     }).ToList();
        }

        public List<DDBinding> GetAllCustomers(STATUS customerStatus = STATUS.ACTIVE)
        {
            return GetAll<MASTER>().Where(x => x.MASTER_VALUE.Equals(customerStatus.ToString()))
                 .Join(GetAll<CUSTOMER>(), m => m.MASTER_ID, s => s.STATUS_ID, (a, b) => b)
                 .Select(s =>
                     new DDBinding
                     {
                         Id = s.CUSTOMER_ID,
                         Name = s.CUSTOMER_NAME
                     }).ToList();
        }

        public List<SpareScarcityModel> GetSpareScarcity()
        {
            return GetAll<SPARE_PART>().GroupJoin(GetAll<SPARE_PURCHASES_SALE>(), a => a, b => b.SPARE_PART, (a, b) =>
                new { a, stock = (b.Where(s => s.TRANSACTION_TYPE == GetMasterId(TRANSACTIONTYPE.PURCHASE.ToString())).Sum(s => s.QUANTITY) - b.Where(s => s.TRANSACTION_TYPE == GetMasterId(TRANSACTIONTYPE.SALE.ToString())).Sum(s => s.QUANTITY)) })
                    .Where(q => q.stock <= q.a.SPARE_MINLEVEL && q.stock != 0)
                    .Select(s => new SpareScarcityModel()
                    {
                        SparePartId = s.a.SPARE_PART_ID,
                        SaprePartCode = s.a.SPARE_PART_CODE,
                        Quantity = s.stock,
                        PartDescription = s.a.SPARE_PART_DESCRIPTION,
                    }).ToList();
        }

        public List<T> GetDetails<T>(Func<T, int> joinOn, string invoiceNo, string invoiceDate, TYPE purchaseType) where T : class
        {
            return GetAll<INVOICE>(s => s.PURCHASE_TYPE == GetMasterId(purchaseType.ToString()))
                .Join(GetAll<T>(), i => i.INVOICE_ID, joinOn, (a, b) => new { a, b }).Where(s => s.a.INVOICE_NO.Equals(invoiceNo) && s.a.INVOICE_DATE.Equals(Convert.ToDateTime(invoiceDate)))
                .Select(s => s.b).ToList();
        }
    }
}
