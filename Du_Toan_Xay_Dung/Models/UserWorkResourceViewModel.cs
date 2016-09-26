using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class UserWorkResourceViewModel
    {
        public UserWorkResourceViewModel() { }


        public UserWorkResourceViewModel(UserWork_Resource obj)
        {
            BuildingItem_ID = obj.BuildingItem_ID;
            UserWork_ID = obj.UserWork_ID;
            UnitPrice_ID = obj.UnitPrice_ID;
            Name = obj.Name;
            Unit = obj.Unit;
            Number_Norm = obj.Number_Norm;
            Price = obj.Price;
        }

        public long BuildingItem_ID { get; set; }
        public string UserWork_ID { get; set; }
        public string UnitPrice_ID { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal Number_Norm { get; set; }
        public decimal Price { get; set; }

    }
}