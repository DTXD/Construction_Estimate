using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class DinhMucViewModel
    {
        public DinhMucViewModel() { }

        public DinhMucViewModel(DinhMuc obj)
        {
            MaHieuCV_DM = obj.MaHieuCV_DM;
            CongTac = obj.CongTac;
            DonVi = obj.DonVi;
            RangBuoc = obj.RangBuoc;

        }

        public string MaHieuCV_DM { get; set; }
        public string CongTac { get; set; }
        public string DonVi { get; set; }
        public string RangBuoc { get; set; }

    }
}