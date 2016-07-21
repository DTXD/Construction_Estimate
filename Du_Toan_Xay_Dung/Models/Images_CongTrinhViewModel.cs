using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Du_Toan_Xay_Dung.Models
{
    public class Images_CongTrinhViewModel
    {
        public Images_CongTrinhViewModel() { }

        public Images_CongTrinhViewModel(Images_CongTrinh obj)
        {
            MaCT = obj.MaCT;
            Url = obj.Url;
        }

        public string MaCT { get; set; }
        public string Url { get; set; }
    }
}