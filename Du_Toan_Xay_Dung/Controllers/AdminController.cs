using OfficeOpenXml;
using Du_Toan_Xay_Dung.Filter;
using System.Web.Mvc;
using Du_Toan_Xay_Dung.Models;
using Du_Toan_Xay_Dung.Handlers;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
//using CRUDDeom.Models;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml;
using System.Data.OleDb;
//using CRUDDeom.Models;
using PagedList.Mvc;
using PagedList;
namespace Du_Toan_Xay_Dung.Controllers
{
    [HandleError]
    public class AdminController : Controller
    {
        DataDTXDDataContext _db = new DataDTXDDataContext();
        public ActionResult Index()
        {
            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Title = "Dự toán xây dựng";
            return View();
        }
        public ViewResult themnguoidung()
        {
            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return View("Login", "Account");
            }
            ViewBag.Title = "Dự toán xây dựng";
            return View();
        }
        public ActionResult suanguoidung(string Email)
        {
            var nguoidung = _db.Users.Where(i => i.Email.Equals(Email)).Select(i => new UserViewModel(i)).FirstOrDefault();
            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Title = "Dự toán xây dựng";
            return View(nguoidung);
        }
        [HttpPost]
        public ActionResult post_suanguoidung(string email, FormCollection form)
        {
            var nguoidung = _db.Users.First(m => m.Email == email);
            string Email = form["email"];
            string hotenlot = form["middle-name"];
            string ten = form["name"];
            string thanhpho = form["city"];
            string noilamviec = form["placework"];
            string sodienthoai = form["phone"];
            string hinhanh = form["image"];
            // gán dữ liệu
            nguoidung.Email = Email;
            nguoidung.First_Name = hotenlot;
            nguoidung.Last_Name = ten;
            nguoidung.City = thanhpho;
            nguoidung.Workplace = noilamviec;
            nguoidung.Phone = sodienthoai;
            nguoidung.Url_Image = hinhanh;
            // thực thi câu truy vấn
            UpdateModel(nguoidung);
            _db.SubmitChanges();
            return RedirectToAction("suaxoanguoidung");
        }
        public IEnumerable<User> ListAllPageging1(int page, int pagesize)
        {
            return _db.Users.OrderByDescending(x => x.First_Name).ToPagedList(page, pagesize);
        }
        public ActionResult suaxoanguoidung(int page = 1, int pagesize = 10)
        {
            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Title = "Dự toán xây dựng";
            var model = ListAllPageging1(page, pagesize);
            return View(model);

        }

        public ActionResult upload()
        {
            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public ActionResult upload(HttpPostedFileBase file)
        {
            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
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
            return RedirectToAction("danhsachdongia");
        }

        public ActionResult uploadinhmuc()
        {
            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Title = "Dự toán xây dựng";
            return View();
        }
        [HttpPost]
        public ActionResult post_uploaddinhmuc(HttpPostedFileBase file)
        {
            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
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
                    var dmuc = new NormWork();
                    dmuc.ID = ds.Tables[0].Rows[i][0].ToString();
                    dmuc.Name = ds.Tables[0].Rows[i][1].ToString();
                    dmuc.Unit = ds.Tables[0].Rows[i][3].ToString();

                    _db.NormWorks.InsertOnSubmit(dmuc);
                }
                _db.SubmitChanges();

            }
            return View();
        }
        public ActionResult nhapdinhmuc()
        {

            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Title = "Dự toán xây dựng";
            ViewData["DSDonGia"] = _db.UnitPrices.Select(i => new DonGiaViewModel(i)).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult post_nhapdinhmuc(FormCollection form)
        {
            // nhận dữ liệu từ form
            var madinhmuc = form["madinhmuc"];
            var tendinhmuc = form["tendinhmuc"];
            var donvidinhmuc = form["donvi"];
            var makhuvuc = form["makhuvuc"];
            var mathanhphan = form["mathanhphan"];
            string[] amathanhphan = mathanhphan.Split(new Char[] { ',' });
            var tenthanhphan = form["ten"];
            string[] atenthanhphan = tenthanhphan.Split(new Char[] { ',' });
            var rangbuoc = form["rangbuoc"];
            string[] arangbuoc = rangbuoc.Split(new Char[] { ',' });
            var haophi = form["haophi1"];
            string[] ahaophi = haophi.Split(new Char[] { ',' });
            var donvithanhphan = form["donvithanhphan"];
            string[] adonvithanhphan = donvithanhphan.Split(new Char[] { ',' });

            // nhập dữ liệu cho đối tượng
            NormWork dmuc = new NormWork()
            {
                ID = madinhmuc,
                Name = tendinhmuc,
                Unit = donvidinhmuc,
            };
            _db.NormWorks.InsertOnSubmit(dmuc);
            _db.SubmitChanges();
            for (var i = 0; i < amathanhphan.Length; i++)
            {
                NormDetail dgia = new NormDetail()
                {

                    NormWork_ID= madinhmuc,
                    UnitPrice_ID = amathanhphan[i].ToString(),
                    Numbers = Convert.ToDecimal(ahaophi[i].ToString()),
                  
                };
                _db.NormDetails.InsertOnSubmit(dgia);
                _db.SubmitChanges();
            }
            return RedirectToAction("index");
         }
        [HttpPost]
        public ActionResult Post_themnguoidung(FormCollection form)
        {
            string ho_tenlot = form["middle-name"];
            string ten = form["name"];
            string email = form["email"];
            string matkhau = form["password"];
            string quyen = form["security"];
            string sodienthoai = form["phone"];
            string noilamviec = form["placework"];
            string thanhpho = form["city"];
            string hinhanh = form["image"];
            User user = new User()
            {
                Email = email,
                Password = matkhau,
                First_Name = ho_tenlot,
                Last_Name = ten,
                Role = quyen,
                Workplace = noilamviec,
                City = thanhpho,
                Phone = sodienthoai,
                Url_Image = hinhanh
            };
            _db.Users.InsertOnSubmit(user);
            _db.SubmitChanges();

            return RedirectToAction("suaxoanguoidung", "Admin");
        }

        public ActionResult Delete(string email)
        {

            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            User nguoidung = new User();
            nguoidung = _db.Users.Where(i => i.Email.Equals(email)).FirstOrDefault();
            var congtrinh = _db.Buildings.Where(i => i.Email.Equals(email)).ToList();
            if(congtrinh.Count>0)
            {
                for(var i=0;i<congtrinh.Count;i++)
                {
                    _db.Buildings.DeleteOnSubmit(congtrinh[i]);
                }
            }

            _db.Users.DeleteOnSubmit(nguoidung);
            _db.SubmitChanges();
            return RedirectToAction("suaxoanguoidung");
        }
        [HttpPost]
        public ActionResult timkiemnguoidung(FormCollection f, int? page)
        {
            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            string tukhoa = f["txttimkiem"].ToString();
            ViewBag.tukhoa = tukhoa;
            List<User> listnguoidung = _db.Users.Where(n => n.Last_Name.Contains(tukhoa)).ToList();
            // phân trang
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            if (listnguoidung.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy bản ghi phù hợp";
                return View(_db.Users.OrderBy(n => n.Last_Name).ToPagedList(pagenumber, pagesize));
            }
            ViewBag.ThongBao = "Đã tìm thấy" + "    " + listnguoidung.Count + "kết quả";
            return View(listnguoidung.OrderBy(n => n.Last_Name).ToPagedList(pagenumber, pagesize));
        }
        [HttpGet]
        public ActionResult timkiemnguoidung(string tukhoa, int? page)
        {
            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            string tukhoa1 = tukhoa;
            ViewBag.tukhoa = tukhoa1;
            List<User> listnguoidung = _db.Users.Where(n => n.Last_Name.Contains(tukhoa)).ToList();
            // phân trang
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            if (listnguoidung.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy bản ghi phù hợp";
                return View(_db.UnitPrices.OrderBy(n => n.Name).ToPagedList(pagenumber, pagesize));
            }
            ViewBag.ThongBao = "Đã tìm thấy" + listnguoidung.Count + "kết quả";
            return View(listnguoidung.OrderBy(n => n.Last_Name).ToPagedList(pagenumber, pagesize));
        }
        [HttpPost]
        public ActionResult timkiem(FormCollection f, int? page)
        {
            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            string tukhoa = f["txttimkiem"].ToString();
            ViewBag.tukhoa = tukhoa;
            List<UnitPrice> listdongia = _db.UnitPrices.Where(n => n.Name.Contains(tukhoa)).ToList();
            // phân trang
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            if (listdongia.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy bản ghi phù hợp";
                return View(_db.UnitPrices.OrderBy(n => n.Name).ToPagedList(pagenumber, pagesize));
            }
            ViewBag.ThongBao = "Đã tìm thấy" + "    " + listdongia.Count + "kết quả";
            return View(listdongia.OrderBy(n => n.Name).ToPagedList(pagenumber, pagesize));
        }
        [HttpGet]
        public ActionResult timkiem(string tukhoa, int? page)
        {
            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            string tukhoa1 = tukhoa;
            ViewBag.tukhoa = tukhoa1;
            List<UnitPrice> listdongia = _db.UnitPrices.Where(n => n.Name.Contains(tukhoa)).ToList();
            // phân trang
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            if (listdongia.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy bản ghi phù hợp";
                return View(_db.UnitPrices.OrderBy(n => n.Name).ToPagedList(pagenumber, pagesize));
            }
            ViewBag.ThongBao = "Đã tìm thấy" + listdongia.Count + "kết quả";
            return View(listdongia.OrderBy(n => n.Name).ToPagedList(pagenumber, pagesize));
        }
        public IEnumerable<UnitPrice> ListAllPageging(int page, int pagesize)
        {
            return _db.UnitPrices.OrderByDescending(x => x.ID).ToPagedList(page, pagesize);
        }
        public ActionResult danhsachdongia(int page = 1, int pagesize = 10)
        {
            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            var model = ListAllPageging(page, pagesize);
            return View(model);
        }
        public ActionResult suadongia(string MaVL_NC_MTC)
        {
            ViewData["dongia"] = _db.UnitPrices.Where(i => i.ID.Equals(MaVL_NC_MTC)).Select(i => new DonGiaViewModel(i)).FirstOrDefault();
            ViewData["dongia_khuvuc"] = _db.UnitPrice_Areas.Where(i => i.UnitPrice_ID.Equals(MaVL_NC_MTC)).Select(i => new UnitPrice_AreaViewModel(i)).FirstOrDefault();
            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Title = "Dự toán xây dựng";
            return View();
        }
        [HttpPost]
        public ActionResult post_suadongia(string mathanhphan, FormCollection form)
        {
            var dongia = _db.UnitPrices.First(n => n.ID.Equals(mathanhphan));
            var dongia_khuvuc = _db.UnitPrice_Areas.First(i => i.UnitPrice_ID.Equals(mathanhphan));
            string gia1 = form["dongia"];
            decimal gia;
            gia = Convert.ToDecimal(gia1);
            string donvi = form["donvi"];
            string mathanhphan1 = form["mathanhphan"];
            // gán dữ liệu
            dongia_khuvuc.Price = gia;
            dongia.Unit = donvi;
            dongia.ID = mathanhphan1;
            // thực thi câu truy vấn
            UpdateModel(dongia);
            UpdateModel(dongia_khuvuc);
            _db.SubmitChanges();
            return RedirectToAction("danhsachdongia");
        }
        /* [PageLogin]
         [HttpPost]
         public JsonResult post_themnguoidung(UserViewModel obj)
         {
             try
             {
                 var index = _db.Nguoi_Dungs.OrderByDescending(i => i.Ten).Select(i => i.Ten).FirstOrDefault();
                 index = index + 1;

                 /*if (obj.img_user.Count() > 0)
                 {
                     string url_location = Server.MapPath("~/Images/Nguoidung");
                     if (Directory.Exists(url_location))
                     {
                         foreach (var file in obj.img_user)
                         {
                             if (file != null && file.ContentLength > 0)
                             {
                                 string fileLocation = Server.MapPath("~/Images/Admin/") + file.FileName;
                                 file.SaveAs(fileLocation);
                             }
                         }
                     }
                 }
                 var nguoidung = new Nguoi_Dung();
                 nguoidung.Email = obj.Email;
                 nguoidung.Ten = obj.Ten;
                 nguoidung.Ho_TenLot = obj.Ho_TenLot;
                 nguoidung.Quyen = obj.Quyen;
                 nguoidung.ThanhPho = obj.ThanhPho;
                 nguoidung.SDT = obj.SDT;

                 _db.Nguoi_Dungs.InsertOnSubmit(nguoidung);

                 /*foreach (var file in obj.img_user)
                 {
                     if (file != null && file.ContentLength > 0)
                     {
                         //them image vao database
                         var image = new img_user();

                         image.Url = "~/Images/Admin/" + file.FileName;
                         _db.iInsertOnSubmit(image);
                     }
                 }
                 _db.SubmitChanges();

                 return Json("ok");
             }
             catch (Exception)
             {
                 return Json("error");
             }
         }*/


    }
}





