using CommonLayer;
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
using EntitiesLayer.ViewModels;

namespace TSUILayer.Views.Sales
{
    /// <summary>
    /// Interaction logic for JobCardBView.xaml
    /// </summary>
    public partial class JobCardBView : UserControl
    {
        DataLayer data = null;
        IEnumerable<DDBinding> _autoSource = null;
        List<JobCardViewModel> lstSpareService = new List<JobCardViewModel>();
        SPARE_RATE selectedPart = null;
        static JOB_CARD _jobCard = null;
        public JobCardBView()
        {
            InitializeComponent();
            data = DataLayer.Instance;
            lstSugesstions.DisplayMemberPath = "Name";
            lstSugesstions.SelectedValuePath = "Id";
            _autoSource = data.GetAll<SPARE_PART>(s => s.MASTER.MASTER_VALUE.Equals(CommonLayer.STATUS.ACTIVE))
                .Join(data.GetAll<SPARE_RATE>(), sp => sp.SPARE_PART_ID, sr => sr.SPARE_PART_ID, (a, b) => a)
                .Select(d => new CommonLayer.DDBinding() { Id = d.SPARE_PART_ID, Name = d.SPARE_PART_CODE });
            txtOtherRepairs.Text = _jobCard.REPEAT_FIR_DETAIL;
        }
        public JobCardBView(JOB_CARD objJobCard)
        {
            _jobCard = objJobCard;            
        }

        private void txtSparePartCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_autoSource != null)
            {
                var result = _autoSource.Where(s => s.Name.ToLower().Contains(txtSparePartCode.Text.ToLower()) && !string.IsNullOrEmpty(txtSparePartCode.Text)).Select(s => s);
                lstSugesstions.Visibility = result.Count() <= 0 ? Visibility.Collapsed : Visibility.Visible;
                lstSugesstions.ItemsSource = result;
            }
        }

        private void lstSugesstions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtSparePartCode.TextChanged -= new TextChangedEventHandler(txtSparePartCode_TextChanged);
            if (lstSugesstions.SelectedValue != null)
            {
                txtSparePartCode.Text = (lstSugesstions.SelectedItem as DDBinding).Name;
                selectedPart = data.GetAll<SPARE_RATE>(s => s.SPARE_PART_ID == Convert.ToInt32(lstSugesstions.SelectedValue)).Select(s => s).FirstOrDefault();               
            }
            lstSugesstions.Visibility = Visibility.Collapsed;
            txtSparePartCode.TextChanged += new TextChangedEventHandler(txtSparePartCode_TextChanged);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPart != null)
            {
                txtSparePartCode.Clear();
                JobCardViewModel spareModel = new JobCardViewModel();
                spareModel.PartCode = selectedPart.SPARE_PART.SPARE_PART_CODE;
                spareModel.PartDescription = selectedPart.SPARE_PART.SPARE_PART_DESCRIPTION;
                spareModel.PartId = selectedPart.SPARE_PART_ID;
                spareModel.UnitPrice = selectedPart.SUPPLIER.SPARE_RATEs.Max(s => s.SPARE_RATE_VALUE);                
                lstSpareService.Add(spareModel);
                gridSpareService.ItemsSource = lstSpareService;
                gridSpareService.Items.Refresh();
            }
        }

        private void txtValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            var selectedRow = gridSpareService.CurrentItem as JobCardViewModel;
            if (selectedRow is JobCardViewModel)
            {                
                selectedRow.Value = selectedRow.UnitPrice * selectedRow.Quantity;
                var vatAmount = selectedRow.Value * (selectedRow.VatPrecent / 100);
                selectedRow.TotalAmount = decimal.Round((selectedRow.Value + vatAmount), 2);
            }

            lblTotal.Content = decimal.Round(lstSpareService.Sum(s => s.TotalAmount), 2);
        }

        private void chkWarranty_Click(object sender, RoutedEventArgs e)
        {
            lblWarrantyAmount.Content = decimal.Round(lstSpareService.Where(s => s.IsWarrantty).Sum(s => s.TotalAmount), 2);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow._mainInstance.frmContent.NavigationService.Navigate(new Uri("Views/Service/JobCardForServiceView.xaml", UriKind.Relative));
        }

    }
}
