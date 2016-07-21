using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{

    public class UserViewModel
    {
        public UserViewModel() { }

        public UserViewModel(Nguoi_Dung obj)
        {
            Email = obj.Email;
            MatKhau = obj.MatKhau;
            Ho_TenLot = obj.Ho_TenLot;
            Ten = obj.Ten;
            SDT = obj.SDT;
            NoiLamViec = obj.NoiLamViec;
            ThanhPho = obj.ThanhPho;
            Quyen = obj.Quyen;
        }
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public string Ho_TenLot { get; set; }
        public string Ten { get; set; }
        public string SDT { get; set; }
        public string NoiLamViec { get; set; }
        public string ThanhPho { get; set; }
        public string Quyen { get; set; }
        public List<HttpPostedFileBase> img_user { get; set; }

    }
}