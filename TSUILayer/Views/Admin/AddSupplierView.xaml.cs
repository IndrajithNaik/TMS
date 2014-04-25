using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EntitiesLayer.Entities;
using DataBaseLayer;
using TSUILayer;
using System.ComponentModel;
using CommonLayer;

namespace TSUILayer.Views.Admin
{
    /// <summary>
    /// Interaction logic for AddSupplierView.xaml
    /// </summary>
    public partial class AddSupplierView : UserControl
    {
        DataLayer data = null;

        public AddSupplierView()
        {
            InitializeComponent();
            data = DataLayer.Instance;           
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            SUPPLIER supplier = new SUPPLIER();

            if (txtFullName.Text != string.Empty && txtAddress.Text != string.Empty && cboSupplierStatus.Text != string.Empty)
            {
                supplier.SUPPLIER_NAME = txtFullName.Text;
                supplier.SUPPLIER_ADDRESS = txtAddress.Text;
                supplier.SUPPLIER_CONTACT_NO = Convert.ToInt64(txtContactNo1.Text);
                supplier.SUPPLIER_TYPE = Convert.ToInt32(cmbSupplierType.SelectedValue);
                supplier.SUPPLIER_STATUS = Convert.ToInt32(cboSupplierStatus.SelectedValue);

                data.Insert<SUPPLIER>(supplier);

                BindSuppliers();

            }
            else
            {
                MessageBox.Show("Please Fill All the mandatory fields");
                return;
            }

            Common.ClearAllControls<TextBox>(addSupplier, 1);
            Common.ClearAllControls<ComboBox>(addSupplier);

            MessageBox.Show("Supplier Added Succesfully");
        }

        private void BindSuppliers()
        {
            gridSuppliers.ItemsSource = data.GetAll<SUPPLIER>().Select((s, i) => new
              {
                  SlNo = ++i,
                  SupplierName = s.SUPPLIER_NAME,
                  SupplierContactNumber = s.SUPPLIER_CONTACT_NO,
                  SupplierAddress = s.SUPPLIER_ADDRESS
              });
            gridSuppliers.Items.Refresh();
        }

        private void btnSearchSupplier_Click(object sender, RoutedEventArgs e)
        {
            gridSuppliers.ItemsSource = data.GetAll<SUPPLIER>().Where(s => s.SUPPLIER_NAME.ToLower().Contains(txtNameForSearch.Text.ToLower())).Select((s, i) => new
            {
                SlNo = ++i,
                SupplierName = s.SUPPLIER_NAME,
                SupplierContactNumber = s.SUPPLIER_CONTACT_NO,
                SupplierAddress = s.SUPPLIER_ADDRESS
            });
            gridSuppliers.Items.Refresh();
        }

        private void btnExportSuppliersToExcel_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcel<Supplier, List<Supplier>> s = new ExportToExcel<Supplier, List<Supplier>>();

            if (null != gridSuppliers.ItemsSource && gridSuppliers.Items.Count > 0)
            {
                s.dataToPrint = (List<Supplier>)gridSuppliers.ItemsSource;
                s.GenerateReport();
            }
            else
            {
                MessageBox.Show("No Data Available in Grid");
            }
        }


        #region - UI Validations.
        private void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
        }
        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);

        }


        private void AlphabetOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsAlphabet(e.Text);
        }
        private static bool IsAlphabet(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^A-Za-z]");
            return reg.IsMatch(str);
        }


        private void FloatingPointOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsFloatingPointvalue(e.Text);
        }
        private static bool IsFloatingPointvalue(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9.-]");
            return reg.IsMatch(str);
        }

        #endregion - UI Validations.

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BindSuppliers();
            cmbSupplierType.ItemsSource = data.GetMasterValues(MASTERENUM.TYPE);
            cboSupplierStatus.ItemsSource = data.GetMasterValues(MASTERENUM.STATUS);
            btnUpdateSupplier.IsEnabled = false;
        }

        private void btnCloseCustomer_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        public Supplier _SupplierTobeEdited = null;
        private void gridSuppliers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnAddSupplier.IsEnabled = false;
            btnUpdateSupplier.IsEnabled = true;
        }

        private void BindSupplierToRespectiveFields(Supplier s)
        {
            //txtFullName.Text = s.supplierName;
            //txtContactNo1.Text = s.supplierContactNumber;
            //txtAddress.Text = s.supplierAddress;
            //cboSupplierStatus.Text = s.supplierStatus;
        }

        private void btnUpdateSupplier_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Supplier details updated succesfully");

            //btnAddSupplier.IsEnabled = true;
            //btnUpdateSupplier.IsEnabled = false;
        }

    }
}
