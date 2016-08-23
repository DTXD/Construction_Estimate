using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Du_Toan_Xay_Dung.Models;
using Du_Toan_Xay_Dung.Handlers;

namespace Du_Toan_Xay_Dung.Controllers
{

    public class AccountController : Controller
    {
        DataDTXDDataContext _db = new DataDTXDDataContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ValidateUser(FormCollection form)
        {
            string email = form["Email"];
            string matkhau = form["Password"];

            SessionHandler.User = null;
            var user = _db.Nguoi_Dungs.Where(i => i.Email.Equals(email) && i.MatKhau.Equals(matkhau)).FirstOrDefault();
            if (user != null)
            {
                if (user.Quyen == "user")
                {
                    var userviewmodel = new UserViewModel(user);
                    userviewmodel.MatKhau = string.Empty;
                    SessionHandler.User = userviewmodel;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var userviewmodel = new UserViewModel(user);
                    userviewmodel.MatKhau = string.Empty;
                    SessionHandler.User = userviewmodel;
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Post_Register(FormCollection form)
        {
            string ho_tenlot = form["FirstName"];
            string ten = form["LastName"];
            string email = form["Email"];
            string matkhau = form["Password"];
            string quyen = "user";

            Nguoi_Dung user = new Nguoi_Dung()
            {
                Email = email,
                MatKhau = matkhau,
                Ho_TenLot = ho_tenlot,
                Ten = ten,
                Quyen = quyen
                
            };

            _db.Nguoi_Dungs.InsertOnSubmit(user);
            _db.SubmitChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            SessionHandler.User = null;
            return RedirectToAction("Login");
        }
    }
}