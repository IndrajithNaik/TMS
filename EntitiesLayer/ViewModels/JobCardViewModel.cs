using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EntitiesLayer.ViewModels
{
    public class JobCardViewModel : INotifyPropertyChanged
    {
        public int PartId { get; set; }

        public string PartCode { get; set; }

        public string PartDescription { get; set; }

        private int _quantity;

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Quantity"));
            }
        }

        public decimal UnitPrice { get; set; }

        public decimal Value { get; set; }

        private decimal _vatPrecent;

        public decimal VatPrecent
        {
            get { return _vatPrecent; }
            set
            {
                _vatPrecent = value; 
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("VatPrecent"));
            }
        }

        public decimal TotalAmount { get; set; }

        private string _wcNo;

        public string WCNo
        {
            get { return _wcNo; }
            set { _wcNo = value;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("WCNo"));
            }
        }

        private bool _isWarrantty;

        public bool IsWarrantty
        {
            get { return _isWarrantty; }
            set { _isWarrantty = value;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("IsWarrantty"));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
