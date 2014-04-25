using DataBaseLayer;
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

namespace TSUILayer.Views.Sales
{
    /// <summary>
    /// Interaction logic for BalanceDetails.xaml
    /// </summary>
    public partial class BalanceDetails : UserControl
    {
        DataLayer data = null;
        public BalanceDetails()
        {
            InitializeComponent();
            data = DataLayer.Instance;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //cmbInvoices.ItemsSource = data.GetAll<SALES_INVOICE>().Where(s=>s.INVOICE_GRANDTOTAL>) .Select(s => s.SALES_INVOICE_ID);

            var dd = data.GetAll<SALES_INVOICE>().Join(data.GetAll<BALANCE>(), i => i.SALES_INVOICE_ID, b => b.SALES_INVOICE_ID, (a, b) => a);

            cmbInvoices.ItemsSource = from i in data.GetAll<SALES_INVOICE>()
                                      join b in data.GetAll<BALANCE>() on i.SALES_INVOICE_ID equals b.SALES_INVOICE_ID into ib
                                      where i.INVOICE_GRANDTOTAL > ib.Sum(s => s.AMOUNT_PAID)
                                      select i.SALES_INVOICE_ID;

        }

        private void cmbInvoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblBalance.Content = (from s in data.GetAll<SALES_INVOICE>()
                                  join b in data.GetAll<BALANCE>() on s equals b.SALES_INVOICE into sb
                                  from b in sb.DefaultIfEmpty()
                                  where s.SALES_INVOICE_ID == Convert.ToInt32(cmbInvoices.SelectedValue)
                                  select (s.INVOICE_GRANDTOTAL - sb.Sum(d => d.AMOUNT_PAID))).FirstOrDefault();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            BALANCE balance = new BALANCE()
            {
                AMOUNT_PAID = Convert.ToDecimal(txtAmountPaid.Text),
                PAID_DATE = datePaid.SelectedDate ?? DateTime.Now,
                SALES_INVOICE_ID = Convert.ToInt32(cmbInvoices.SelectedValue)
            };

            data.Insert<BALANCE>(balance);
            txtAmountPaid.Text = string.Empty;
            cmbInvoices_SelectionChanged(null, null);

        }
    }
}
