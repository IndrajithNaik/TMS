using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;

namespace EntitiesLayer.ViewModels
{
    public class SpareViewModel : INotifyPropertyChanged
    {
        public string SparePartCode { get; set; }

        public List<SpareSuppliers> Suppliers { get; set; }

        public string SparePartDescription { get; set; }

        private decimal selectedPrice;

        public decimal SelectedPrice
        {
            get { return selectedPrice; }
            set
            {
                selectedPrice = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedPrice"));

            }
        }

        public string BinNo { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class SpareSuppliers
    {
        public decimal Id { get; set; }

        public string Name { get; set; }
    }
}
