﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Du_Toan_Xay_Dung.Models;
using Du_Toan_Xay_Dung.Handlers;
using Du_Toan_Xay_Dung.Filter;
using System.Data;
using System.Data.OleDb;
using System.Xml;

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
            var list = _db.UnitPrices.Select(i => new
            {
                UnitPrice_ID = i.ID,
                Name = i.Name,
                Unit = i.Unit
            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetailNormWork_Price(string area_id)
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
                }).Where(i => i.UnitPrice_Areas.Area_ID.Equals(area_id))
                    .Select(s => new
                    {
                        Key_NormWork = s.UnitPrices.NormDetails.NormWork_ID,
                        Key_Material = s.UnitPrices.UnitPrices.ID,
                        Number = s.UnitPrices.NormDetails.Numbers,
                        Unit = s.UnitPrices.UnitPrices.Unit,
                        Name_Material = s.UnitPrices.UnitPrices.Name,
                        Price = s.UnitPrice_Areas.Price
                    }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllSheets(string buildingitem_id)
        {
            if (buildingitem_id != null)
            {
                var sheets = _db.UserWorks.Where(i => i.BuildingItem_ID.Equals(buildingitem_id)).Select(i => new UserWorkViewModel(i)).ToList();
                return Json(sheets, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getAllResources(string buildingitem_id)
        {
            if (buildingitem_id != null)
            {
                var resources = _db.UserWork_Resources.Where(i => i.BuildingItem_ID.Equals(buildingitem_id)).Select(i => new UserWorkResourceViewModel(i)).ToList();
                return Json(resources, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getGroupbyResources(string buildingitem_id)
        {
            if (buildingitem_id != null)
            {
                var resources = _db.UserWork_Resources.Where(i => i.BuildingItem_ID.Equals(buildingitem_id))
                    .GroupBy(i => new { i.UnitPrice_ID, i.Name, i.Unit, i.Price })
                    .Select(s => new UserWorkResourceViewModel {
                        UnitPrice_ID = s.First().UnitPrice_ID,
                        Name = s.First().Name,
                        Unit = s.First().Unit,
                        Number_Norm = s.Sum(i=>i.Number_Norm),
                        Price = s.First().Price
                        
                    });
                return Json(resources, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetArea_Price()
        {
            var list = _db.Areas.Select(i => new AreaViewModel(i)).ToList();
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

        public JsonResult post_savework(UserWorkViewModel model)
        {
            try
            {
                if (model.BuildingItem_ID != 0)
                {
                    var work = _db.UserWorks.FirstOrDefault(i => i.IndexSheet.Equals(model.IndexSheet));
                    if (work != null)
                    {
                        work.ID = model.ID;
                        work.NormWork_ID = model.NormWork_ID;
                        work.Name = model.Name;
                        work.Unit = model.Unit;
                        work.Number = model.Number;
                        work.Horizontal = model.Horizontal;
                        work.Vertical = model.Vertical;
                        work.Height = model.Height;
                        work.Area = model.Area;
                        work.SumMaterial = model.SumMaterial;
                        work.SumLabor = model.SumLabor;
                        work.SumMachine = model.SumMachine;
                    }
                    else
                    {
                        UserWork ew = new UserWork();
                        ew.BuildingItem_ID = model.BuildingItem_ID;
                        ew.Sub_BuildingItem_ID = model.Sub_BuildingItem_ID;
                        ew.IndexSheet = model.IndexSheet;
                        ew.ID = model.ID;
                        ew.NormWork_ID = model.NormWork_ID;
                        ew.Name = model.Name;
                        ew.Unit = model.Unit;
                        ew.Number = model.Number;
                        ew.Horizontal = model.Horizontal;
                        ew.Vertical = model.Vertical;
                        ew.Height = model.Height;
                        ew.Area = model.Area;
                        ew.PriceLabor = model.PriceLabor;
                        ew.PriceMaterial = model.PriceMaterial;
                        ew.PriceMachine = model.PriceMachine;
                        ew.SumMaterial = model.SumMaterial;
                        ew.SumLabor = model.SumLabor;
                        ew.SumMachine = model.SumMachine;
                        _db.UserWorks.InsertOnSubmit(ew);
                    }
                    _db.SubmitChanges();
                }
                return Json("ok");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        public JsonResult post_updatework(UserWorkViewModel model)
        {
            try
            {
                if (model.BuildingItem_ID != 0)
                {
                    var work = _db.UserWorks.FirstOrDefault(i => i.IndexSheet.Equals(model.IndexSheet));
                    if (work != null)
                    {
                        work.Area = model.Area;
                        work.SumMaterial = model.SumMaterial;
                        work.SumLabor = model.SumLabor;
                        work.SumMachine = model.SumMachine;
                    }
                    _db.SubmitChanges();
                }
                return Json("ok");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        public JsonResult post_updateresource(List<UserWorkResourceViewModel> list)
        {
            try
            {
                if (list[0].BuildingItem_ID != 0)
                {
                    foreach (var item in list)
                    {
                        UserWork_Resource ewr = new UserWork_Resource();
                        ewr.BuildingItem_ID = item.BuildingItem_ID;
                        ewr.UserWork_ID = item.UserWork_ID;
                        ewr.UnitPrice_ID = item.UnitPrice_ID;
                        ewr.Name = item.Name;
                        ewr.Unit = item.Unit;
                        ewr.Number_Norm = item.Number_Norm;
                        ewr.Price = item.Price;
                        _db.UserWork_Resources.InsertOnSubmit(ewr);
                    }
                    _db.SubmitChanges();
                }
                return Json("ok");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }
    }
}
