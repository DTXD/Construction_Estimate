using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class DetailNormWork_PriceViewModel
    {
        private NormDetail i;

        public DetailNormWork_PriceViewModel() {}

        public DetailNormWork_PriceViewModel(NormDetail i)
        {
            // TODO: Complete member initialization
            this.i = i;
        }
        public string Key_NormWork { get; set; }
        public string Key_Material { get; set; }
        public decimal Number { get; set; }
        public string Unit { get; set; }
        public string Name_Material { get; set; }
        public decimal Price_Material { get; set; }
    }
}