using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class HangMucViewModel
    {
        public HangMucViewModel() { }

        public HangMucViewModel(HangMuc obj)
        {
            Id = obj.Id;
            MaHM = obj.MaHM;
            MaCT = obj.MaCT;
            TenHM = obj.TenHM;
            MoTa = obj.MoTa;
            Gia = obj.Gia;
        }
        public long Id { get; set; }
        public string MaHM { get; set; }
        public string MaCT { get; set; }
        public string TenHM { get; set; }
        public string MoTa { get; set; }
        public decimal Gia { get; set; }
    }
}