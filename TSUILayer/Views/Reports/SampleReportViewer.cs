using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace TSUILayer.Views.Reports
{
    public partial class SampleReportViewer : Form
    {
        public SampleReportViewer()
        {
            InitializeComponent();
        }

        private void SampleReportViewer_Load(object sender, EventArgs e)
        {
            try
            {
                string ReportServerUrl = System.Configuration.ConfigurationManager.AppSettings["ReportServerUrl"];
                string ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportFolderPath"];
                string ReportUserID = System.Configuration.ConfigurationManager.AppSettings["ReportUserID"];
                string ReportPwd = System.Configuration.ConfigurationManager.AppSettings["ReportPwd"];
                string ReportDomain = System.Configuration.ConfigurationManager.AppSettings["ReportDomain"];

                reportViewer1.ProcessingMode = ProcessingMode.Remote;
                reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials =
                    new CustomReportCredentials(ReportUserID, ReportPwd, ReportDomain).NetworkCredentials;

                reportViewer1.ServerReport.ReportServerUrl = new Uri(@ReportServerUrl);
                reportViewer1.ServerReport.ReportPath = "/"+ReportPath +"/Report3";
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                

            }
           
            
        }
    }
}
