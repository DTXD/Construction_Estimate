using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class ChiTiet_DinhMucViewModel
    {
        public ChiTiet_DinhMucViewModel() { }

        public ChiTiet_DinhMucViewModel(ChiTiet_DinhMuc obj) 
        {
            MaHieuCV_DM = obj.MaHieuCV_DM;
            MaVL_NC_MTC = obj.MaVL_NC_MTC;
            SoLuong = obj.SoLuong;
            DonVi = obj.DonVi;
        }

        public string MaHieuCV_DM { get; set; }
        public string MaVL_NC_MTC { get; set; }
        public decimal SoLuong { get; set; }
        public string DonVi { get; set; }
    }
}