using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Du_Toan_Xay_Dung.Models;
using Du_Toan_Xay_Dung.Handlers;
using Du_Toan_Xay_Dung.Filter;
using System.Data;

namespace Du_Toan_Xay_Dung.Controllers
{
    public class UnitPriceController : Controller
    {
        public DataDTXDDataContext _db = new DataDTXDDataContext();
        //
        // GET: /UnitPrice/

        [PageLogin]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetDSDonGia()
        {
            var list = _db.UnitPrices.Select(i => new
            {
                UnitPrice_ID = i.ID,
                Name = i.Name,
                Unit = i.Unit
            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetArea_PriceUser()
        {
            if (SessionHandler.User != null)
            {
                var list = _db.Areas.Where(i => i.Email.Equals(SessionHandler.User.Email)).Select(i => new AreaViewModel(i));
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error");
            }
        }

        public JsonResult GetResource_User(string id)
        {
            if (SessionHandler.User != null)
            {
                var list = _db.UnitPrices.Join(_db.UnitPrice_Areas, up => up.ID, upa => upa.UnitPrice_ID, (up, upa) => new { Unit_Price = up, UnitPrice_Area = upa }).Where(i => i.UnitPrice_Area.Area_ID.Equals(id))
                .Select(s => new
                {
                    Area_ID = s.UnitPrice_Area.Area_ID,
                    UnitPrice_ID = s.Unit_Price.ID,
                    Price = s.UnitPrice_Area.Price,
                    Name = s.Unit_Price.Name,
                    Unit = s.Unit_Price.Unit,
                }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error");
            }
        }



        [HttpPost]
        public JsonResult Save_Resource(List<UnitPrice_AreaViewModel> list)
        {
            try
            {
                foreach (var item in list)
                {
                    var upa = _db.UnitPrice_Areas.Where(i => i.Area_ID.Equals(item.Area_ID) && i.UnitPrice_ID.Equals(item.UnitPrice_ID)).FirstOrDefault();
                    if (upa != null)
                    {
                        upa.Price = item.Price;
                    }
                    else
                    {
                        UnitPrice_Area u = new UnitPrice_Area();
                        u.Area_ID = item.Area_ID;
                        u.UnitPrice_ID = item.UnitPrice_ID;
                        u.Price = item.Price;
                        _db.UnitPrice_Areas.InsertOnSubmit(u);
                    }
                    _db.SubmitChanges();
                }
                return Json("ok");
            }
            catch (Exception e)
            {
                return Json("error");
            }
        }


        [HttpPost]
        public JsonResult post_createUnitPrice(AreaViewModel model)
        {
            try
            {
                Area a = new Area();
                a.Email = SessionHandler.User.Email;
                a.Name = model.Name;
                a.Address = model.Address;
                _db.Areas.InsertOnSubmit(a);
                _db.SubmitChanges();

                return Json("ok");
            }
            catch (Exception e)
            {
                return Json("error");
            }

        }
	}
}