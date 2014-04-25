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

namespace TSUILayer.Views.Reports
{
    /// <summary>
    /// Interaction logic for StockReportView.xaml
    /// </summary>
    public partial class StockReportView : UserControl
    {
        public StockReportView()
        {
            InitializeComponent();
        }

        private void btnPrintStockScarcityreport_Click(object sender, RoutedEventArgs e)
        {
            List<Spare> _spares = null;
            DataLayer dc = new DataLayer();

            //if (null != dc.GetSparesStocks())
            //{
            //    _spares = dc.GetSparesStocks();
            //    gridSparesAdded.ItemsSource = _spares;
            //    _spares = null;
            //}
        }

        private void btnExportToXLSheet_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
