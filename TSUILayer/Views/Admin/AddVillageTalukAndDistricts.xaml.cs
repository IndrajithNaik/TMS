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

namespace TSUILayer.Views.Admin
{
    /// <summary>
    /// Interaction logic for AddVillageTalukAndDistricts.xaml
    /// </summary>
    /// SVN TEST

    public partial class AddVillageTalukAndDistricts : UserControl
    {
        DataLayer data = null;

        int selectedDistrict = -1;
        int selectedTaluk = -1;

        public AddVillageTalukAndDistricts()
        {
            InitializeComponent();
            data = DataLayer.Instance;
            BindGrid();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDistrictName.SelectedValue == null)
            {
                DISTRICT district = new DISTRICT();
                district.DISTRICT_NAME = cmbDistrictName.Text;
                data.Insert<DISTRICT>(district);
            }
            if (cmbTalukName.SelectedValue == null)
            {
                TALUK taluk = new TALUK();
                taluk.TALUK_NAME = cmbTalukName.Text;
                taluk.DISTRICT_ID = selectedDistrict = cmbDistrictName.SelectedValue == null ? data.GetAll<DISTRICT>().Max(s => s.DISTRICT_ID) : Convert.ToInt32(cmbDistrictName.SelectedValue);
                data.Insert<TALUK>(taluk);
            }
            if (null != txtVillageName.Text && txtVillageName.Text != string.Empty)
            {
                VILLAGE village = new VILLAGE();
                village.VILLAGE_NAME = txtVillageName.Text;
                village.TALUK_ID = selectedTaluk = cmbTalukName.SelectedValue == null ? data.GetAll<TALUK>().Max(s => s.TALUK_ID) : Convert.ToInt32(cmbTalukName.SelectedValue);
                data.Insert(village);
                MessageBox.Show("New Village Added Succesfully.");
            }

            txtVillageName.Text = string.Empty;

            BindGrid();
            cmbDistrictName.ItemsSource = data.GetAll<DISTRICT>().Select(s => new { Id = s.DISTRICT_ID, Name = s.DISTRICT_NAME });
            cmbDistrictName.SelectedValue = cmbDistrictName.SelectedValue == null ? selectedDistrict : Convert.ToInt32(cmbDistrictName.SelectedValue); ;
            cmbTalukName.ItemsSource = data.GetAll<TALUK>(s => s.DISTRICT_ID == Convert.ToInt32(cmbDistrictName.SelectedValue)).Select(s => new { Id = s.TALUK_ID, Name = s.TALUK_NAME });
            cmbTalukName.SelectedValue = cmbTalukName.SelectedValue == null ? selectedTaluk : Convert.ToInt32(cmbTalukName.SelectedValue);
        }

        private void BindGrid()
        {
            dataGridVillages.ItemsSource = data.GetAll<VILLAGE>().Select((s, i) =>
                new
                {
                    VillageId = ++i,
                    DistrictName = s.TALUK.DISTRICT.DISTRICT_NAME,
                    TalukName = s.TALUK.TALUK_NAME,
                    VillageName = s.VILLAGE_NAME
                });

            dataGridVillages.Items.Refresh();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {            
            cmbDistrictName.ItemsSource = data.GetAll<DISTRICT>().Select(s => new { Id = s.DISTRICT_ID, Name = s.DISTRICT_NAME });
        }

        private void cmbDistrictName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != cmbDistrictName.SelectedValue && cmbDistrictName.SelectedValue.ToString() != string.Empty)
            {
                cmbTalukName.ItemsSource = data.GetAll<TALUK>(s => s.DISTRICT_ID == Convert.ToInt32(cmbDistrictName.SelectedValue)).Select(s => new { Id = s.TALUK_ID, Name = s.TALUK_NAME });
            }
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            cmbDistrictName.Text = string.Empty;
            cmbTalukName.Text = string.Empty;
            txtVillageName.Text = string.Empty;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
    }
}
