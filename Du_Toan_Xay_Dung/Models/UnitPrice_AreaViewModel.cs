using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class UnitPrice_AreaViewModel
    {
        public UnitPrice_AreaViewModel(){}
        public UnitPrice_AreaViewModel(UnitPrice_Area obj)
        {
            UnitPrice_ID = obj.UnitPrice_ID;
            Area_ID = obj.Area_ID;
            Price = obj.Price;
        }
        public string UnitPrice_ID { get; set; }
        public long Area_ID { get; set; }
        public decimal Price { get; set; }

    }
}