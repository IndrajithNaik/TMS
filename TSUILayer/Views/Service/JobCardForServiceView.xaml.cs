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

namespace TSUILayer.Views.Sales
{
    /// <summary>
    /// Interaction logic for JobCardAView.xaml
    /// </summary>
    public partial class JobCardAView : UserControl
    {
        DataLayer data = null;
        //public static JobCardAView _jobAInstance = null;
        JOB_CARD jobCard = new JOB_CARD();

        public JobCardAView()
        {
            InitializeComponent();
            data = DataLayer.Instance;
            //_jobAInstance = this;
        }

        private void btnAddComplaint_Click(object sender, RoutedEventArgs e)
        {
            JOB_COMPLAINT complaint = new JOB_COMPLAINT();
            complaint.ACTION_TAKEN = txtActionOnComplaint.Text;
            complaint.ACTUAL_COMPLAINT = txtComplaint.Text;
            complaint.LABOUR_CHARGES = Convert.ToDecimal(txtLabourEstimation.Text);
            jobCard.JOB_COMPLAINTs.Add(complaint);

            gridComplaint.ItemsSource = jobCard.JOB_COMPLAINTs.Select((s, i) => new { SlNo = ++i, Complaint = s.ACTUAL_COMPLAINT, Action = s.ACTION_TAKEN, Charge = s.LABOUR_CHARGES }).ToList();

            txtActionOnComplaint.Clear();
            txtComplaint.Clear();
            txtLabourEstimation.Clear();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            MainWindow._mainInstance.frmContent.NavigationService.Navigate(new Uri("Views/Service/JobCardForSparesView.xaml", UriKind.Relative));
            //string tractorApp = string.Empty;
            //TractorAppGrid.Children.OfType<CheckBox>().Where(s => s.IsChecked ?? false).All(s => { tractorApp += s.Content.ToString() + ","; return true; });

            //jobCard.SERIAL_NUMBER = Convert.ToInt32(txtJobCardSerialNo.Text);
            //jobCard.ESTIMATED_COST = Convert.ToDecimal(txtEstimatedCost.Text);
            //jobCard.SERVICE_COUPON_NO = Convert.ToInt32(txtServiceCouponNo.Text);
            //jobCard.TRACTOR_APPLICATION = tractorApp.Remove(tractorApp.Length - 1);
            //jobCard.IS_ACCEDENTAL = chkAccidental.IsChecked;
            //jobCard.IS_MOBILESERVICE = chkMobileService.IsChecked;
            //jobCard.IS_PAID = chkPaid.IsChecked;
            //jobCard.IS_SERVICECLINIC = chkServiceClinic.IsChecked;
            //jobCard.ESTIMATED_DELIVERY = dateEsimatedDelivery.SelectedDate;
            //jobCard.DATETIME_IN = dateTimeIn.SelectedDate;
            //jobCard.SERVICE_STARTDATETIME = dateTimeServiceStart.SelectedDate;
            //jobCard.SERVICE_ENDDATETIME = dateTimeServiceFinish.SelectedDate;
            jobCard.REPEAT_FIR_DETAIL = txtRepeat_PreviousFIRDetail.Text;

            new JobCardBView(jobCard);
        }


    }
}
