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
using DataBaseLayer;
using EntitiesLayer.Entities;
using TSUILayer.Views.PopUps;
using CommonLayer;
using EntitiesLayer.ViewModels;
using System.Linq.Expressions;

namespace TSUILayer.Views
{
    /// <summary>
    /// Interaction logic for AddSpearsPurchaseDetailView.xaml
    /// </summary>
    public partial class AddSpearsPurchaseDetailView : UserControl
    {
        DataLayer data = null;
        List<SPARE_PURCHASES_SALE> lstSpareSale = new List<SPARE_PURCHASES_SALE>();
        IEnumerable<DDBinding> _autoSource = null;
        INVOICE SparePurchaseInvoice = new INVOICE();

        public AddSpearsPurchaseDetailView()
        {
            InitializeComponent();
            lstSugesstions.DisplayMemberPath = "Name";
            lstSugesstions.SelectedValuePath = "Id";
            data = DataLayer.Instance;
            btnSaveAllSparesPurchase.IsEnabled = false;
            btnUpdate.IsEnabled = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridInvoices.ItemsSource = data.GetAll<INVOICE>(s => s.PURCHASE_TYPE == data.GetMasterId(CommonLayer.TYPE.SPARE.ToString()))
                .Select(s => new InvoiceViewModel()
                {
                    InvoiceNumber = s.INVOICE_NO,
                    InvoiceDate = s.INVOICE_DATE.ToString("dd/MM/yyyy"),
                    GrandTotal = s.INVOICE_GRANDTOTAL
                });
            cmbSupplier.ItemsSource = data.GetAllSuppliers(CommonLayer.TYPE.SPARE);
            radioButtonSpares.IsChecked = true;
        }

        decimal _totalAmount = 0;

        private void btnAddSpares_Click(object sender, RoutedEventArgs e)
        {
            SPARE_PURCHASES_SALE spareSale = new SPARE_PURCHASES_SALE();

            if (ValidateMandatoryFields())
                return;

            spareSale.SUPPLIER_ID = Convert.ToInt32(cmbSupplier.SelectedValue);
            spareSale.SPARE_PART_ID = Convert.ToInt32(lstSugesstions.SelectedValue);
            spareSale.SUPPLIER = data.GetById<SUPPLIER>(s => s.SUPPLIER_ID == spareSale.SUPPLIER_ID);
            spareSale.SPARE_PART = data.GetById<SPARE_PART>(s => s.SPARE_PART_ID == spareSale.SPARE_PART_ID);
            //lblUnitPrice.Content = spareSale.SUPPLIER.SPARE_RATEs.Where(s => s.SPARE_PART_ID == spareSale.SPARE_PART_ID).Select(s => s.SPARE_RATE_VALUE.ToString()).FirstOrDefault();
            spareSale.QUANTITY = Convert.ToInt32(txtQuantity.Text);
            spareSale.TRANSACTION_TYPE = data.GetMasterId(CommonLayer.TRANSACTIONTYPE.PURCHASE.ToString());
            spareSale.SUB_TOTAL = Convert.ToDecimal(txtSubTotal.Text);
            SparePurchaseInvoice.SPARE_PURCHASES_SALEs.Add(spareSale);

            gridSparesAdded.ItemsSource = SparePurchaseInvoice.SPARE_PURCHASES_SALEs.ToList();
            _totalAmount += spareSale.SUB_TOTAL;
            lblTotalAmount.Content = _totalAmount.ToString();
            lblTotalAmount.Visibility = Visibility.Visible;

            gridSparesAdded.Items.Refresh();
            ClearControls();
            btnSaveAllSparesPurchase.IsEnabled = true;
        }

        private void ClearControls()
        {
            Common.ClearAllControls<ComboBox>(gridSparePurchase, 1);
            Common.ClearAllControls<TextBox>(gridSparePurchase, 1);
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
            if (txtUnitCost.Text.ToString() != string.Empty && txtQuantity.Text.ToString() != string.Empty)
            {
                txtSubTotal.Text = (float.Parse(txtUnitCost.Text.ToString()) * float.Parse(txtQuantity.Text)).ToString();
            }
        }

        private void btnSaveAllSparesPurchase_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Please verify the Purchase Invoice by Clicking View Invoice Once. Because Wrong entry might consume extra efforts or Loss of Data for Future Edit. You want to Proceed? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result.Equals(MessageBoxResult.No))
                return;

            SparePurchaseInvoice.INVOICE_NO = txtInvoiceNo.Text;
            SparePurchaseInvoice.INVOICE_DATE = dateTimeInvoiceDate.SelectedDate.Value;
            SparePurchaseInvoice.INVOICE_VAT_PERCENT = Convert.ToDecimal(lblVatInPercentage.Content);
            SparePurchaseInvoice.PURCHASE_TYPE = data.GetMasterId(CommonLayer.TYPE.SPARE.ToString());
            SparePurchaseInvoice.INVOICE_DISCOUNT = Convert.ToDecimal(txtDiscountAmount.Text);
            SparePurchaseInvoice.INVOICE_GRANDTOTAL = Convert.ToDecimal(txtGrandTotalOfInvoice.Text);
            SparePurchaseInvoice.EXPENDITURE_ON_SUBTOAL = Convert.ToDecimal(txtExpOnTotalAmount.Text);

            data.Insert<INVOICE>(SparePurchaseInvoice);

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
            lblDiscountInPercentage.Content = "27.20";

            SetDiscountVatAndTotalAmount();

        }

        private void txtExpOnTotalAmount_KeyUp(object sender, KeyEventArgs e)
        {
            SetDiscountVatAndTotalAmount();
        }

        private void SetDiscountVatAndTotalAmount()
        {
            if ((radioButtonLubricants.IsChecked == true || radioButtonSpares.IsChecked == true) && !string.IsNullOrEmpty(txtExpOnTotalAmount.Text))
            {
                txtDiscountAmount.Text = getDiscountAmount().ToString();
                txtAmountAfterDiscount.Text = getAmountAfterDiscount().ToString();
                txtVatInPercenAmount.Text = getVatAmount().ToString();
                txtGrandTotalOfInvoice.Text = (getVatAmount() + getAmountAfterDiscount() + Convert.ToDecimal(txtExpOnTotalAmount.Text)).ToString();
            }
        }

        private decimal getAmountAfterDiscount()
        {
            return decimal.Round(_totalAmount - getDiscountAmount(), 2);
        }

        private decimal getVatAmount()
        {
            var result = (Convert.ToDecimal(lblVatInPercentage.Content) / 100) * (_totalAmount + Convert.ToDecimal(txtExpOnTotalAmount.Text) - getDiscountAmount());
            return decimal.Round(result, 2);
        }

        private decimal getDiscountAmount()
        {
            var result = ((Convert.ToDecimal(lblDiscountInPercentage.Content) / 100) * _totalAmount);
            return decimal.Round(result, 2);
        }

        private void radioButtonSpares_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonLubricants.IsChecked = false;

            lblVatInPercentage.Content = "5.5";
            lblDiscountInPercentage.Content = "23.10";

            SetDiscountVatAndTotalAmount();
        }

        private void btnSearchInvoice_Click(object sender, RoutedEventArgs e)
        {
            dataGridInvoices.ItemsSource = data.GetAll<INVOICE>(s => s.PURCHASE_TYPE == data.GetMasterId(CommonLayer.TYPE.SPARE.ToString()))
                .Where(s => s.INVOICE_NO.ToLower().Contains(txtInvoiceSearch.Text.ToLower()))
                .Select(s => new InvoiceViewModel()
            {
                InvoiceNumber = s.INVOICE_NO,
                InvoiceDate = s.INVOICE_DATE.ToString("dd/MM/yyyy"),
                GrandTotal = s.INVOICE_GRANDTOTAL
            });

        }

        private void gridSparesAdded_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void FillDataToUIFields(object sender)
        {

        }

        private void GetSelectedSpareObject(ref Spare _selectedSpareForEdit, object sender)
        {
            _selectedSpareForEdit.SparePuchaseId = ((EntitiesLayer.Entities.Spare)(((System.Windows.Controls.DataGrid)(sender)).CurrentItem)).SparePuchaseId;
            _selectedSpareForEdit.SpareName = ((EntitiesLayer.Entities.Spare)(((System.Windows.Controls.DataGrid)(sender)).CurrentItem)).SpareName.ToString();
            _selectedSpareForEdit.SparePartCode = ((EntitiesLayer.Entities.Spare)(((System.Windows.Controls.DataGrid)(sender)).CurrentItem)).SparePartCode.ToString();
            _selectedSpareForEdit.Quantity = int.Parse(((EntitiesLayer.Entities.Spare)(((System.Windows.Controls.DataGrid)(sender)).CurrentItem)).Quantity.ToString());
            _selectedSpareForEdit.SpareUnitPriceForSale = float.Parse(((EntitiesLayer.Entities.Spare)(((System.Windows.Controls.DataGrid)(sender)).CurrentItem)).SpareUnitPriceForSale.ToString());
            _selectedSpareForEdit.SubTotal = float.Parse(((EntitiesLayer.Entities.Spare)(((System.Windows.Controls.DataGrid)(sender)).CurrentItem)).SubTotal.ToString());
            //_selectedSpareForEdit.supplierName = ((EntitiesLayer.Entities.Spare)(((System.Windows.Controls.DataGrid)(sender)).CurrentItem)).supplierName.ToString();
            _selectedSpareForEdit.SparePartDescription = ((EntitiesLayer.Entities.Spare)(((System.Windows.Controls.DataGrid)(sender)).CurrentItem)).SparePartDescription.ToString();

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Take all the New Data and Update the DB with New Values.

            //_selectedSparePurchaseDetailForEdit.SparePartCode = cmbSparePartCode.Text;
            //_selectedSparePurchaseDetailForEdit.SparePartDescription = txtPartCodeDescription.Text;
            //_selectedSparePurchaseDetailForEdit.SpareName = cmbSpares.Text;
            //_selectedSparePurchaseDetailForEdit.Quantity = int.Parse(txtQuantity.Text);
            //_selectedSparePurchaseDetailForEdit.SpareUnitPriceForSale = float.Parse(txtUnitCost.Text.ToString());
            //_selectedSparePurchaseDetailForEdit.SubTotal = float.Parse(txtSubTotal.Text.ToString());
            //_selectedSparePurchaseDetailForEdit.supplierName = cmbSupplier.Text;

            //UpdateLatestInvoiceRecieved(_selectedSparePurchaseDetailForEdit);

            //int purchaseDetailId = dc.GetPurchaseDetailId(_invoiceNumber, cmbSparePartCode.Text);

            // Update the Corresponding Invoice Amount.

            //dc.UpdateSparePurchaseInvoice(_selectedInvoiceForEdit);

            // Update the Stock Report.

            //dc.UpdateSelectedPurchaseDetail(_selectedSparePurchaseDetailForEdit);

            MessageBox.Show("Spares Details Updated Successfully");
        }

        private void btnViewInvoice_Click(object sender, RoutedEventArgs e)
        {
            //SparePurchaseDetail purchaseDetail = new SparePurchaseDetail(lstSpareSale);
            //string totalAmount = _totalAmount.ToString();

            //VerifyPurchaseInvoicePopUp VPIPP = new VerifyPurchaseInvoicePopUp(purchaseDetail, totalAmount);
            //VPIPP.Show();
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
                    txtQuantity_KeyUp(null, null);
                }
            }
            lstSugesstions.Visibility = Visibility.Collapsed;
            txtSparePartCode.TextChanged += new TextChangedEventHandler(txtSparePartCode_TextChanged);
        }

        private void txtQuantity_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUnitCost.Text) && !string.IsNullOrEmpty(txtQuantity.Text))
            {
                var temp = Convert.ToDecimal(txtUnitCost.Text) * Convert.ToDecimal(txtQuantity.Text);
                txtSubTotal.Text = temp.ToString();
            }
        }

        private void dataGridInvoices_SelectedCellsChanged(object sender, EventArgs e)
        {
            var selectedRow = dataGridInvoices.CurrentItem as InvoiceViewModel;

            if (selectedRow is InvoiceViewModel)
            {
                lstSpareSale.Clear();
                lstSpareSale = data.GetDetails<SPARE_PURCHASES_SALE>(s => s.INVOICE_ID ?? 0, selectedRow.InvoiceNumber, selectedRow.InvoiceDate, CommonLayer.TYPE.SPARE);
                gridSparesAdded.ItemsSource = lstSpareSale;
            }
            _totalAmount = 0;
            lstSpareSale.ForEach(s => { _totalAmount += s.SUB_TOTAL; });
            if (lstSpareSale.Count > 0)
            {
                txtExpOnTotalAmount.Text = lstSpareSale.First().INVOICE.EXPENDITURE_ON_SUBTOAL.ToString();
                txtExpOnTotalAmount_KeyUp(sender, null);
            }
            lblTotalAmount.Content = _totalAmount;
            lblTotalAmount.Visibility = Visibility.Visible;
        }
    }
}
