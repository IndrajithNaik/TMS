using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using CommonLayer;

namespace DataBaseLayer
{
    interface IDataLayer
    {
        List<T> GetAll<T>() where T : class;

        List<T> GetAll<T>(Expression<Func<T, bool>> condition) where T : class;

        void Insert<T>(T obj) where T : class;

        void Update<T>() where T : class;

        T GetById<T>(Expression<Func<T, bool>> condition) where T : class;

        void Insert<T>(List<T> obj) where T : class;

        int GetMasterId(string value);

        List<DDBinding> GetStatus();

        List<DDBinding> GetSupplierType();

        List<DDBinding> GetMasterValues(MASTERENUM valuesOf);

        List<DDBinding> GetAllSuppliers(TYPE supplierType, TYPE otherType = TYPE.BOTH);

        List<DDBinding> GetAllTractorModels(int supplierId, STATUS tractorStatus = STATUS.ACTIVE);

        List<DDBinding> GetAllCustomers(STATUS customerStatus = STATUS.ACTIVE);

        List<SpareScarcityModel> GetSpareScarcity();

        List<T> GetDetails<T>(Func<T, int> joinOn, string invoiceNo, string invoiceDate, TYPE purchaseType) where T : class;

    }
}
