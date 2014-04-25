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
    /// Interaction logic for ExpenditureHead.xaml
    /// </summary>
    public partial class ExpenditureHeadView : UserControl
    {
        DataLayer data = null;

        
        List<EXPENDITUREHEAD> _expenditureHeads = null;

        public ExpenditureHeadView()
        {
            InitializeComponent();
            data = DataLayer.Instance;
            _expenditureHeads = new List<EXPENDITUREHEAD>();
        }

        private void btnExpenditureHead_Click(object sender, RoutedEventArgs e)
        {
            EXPENDITUREHEAD eH = new EXPENDITUREHEAD();
            
            eH.EXPENDITURE_TYPE = txtExpenditureType.Text;

            _expenditureHeads.Add(eH);

            gridExpenditureHeads.ItemsSource = _expenditureHeads;
            gridExpenditureHeads.Items.Refresh();

            txtExpenditureType.Text = string.Empty;
        }

        private void btnSaveAllExpenditures_Click(object sender, RoutedEventArgs e)
        {           
            data.Insert(_expenditureHeads);
            MessageBox.Show("Added Expenditures Succesfully");
           

            gridExpenditureHeads.ItemsSource = null;
            gridExpenditureHeads.Items.Refresh();

            _expenditureHeads = null;
            _expenditureHeads = new List<EXPENDITUREHEAD>();

            dataGridExpenditureHeads.ItemsSource = data.GetAll<EXPENDITUREHEAD>();
            dataGridExpenditureHeads.Items.Refresh();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridExpenditureHeads.ItemsSource = data.GetAll<EXPENDITUREHEAD>();
            dataGridExpenditureHeads.Items.Refresh();
        } 
    }
}
