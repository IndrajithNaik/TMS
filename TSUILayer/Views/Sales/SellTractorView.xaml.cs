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

namespace TSUILayer.Views
{
    /// <summary>
    /// Interaction logic for SellTractorView.xaml
    /// </summary>
    public partial class SellTractorView : UserControl
    {
        DataLayer data = null;

        List<TRACTOR_PURCHASE> lstPurchaseDetails = null;

        List<DELIVERY_CHALAN> lstDeliveryChalan = null;

        DELIVERY_CHALAN objDeliveryChalan = null;

        public SellTractorView()
        {
            InitializeComponent();
            data = DataLayer.Instance;
            lstDeliveryChalan = new List<DELIVERY_CHALAN>();
            objDeliveryChalan = new DELIVERY_CHALAN();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbCustomer.ItemsSource = data.GetAllCustomers();
            cmbSupplier.ItemsSource = data.GetAllSuppliers(CommonLayer.TYPE.TRACTOR);
            lblInvoiceNo.Content = data.GetAll<SALES_INVOICE>().Count > 0 ? data.GetAll<SALES_INVOICE>().Max(s => s.SALES_INVOICE_ID) + 1 : 1;
        }

        private void cmbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbTractorModel.ItemsSource = data.GetAllTractorModels(Convert.ToInt32(cmbSupplier.SelectedValue))
                .Join(data.GetAll<TRACTOR_PURCHASE>(), m => m.Id, t => t.TRACTOR_MODEL_ID, (a, b) => a).Distinct();
        }

        private void cmbTractorModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstPurchaseDetails = data.GetAll<TRACTOR_PURCHASE>(s => s.TRACTOR_MODEL_ID == Convert.ToInt32(cmbTractorModel.SelectedValue));

            if (lstPurchaseDetails != null)
            {
                cmbEngineNo.ItemsSource = lstPurchaseDetails.GroupJoin(data.GetAll<DELIVERY_CHALAN>(x => x.IS_DC), p => p, d => d.TRACTOR_PURCHASE, (a, b) => a)
                    .Where(s => s.DELIVERY_CHALANs.Count == 0 && s.TRACTOR_PDI_HOURS != null)
                    .Select(s => new DDBinding() { Id = s.TRACTOR_ID, Name = s.TRACTOR_ENGINE_NO });

                //cmbEngineNo.ItemsSource = from l in lstPurchaseDetails
                //                          join d in data.GetAll<SALES_INVOICE>(x => x.IS_DC.Value) on l equals d.TRACTOR_PURCHASE into ld
                //                          from d in ld.DefaultIfEmpty()
                //                          where l.SALES_INVOICEs.Count == 0 && l.TRACTOR_PDI_HOURS != null
                //                          select new DDBinding() { Id = l.TRACTOR_ID, Name = l.TRACTOR_ENGINE_NO };
            }
        }

        private void btnAddTractor_Click(object sender, RoutedEventArgs e)
        {
            if (objDeliveryChalan!=null)
            {
                objDeliveryChalan = new DELIVERY_CHALAN();
            }
            objDeliveryChalan.CUSTOMER_ID = Convert.ToInt32(cmbCustomer.SelectedValue);
            objDeliveryChalan.CUSTOMER = data.GetById<CUSTOMER>(s => s.CUSTOMER_ID == Convert.ToInt32(cmbCustomer.SelectedValue));
            objDeliveryChalan.TRACTOR_ID = Convert.ToInt32(cmbEngineNo.SelectedValue);
            objDeliveryChalan.TRACTOR_PURCHASE = data.GetById<TRACTOR_PURCHASE>(s => s.TRACTOR_ID == objDeliveryChalan.TRACTOR_ID);
            objDeliveryChalan.HYPOTHETICATION = txtHypothetication.Text;
            if (!string.IsNullOrEmpty(cmbDCNo.Text))
            {
                objDeliveryChalan.DC_DATE = dateOfDC.SelectedDate.Value;
                objDeliveryChalan.DC_NO = cmbDCNo.Text;
                objDeliveryChalan.IS_DC = chkDC.IsChecked.Value;
            }
            lstDeliveryChalan.Add(objDeliveryChalan);

            //lstSalesDetails.Add(TractorSaleInvoice);
            gridTractors.ItemsSource = lstDeliveryChalan;
            gridTractors.Items.Refresh();
            txtTractorSpecification.Clear();
            txtChassisNo.Clear();
            cmbEngineNo.Text = string.Empty;
            cmbTractorModel.Text = string.Empty;
        }

        private void btnSaveTractor_Click(object sender, RoutedEventArgs e)
        {
            if (!chkDC.IsChecked.Value)
            {
                SALES_INVOICE TractorSaleInvoice = new SALES_INVOICE();
                TractorSaleInvoice.CUSTOMER_ID = lstDeliveryChalan.First().CUSTOMER_ID;
                TractorSaleInvoice.CUSTOMER = lstDeliveryChalan.First().CUSTOMER;
                TractorSaleInvoice.INVOICE_VAT_PERCENT = Convert.ToDecimal(string.IsNullOrEmpty(txtVatInPercent.Text) ? null : txtVatInPercent.Text);
                TractorSaleInvoice.INVOICE_DATE = dateOfInvoice.SelectedDate;
                TractorSaleInvoice.INVOICE_DISCOUNT = Convert.ToDecimal(string.IsNullOrEmpty(txtTradeDiscount.Text) ? null : txtTradeDiscount.Text);
                TractorSaleInvoice.INVOICE_GRANDTOTAL = Convert.ToDecimal(lblTotalAmount.Content);
                TractorSaleInvoice.SELL_TYPE = data.GetMasterId(CommonLayer.TYPE.TRACTOR.ToString());
                lstDeliveryChalan.ForEach(s => { s.HYPOTHETICATION = txtHypothetication.Text; s.IS_DC = chkDC.IsChecked.Value; TractorSaleInvoice.DELIVERY_CHALANs.Add(s); });
                data.Insert<SALES_INVOICE>(TractorSaleInvoice);
                MessageBox.Show("Invoice Generated Sucessfully.");
            }
            else
            {
                data.Insert<DELIVERY_CHALAN>(objDeliveryChalan);
                MessageBox.Show("DC Generated Sucessfully.");
            }
        }

        private void btnSearchInvoice_Click(object sender, RoutedEventArgs e)
        {
            //dataGridInvoices.ItemsSource = null;
            //if (!string.IsNullOrEmpty(txtInvoiceForSearch.Text))
            //{
            //    dataGridInvoices.Columns.Where(s => s.Header.Equals("DC No.") || s.Header.Equals("Date")).All(s =>
            //    {
            //        if (s.Header.Equals("DC No."))
            //        {
            //            s.Header = "Invoice";
            //            (s as DataGridTextColumn).Binding = new Binding("INVOICE_ID");
            //        }
            //        else
            //        {
            //            (s as DataGridTextColumn).Binding = new Binding("INVOICE_DATE");
            //        }
            //        return true;
            //    });
            //    dataGridInvoices.ItemsSource = data.GetAll<SALES_INVOICE>(s => s.SALES_INVOICE_ID.ToString().Contains(txtInvoiceForSearch.Text));
            //}
            //else if (!string.IsNullOrEmpty(txtDCForSearch.Text))
            //{

            //   dataGridInvoices.Columns.Where(s => s.Header.Equals("Invoice") || s.Header.Equals("Date")).All(s =>
            //    {
            //        if (s.Header.Equals("Invoice"))
            //        {
            //            s.Header = "DC No.";
            //            (s as DataGridTextColumn).Binding = new Binding("DC_NO");
            //        }
            //        else
            //        {
            //            (s as DataGridTextColumn).Binding = new Binding("DC_DATE");
            //        }
            //        return true;
            //    });
            //    dataGridInvoices.ItemsSource = data.GetAll<SALES_INVOICE>().Where(s => s.DC_NO.Contains(txtDCForSearch.Text));
            //}
        }

        private void cmbCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var result = data.GetAll<DELIVERY_CHALAN>(s => s.CUSTOMER_ID == Convert.ToInt32(cmbCustomer.SelectedValue));
            if (result.Count > 0)
            {
                //chkDC.IsChecked = false;cmbDCNo.ItemsSource
                cmbDCNo.ItemsSource = result.Where(s => s.IS_DC).GroupBy(s => s.DC_NO, d => d.DC_NO).Select(s => s.Key);
            }
            //else
            //{
            //    chkDC.IsChecked = true;
            //    cmbDCNo.ItemsSource = null;
            //}
            //chkDC_Click(sender, e);
        }

        private void cmbEngineNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tractor = lstPurchaseDetails.Where(s => s.TRACTOR_ID == Convert.ToInt32(cmbEngineNo.SelectedValue)).FirstOrDefault();
            if (tractor != null)
            {
                txtTractorSpecification.Text = tractor.TRACTOR_SPECIFICATION;
                txtChassisNo.Text = tractor.TRACTOR_CHASSIS_NO;
                txtUnitCost.Text = tractor.TRACTOR_MODEL.TRACTOR_SHOWROOMRATE.ToString();
                txtVatInPercent.Text = tractor.INVOICE.INVOICE_VAT_PERCENT.ToString();
            }
            else
            {
                txtTractorSpecification.Text = string.Empty;
                txtChassisNo.Text = string.Empty;
                txtUnitCost.Text = string.Empty;
                txtVatInPercent.Text = string.Empty;
            }
        }

        private void txtTradeDiscount_KeyUp(object sender, KeyEventArgs e)
        {
            //txtGrandTotal.Text = decimal.Round((Convert.ToDecimal(txtUnitCost.Text) - Convert.ToDecimal(txtTradeDiscount.Text)), 2).ToString();
        }

        private void chkDC_Click(object sender, RoutedEventArgs e)
        {
            txtGrandTotal.IsEnabled=txtHypothetication.IsEnabled = dateOfInvoice.IsEnabled = txtUnitCost.IsEnabled = txtVatInPercent.IsEnabled = txtTradeDiscount.IsEnabled = !chkDC.IsChecked.Value;
            lblInvoiceNo.Visibility = chkDC.IsChecked.Value ? Visibility.Hidden : Visibility.Visible;
        }

        private void cmbDCNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstDeliveryChalan.Clear();
            if (cmbDCNo.SelectedValue != null)
            {
                lstDeliveryChalan = data.GetAll<DELIVERY_CHALAN>(s => s.DC_NO.Equals(cmbDCNo.SelectedValue));
                //lblInvoiceNo.Content = TractorSaleInvoice.SALES_INVOICE_ID;
                dateOfDC.SelectedDate = lstDeliveryChalan.First().DC_DATE;
                //cmbSupplier.Text = TractorSaleInvoice.TRACTOR_PURCHASE.TRACTOR_MODEL.SUPPLIER.SUPPLIER_NAME;
                //cmbTractorModel.Text = TractorSaleInvoice.TRACTOR_PURCHASE.TRACTOR_MODEL.TRACTOR_MODEL_NAME;
                //cmbEngineNo.Text = TractorSaleInvoice.TRACTOR_PURCHASE.TRACTOR_ENGINE_NO;
                //txtChassisNo.Text = TractorSaleInvoice.TRACTOR_PURCHASE.TRACTOR_CHASSIS_NO;
                //txtTractorSpecification.Text = TractorSaleInvoice.TRACTOR_PURCHASE.TRACTOR_SPECIFICATION;
                //txtUnitCost.Text = TractorSaleInvoice.TRACTOR_PURCHASE.TRACTOR_MODEL.TRACTOR_SHOWROOMRATE.ToString();
                txtVatInPercent.Text = lstDeliveryChalan.First().TRACTOR_PURCHASE.INVOICE.INVOICE_VAT_PERCENT.ToString();
                dateOfDC.IsEnabled = false;
                //chkDC.IsChecked = dateOfDC.IsEnabled = cmbSupplier.IsEnabled = cmbTractorModel.IsEnabled = cmbEngineNo.IsEnabled = false;
                //lstSalesDetails.Add(TractorSaleInvoice);
            }
            else
            {
                lblInvoiceNo.Content = dateOfDC.SelectedDate = null;
                cmbSupplier.Text = cmbTractorModel.Text = cmbEngineNo.Text = string.Empty;
                dateOfDC.IsEnabled = true;
                //chkDC.IsChecked = dateOfDC.IsEnabled = cmbSupplier.IsEnabled = cmbTractorModel.IsEnabled = cmbEngineNo.IsEnabled = true;
            }
            gridTractors.ItemsSource = lstDeliveryChalan;
            gridTractors.Items.Refresh();
            //chkDC_Click(sender, e);
        }

        private void txtTradeDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            var _vatAmount = Convert.ToDecimal(string.IsNullOrEmpty(txtUnitCost.Text) ? null : txtUnitCost.Text) * (Convert.ToDecimal(string.IsNullOrEmpty(txtVatInPercent.Text) ? null : txtVatInPercent.Text) / Convert.ToDecimal(105.5));
            var _grandtotal = Convert.ToDecimal(string.IsNullOrEmpty(txtUnitCost.Text) ? null : txtUnitCost.Text) + _vatAmount - Convert.ToDecimal(string.IsNullOrEmpty(txtTradeDiscount.Text) ? null : txtTradeDiscount.Text);
            lblTotalAmount.Content = decimal.Round(_grandtotal, 2).ToString();
        }

        private void txtGrandTotal_TextChanged(object sender, TextChangedEventArgs e)
        {
            var _grandTotal = Convert.ToDecimal(string.IsNullOrEmpty(txtGrandTotal.Text) ? null : txtGrandTotal.Text);
            var _vatAmount = _grandTotal * (Convert.ToDecimal(string.IsNullOrEmpty(txtVatInPercent.Text) ? null : txtVatInPercent.Text) / Convert.ToDecimal(105.5));
            var _unitCost = _grandTotal - _vatAmount;
            txtUnitCost.Text = decimal.Round(_unitCost, 2).ToString();
            lblTotalAmount.Content = decimal.Round(_grandTotal, 2);
        }
        /* http://download.teamviewer.com/download/TeamViewer_Setup_en.exe */
    }
}
