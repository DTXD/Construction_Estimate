using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class UserWorkViewModel
    {
        public UserWorkViewModel(UserWork obj) 
        {

        }
        public string BuildingItem_ID { get; set; }
        public int ID { get; set; }
        public string NormWork_ID { get; set; }
        public string Name { get; set; }
        public decimal Unit { get; set; }
        public decimal Number { get; set; }
        public decimal Horizontal { get; set; }
        public decimal Vertical { get; set; }
        public decimal Height { get; set; }
        public decimal Area { get; set; }
        public decimal SumMaterial { get; set; }
        public decimal SumLabor { get; set; }
        public decimal SumMachine { get; set; }

    }
}