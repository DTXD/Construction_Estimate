using OfficeOpenXml;
using Du_Toan_Xay_Dung.Filter;
using System.Web.Mvc;
using Du_Toan_Xay_Dung.Models;
using Du_Toan_Xay_Dung.Handlers;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
//using CRUDDeom.Models;
using OfficeOpenXml;
using Du_Toan_Xay_Dung.Filter;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml;
using System.Data.OleDb;
using System.Web.Mvc;
using Du_Toan_Xay_Dung.Models;
using Du_Toan_Xay_Dung.Handlers;
using System.Web.UI.WebControls;
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
            var nguoidung = _db.Nguoi_Dungs.Where(i => i.Email.Equals(Email)).Select(i => new UserViewModel(i)).FirstOrDefault();
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
            var nguoidung = _db.Nguoi_Dungs.First(m => m.Email == email);
            string Email = form["email"];
            string hotenlot = form["middle-name"];
            string ten = form["name"];
            string thanhpho = form["city"];
            string noilamviec = form["placework"];
            string sodienthoai = form["phone"];
            string hinhanh = form["image"];
            // gán dữ liệu
            nguoidung.Email = Email;
            nguoidung.Ho_TenLot = hotenlot;
            nguoidung.Ten = ten;
            nguoidung.ThanhPho = thanhpho;
            nguoidung.NoiLamViec = noilamviec;
            nguoidung.SDT = sodienthoai;
            nguoidung.Url_HinhAnh = hinhanh;
            // thực thi câu truy vấn
            UpdateModel(nguoidung);
            _db.SubmitChanges();
            return RedirectToAction("suaxoanguoidung");
        }
        public IEnumerable<Nguoi_Dung> ListAllPageging1(int page, int pagesize)
        {
            return _db.Nguoi_Dungs.OrderByDescending(x => x.Ten).ToPagedList(page, pagesize);
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
                    var dongia = new DonGia();
                    dongia.MaKV = ds.Tables[0].Rows[i][0].ToString();
                    dongia.MaVL_NC_MTC = ds.Tables[0].Rows[i][1].ToString();
                    dongia.Ten = ds.Tables[0].Rows[i][2].ToString();
                    dongia.DonVi = ds.Tables[0].Rows[i][3].ToString();
                    dongia.Gia = Convert.ToDecimal(ds.Tables[0].Rows[i][4].ToString());

                    _db.DonGias.InsertOnSubmit(dongia);
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
                    var dmuc = new DinhMuc();
                    dmuc.MaHieuCV_DM = ds.Tables[0].Rows[i][0].ToString();
                    dmuc.CongTac = ds.Tables[0].Rows[i][1].ToString();
                    dmuc.RangBuoc = ds.Tables[0].Rows[i][2].ToString();
                    dmuc.DonVi = ds.Tables[0].Rows[i][3].ToString();

                    _db.DinhMucs.InsertOnSubmit(dmuc);
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
            ViewData["DSDonGia"] = _db.DonGias.Select(i => new DonGiaViewModel(i)).ToList();
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
            DinhMuc dmuc = new DinhMuc()
            {
                MaHieuCV_DM = madinhmuc,
                CongTac = tendinhmuc,
                RangBuoc = rangbuoc,
                DonVi = donvidinhmuc,
            };
            _db.DinhMucs.InsertOnSubmit(dmuc);
            _db.SubmitChanges();
            for (var i = 0; i < amathanhphan.Length; i++)
            {
                ChiTiet_DinhMuc dgia = new ChiTiet_DinhMuc()
                {

                    MaHieuCV_DM = madinhmuc,
                    MaVL_NC_MTC = amathanhphan[i].ToString(),
                    SoLuong = Convert.ToDecimal(ahaophi[i].ToString()),
                    DonVi = adonvithanhphan[i].ToString()
                };
                _db.ChiTiet_DinhMucs.InsertOnSubmit(dgia);
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
            Nguoi_Dung user = new Nguoi_Dung()
            {
                Email = email,
                MatKhau = matkhau,
                Ho_TenLot = ho_tenlot,
                Ten = ten,
                Quyen = quyen,
                NoiLamViec = noilamviec,
                ThanhPho = thanhpho,
                SDT = sodienthoai,
                Url_HinhAnh = hinhanh
            };
            _db.Nguoi_Dungs.InsertOnSubmit(user);
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
            Nguoi_Dung nguoidung = new Nguoi_Dung();
            nguoidung = _db.Nguoi_Dungs.Where(i => i.Email.Equals(email)).FirstOrDefault();
            var congtrinh = _db.CongTrinhs.Where(i => i.Email.Equals(email)).ToList();
            if(congtrinh.Count>0)
            {
                for(var i=0;i<congtrinh.Count;i++)
                {
                    _db.CongTrinhs.DeleteOnSubmit(congtrinh[i]);
                }
            }

            _db.Nguoi_Dungs.DeleteOnSubmit(nguoidung);
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
            List<Nguoi_Dung> listnguoidung = _db.Nguoi_Dungs.Where(n => n.Ten.Contains(tukhoa)).ToList();
            // phân trang
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            if (listnguoidung.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy bản ghi phù hợp";
                return View(_db.Nguoi_Dungs.OrderBy(n => n.Ten).ToPagedList(pagenumber, pagesize));
            }
            ViewBag.ThongBao = "Đã tìm thấy" + "    " + listnguoidung.Count + "kết quả";
            return View(listnguoidung.OrderBy(n => n.Ten).ToPagedList(pagenumber, pagesize));
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
            List<Nguoi_Dung> listnguoidung = _db.Nguoi_Dungs.Where(n => n.Ten.Contains(tukhoa)).ToList();
            // phân trang
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            if (listnguoidung.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy bản ghi phù hợp";
                return View(_db.DonGias.OrderBy(n => n.Ten).ToPagedList(pagenumber, pagesize));
            }
            ViewBag.ThongBao = "Đã tìm thấy" + listnguoidung.Count + "kết quả";
            return View(listnguoidung.OrderBy(n => n.Ten).ToPagedList(pagenumber, pagesize));
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
            List<DonGia> listdongia = _db.DonGias.Where(n => n.Ten.Contains(tukhoa)).ToList();
            // phân trang
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            if (listdongia.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy bản ghi phù hợp";
                return View(_db.DonGias.OrderBy(n => n.Ten).ToPagedList(pagenumber, pagesize));
            }
            ViewBag.ThongBao = "Đã tìm thấy" + "    " + listdongia.Count + "kết quả";
            return View(listdongia.OrderBy(n => n.Ten).ToPagedList(pagenumber, pagesize));
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
            List<DonGia> listdongia = _db.DonGias.Where(n => n.Ten.Contains(tukhoa)).ToList();
            // phân trang
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            if (listdongia.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy bản ghi phù hợp";
                return View(_db.DonGias.OrderBy(n => n.Ten).ToPagedList(pagenumber, pagesize));
            }
            ViewBag.ThongBao = "Đã tìm thấy" + listdongia.Count + "kết quả";
            return View(listdongia.OrderBy(n => n.Ten).ToPagedList(pagenumber, pagesize));
        }
        public IEnumerable<DonGia> ListAllPageging(int page, int pagesize)
        {
            return _db.DonGias.OrderByDescending(x => x.MaKV).ToPagedList(page, pagesize);
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
            var dongia = _db.DonGias.Where(i => i.MaVL_NC_MTC.Equals(MaVL_NC_MTC)).Select(i => new DonGiaViewModel(i)).FirstOrDefault();
            if (SessionHandler.User != null)
            {
                var user = SessionHandler.User;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Title = "Dự toán xây dựng";
            return View(dongia);
        }
        [HttpPost]
        public ActionResult post_suadongia(string mathanhphan, FormCollection form)
        {
            var dongia = _db.DonGias.First(n => n.MaVL_NC_MTC.Equals(mathanhphan));
            string gia1 = form["dongia"];
            decimal gia;
            gia = Convert.ToDecimal(gia1);
            string donvi = form["donvi"];
            string mathanhphan1 = form["mathanhphan"];
            // gán dữ liệu
            dongia.Gia = gia;
            dongia.DonVi = donvi;
            dongia.MaVL_NC_MTC = mathanhphan1;
            // thực thi câu truy vấn
            UpdateModel(dongia);
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





