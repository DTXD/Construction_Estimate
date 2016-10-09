using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class DonGiaChiTiet_DM_ViewModel
    {
        public DonGiaChiTiet_DM_ViewModel() { }
        public DonGiaChiTiet_DM_ViewModel(NormDetail obj)
        {
            ID = obj.ID;
            NormWork_ID = obj.NormWork_ID;
            UnitPrice_ID = obj.UnitPrice_ID;
            Numbers = obj.Numbers;
        }

        public long ID{ get; set; }
        public string NormWork_ID { get; set; }
        public string UnitPrice_ID { get; set; }
        public decimal ?Numbers { get; set; }
    }
}