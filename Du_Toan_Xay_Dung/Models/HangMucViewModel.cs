using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class HangMucViewModel
    {
        public HangMucViewModel() { }

        public HangMucViewModel(BuildingItem obj)
        {
            ID = obj.ID;
            Building_ID = obj.Building_ID;
            Name = obj.Name;
            Description = obj.Description;
            Sum = obj.Sum;
        }
        public long ID { get; set; }
        public long Building_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal? Sum { get; set; }
    }
}