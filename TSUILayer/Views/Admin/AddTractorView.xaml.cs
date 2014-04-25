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
using CommonLayer;
using System.Configuration;
using System.Threading;
using System.IO;
//using TSUILayer.Views.PopUps;

namespace TSUILayer.Views.Admin
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class AddTractorView : UserControl
    {
        DataLayer data = null;

        public AddTractorView()
        {
            InitializeComponent();
            data = DataLayer.Instance;
            btnUpdateTractor.IsEnabled = false;
        }

        private void btnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openfile = new Microsoft.Win32.OpenFileDialog();
            openfile.Filter = "Image Files|*.jpg";
            if (openfile.ShowDialog() ?? false)
            {
                txtTractorImagePath.Text = openfile.FileName.ToString();

                BitmapImage src = new BitmapImage(new Uri(openfile.FileName));
                imgStackPanel.Children.Clear();
                imgStackPanel.Children.Add(new Image() { Source = src, Height = 240, Width = 400 });
            }

        }

        private void btnAddTractor_Click(object sender, RoutedEventArgs e)
        {
            if (txtTractorModel.Text != string.Empty && txtShowRoomRate.Text != string.Empty && cmbSupplier.Text != string.Empty && txtTractorImagePath.Text != string.Empty)
            {
                TRACTOR_MODEL tractorModel = new TRACTOR_MODEL();
                tractorModel.TRACTOR_MODEL_NAME = txtTractorModel.Text;
                tractorModel.SUPPLIER_ID = Convert.ToInt32(cmbSupplier.SelectedValue);
                tractorModel.TRACTOR_SHOWROOMRATE = Convert.ToDecimal(txtShowRoomRate.Text);
                tractorModel.TRACTOR_IMAGE = File.ReadAllBytes(txtTractorImagePath.Text);
                tractorModel.TRACTOR_STATUS = Convert.ToInt32(cmbTractorStatus.SelectedValue);
                data.Insert<TRACTOR_MODEL>(tractorModel);
            }
            else
            {
                MessageBox.Show("Please fill all mandatory fields.");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Verify If You have Choosen correct Supplier Becuase Supplier Information Will not be Edited in Future. You want to Proceed? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result.Equals(MessageBoxResult.Yes))
            {
                MessageBox.Show("Tractor Added Succesfully");

                Common.ClearAllControls<TextBox>(addTractors, 1);
                Common.ClearAllControls<ComboBox>(addTractors);
                imgStackPanel.Children.Clear();
                BindTractor();
            }
        }

        private void btnSearchTractor_Click(object sender, RoutedEventArgs e)
        {
            gridTractors.ItemsSource = data.GetAll<TRACTOR_MODEL>().Where(s => s.TRACTOR_MODEL_NAME.ToLower().Contains(txtTractorModelSearch.Text.ToLower())).Select((s, i) => new
            {
                SlNo = ++i,
                SupplierName = s.SUPPLIER.SUPPLIER_NAME,
                TractorModel = s.TRACTOR_MODEL_NAME,
                TractorShowRoomPrice = s.TRACTOR_SHOWROOMRATE
            });
            gridTractors.Items.Refresh();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BindTractor();
            cmbSupplier.ItemsSource = data.GetAllSuppliers(CommonLayer.TYPE.TRACTOR);
            cmbTractorStatus.ItemsSource = data.GetMasterValues(MASTERENUM.STATUS);
        }

        private void BindTractor()
        {
            gridTractors.ItemsSource = data.GetAll<TRACTOR_MODEL>().Select((s, i) => new
            {
                SlNo = ++i,
                SupplierName = s.SUPPLIER.SUPPLIER_NAME,
                TractorModel = s.TRACTOR_MODEL_NAME,
                TractorShowRoomPrice = s.TRACTOR_SHOWROOMRATE
            });
            gridTractors.Items.Refresh();
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

        private void btnUpdateTractor_Click(object sender, RoutedEventArgs e)
        {
            //GettblTractorEntityFromUI(_tractoBeUpdated);
            //data.UpdateTractorDetails(_tractoBeUpdated);
            //btnAddTractor.IsEnabled = true;

            //MessageBox.Show("Tractor Details Updated Succesfully");
        }


        private void gridTractors_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnAddTractor.IsEnabled = false;
            btnUpdateTractor.IsEnabled = true;

            //int tractorId = 0;
            //tractorId = ((EntitiesLayer.Entities.TractorPurchase)(((System.Windows.Controls.DataGrid)(sender)).CurrentItem)).TractorId;

            //_tractoBeUpdated = data.GetTractorFromId(tractorId);
            //FillUIFromTractorToBeEdited();
        }

        private void FillUIFromTractorToBeEdited()
        {
            //txtFullName.Text = _tractoBeUpdated.tractorName;
            //txtTractorModel.Text = _tractoBeUpdated.tractorModel;
            //txtShowRoomRate.Text = _tractoBeUpdated.tractorShowroomPrice.ToString();
            //cboSupplier.Text = data.GetSupplierFromId(_tractoBeUpdated.supplierId).supplierName;
            //txtTractorImagePath.Text = _tractoBeUpdated.tractorImagePath;
            //cmbTractorStatus.SelectedValue = _tractoBeUpdated.tractorStatus;

        }

        private void btnCloseTractor_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

    }
}
