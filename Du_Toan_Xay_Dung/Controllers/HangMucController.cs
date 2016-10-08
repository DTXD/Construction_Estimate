using System;
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

        /*
        public JsonResult upload_file_unitprice(Create_UnitpriceViewModel model)
        {
            try
            {
                DataSet ds = new DataSet();
                if (model.file.ContentLength > 0 && SessionHandler.User != null)
                {
                    string fileExtension =
                                         System.IO.Path.GetExtension(model.file.FileName);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        string fileLocation = Server.MapPath("~/Upload/") + model.file.FileName;
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                        model.file.SaveAs(fileLocation);
                        string excelConnectionString = string.Empty;
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        //connection String for xls file format.
                        if (fileExtension == ".xls")
                        {
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        //connection String for xlsx file format.
                        else if (fileExtension == ".xlsx")
                        {
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        }
                        //Create Connection to Excel work book and add oledb namespace
                        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                        excelConnection.Open();
                        DataTable dt = new DataTable();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }

                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                        string query = string.Format("Select * from [{0}]", excelSheets[0]);
                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                        {
                            dataAdapter.Fill(ds);
                        }
                    }
                    if (fileExtension.ToString().ToLower().Equals(".xml"))
                    {
                        string fileLocation = Server.MapPath("~/Upload/") + model.file.FileName;
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }

                        model.file.SaveAs(fileLocation);
                        XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                        // DataSet ds = new DataSet();
                        ds.ReadXml(xmlreader);
                        xmlreader.Close();
                    }

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var khuvuc = new Area();
                        khuvuc.Name = model.Name;
                        khuvuc.Address = model.Address;
                        khuvuc.Email = model.Email;

                        var dongia = new UnitPrice();
                        var dongia_khuvuc = new UnitPrice_Area();

                        

                        dongia.ID = ds.Tables[0].Rows[i][1].ToString();
                        dongia_khuvuc.Area_ID = Convert.ToInt64(ds.Tables[0].Rows[i][0].ToString());
                        dongia_khuvuc.UnitPrice_ID = ds.Tables[0].Rows[i][1].ToString();
                        dongia.Name = ds.Tables[0].Rows[i][2].ToString();
                        dongia.Unit = ds.Tables[0].Rows[i][3].ToString();
                        dongia_khuvuc.Price = Convert.ToDecimal(ds.Tables[0].Rows[i][4].ToString());
                        _db.Areas.InsertOnSubmit(khuvuc);
                        _db.UnitPrices.InsertOnSubmit(dongia);
                        _db.UnitPrice_Areas.InsertOnSubmit(dongia_khuvuc);
                    }
                    _db.SubmitChanges();
                    return Json("ok");
                }
                else
                {
                    return Json("error");
                }
            }
            catch (Exception e)
            {
                return Json("error");
            }

        }
        */
        public JsonResult post_updatework(UserWorkViewModel model)
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
