using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace CommonLayer
{
    public enum MASTERENUM
    {
        STATUS,
        TYPE,
        SPARETYPE,
        PARTTYPE
    }

    public enum STATUS
    {
        ACTIVE,
        BLOCK
    };

    public enum TYPE
    {
        TRACTOR,
        SPARE,
        BOTH
    }

    public enum SPARETYPE
    {
        LUBRICANT,
        ITEMS
    }

    public enum TRANSACTIONTYPE
    {
        PURCHASE,
        SALE
    }

    public enum PARTTYPE
    {
        BATTERY,
        TYRE
    }
    

    public class Common
    {
        public static void ClearAllControls<T>(Grid userGrid, int skipCount = 0) where T : class
        {
            if (userGrid != null)
            {
                userGrid.Children.OfType<T>().Skip(skipCount).ToList().ForEach(s =>
                   {
                       if (s is TextBox)
                           (s as TextBox).Text = string.Empty;

                       if (s is ComboBox)
                           (s as ComboBox).SelectedIndex = -1;

                       if (s is DatePicker)
                           (s as DatePicker).SelectedDate = null;

                   });
            }
        }
    }

    public class DDBinding
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SpareScarcityModel
    {
        public int SparePartId { get; set; }

        public string SaprePartCode { get; set; }

        public string PartDescription { get; set; }

        public int Quantity { get; set; }

    }
}
