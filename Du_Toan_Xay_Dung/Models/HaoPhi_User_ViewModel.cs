using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class HaoPhi_User_ViewModel
    {
        public HaoPhi_User_ViewModel() { }

        public HaoPhi_User_ViewModel(ThanhPhanHaoPhi obj)
        {
            Id = obj.Id;
            MaHP = obj.MaHP;
            MaHieuCV_User = obj.MaHieuCV_User;
            Ten = obj.Ten;
            DonVi = obj.DonVi;
            SoLuong_DM = obj.SoLuong_DM;
            Gia = obj.Gia;
        }
        public long Id { get; set; }
        public string MaHP { get; set; }
        public string MaHieuCV_User { get; set; }
        public string Ten { get; set; }
        public string DonVi { get; set; }
        public decimal SoLuong_DM { get; set; }
        public decimal Gia { get; set; }
    }

    public class Update_HaoPhi_User
    {
        public Update_HaoPhi_User() { }
        public string MaHP { get; set; }
        public string Gia { get; set; }
    }
}