using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class Submit_CongViec_User_ViewModel
    {
        public Submit_CongViec_User_ViewModel() 
        {
            HaoPhis = new List<HaoPhis>();
        }
        public string MaCT { get; set; }
        public string MaHM { get; set; }
        public string TenHM { get; set; }
        public string MaHieuCV_DM { get; set; }
        public List<HaoPhis> HaoPhis { get; set; }
    }

    public class HaoPhis
    {
        public HaoPhis() { }
        public string MaHP { get; set; }
        public string Ten { get; set; }
        public string MaHieuCV_DM { get; set; }
        public string DonVi { get; set; }
        public decimal SoLuong_DM { get; set; }
        public decimal Gia { get; set; }
    }
}