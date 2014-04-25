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
//using TSUILayer.Views.Sales;
using DataBaseLayer;
using CommonLayer;

namespace TSUILayer.Views.Admin
{
    /// <summary>
    /// Interaction logic for AddCustomerView.xaml
    /// </summary>
    public partial class AddCustomerView : UserControl
    {
        DataLayer data = null;

        public AddCustomerView()
        {
            InitializeComponent();
            data = DataLayer.Instance;
            this.btnUpdate.IsEnabled = false;
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {

            if (txtCustomerFullName.Text != string.Empty && txtCustomerAddress.Text != string.Empty && cmbCustomerTaluk.Text != string.Empty && cmbCustomerVillage.Text != string.Empty)
            {
                CUSTOMER customer = new CUSTOMER();
                customer.CUSTOMER_NAME = txtCustomerFullName.Text;
                customer.CONTACT_NO_1 = Convert.ToInt64(txtCustomerContactNo1.Text);
                customer.CUSTOMER_ADDRESS = txtCustomerAddress.Text;
                customer.VILLAGE_ID = Convert.ToInt32(cmbCustomerVillage.SelectedValue);
                customer.STATUS_ID = Convert.ToInt32(cmbCustomerStatus.SelectedValue);
                data.Insert<CUSTOMER>(customer);
                MessageBox.Show("Customer Added Sucessfully");

                Common.ClearAllControls<TextBox>(addCustomer, 1);
                Common.ClearAllControls<ComboBox>(addCustomer);

                BindCustomers();
            }
            else
            {
                MessageBox.Show("Please enter the mandatory fields");
            }

        }

        private void BindCustomers()
        {
            gridCustomer.ItemsSource = data.GetAll<CUSTOMER>().Select((s, i) => new
            {
                SlNO = ++i,
                CustomerName = s.CUSTOMER_NAME,
                CustomerContactNo = s.CONTACT_NO_1,
                CustomerAdress = s.CUSTOMER_ADDRESS
            });
            gridCustomer.Items.Refresh();
        }

        private void btnSearchCustomerByName_Click(object sender, RoutedEventArgs e)
        {
            gridCustomer.ItemsSource = data.GetAll<CUSTOMER>().Where(s => s.CUSTOMER_NAME.ToLower().Contains(txtSearchCustomer.Text.ToLower())).Select((s, i) => new
            {
                SlNO = ++i,
                CustomerName = s.CUSTOMER_NAME,
                CustomerContactNo = s.CONTACT_NO_1,
                CustomerAdress = s.CUSTOMER_ADDRESS
            });
            gridCustomer.Items.Refresh();
        }

        private void gridCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbCustomerDistrict_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != cmbCustomerDistrict.SelectedValue && cmbCustomerDistrict.SelectedValue.ToString() != string.Empty)
            {
                cmbCustomerTaluk.ItemsSource = data.GetAll<TALUK>(s => s.DISTRICT_ID == Convert.ToInt32(cmbCustomerDistrict.SelectedValue)).Select(s => new { Id = s.TALUK_ID, Name = s.TALUK_NAME });
            }
        }

        private void cmbCustomerTaluk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != cmbCustomerTaluk.SelectedValue && cmbCustomerTaluk.SelectedValue.ToString() != string.Empty)
            {
                cmbCustomerVillage.ItemsSource = data.GetAll<VILLAGE>(s => s.TALUK_ID == Convert.ToInt32(cmbCustomerTaluk.SelectedValue)).Select(s => new { Id = s.VILLAGE_ID, Name = s.VILLAGE_NAME });
            }
        }

        private void cmbCustomerVillage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BindCustomers();
            cmbCustomerDistrict.ItemsSource = data.GetAll<DISTRICT>().Select(s => new { Id = s.DISTRICT_ID, Name = s.DISTRICT_NAME });
            cmbCustomerStatus.ItemsSource = data.GetMasterValues(MASTERENUM.STATUS);
        }

        Customer _customerToBeUpdated = null;
        private void gridCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.btnUpdate.IsEnabled = true;
            this.btnAddCustomer.IsEnabled = false;


            int cusId = 1;
            cusId = ((EntitiesLayer.Entities.Customer)(((System.Windows.Controls.DataGrid)(sender)).CurrentItem)).CustomerId;
            //Customer C = data.GetCustomerWithId(cusId);

            //_customerToBeUpdated = new Customer();
            //_customerToBeUpdated = C;

            //BindCustomerToRespectiveFields(C);
        }


        private void BindCustomerToRespectiveFields(Customer C)
        {
            //txtCustomerAddress.Text = _customerToBeUpdated.CustomerAdress;
            //txtCustomerContactNo1.Text = _customerToBeUpdated.CustomerContactNos.Split(',')[0];
            //txtCustomerContactNo2.Text = _customerToBeUpdated.CustomerContactNos.Split(',')[1];
            //cmbCustomerDistrict.Text = _customerToBeUpdated.CustomerDistrict;
            //txtCustomerFullName.Text = _customerToBeUpdated.CustomerName;
            //cmbCustomerTaluk.Text = _customerToBeUpdated.CustomerTaluk;
            //cmbCustomerVillage.Text = _customerToBeUpdated.CustomerVillage;

            //if (_customerToBeUpdated.CustomerStatus)
            //{
            //    cmbCustomerStatus.SelectedIndex = 0;
            //}
            //else
            //{
            //    cmbCustomerStatus.SelectedIndex = 1;
            //}

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //GetCustomerEntityFromUI(_customerToBeUpdated);

            //data.UpdateCustomerDetails(_customerToBeUpdated);

            MessageBox.Show("Customer details updated succesfully");

            Common.ClearAllControls<TextBox>(addCustomer, 1);


            BindCustomers();

            btnAddCustomer.IsEnabled = true;
            btnUpdate.IsEnabled = false;
        }


    }
}
