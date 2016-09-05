using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class DinhMucViewModel
    {
        public DinhMucViewModel() { }

        public DinhMucViewModel(NormWork obj)
        {
            ID = obj.ID;
            Name = obj.Name;
            Unit = obj.Unit;
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }

    }
}