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
using CommonLayer;
using EntitiesLayer.ViewModels;

namespace TSUILayer.Views
{
    /// <summary>
    /// Interaction logic for AddTractorPurchaseDetailView.xaml
    /// </summary>
    public partial class AddTractorPurchaseDetailView : UserControl
    {
        DataLayer data = null;
        List<TRACTOR_PURCHASE> tractors = new List<TRACTOR_PURCHASE>();
        INVOICE TractorInvoice = new INVOICE();

        public AddTractorPurchaseDetailView()
        {
            InitializeComponent();
            data = DataLayer.Instance;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridInvoices.ItemsSource = data.GetAll<INVOICE>(s => s.PURCHASE_TYPE == data.GetMasterId(CommonLayer.TYPE.TRACTOR.ToString()))
                .Select(s => new InvoiceViewModel()
            {
                InvoiceNumber = s.INVOICE_NO,
                InvoiceDate = s.INVOICE_DATE.ToString("dd/MM/yyyy"),
                GrandTotal = s.INVOICE_GRANDTOTAL
            });
            cmbSupplier.ItemsSource = data.GetAllSuppliers(CommonLayer.TYPE.TRACTOR);
        }

        decimal _total = 0;

        private void btnAddTractor_Click(object sender, RoutedEventArgs e)
        {
            TRACTOR_PURCHASE tractor = new TRACTOR_PURCHASE();
            tractor.TRACTOR_MODEL = data.GetById<TRACTOR_MODEL>(s => s.TRACTOR_MODEL_ID == Convert.ToInt32(cmbTractorModel.SelectedValue));
            tractor.TRACTOR_SPECIFICATION = txtTractorSpecification.Text;
            tractor.TRACTOR_ENGINE_NO = txtEngineNo.Text;
            tractor.TRACTOR_CHASSIS_NO = txtChassisNo.Text;
            tractor.TRACTOR_PURCHASE_RATE = Convert.ToDecimal(txtUnitCost.Text);
            TractorInvoice.TRACTOR_PURCHASEs.Add(tractor);
            gridTractors.ItemsSource = TractorInvoice.TRACTOR_PURCHASEs.ToList();
            Common.ClearAllControls<TextBox>(gridTractorPurchase, 2);
            Common.ClearAllControls<DatePicker>(gridTractorPurchase, 1);
            Common.ClearAllControls<ComboBox>(gridTractorPurchase, 1);
            _total += tractor.TRACTOR_PURCHASE_RATE;
            ShowTotal();
            btnSaveTractor.IsEnabled = true;
        }

        private void ShowTotal()
        {
            lblTotal.Content = decimal.Round(_total, 2).ToString();
            lblTotal.Visibility = Visibility.Visible;
        }

        private void btnSaveTractor_Click(object sender, RoutedEventArgs e)
        {
            TractorInvoice.INVOICE_VAT_PERCENT = Convert.ToDecimal(txtVatInPercent.Text);
            TractorInvoice.INVOICE_DISCOUNT = Convert.ToDecimal(txtTradeDiscount.Text);
            TractorInvoice.INVOICE_NO = txtInvoiceNumber.Text;
            TractorInvoice.INVOICE_DATE = dateOfInvoice.SelectedDate.Value;
            TractorInvoice.PURCHASE_TYPE = data.GetMasterId(CommonLayer.TYPE.TRACTOR.ToString());
            TractorInvoice.INVOICE_GRANDTOTAL = Convert.ToDecimal(txtGrandTotal.Text);
            data.Insert<INVOICE>(TractorInvoice);
            MessageBox.Show("Added Tractor Succesfully");
            Common.ClearAllControls<TextBox>(gridTractorPurchase);
            Common.ClearAllControls<DatePicker>(gridTractorPurchase);
            Common.ClearAllControls<ComboBox>(gridTractorPurchase);
            gridTractors.ItemsSource = null;
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

        private void txtTradeDiscount_KeyUp(object sender, KeyEventArgs e)
        {
            var vatAmount = (_total + Convert.ToDecimal(txtInsuranceAndOthers.Text)) * (Convert.ToDecimal(txtVatInPercent.Text) / 100);
            var grandTotal = _total + vatAmount + Convert.ToDecimal(txtInsuranceAndOthers.Text) - (!string.IsNullOrEmpty(txtTradeDiscount.Text) ? Convert.ToDecimal(txtTradeDiscount.Text) : 0);
            txtGrandTotal.Text = decimal.Round(grandTotal, 2).ToString();
        }

        private void btnViewInvoice_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void cmbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbTractorModel.ItemsSource = data.GetAllTractorModels(Convert.ToInt32(cmbSupplier.SelectedValue));
        }

        private void btnSearchInvoice_Click(object sender, RoutedEventArgs e)
        {
            dataGridInvoices.ItemsSource = data.GetAll<INVOICE>(s => s.PURCHASE_TYPE == data.GetMasterId(CommonLayer.TYPE.TRACTOR.ToString()))
               .Where(s => s.INVOICE_NO.ToLower().Contains(txtInvoiceForSearch.Text.ToLower()))
               .Select(s => new InvoiceViewModel()
               {
                   InvoiceNumber = s.INVOICE_NO,
                   InvoiceDate = s.INVOICE_DATE.ToString("dd/MM/yyyy"),
                   GrandTotal = s.INVOICE_GRANDTOTAL
               });
        }

        private void dataGridInvoices_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var selectedRow = dataGridInvoices.CurrentItem as InvoiceViewModel;

            if (selectedRow is InvoiceViewModel)
            {                
                _total = 0;
                tractors.Clear();
                tractors = data.GetDetails<TRACTOR_PURCHASE>(s => s.INVOICE_ID, selectedRow.InvoiceNumber, selectedRow.InvoiceDate, CommonLayer.TYPE.TRACTOR);
                gridTractors.ItemsSource = tractors;
                tractors.All(s => { _total += s.TRACTOR_PURCHASE_RATE; return true; });
                ShowTotal();
            }
        }

    }
}
