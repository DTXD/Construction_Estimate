using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class BuildingViewModel
    {
        public BuildingViewModel() { }

        public BuildingViewModel(Building obj)
        {
            ID = obj.ID;  
            Email = obj.Email;
            Name = obj.Name;
            Description = obj.Description;
            Address = obj.Address;
            Sum = obj.Sum;
        }
        public long ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public decimal Sum { get; set; }
        public int Count_BuildingItem { get; set; }
        public List<HttpPostedFileBase> img_congtrinh { get; set; }
        public List<string> img_old { get; set; }
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