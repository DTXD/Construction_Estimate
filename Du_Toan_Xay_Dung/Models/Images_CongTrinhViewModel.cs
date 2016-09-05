using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Du_Toan_Xay_Dung.Models
{
    public class Images_CongTrinhViewModel
    {
        public Images_CongTrinhViewModel() { }

        public Images_CongTrinhViewModel(Images_Url obj)
        {
            ID = obj.ID;
            Building_ID = obj.Building_ID;
            Url = obj.Url;
        }

        public long ID { get; set; }
        public long Building_ID { get; set; }
        public string Url { get; set; }
    }
}