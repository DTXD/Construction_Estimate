using Du_Toan_Xay_Dung.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Du_Toan_Xay_Dung.Filter
{
    public class PageLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = SessionHandler.User;
            if (user == null)
            {
                filterContext.Result = new RedirectResult("/Account/Login");
            }
        }
    }
}