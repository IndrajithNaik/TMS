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
using TSUILayer.Views.PopUps;
using CommonLayer;
using EntitiesLayer.ViewModels;

namespace TSUILayer.Views.Sales
{
    /// <summary>
    /// Interaction logic for SellSpares.xaml
    /// </summary>
    public partial class SellSpares : UserControl
    {
        DataLayer data = null;
        List<SPARE_PURCHASES_SALE> lstSpareSale = new List<SPARE_PURCHASES_SALE>();
        IEnumerable<DDBinding> _autoSource = null;
        SALES_INVOICE SpareSaleInvoice = new SALES_INVOICE();

        public SellSpares()
        {
            InitializeComponent();
            data = DataLayer.Instance;
            lstSugesstions.DisplayMemberPath = "Name";
            lstSugesstions.SelectedValuePath = "Id";
            btnSaveAllSparesPurchase.IsEnabled = false;
            btnUpdate.IsEnabled = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridInvoices.ItemsSource = data.GetAll<SALES_INVOICE>(s => s.SELL_TYPE == data.GetMasterId(CommonLayer.TYPE.SPARE.ToString()))
                .Select(s => new InvoiceViewModel()
            {
                InvoiceNumber = s.SALES_INVOICE_ID.ToString(),
                InvoiceDate = s.INVOICE_DATE.Value.ToString("dd/MM/yyyy"),
                GrandTotal = s.INVOICE_GRANDTOTAL.Value
            });

            radioButtonSpares.IsChecked = true;
            cmbSupplier.ItemsSource = data.GetAllSuppliers(CommonLayer.TYPE.SPARE);
            cmbCustomer.ItemsSource = data.GetAllCustomers();
            lblInvoiceNo.Content = data.GetAll<SALES_INVOICE>().Count > 0 ? data.GetAll<SALES_INVOICE>().Max(s => s.SALES_INVOICE_ID) + 1 : 1;
        }

        decimal _SpareTotalAmount = 0;
        decimal _lubricantTotal = 0;
        decimal _spareTotalVat = 0;
        decimal _lubTotalVat = 0;

        private void btnAddSpares_Click(object sender, RoutedEventArgs e)
        {
            SPARE_PURCHASES_SALE spareSale = new SPARE_PURCHASES_SALE();

            if (ValidateMandatoryFields())
                return;

            spareSale.SUPPLIER_ID = Convert.ToInt32(cmbSupplier.SelectedValue);
            spareSale.SPARE_PART_ID = Convert.ToInt32(lstSugesstions.SelectedValue);
            spareSale.SUPPLIER = data.GetById<SUPPLIER>(s => s.SUPPLIER_ID == spareSale.SUPPLIER_ID);
            spareSale.SPARE_PART = data.GetById<SPARE_PART>(s => s.SPARE_PART_ID == spareSale.SPARE_PART_ID);
            spareSale.QUANTITY = Convert.ToInt32(txtQuantity.Text);
            spareSale.SUB_TOTAL = Convert.ToDecimal(txtSubTotal.Text);
            SpareSaleInvoice.SPARE_PURCHASES_SALEs.Add(spareSale);
            spareSale.TRANSACTION_TYPE = data.GetMasterId((radioButtonLubricants.IsChecked.Value ? CommonLayer.SPARETYPE.LUBRICANT : CommonLayer.SPARETYPE.ITEMS).ToString());
            gridSpares.ItemsSource = SpareSaleInvoice.SPARE_PURCHASES_SALEs.ToList();
            //var dummy = radioButtonSpares.IsChecked.Value ? _SpareTotalAmount += spareSale.INVOICE.INVOICE_SUBTOTAL : _lubricantTotal += spareSale.INVOICE.INVOICE_SUBTOTAL;
            if (radioButtonSpares.IsChecked ?? false)
                _SpareTotalAmount += spareSale.SUB_TOTAL;
            else
                _lubricantTotal += spareSale.SUB_TOTAL;
            lblTotalAmount.Content = (_SpareTotalAmount + _lubricantTotal).ToString();
            lblTotalAmount.Visibility = Visibility.Visible;

            gridSpares.Items.Refresh();
            ClearControls();
            SetDiscountVatAndTotalAmount();
            btnSaveAllSparesPurchase.IsEnabled = true;
        }

        private void ClearControls()
        {
            Common.ClearAllControls<TextBox>(gridControls);
            Common.ClearAllControls<ComboBox>(gridControls, 2);
        }

        private bool ValidateMandatoryFields()
        {
            if (txtSparePartCode.Text != string.Empty || txtPartCodeDescription.Text != string.Empty || txtQuantity.Text != string.Empty || txtUnitCost.Text.ToString() != string.Empty || txtSubTotal.Text.ToString() != string.Empty || cmbSupplier.Text != string.Empty)
            {
                return false;
            }
            else
            {
                MessageBox.Show("Please Fill the Mandatory Fields");
                return true;
            }
        }

        public void SetTotalAmount(object sender, RoutedEventArgs e)
        {
            lblGrandTotal.Content = (getGrandTotalInvoice() - (string.IsNullOrEmpty(txtDiscount.Text) ? 0 : Convert.ToDecimal(txtDiscount.Text))).ToString();
        }

        public void SetSubTotal(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUnitCost.Text))
            {
                txtSubTotal.Text = (Convert.ToDecimal(txtUnitCost.Text) * Convert.ToDecimal(string.IsNullOrEmpty(txtQuantity.Text) ? null : txtQuantity.Text)).ToString();
            }
        }

        private void btnSaveAllSparesPurchase_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Please verify the Purchase Invoice by Clicking View Invoice Once. Because Wrong entry might consume extra efforts or Loss of Data for Future Edit. You want to Proceed? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result.Equals(MessageBoxResult.No))
                return;
            SpareSaleInvoice.CUSTOMER_ID = Convert.ToInt32(cmbCustomer.SelectedValue);
            SpareSaleInvoice.INVOICE_DATE = dateTimeInvoiceDate.SelectedDate.Value;
            SpareSaleInvoice.INVOICE_DISCOUNT = string.IsNullOrEmpty(txtDiscount.Text) ? 0 : Convert.ToDecimal(txtDiscount.Text);
            SpareSaleInvoice.INVOICE_VAT_PERCENT = Convert.ToDecimal(lblVatInPercentage.Content);
            SpareSaleInvoice.INVOICE_GRANDTOTAL = Convert.ToDecimal(lblGrandTotal.Content);
            SpareSaleInvoice.SELL_TYPE = data.GetMasterId(CommonLayer.TYPE.SPARE.ToString());
            if (!string.IsNullOrEmpty(txtAmountPaid.Text))
            {
                BALANCE balancePaid = new BALANCE()
                {
                    AMOUNT_PAID = Convert.ToDecimal(txtAmountPaid.Text),
                    PAID_DATE = dateTimeInvoiceDate.SelectedDate.Value
                };
                SpareSaleInvoice.BALANCEs.Add(balancePaid);
            }
            data.Insert<SALES_INVOICE>(SpareSaleInvoice);
            gridSpares.ItemsSource = null;
            MessageBox.Show("Spares Details Added Successfully");
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


        private void AlphaNumerictOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsAlphaNumeric(e.Text);
        }
        private static bool IsAlphaNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]");
            return reg.IsMatch(str);
        }
        #endregion - UI Validations.

        private void radioButtonLubricants_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonSpares.IsChecked = false;
            lblVatInPercentage.Content = "14.5";
            if (!string.IsNullOrEmpty(txtSubTotal.Text))
                SetDiscountVatAndTotalAmount();
            ClearControls();

        }

        private void txtExpOnTotalAmount_KeyUp(object sender, KeyEventArgs e)
        {
            SetDiscountVatAndTotalAmount();
        }

        private void SetDiscountVatAndTotalAmount()
        {
            if (radioButtonLubricants.IsChecked == true || radioButtonSpares.IsChecked == true)
            {
                lblVatAmount.Content = getVatAmount().ToString();
                lblGrandTotal.Content = getGrandTotalInvoice().ToString();
            }
            else
            {
                MessageBox.Show("Please select the Radio Buttons");
            }
        }

        private decimal getGrandTotalInvoice()
        {
            return decimal.Round(_SpareTotalAmount + _lubricantTotal, 2);
        }

        private decimal getVatAmount()
        {
            if (radioButtonSpares.IsChecked.Value)

                _spareTotalVat = (Convert.ToDecimal(lblVatInPercentage.Content) / Convert.ToDecimal(105.5)) * _SpareTotalAmount;
            else
                _lubTotalVat = (Convert.ToDecimal(lblVatInPercentage.Content) / Convert.ToDecimal(114.5)) * _lubricantTotal;

            var _totalVat = _spareTotalVat + _lubTotalVat;
            
           //var _totalVat = ((Convert.ToDecimal(lblVatInPercentage.Content) / Convert.ToDecimal(105.5)) * _SpareTotalAmount) + ((Convert.ToDecimal(lblVatInPercentage.Content) / Convert.ToDecimal(114.5)) * _lubricantTotal);         
            return decimal.Round(_totalVat, 2);
        }

        private string getDiscountAmount()
        {
            double discountAmount = 0;
            return discountAmount.ToString();
        }

        private void radioButtonSpares_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonLubricants.IsChecked = false;
            lblVatInPercentage.Content = "5.5";
            if (!string.IsNullOrEmpty(txtSubTotal.Text))
                SetDiscountVatAndTotalAmount();
            ClearControls();
        }

        private void btnSearchInvoice_Click(object sender, RoutedEventArgs e)
        {
            dataGridInvoices.ItemsSource = data.GetAll<SALES_INVOICE>(s => s.SELL_TYPE == data.GetMasterId(CommonLayer.TYPE.SPARE.ToString()))
                .Where(s => s.SALES_INVOICE_ID == Convert.ToInt32(txtInvoiceSearch.Text))
               .Select(s => new InvoiceViewModel()
               {
                   InvoiceNumber = s.SALES_INVOICE_ID.ToString(),
                   InvoiceDate = s.INVOICE_DATE.Value.ToString("dd/MM/yyyy"),
                   GrandTotal = s.INVOICE_GRANDTOTAL.Value
               });
        }

        private void gridSparesAdded_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnViewInvoice_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _autoSource = data.GetAll<SPARE_PART>(s => s.MASTER.MASTER_VALUE.Equals(CommonLayer.STATUS.ACTIVE))
               .Join(data.GetAll<SPARE_RATE>(s => s.SUPPLIER_ID == Convert.ToInt32(cmbSupplier.SelectedValue)), sp => sp.SPARE_PART_ID, sr => sr.SPARE_PART_ID, (a, b) => a)
               .Select(d => new CommonLayer.DDBinding() { Id = d.SPARE_PART_ID, Name = d.SPARE_PART_CODE });
            txtPartCodeDescription.Text = string.Empty;
            txtUnitCost.Text = string.Empty;
            txtSubTotal.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtSparePartCode.Text = string.Empty;
        }

        private void txtSparePartCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_autoSource != null)
            {
                var result = _autoSource.Where(s => s.Name.ToLower().Contains(txtSparePartCode.Text.ToLower()) && !string.IsNullOrEmpty(txtSparePartCode.Text)).Select(s => s);
                lstSugesstions.Visibility = result.Count() <= 0 ? Visibility.Collapsed : Visibility.Visible;
                lstSugesstions.ItemsSource = result;
            }
        }

        private void lstSugesstions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtSparePartCode.TextChanged -= new TextChangedEventHandler(txtSparePartCode_TextChanged);
            if (lstSugesstions.SelectedValue != null)
            {
                txtSparePartCode.Text = (lstSugesstions.SelectedItem as DDBinding).Name;
                var result = data.GetAll<SPARE_RATE>(s => s.SPARE_PART_ID == Convert.ToInt32(lstSugesstions.SelectedValue) && s.SUPPLIER_ID == Convert.ToInt32(cmbSupplier.SelectedValue))
                    .Select(s => new { description = s.SPARE_PART.SPARE_PART_DESCRIPTION, unitCost = s.SPARE_RATE_VALUE.ToString() }).FirstOrDefault();
                if (result != null)
                {
                    txtPartCodeDescription.Text = result.description;
                    txtUnitCost.Text = result.unitCost;
                    SetSubTotal(null, null);
                }
            }
            lstSugesstions.Visibility = Visibility.Collapsed;
            txtSparePartCode.TextChanged += new TextChangedEventHandler(txtSparePartCode_TextChanged);
        }

        private void dataGridInvoices_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var selectedRow = dataGridInvoices.CurrentItem as InvoiceViewModel;

            if (selectedRow is InvoiceViewModel)
            {
                lstSpareSale.Clear();
                lstSpareSale = data.GetAll<SPARE_PURCHASES_SALE>(s => s.SALES_INVOICE_ID == Convert.ToInt32(selectedRow.InvoiceNumber));
                gridSpares.ItemsSource = lstSpareSale;
            }
            _SpareTotalAmount = 0;
            lstSpareSale.ForEach(s => { _SpareTotalAmount += s.SUB_TOTAL; });
            //if (lstSpareSale.Count > 0)
            //{
            //    txtDiscount.Text = lstSpareSale.First().SALES_INVOICE.INVOICE_DISCOUNT.ToString();
            //    txtExpOnTotalAmount_KeyUp(sender, null);
            //}
            lblTotalAmount.Content = _SpareTotalAmount;
            lblTotalAmount.Visibility = Visibility.Visible;
        }
    }
}
