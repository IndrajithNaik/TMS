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
using CommonLayer;
namespace TSUILayer.Views.Sales
{
    /// <summary>
    /// Interaction logic for OwnerShipAndTractorDetailsView.xaml
    /// </summary>
    public partial class OwnerShipAndTractorDetailsView : UserControl
    {
        DataLayer data = null;
        //SALES_INVOICE salesDetails = null;
        public OwnerShipAndTractorDetailsView()
        {
            InitializeComponent();
            data = DataLayer.Instance;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
        
        private void cmbInvoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //salesDetails = data.GetById<SALES_INVOICE>(s => s.SALES_INVOICE_ID.Equals(cmbInvoices.SelectedValue));
            //txtCustomerName.Text = salesDetails.CUSTOMER.CUSTOMER_NAME;
            //txtTractorModel.Text = salesDetails.TRACTOR_PURCHASE.TRACTOR_MODEL.TRACTOR_MODEL_NAME;
            //txtFIPNo.Text = salesDetails.TRACTOR_PURCHASE.TRACTOR_FIP_NO;
            //txtAlternateMaker.Text = salesDetails.TRACTOR_PURCHASE.TRACTOR_ALTERNATE_MAKER;
            //txtStarterMotorMake.Text = salesDetails.TRACTOR_PURCHASE.TRACTOR_SELFSTARTMAKER;
            //txtPDIHours.Text = salesDetails.TRACTOR_PURCHASE.TRACTOR_PDI_HOURS.ToString();

            //TRACTOR_PART[] partsArray = null;
            //if ((partsArray = salesDetails.TRACTOR_PURCHASE.TRACTOR_PARTs.ToArray()).Length > 0)
            //{
            //    int i = 0;
            //    int j = 0;
            //    gridTyreDetails.Children.OfType<TextBox>().All(s =>
            //    {
            //        switch (i++)
            //        {
            //            case 0: s.Text = partsArray[j].PART_MAKER;
            //                break;
            //            case 1: s.Text = partsArray[j].PART_SIZE;
            //                break;
            //            case 2: s.Text = partsArray[j].PART_SERIAL_NO;
            //                break;
            //            case 3: s.Text = partsArray[j++].PART_REMARKS;
            //                i = 0;
            //                break;
            //        }
            //        return true;
            //    });
            //}
            //else
            //{
            //    gridTyreDetails.Children.OfType<TextBox>().All(s => { s.Text = string.Empty; return true; });
            //}
        }

        private void btnSaveTractor_Click(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtServiceCouponNo.Text))
            //{
            //    data.GetById<TRACTOR_PURCHASE>(s => s.TRACTOR_ID.Equals(salesDetails.TRACTOR_ID)).TRACTOR_SERVICE_BOOK_NO = txtServiceCouponNo.Text;
            //    data.Update<TRACTOR_PURCHASE>();
            //    MessageBox.Show("Data Saved Successfully.");
            //}

        }

    }
}
