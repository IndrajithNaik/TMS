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
using System.Windows.Shapes;
using EntitiesLayer.Entities;

namespace TSUILayer.Views.PopUps
{
    /// <summary>
    /// Interaction logic for VerifyPurchaseInvoicePopUp.xaml
    /// </summary>
    public partial class VerifyPurchaseInvoicePopUp : Window
    {
        public VerifyPurchaseInvoicePopUp(SparePurchaseDetail _purchaseDetail, string totalAmount)
        {
            InitializeComponent();

            gridSparesAdded.ItemsSource = _purchaseDetail._listOfSpares;

            lblTotalAmount.Content = totalAmount;
        }
    }
}
