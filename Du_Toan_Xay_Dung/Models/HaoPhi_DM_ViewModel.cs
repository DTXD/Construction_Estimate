using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class HaoPhi_DM_ViewModel
    {
        public HaoPhi_DM_ViewModel() { }
        public string MaHP { get; set; }
        public string MaHieuCV_User { get; set; }
        public string MaHieuCV_DM { get; set; }
        public string Ten { get; set; }
        public string DonVi { get; set; }
        public decimal SoLuong_DM { get; set; }
        public decimal Gia { get; set; }

    }
}