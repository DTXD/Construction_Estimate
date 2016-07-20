using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class DonGiaViewModel
    {
        public DonGiaViewModel() { }

        public DonGiaViewModel(DonGia obj) 
        {
            MaVL_NC_MTC = obj.MaVL_NC_MTC;
            MaKV = obj.MaKV;
            Ten = obj.Ten;
            DonVi = obj.DonVi;
            Gia = obj.Gia;
        }
        public string MaVL_NC_MTC { get; set; }
        public string MaKV { get; set; }
        public string Ten { get; set; }
        public string DonVi { get; set; }
        public decimal ?Gia { get; set; }

    }
}