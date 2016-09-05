using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class CongViec_User_ViewModel
    {

        public CongViec_User_ViewModel() { }

        public CongViec_User_ViewModel(UserWork obj)
        {
            
            BuildingItem_ID = obj.BuildingItem_ID;
            ID = obj.ID;
            UserWork_ID = obj.UserWork_ID;
            NormWork_ID = obj.NormWork_ID;
            Name = obj.Name;
            Unit_Measure = obj.Unit_Measure;
            Amount_Work = obj.Amount_Work;
        }

        public long BuildingItem_ID  { get; set; }
        public long ID { get; set; }
        public string UserWork_ID { get; set; }
        public string NormWork_ID { get; set; }
        public string Name { get; set; }
        public string Unit_Measure { get; set; }
        public decimal ?Amount_Work { get; set; }

    }
}