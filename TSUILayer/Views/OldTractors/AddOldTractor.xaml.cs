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

namespace TSUILayer.Views.OldTractors
{
    /// <summary>
    /// Interaction logic for AddOldTractor.xaml
    /// </summary>
    public partial class AddOldTractor : UserControl
    {
        DataLayer data = null;

        public AddOldTractor()
        {
            InitializeComponent();
            data = DataLayer.Instance;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            
        }

        private void btnUpdateOldTractorDetails_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnSaveOldTractorDetail_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCustomerName.Text != string.Empty && cmbTractorMake.Text != string.Empty && cmbTractorModel.Text != string.Empty && txtPurchaseCost.Text != string.Empty && txtSellingCost.Text != string.Empty && txtYear.Text != string.Empty)
            {
                OLD_TRACTOR oldTractorObj = new OLD_TRACTOR();
                
                oldTractorObj.CUSTOMER_NAME = cmbCustomerName.Text;
                oldTractorObj.TRACTOR_MAKE = cmbTractorMake.Text;
                oldTractorObj.TRACTOR_MODEL = cmbTractorModel.Text;
                if (chkRcBookRecieved.IsChecked == true)
                {
                    oldTractorObj.TRACTOR_RCBOOK_NO = txtRcBookNo.Text;
                }
                oldTractorObj.TRACTOR_REGISTRATION_NO = txtRegisterNo.Text;
                oldTractorObj.TRACTOR_YEAR = int.Parse(txtYear.Text);

                oldTractorObj.PURCHASE_COST = decimal.Parse(txtPurchaseCost.Text);
                oldTractorObj.SELLING_COST = decimal.Parse(txtSellingCost.Text);

                if (chkExchanged.IsChecked == true)
                {
                    cmbTractorModel.ItemsSource = data.GetAllTractorModels(Convert.ToInt32(cmbTractorMake.SelectedValue)); // We need to hard code this to SONALICA.
                    oldTractorObj.TRACTOR_EXCHANGE_MODELID = (int)cmbExchangeTractorMakeForExchange.SelectedValue;
                }
                data.Insert<OLD_TRACTOR>(oldTractorObj);
                MessageBox.Show("Old Tractor Details Added Sucessfully");

                Common.ClearAllControls<TextBox>(oldTractor, 1);
                Common.ClearAllControls<ComboBox>(oldTractor);

                BindOldTractors();

                
            }
            else
            {
                MessageBox.Show("Please enter the mandatory fields");
            }
        }

        private void BindOldTractors()
        {
            gridOldTractors.ItemsSource = data.GetAll<OLD_TRACTOR>();
            gridOldTractors.Items.Refresh();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BindOldTractors();
            cmbTractorMake.ItemsSource = data.GetAllSuppliers(CommonLayer.TYPE.TRACTOR);            
        }

        private void cmbTractorMake_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbTractorModel.ItemsSource = data.GetAllTractorModels(Convert.ToInt32(cmbTractorMake.SelectedValue));
        }

     
    }
}
