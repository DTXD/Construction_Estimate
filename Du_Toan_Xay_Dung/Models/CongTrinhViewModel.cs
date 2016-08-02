using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class CongTrinhViewModel
    {
        public CongTrinhViewModel() { }

        public CongTrinhViewModel(CongTrinh obj)
        {
            Id = obj.Id;
            MaCT = obj.MaCT;
            Email = obj.Email;
            TenCT = obj.TenCT;
            MoTa = obj.MoTa;
            DiaChi = obj.DiaChi;
            Gia = obj.Gia;
        }
        public long Id { get; set; }
        public string MaCT { get; set; }
        public string Email { get; set; }
        public string TenCT { get; set; }
        public string MoTa { get; set; }
        public string DiaChi { get; set; }
        public decimal Gia { get; set; }
        public List<HttpPostedFileBase> img_congtrinh { get; set; }
        public List<HttpPostedFileBase> img_old { get; set; }
    }

    /*
    public class CongTrinhCuaUser
    {
        public CongTrinhCuaUser() { }

        public CongTrinhCuaUser(User_CongTrinh obj)
        {
            MaCT = obj.MaCT;
            TenCT = obj.CongTrinh.TenCT;
            MoTa = obj.CongTrinh.MoTa;
            Gia = obj.CongTrinh.Gia;
        }
        public string MaCT { get; set; }
        public string TenCT { get; set; }
        public string MoTa { get; set; }
        public decimal Gia { get; set; }

    }

    public class CongTrinhCuaUserFull
    {
        public CongTrinhCuaUserFull() { }

        public CongTrinhCuaUserFull(User_CongTrinh obj)
        {
            MaCT = obj.MaCT;
            TenCT = obj.CongTrinh.TenCT;
            MoTa = obj.CongTrinh.MoTa;
            Gia = obj.CongTrinh.Gia;
            HangMucCongTrinh = obj.CongTrinh.HangMuc_CongTrinhs.ToList();
        }
        public string MaCT { get; set; }
        public string TenCT { get; set; }
        public string MoTa { get; set; }
        public decimal Gia { get; set; }
        public HangMuc HangMuc { get; set; }
        public List<HangMuc_CongTrinh> HangMucCongTrinh { get; set; }
     
    }
    */
}