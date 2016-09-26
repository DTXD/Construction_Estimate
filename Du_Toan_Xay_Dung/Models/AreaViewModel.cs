using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{  
    public class AreaViewModel
    {
        public AreaViewModel() { }
        public AreaViewModel(Area obj)
        {
            ID = obj.ID;
            Name = obj.Name;
            Address = obj.Address;
        }
        public long ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}