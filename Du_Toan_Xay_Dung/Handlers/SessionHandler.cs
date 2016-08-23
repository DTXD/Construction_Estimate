using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Du_Toan_Xay_Dung.Models;

namespace Du_Toan_Xay_Dung.Handlers
{
    public class SessionHandler
    {
        private const string _user = "USER";
        public static UserViewModel User
        {
            get
            {
                if (HttpContext.Current.Session[_user] == null)
                {
                    return null;
                }
                return (UserViewModel)HttpContext.Current.Session[_user];
            }
            set
            {
                HttpContext.Current.Session[_user] = value;
            }
        }
    }
}