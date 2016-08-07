using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class All_PriceMaterialViewModel
    {
        public All_PriceMaterialViewModel(string id, decimal giavl, decimal gianc, decimal giamtc) 
        {
            this.id = id;
            this.giavl = giavl;
            this.gianc = gianc;
            this.giamtc = giamtc;
        }

        public string id { get; set; }
        public decimal giavl { get; set; }
        public decimal gianc { get; set; }
        public decimal giamtc { get; set; }
    }
}