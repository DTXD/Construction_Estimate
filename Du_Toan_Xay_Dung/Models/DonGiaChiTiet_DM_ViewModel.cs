using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class DonGiaChiTiet_DM_ViewModel
    {
        public DonGiaChiTiet_DM_ViewModel() { }
        public DonGiaChiTiet_DM_ViewModel(ChiTiet_DinhMuc obj)
        {
            MaHieuCV_DM = obj.MaHieuCV_DM;
            SoLuong = obj.SoLuong;
            DonViCV = obj.DonVi;
            TenHP = obj.DonGia.Ten;
            DonViHP = obj.DonGia.DonVi;
            Gia = obj.DonGia.Gia;
        }

        public string MaHieuCV_DM { get; set; }
        public decimal SoLuong { get; set; }
        public string DonViCV { get; set; }
        public string TenHP { get; set; }
        public string DonViHP { get; set; }
        public decimal? Gia { get; set; }
    }
}