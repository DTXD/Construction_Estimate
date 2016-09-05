using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{

    public class UserViewModel
    {
        public UserViewModel() { }

        public UserViewModel(User obj)
        {
            Email = obj.Email;
            Passwork = obj.Password;
            Last_Name = obj.Last_Name;
            First_Name = obj.First_Name;
            Phone = obj.Phone;
            WorkPlace = obj.Workplace;
            City = obj.City;
            Role = obj.Role;
        }
        public string Email { get; set; }
        public string Passwork { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Phone { get; set; }
        public string WorkPlace { get; set; }
        public string City { get; set; }
        public string Role { get; set; }
        public List<HttpPostedFileBase> Url_Image { get; set; }

    }
}