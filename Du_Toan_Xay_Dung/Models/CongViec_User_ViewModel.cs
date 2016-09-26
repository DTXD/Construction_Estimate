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
            ID = Convert.ToInt64(obj.ID);
            NormWork_ID = obj.NormWork_ID;
            Name = obj.Name;
            Unit = obj.Unit;
            Numbers = Convert.ToDecimal(obj.Number);
            Horizontal = Convert.ToDecimal(obj.Horizontal);
            Vertical = Convert.ToDecimal(obj.Vertical);
            Height = Convert.ToDecimal(obj.Height);
            Area = Convert.ToDecimal(obj.Area);
            SumMaterial = Convert.ToDecimal(obj.SumMaterial);
            SumLabor = Convert.ToDecimal(obj.SumLabor);
            SumMachine = Convert.ToDecimal(obj.SumMachine);

        }

        public long BuildingItem_ID  { get; set; }
        public long ID { get; set; }
        public string UserWork_ID { get; set; }
        public string NormWork_ID { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal Numbers { get; set; }
        public decimal Horizontal { get; set; }
        public decimal Vertical { get; set; }
        public decimal Height { get; set; }
        public decimal Area { get; set; }
        public decimal SumMaterial { get; set; }
        public decimal SumLabor { get; set; }
        public decimal SumMachine { get; set; }

    }
}