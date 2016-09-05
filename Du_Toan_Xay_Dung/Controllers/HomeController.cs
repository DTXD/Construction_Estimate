using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Du_Toan_Xay_Dung.Models;
using Du_Toan_Xay_Dung.Handlers;

namespace Du_Toan_Xay_Dung.Controllers
{
    public class HomeController : Controller
    {
        DataDTXDDataContext _db = new DataDTXDDataContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult post_formAngular(string MaHieuCV_DM)
        {
            var congviec = _db.NormWorks.Where(i => i.ID.Equals(MaHieuCV_DM)).Select(i => new DinhMucViewModel(i)).ToList();
            return Json(congviec);
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public ActionResult About(HttpPostedFileBase file)
        {
            DataSet ds = new DataSet();
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension =
                                     System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/Upload/") + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);
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
                    string fileLocation = Server.MapPath("~/Upload/") + Request.Files["FileUpload"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["FileUpload"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    // DataSet ds = new DataSet();
                    ds.ReadXml(xmlreader);
                    xmlreader.Close();
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var khuvuc = new Area();
                    var dongia = new UnitPrice();
                    var dongia_khuvuc = new UnitPrice_Area();
                    khuvuc.ID = Convert.ToInt64(ds.Tables[0].Rows[i][0].ToString());
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

            }
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}