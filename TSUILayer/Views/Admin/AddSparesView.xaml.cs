using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataBaseLayer;
using CommonLayer;
using EntitiesLayer.ViewModels;
using System.Collections.Generic;
//using TSUILayer.Views.PopUps;

namespace TSUILayer.Views.Admin
{
    /// <summary>
    /// Interaction logic for AddSparesView.xaml
    /// </summary>
    public partial class AddSparesView : UserControl
    {
        DataLayer data = null;
        static int _index = 0;
        static int _offSet = 0;
        List<SPARE_PART> spareParts = null;
        public AddSparesView()
        {
            InitializeComponent();
            data = DataLayer.Instance;
            btnUpdateSpare.IsEnabled = false;
            spareParts = data.GetAll<SPARE_PART>();
            BindSpares();
        }

        private void BindSpares()
        {
            _offSet = (spareParts.Count - _index) < 20 ? (spareParts.Count - _index) : 20;
            gridSpares.ItemsSource = null;

            if (spareParts.Count > 0)
            {
                gridSpares.ItemsSource = spareParts.GetRange(_index, _offSet).Select(s => new SpareViewModel()
                   {
                       SparePartCode = s.SPARE_PART_CODE,
                       Suppliers = s.SPARE_RATEs.Select(sp => new SpareSuppliers() { Id = sp.SPARE_RATE_VALUE, Name = sp.SUPPLIER.SUPPLIER_NAME }).ToList(),
                       SparePartDescription = s.SPARE_PART_DESCRIPTION,
                       BinNo = s.SPARE_BIN_NO
                   }).ToList();
            }
        }

        public AddSparesView(string userType)
        {
            if (userType.ToUpper() == "ADMIN")
            {
                btnUpdateSpare.Visibility = Visibility.Hidden;
            }
        }

        private void btnAddSpare_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSupplier.Text != string.Empty && txtSparePartCode.Text != string.Empty && txtPartDescription.Text != string.Empty && txtSpareUnitPrice.Text != string.Empty && txtMinLevel.Text != string.Empty)
            {
                SPARE_RATE rates = new SPARE_RATE();
                var result = data.GetAll<SPARE_PART>(s => s.SPARE_PART_CODE.ToUpper().Equals(txtSparePartCode.Text.ToUpper())).FirstOrDefault();
                if (result == null)
                {
                    rates.SPARE_RATE_VALUE = Convert.ToDecimal(txtSpareUnitPrice.Text);
                    rates.SUPPLIER_ID = Convert.ToInt32(cmbSupplier.SelectedValue);
                    rates.SPARE_PART = new SPARE_PART()
                    {
                        SPARE_PART_CODE = txtSparePartCode.Text,
                        SPARE_PART_DESCRIPTION = txtPartDescription.Text,
                        SPARE_MINLEVEL = Convert.ToInt32(txtMinLevel.Text),
                        SPARE_BIN_NO = txtSpareBinNumber.Text,
                        SPARE_STATUS = Convert.ToInt32(cmbSpareStatus.SelectedValue)
                    };
                    data.Insert<SPARE_RATE>(rates);
                }
                else
                {
                    var currentSpareRate = result.SPARE_RATEs.Where(s => s.SUPPLIER_ID == Convert.ToInt32(cmbSupplier.SelectedValue)).Select(s => s).FirstOrDefault();
                    if (currentSpareRate != null && Convert.ToInt32(cmbSupplier.SelectedValue) == currentSpareRate.SUPPLIER_ID)
                    {
                        var updateMsgRes = MessageBox.Show("Unit Cost value will be updated from " + currentSpareRate.SPARE_RATE_VALUE + " to " + txtSpareUnitPrice.Text, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (updateMsgRes.Equals(MessageBoxResult.Yes))
                        {
                            currentSpareRate.SPARE_RATE_VALUE = Convert.ToDecimal(txtSpareUnitPrice.Text);
                            data.Update<SPARE_RATE>();
                            MessageBox.Show("Spare Updated Successfully.");
                            Common.ClearAllControls<TextBox>(addSpares, 1);
                            Common.ClearAllControls<ComboBox>(addSpares);
                        }
                        return;
                    }
                    else
                    {
                        rates.SUPPLIER_ID = Convert.ToInt32(cmbSupplier.SelectedValue);
                        rates.SPARE_PART_ID = result.SPARE_PART_ID;
                        rates.SPARE_RATE_VALUE = Convert.ToDecimal(txtSpareUnitPrice.Text);
                        data.Insert<SPARE_RATE>(rates);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Fill All the mandatory fields");
                return;
            }

            MessageBoxResult MsgResult = MessageBox.Show("Verify If You have Chosen correct Supplier Becuase Supplier Information Will not be Edited in Future. You want to Proceed? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (MsgResult.Equals(MessageBoxResult.Yes))
            {
                BindSpares();
                MessageBox.Show("Spare Added Successfully");
                Common.ClearAllControls<TextBox>(addSpares, 1);
                Common.ClearAllControls<ComboBox>(addSpares);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbSpareStatus.ItemsSource = data.GetMasterValues(MASTERENUM.STATUS);
            cmbSupplier.ItemsSource = data.GetAllSuppliers(CommonLayer.TYPE.SPARE);
        }

        private void btnSearchSpare_Click(object sender, RoutedEventArgs e)
        {
            gridSpares.ItemsSource = spareParts.GetRange(_index, _offSet).Where(s => s.SPARE_PART_CODE.ToLower().Contains(txtSearchPartCode.Text.ToLower())).Select(s => new SpareViewModel()
            {
                SparePartCode = s.SPARE_PART_CODE,
                Suppliers = s.SPARE_RATEs.Select(sp => new SpareSuppliers() { Id = sp.SPARE_RATE_VALUE, Name = sp.SUPPLIER.SUPPLIER_NAME }).ToList(),
                SparePartDescription = s.SPARE_PART_DESCRIPTION,
                BinNo = s.SPARE_BIN_NO
            });
            gridSpares.Items.Refresh();
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

        private void btnUpdateSpare_Click(object sender, RoutedEventArgs e)
        {
            btnAddSpare.IsEnabled = true;
            btnUpdateSpare.IsEnabled = false;

            MessageBox.Show("Spare Updated Succesfully");
        }

        private void btnCloseSpares_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void gridSpares_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //btnAddSpare.IsEnabled = false;
            //btnUpdateSpare.IsEnabled = true;


            //int spareId = 0;
            //if (null != ((EntitiesLayer.Entities.Spare)(((System.Windows.Controls.DataGrid)(sender)).CurrentItem)))
            //{
            //    spareId = ((EntitiesLayer.Entities.Spare)(((System.Windows.Controls.DataGrid)(sender)).CurrentItem)).SpareId;
            //    //_spareUpdated = dc.GetSpareWithId(spareId);
            //    FillUIFromEditedSpare();
            //}

        }

        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SpareViewModel spareView = null;
            if ((spareView = gridSpares.CurrentItem as SpareViewModel) is SpareViewModel)
                spareView.SelectedPrice = Convert.ToDecimal((sender as ComboBox).SelectedValue);
        }

        private void pageBtn_Click(object sender, RoutedEventArgs e)
        {
            _index = (sender as Button).Content.Equals("Next") ? _index + 20 : (_index <= 0 ? 0 : _index - 20);
            BindSpares();
        }

        private void txtSparePartCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (_autoSource != null)
            //{
            //    var result = _autoSource.Where(s => s.Name.ToLower().Contains(txtSparePartCode.Text.ToLower()) && !string.IsNullOrEmpty(txtSparePartCode.Text)).Select(s => s);
            //    lstSugesstions.Visibility = result.Count() <= 0 ? Visibility.Collapsed : Visibility.Visible;
            //    lstSugesstions.ItemsSource = result;
            //}
        }

        private void lstSugesstions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //txtSparePartCode.TextChanged -= new TextChangedEventHandler(txtSparePartCode_TextChanged);
            //if (lstSugesstions.SelectedValue != null)
            //{
            //    txtSparePartCode.Text = (lstSugesstions.SelectedItem as DDBinding).Name;
            //    var result = data.GetAll<SPARE_RATE>(s => s.SPARE_PART_ID == Convert.ToInt32(lstSugesstions.SelectedValue) && s.SUPPLIER_ID == Convert.ToInt32(cmbSupplier.SelectedValue))
            //        .Select(s => new { description = s.SPARE_PART.SPARE_PART_DESCRIPTION, unitCost = s.SPARE_RATE_VALUE.ToString() }).FirstOrDefault();
            //    if (result != null)
            //    {
            //        txtPartCodeDescription.Text = result.description;
            //        txtUnitCost.Text = result.unitCost;
            //        SetSubTotal(null, null);
            //    }
            //}
            //lstSugesstions.Visibility = Visibility.Collapsed;
            //txtSparePartCode.TextChanged += new TextChangedEventHandler(txtSparePartCode_TextChanged);
        }

    }
}
