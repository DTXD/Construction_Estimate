using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class CongViec_User_ViewModel
    {

        public CongViec_User_ViewModel() { }

        public CongViec_User_ViewModel(CongViec obj)
        {
            Id = obj.Id;
            MaHieuCV_User = obj.MaHieuCV_User;
            MaHM = obj.MaHM;
            MaHieuCV_DM = obj.MaHieuCV_DM;
            TenCongViec = obj.TenCongViec;
            DonVi = obj.DonVi;
            KhoiLuong = obj.KhoiLuong;
            GiaVL = obj.GiaVL;
            GiaNC = obj.GiaNC;
            GiaMTC = obj.GiaMTC;
            ThanhTien = obj.ThanhTien;
        }

        public long Id { get; set; }
        public string MaHieuCV_User { get; set; }
        public string MaHM { get; set; }
        public string MaHieuCV_DM { get; set; }
        public string TenCongViec { get; set; }
        public string DonVi { get; set; }
        public decimal ?KhoiLuong { get; set; }
        public decimal ?GiaVL { get; set; }
        public decimal ?GiaNC { get; set; }
        public decimal ?GiaMTC { get; set; }
        public decimal ?ThanhTien { get; set; }

    }
}