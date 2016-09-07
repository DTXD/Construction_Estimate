using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Du_Toan_Xay_Dung.Models;
using Du_Toan_Xay_Dung.Handlers;
using Du_Toan_Xay_Dung.Filter;

namespace Du_Toan_Xay_Dung.Controllers
{
    public class HangMucController : Controller
    {
        public DataDTXDDataContext _db = new DataDTXDDataContext();

        public ActionResult Estimate_Work()
        {
            return View();
        }
        public JsonResult GetNormWorks()
        {
            var list_normwork = _db.NormWorks.Select(i => new DinhMucViewModel(i)).ToList();

            return Json(list_normwork, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDSDonGia()
        {
            var list = _db.UnitPrices.Select(i => new DonGiaViewModel(i)).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetailNormWork_Price()
        {
            var list = _db.NormDetails.Join(_db.UnitPrices, nd => nd.UnitPrice_ID, up => up.ID, (nd, up) => new
            {
                NormDetails = nd,
                UnitPrices = up
            })
                .Join(_db.UnitPrice_Areas, up => up.UnitPrices.ID, upa => upa.UnitPrice_ID, (up, upa) => new
                {
                    UnitPrices = up,
                    UnitPrice_Areas = upa
                })
                    .Select(s => new
                    {
                        Key_NormWork = s.UnitPrices.NormDetails.NormWork_ID,
                        Key_Material = s.UnitPrices.UnitPrices.ID,
                        Number = s.UnitPrices.NormDetails.Numbers,
                        Unit = s.UnitPrices.UnitPrices.Unit,
                        Name_Material = s.UnitPrices.UnitPrices.Name,
                        Price = s.UnitPrice_Areas.Price
                    });



            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult post_updatework(CongViec_User_ViewModel obj)
        {
            return Json("ok");
        }
    }
}
