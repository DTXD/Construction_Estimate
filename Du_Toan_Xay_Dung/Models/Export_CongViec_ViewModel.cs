using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class Export_CongViec_ViewModel
    {
        public Export_CongViec_ViewModel() { }
        public string TenHM { get; set; }
        public string MaHieuCV { get; set; }
        public string TenCV { get; set; }
        public string DonVi { get; set; }
        public decimal? KhoiLuong { get; set; }
        public decimal? GiaVL { get; set; }
        public decimal? GiaNC { get; set; }
        public decimal? GiaMTC { get; set; }
        public decimal? ThanhTien { get; set; }

    }
}