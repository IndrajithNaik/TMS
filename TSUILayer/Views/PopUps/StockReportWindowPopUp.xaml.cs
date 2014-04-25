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
using DataBaseLayer;
using EntitiesLayer.Entities;
//using TSUILayer.Views.Reports;
using System.Threading.Tasks;

namespace TSUILayer.Views.PopUps
{
    /// <summary>
    /// Interaction logic for StockReportWindowPopUp.xaml
    /// </summary>
    public partial class StockReportWindowPopUp : Window
    {
        DataLayer data = null;

        List<Spare> spares = null;
        List<Spare> sparesDifficient = null;
        public StockReportWindowPopUp()
        {
            InitializeComponent();
            data = DataLayer.Instance;
            var result = data.GetSpareScarcity();
            gridSparesAdded.ItemsSource = result;
            if (result.Count <= 0)
            {
                lblMessage.Visibility = Visibility.Visible;
            }
        }

        private void btnPrintStockScarcityreport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Currently not available!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //ReportsWindow r = new ReportsWindow();
            //r.Show();
        }

        private void btnExportToXLSheet_Click(object sender, RoutedEventArgs e)
        {
            
            ExportToExcel<Spare, List<Spare>> s = new ExportToExcel<Spare, List<Spare>>();
            
            if (null != gridSparesAdded.ItemsSource && gridSparesAdded.Items.Count > 0)
            {
                s.dataToPrint = (List<Spare>)gridSparesAdded.ItemsSource;
                s.GenerateReport();
            }
            else
            {
                MessageBox.Show("No Data Available in Grid");
            }
            
        }
    }
}
