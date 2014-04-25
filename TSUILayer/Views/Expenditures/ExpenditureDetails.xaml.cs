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

namespace TSUILayer.Views.Expenditures
{
    /// <summary>
    /// Interaction logic for ExpenditureDetails.xaml
    /// </summary>
    public partial class ExpenditureDetails : UserControl
    {
        List<EXPENDITUREDETAIL> _expenditures = null;
        DataLayer data = null;
        public ExpenditureDetails()
        {
            InitializeComponent();
            _expenditures = new List<EXPENDITUREDETAIL>();
            data = DataLayer.Instance;
            dateOfExpenditure.SelectedDate = DateTime.Today;
        }

        private void btnExpenditureHead_Click(object sender, RoutedEventArgs e)
        {
            EXPENDITUREDETAIL eD = new EXPENDITUREDETAIL();

            eD.EXPENDITURE_ID = (int)cmbExpenditureType.SelectedValue;
            
            eD.EXPENDITURE_DATE = dateOfExpenditure.SelectedDate.Value;
            eD.AMOUNT = Convert.ToDouble(txtAmount.Text);
            eD.PERTICULARS = txtperticulars.Text;
            eD.VOUCHER_NO = int.Parse(txtVoucherNumber.Text);

            _expenditures.Add(eD);


            //gridExpenditureDetails.ItemsSource = _expenditures;
            gridExpenditureDetails.ItemsSource = _expenditures;
            gridExpenditureDetails.Items.Refresh();

            ClearUIValues();

            _voucherNumber++;
            txtVoucherNumber.Text = _voucherNumber.ToString();
        }

        private void ClearUIValues()
        {
            txtAmount.Text = string.Empty;
            cmbExpenditureType.SelectedIndex = 0;
            txtperticulars.Text = string.Empty;
        }

        
        private void btnSaveExpenditures_Click(object sender, RoutedEventArgs e)
        {
            data.Insert(_expenditures);
            MessageBox.Show("Expenditure Detail Added Successfully");

            gridExpenditureDetails.ItemsSource = null;
            gridExpenditureDetails.Items.Refresh();

            _expenditures = null;
            _expenditures = new List<EXPENDITUREDETAIL>();

            txtVoucherNumber.Text = string.Empty;
            dateOfExpenditure.SelectedDate = DateTime.Now;

            SetVoucherNumber();

            dataGridExpenditures.ItemsSource = data.GetAll<EXPENDITUREDETAIL>();
            dataGridExpenditures.Items.Refresh();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbExpenditureType.ItemsSource = data.GetAll<EXPENDITUREHEAD>().Select( s => new { Id = s.EXPENDITURE_ID, Name = s.EXPENDITURE_TYPE}).Distinct();

            dataGridExpenditures.ItemsSource = data.GetAll<EXPENDITUREDETAIL>();

            SetVoucherNumber();
        }

        private int _voucherNumber = 0;

        private void SetVoucherNumber()
        {
            _voucherNumber = data.GetAll<EXPENDITUREDETAIL>().Count() +1;
            txtVoucherNumber.Text = (_voucherNumber).ToString();
        }


    }

}
