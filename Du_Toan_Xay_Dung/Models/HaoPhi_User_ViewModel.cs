using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class HaoPhi_User_ViewModel
    {
        public HaoPhi_User_ViewModel() { }

        public HaoPhi_User_ViewModel(ConstructionMaterial obj)
        {
            ID = obj.Id;
            Key_ConstructionMaterial = obj.Key_ConstructionMaterials;
            Key_UserWork = obj.Key_UserWork;
            Name = obj.Name;
            Unit_Measure = obj.Unit_Measure;
            Numbers = obj.Numbers;
            Price = obj.Price;
        }
        public long ID { get; set; }
        public string Key_ConstructionMaterial { get; set; }
        public string Key_UserWork { get; set; }
        public string Name { get; set; }
        public string Unit_Measure { get; set; }
        public decimal ?Price { get; set; }
        public decimal ?Numbers { get; set; }

    }

    public class Update_HaoPhi_User
    {
        public Update_HaoPhi_User() { }
        public string MaHP { get; set; }
        public string Gia { get; set; }
    }
}