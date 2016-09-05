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
            var user = _db.Users.Where(i => i.Email.Equals(email) && i.Password.Equals(matkhau)).FirstOrDefault();
            if (user != null)
            {
                if (user.Role == "user")
                {
                    var userviewmodel = new UserViewModel(user);
                    userviewmodel.Passwork = string.Empty;
                    SessionHandler.User = userviewmodel;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var userviewmodel = new UserViewModel(user);
                    userviewmodel.Passwork = string.Empty;
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

            User user = new User()
            {
                Email = email,
                Password = matkhau,
                First_Name = ho_tenlot,
                Last_Name = ten,
                Role = quyen
                
            };

            _db.Users.InsertOnSubmit(user);
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