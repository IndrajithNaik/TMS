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
using EntitiesLayer.ViewModels;
using CommonLayer;

namespace TSUILayer.Views.Purchase
{
    /// <summary>
    /// Interaction logic for AddPDIReportView.xaml
    /// </summary>
    public partial class AddPDIReportView : UserControl
    {
        DataLayer data = null;
        TRACTOR_PURCHASE tractorPurchase = null;
        List<TRACTOR_PURCHASE> lstTractorPurchases = null;

        public AddPDIReportView()
        {
            InitializeComponent();
            data = DataLayer.Instance;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridTractorInvoices.ItemsSource = data.GetAll<INVOICE>(s => s.PURCHASE_TYPE == data.GetMasterId(CommonLayer.TYPE.TRACTOR.ToString()))        
                .Select(s => new InvoiceViewModel()
            {
                InvoiceNumber = s.INVOICE_NO,
                InvoiceDate = s.INVOICE_DATE.ToString("dd/MM/yyyy")
            });
        }

        private void btnSaveTractorPDIReport_Click(object sender, RoutedEventArgs e)
        {
            tractorPurchase.TRACTOR_FIP_NO = txtFIPNo.Text;
            tractorPurchase.TRACTOR_ALTERNATE_MAKER = txtAlternateMaker.Text;
            tractorPurchase.TRACTOR_SELFSTARTMAKER = txtStarterMotorMake.Text;
            tractorPurchase.TRACTOR_PDI_HOURS = Convert.ToDecimal(txtPDIHours.Text);

            TRACTOR_PART tractorPart = null;
            int i = 0;

            gridTyreDetails.Children.OfType<TextBox>().All(s =>
            {
                switch (i++)
                {
                    case 0: tractorPart = new TRACTOR_PART() { PART_TYPE = data.GetMasterId((s.Name.Contains("Battery") ? CommonLayer.PARTTYPE.BATTERY : CommonLayer.PARTTYPE.TYRE).ToString()) };
                        tractorPart.PART_MAKER = s.Text;
                        break;
                    case 1: tractorPart.PART_SIZE = s.Text;
                        break;
                    case 2: tractorPart.PART_SERIAL_NO = s.Text;
                        break;
                    case 3: tractorPart.PART_REMARKS = s.Text;
                        tractorPurchase.TRACTOR_PARTs.Add(tractorPart);
                        i = 0;
                        break;
                }
                return true;
            });

            data.Update<TRACTOR_PURCHASE>();
            MessageBox.Show("Saved Sucessfully.");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void cmbEngineNos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tractorPurchase = lstTractorPurchases.Where(s => s.TRACTOR_ID == Convert.ToInt32(cmbEngineNos.SelectedValue)).Select(s => s).FirstOrDefault();

            if (tractorPurchase != null)
            {
                txtInvoiceNo.Text = tractorPurchase.INVOICE.INVOICE_NO;
                dateOfInvoice.SelectedDate = tractorPurchase.INVOICE.INVOICE_DATE;
                txtTractorModel.Text = tractorPurchase.TRACTOR_MODEL.TRACTOR_MODEL_NAME;
                txtTractorSpecification.Text = tractorPurchase.TRACTOR_SPECIFICATION;
                txtChassisNo.Text = tractorPurchase.TRACTOR_CHASSIS_NO;
                txtFIPNo.Text = tractorPurchase.TRACTOR_FIP_NO;
                txtAlternateMaker.Text = tractorPurchase.TRACTOR_ALTERNATE_MAKER;
                txtStarterMotorMake.Text = tractorPurchase.TRACTOR_SELFSTARTMAKER;
                txtPDIHours.Text = tractorPurchase.TRACTOR_PDI_HOURS.HasValue ? tractorPurchase.TRACTOR_PDI_HOURS.ToString() : string.Empty;
                TRACTOR_PART[] partsArray = null;
                if ((partsArray = tractorPurchase.TRACTOR_PARTs.ToArray()).Length > 0)
                {
                    int i = 0;
                    int j = 0;
                    gridTyreDetails.Children.OfType<TextBox>().All(s =>
                    {
                        switch (i++)
                        {
                            case 0: s.Text = partsArray[j].PART_MAKER;
                                break;
                            case 1: s.Text = partsArray[j].PART_SIZE;
                                break;
                            case 2: s.Text = partsArray[j].PART_SERIAL_NO;
                                break;
                            case 3: s.Text = partsArray[j++].PART_REMARKS;
                                i = 0;
                                break;
                        }
                        return true;
                    });
                }
                else
                {
                    gridTyreDetails.Children.OfType<TextBox>().All(s => { s.Text = string.Empty; return true; });
                }
                btnSaveTractorPDIReport.IsEnabled = true;
            }
        }

        private void dataGridTractorInvoices_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var selectedRow = dataGridTractorInvoices.CurrentItem as InvoiceViewModel;
            if (selectedRow is InvoiceViewModel)
                lstTractorPurchases = data.GetDetails<TRACTOR_PURCHASE>(s => s.INVOICE_ID, selectedRow.InvoiceNumber,selectedRow.InvoiceDate, CommonLayer.TYPE.TRACTOR);
            if (lstTractorPurchases != null && lstTractorPurchases.Count > 0)
            {
                cmbEngineNos.SelectionChanged -= cmbEngineNos_SelectionChanged;
                cmbEngineNos.ItemsSource = lstTractorPurchases.Select(s => new DDBinding { Id = s.TRACTOR_ID, Name = s.TRACTOR_ENGINE_NO });
                cmbEngineNos.SelectionChanged += cmbEngineNos_SelectionChanged;
                cmbEngineNos.SelectedIndex = 0;
            }
        }
    }
}
