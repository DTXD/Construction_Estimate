﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Du_Toan_Xay_Dung.Models;
using Du_Toan_Xay_Dung.Handlers;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
//using CRUDDeom.Models;
using OfficeOpenXml;
using Du_Toan_Xay_Dung.Filter;

namespace Du_Toan_Xay_Dung.Controllers
{
    public class CongTrinhController : Controller
    {
        DataDTXDDataContext _db = new DataDTXDDataContext();
        [PageLogin]
        public ActionResult Index()
        {
            ViewData["List_CongTrinh"] = _db.CongTrinhs.Where(i => i.Email.Equals(SessionHandler.User.Email)).Select(i => new CongTrinhViewModel(i)).ToList();
            ViewData["List_CongTrinh_Null"] = _db.CongTrinhs.Where(i => i.Email.Equals(SessionHandler.User.Email) && !i.HangMucs.Any(o => o.MaCT.Equals(i.MaCT))).Select(i => i.MaCT).ToList();
            return View();
        }
        [PageLogin]
        public JsonResult get_datafile(string ID)
        {
            var data = _db.Images_CongTrinhs.Where(i => i.MaCT.Equals(ID)).Select(i => i.Url).ToList();
            return Json(data);
        }

        public ActionResult HinhAnhCT(string Id)
        {
            ViewData["image_congtrinh"] = _db.Images_CongTrinhs.Where(i => i.MaCT.Equals(Id)).Select(i => new Images_CongTrinhViewModel(i)).FirstOrDefault();
            ViewData["congtrinh"] = _db.CongTrinhs.Where(a => a.MaCT.Equals(Id)).Select(a => new CongTrinhViewModel(a)).FirstOrDefault();
            return View();
        }


        [PageLogin]
        public ActionResult ChiTiet_CongTrinh(string Id)
        {
            ViewData["List_CongTrinh"] = _db.CongTrinhs.Where(i => i.Email.Equals(SessionHandler.User.Email)).Select(i => new CongTrinhViewModel(i)).ToList();
            ViewData["CongTrinh_Detail"] = _db.CongTrinhs.Where(i => i.MaCT.Equals(Id)).Select(i => new CongTrinhViewModel(i)).FirstOrDefault();
            ViewData["List_HangMuc_IdCT"] = _db.HangMucs.Where(i => i.MaCT.Equals(Id)).Select(i => new HangMucViewModel(i)).ToList();

            return View();
        }

        [PageLogin]
        public JsonResult Delete_CongTrinh(CongTrinhViewModel obj)
        {
            try
            {
                var img_congtrinh = _db.Images_CongTrinhs.Where(i => i.MaCT.Equals(obj.MaCT)).ToList();
                var hangmuc = _db.HangMucs.Where(i => i.MaCT.Equals(obj.MaCT)).ToList();
                var congtrinh = _db.CongTrinhs.Single(i => i.MaCT.Equals(obj.MaCT));
                for (var i = 0; i < img_congtrinh.Count; i++)
                {
                    _db.Images_CongTrinhs.DeleteOnSubmit(img_congtrinh[i]);
                }
                if(hangmuc.Count!=0)
                {
                    for (var i = 0; i < hangmuc.Count; i++)
                    {
                        _db.HangMucs.DeleteOnSubmit(hangmuc[i]);
                    }
                }
                _db.CongTrinhs.DeleteOnSubmit(congtrinh);

                _db.SubmitChanges();
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        [PageLogin]
        public ActionResult UpdateCongTrinh(string ID)
        {
            if (ID == null)
            {
                ID = "";
            }
            ViewData["CongTrinh_Update"] = _db.CongTrinhs.Where(i => i.MaCT.Equals(ID)).Select(i => new CongTrinhViewModel(i)).FirstOrDefault();
            ViewData["image_congtrinh"] = _db.Images_CongTrinhs.Where(a => a.MaCT.Equals(ID)).Select(i => new Images_CongTrinhViewModel(i)).ToList();
            return View();
        }
        [PageLogin]
        [HttpPost]
        public JsonResult post_updatecongtrinh(CongTrinhViewModel obj)
        {
            //try
            //{
                int int_ma = Convert.ToInt32(obj.MaCT.Substring(2, 1));
                var congtrinh = _db.CongTrinhs.First(m => m.MaCT == obj.MaCT);
                if (obj.img_congtrinh != null)
                {
                    if (obj.img_congtrinh.Count() > 0)
                    {
                        string url_location = Server.MapPath("~/Images/CongTrinh");
                        if (Directory.Exists(url_location))
                        {
                            //foreach (var file1 in Request.Files["myFile"])
                            foreach (var file1 in obj.img_congtrinh)

                                if (file1 != null && file1.ContentLength > 0)
                                {
                                    string fileLocation = Server.MapPath("~/Images/CongTrinh/") + file1.FileName;
                                    file1.SaveAs(fileLocation);
                                }
                        }
                    }
                }

                congtrinh.Id = int_ma;
                congtrinh.TenCT = obj.TenCT;
                congtrinh.MoTa = obj.MoTa;
                congtrinh.DiaChi = obj.DiaChi;
                congtrinh.Gia = obj.Gia;

                if (obj.img_congtrinh != null)
                {
                    foreach (var file in obj.img_congtrinh)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                        //them image vao database
                        var image = new Images_CongTrinh();
                        image.MaCT = "CT" + int_ma.ToString();
                        image.Url = "~/Images/CongTrinh/" + file.FileName;
                        _db.Images_CongTrinhs.InsertOnSubmit(image);
                        }
                    }
                }
                /*if (obj.img_old != null)
                //{
                    foreach (var file in obj.img_old)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            var image = new Images_CongTrinh();
                            image.MaCT = "CT" + int_ma.ToString();
                            image.Url = "~/Images/CongTrinh/" + file.FileName;
                            _db.Images_CongTrinhs.DeleteOnSubmit(image);
                            //UpdateModel(image);
                        }
                    }
                //}*/
                _db.SubmitChanges();
                return Json("ok");
            //}
            //catch (Exception)
            //{
                //return Json("error");
            //}
        }
        [PageLogin]
        public ActionResult UpdateHangMuc(string ID)
        {

            ViewData["HangMuc_Update"] = _db.HangMucs.Where(i => i.MaHM.Equals(ID)).Select(i => new HangMucViewModel(i)).FirstOrDefault();
            return View();
        }
        [PageLogin]
        [HttpPost]
        public JsonResult updatehangmuc(HangMucViewModel obj)
        {
            try
            {
                var hangmuc = _db.HangMucs.Where(i => i.MaHM.Equals(obj.MaHM)).FirstOrDefault();
                hangmuc.Gia = obj.Gia;
                hangmuc.MoTa = obj.MoTa;
                hangmuc.TenHM = obj.TenHM;
                _db.SubmitChanges();
                return Json("ok");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }
        [PageLogin]
        public JsonResult Delete(HangMucViewModel obj)
        {
            try
            {
                HangMuc hangmuc = new HangMuc();
                hangmuc = _db.HangMucs.Where(i => i.MaHM.Equals(obj.MaHM)).FirstOrDefault();
                var congtrinh = _db.HangMucs.Where(i => i.MaHM.Equals(hangmuc.MaCT)).ToList();
                if (hangmuc != null)
                {
                    _db.HangMucs.DeleteOnSubmit(hangmuc);
                }
                _db.SubmitChanges();
                if (congtrinh.Count == 0)
                {
                    return Json("0");
                }
                else
                {
                    return Json("1");
                }
            }
            catch(Exception)
            {
                return Json("error");
            }
        }

        [PageLogin]
        public ActionResult ThemCongTrinh()
        {
            return View();
        }

        [PageLogin]
        [HttpPost]
        public JsonResult Post_ThemCongTrinh(CongTrinhViewModel obj)
        {
            try
            {
            var index = _db.CongTrinhs.OrderByDescending(i => i.Id).Select(i => i.Id).FirstOrDefault();
            index = index + 1;

            if (obj.img_congtrinh != null)
            {
                if (obj.img_congtrinh.Count() > 0)
                {
                    string url_location = Server.MapPath("~/Images/CongTrinh");
                    if (Directory.Exists(url_location))
                    {
                        foreach (var file in obj.img_congtrinh)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                string fileLocation = Server.MapPath("~/Images/CongTrinh/") + file.FileName;
                                file.SaveAs(fileLocation);
                            }
                        }
                    }
                }
            }
            /*
            if (obj.img_congtrinh != null)
            {

                if (obj.img_congtrinh.Count() > 0)
                {
                    string url_location = Server.MapPath("~/Images/CongTrinh");
                    if (Directory.Exists(url_location))
                    {
                        foreach (var file in obj.img_congtrinh)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                string fileLocation = Server.MapPath("~/Images/CongTrinh/") + file.FileName;
                                file.SaveAs(fileLocation);
                            }
                        }
                    }
                    /*
                    foreach (var file in obj.img_congtrinh)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            //BinaryReader b = new BinaryReader(file.InputStream);
                            //byte[] binData = b.ReadBytes(file.ContentLength);

                            //string result = System.Text.Encoding.UTF8.GetString(binData);
                            //file.SaveAs(string.Format("{0}/{1}", url_location, file.FileName));
                            con.UploadtoDropbox(token_dropbox, "/Image", file.FileName,file);
                        }
                    }                    */
            // }
            // }
            var congtrinh = new CongTrinh();
            congtrinh.Id = index;
            congtrinh.MaCT = "CT" + index.ToString();
            congtrinh.Email = SessionHandler.User.Email;
            congtrinh.TenCT = obj.TenCT;
            congtrinh.MoTa = obj.MoTa;
            congtrinh.DiaChi = obj.DiaChi;
            congtrinh.Gia = 0;

            _db.CongTrinhs.InsertOnSubmit(congtrinh);
            if (obj.img_congtrinh != null)
            {
                foreach (var file in obj.img_congtrinh)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        //them image vao database
                        var image = new Images_CongTrinh();
                        image.MaCT = "CT" + index.ToString();
                        image.Url = "~/Images/CongTrinh/" + file.FileName;
                        _db.Images_CongTrinhs.InsertOnSubmit(image);
                    }
                }
            }
            _db.SubmitChanges();

            return Json("ok");
            }
            catch (Exception)
             {
             return Json("error");
             }
            //HttpPostedFileBase file;
            //if (Request.Files.AllKeys.Any())
            //{
            //    string url_location = Server.MapPath("~/Images/CongTrinh/CT" + index);
            //    if(!Directory.Exists(url_location))
            //    {
            //        Directory.CreateDirectory(url_location);
            //    }
            //    for (var i = 0; i < Request.Files.Count; i++)
            //    {
            //        var fileData = Request.Files[i];//["Avatar"];
            //        if (fileData != null && fileData.ContentLength > 0)
            //        {
            //            string fileLocation = Server.MapPath("~/Images/CongTrinh/CT" + index + "/") + fileData.FileName;
            //            fileData.SaveAs(fileLocation);
            //        }
            //    }
            //}

        }

        [PageLogin]
        public ActionResult ThemHangMuc(string id)
        {
            ViewData["MaCT_ThemHangMuc"] = id;
            return View();
        }
        [PageLogin]
        [HttpPost]
        public JsonResult post_themhangmuc(HangMucViewModel obj)
        {
            try
            {
                string ID = obj.MaCT;
                //lay dong cuoi cung bang hangmuc
                var idhm_last = _db.HangMucs.OrderByDescending(i => i.Id).Select(i => i.Id).FirstOrDefault();
                idhm_last = idhm_last + 1;
                HangMuc hm = new HangMuc();
                hm.Id = idhm_last;
                hm.MaHM = "HM" + idhm_last.ToString();
                hm.MaCT = ID;
                hm.TenHM = obj.TenHM;
                hm.MoTa = obj.MoTa;
                hm.Gia = 0;
                _db.HangMucs.InsertOnSubmit(hm);
                _db.SubmitChanges();

                return Json("ok");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }
        public ActionResult ExportToExcel(string ID)
        {
            if (ID != null)
            {

                var congtrinh = _db.CongTrinhs.Where(i => i.MaCT.Equals(ID)).Select(i => new CongTrinhViewModel(i)).FirstOrDefault();
                var hangmuc = _db.HangMucs.Where(i => i.MaCT.Equals(ID)).Select(i => new HangMucViewModel(i)).ToList();
                var mahangmucs = _db.HangMucs.Where(i => i.MaCT.Equals(ID)).Select(i => i.MaHM).ToList();
                List<string> mahangmucs1 = _db.HangMucs.Where(i => i.MaCT.Equals(ID)).Select(i => i.MaHM).ToList();
                var congviec = _db.CongViecs.Where(i => mahangmucs1.Contains(i.MaHM)).Select(i => new CongViec_User_ViewModel(i)).ToList();
                List<string> macongviecs = _db.CongViecs.Where(i => mahangmucs.Contains(i.MaHM)).Select(i => i.MaHieuCV_User).ToList();
                var haophi = _db.ThanhPhanHaoPhis.Where(i => macongviecs.Contains(i.MaHieuCV_User)).Select(i => new HaoPhi_User_ViewModel(i)).ToList();
                //var congtrinh = _db.CongTrinhs.Where(i => i.MaCT.Equals(ID)).Select(i => new CongTrinhViewModel(i)).FirstOrDefault();

                //var hangmuc = _db.HangMucs.Where(i => i.MaCT.Equals(ID)).Select(i => new HangMucViewModel(i)).ToList();
                //var mahangmucs = _db.HangMucs.Where(i => i.MaCT.Equals(ID)).Select(i => i.MaHM).ToList();
                var hangmuccout = hangmuc.Count;
                //var congviec = _db.CongViecs.Where(i =>i.HangMuc.Equals(mahangmucs)).Select(i => new CongViec_User_ViewModel(i)).ToList();
                //var macongviecs = _db.CongViecs.Where(i => i.HangMuc.Equals(mahangmucs)).Select(i => i.MaHieuCV_User).ToList();
                var congvieccout = congviec.Count;
                //var haophi = _db.ThanhPhanHaoPhis.Where(i =>i.MaHieuCV_User.Equals(macongviecs)).Select(i => new HaoPhi_User_ViewModel(i)).ToList();
                var haophicout = haophi.Count;

                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Hạng mục");
                    ExcelWorksheet ws1 = pck.Workbook.Worksheets.Add("Công việc");
                    ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("Thành phần hao phí");

                    // ADD dữ liệu cho sheet hạng mục
                    ws.Cells["I5"].Value = "BẢNG DỰ TOÁN HẠNG MỤC CÔNG TRÌNH";
                    ws.Cells["J7"].Value = "Tên công trình;" + "   " + congtrinh.TenCT;

                    // table  
                    ws.Cells["B8"].Value = "Mã hạng mục";
                    ws.Cells["B8:D8"].Merge = true;

                    ws.Cells["E8"].Value = "Mã công trình";
                    ws.Cells["E8:G8"].Merge = true;

                    ws.Cells["H8"].Value = "Tên hạng mục";
                    ws.Cells["H8:M8"].Merge = true;

                    ws.Cells["N8"].Value = "Mô tả";
                    ws.Cells["N8:Q8"].Merge = true;

                    ws.Cells["R8"].Value = "Đơn giá";
                    ws.Cells["R8:T8"].Merge = true;
                    var dong = (hangmuccout + 3 + 8).ToString();
                    var dong1 = "H" + dong;
                    var dong2 = "I" + dong;

                    ws.Cells[dong1].Value = "Tổng tiền công trình";

                    //
                    var r = 9;
                    foreach (var row in hangmuc)
                    {

                        var A = "A" + r;
                        var B = "B" + r;
                        var C = "C" + r;
                        var D = "D" + r;
                        var E = "E" + r;
                        var F = "F" + r;
                        var G = "G" + r;
                        var H = "H" + r;
                        var N = "N" + r;
                        var R = "R" + r;
                        var U = "U" + r;

                        //  content

                        ws.Cells[B].Value = row.MaHM;
                        ws.Cells[E].Value = row.MaCT;
                        ws.Cells[H].Value = row.TenHM;
                        ws.Cells[N].Value = row.MoTa;
                        ws.Cells[R].Formula = "+SUMIFS('Công việc'!U9:U" + (congvieccout + 1 + 9) + ",'Công việc'!D9:D" + (congvieccout + 1 + 9) + ",B" + r + "" + ")";
                        ws.Cells[dong2].Formula = "+SUM(R9:R" + (hangmuccout + 1 + 9) + ")";
                        r++;
                    }


                    var A2 = "A" + (r + 1);
                    var B2 = "B" + (r + 1);
                    var C2 = "A" + (r + 2);
                    var D2 = "B" + (r + 2);
                    // add dữ liệu cho sheet công việc
                    ws1.Cells["I6"].Value = "DANH SÁCH CÔNG VIỆC THUỘC HẠNG MỤC";
                    ws1.Cells["A8"].Value = "Mã công việc- người dùng";
                    ws1.Cells["A8:C8"].Merge = true;

                    ws1.Cells["D8"].Value = "Mã hạng mục";
                    ws1.Cells["D8:E8"].Merge = true;

                    ws1.Cells["F8"].Value = "Mã công việc- định mức";
                    ws1.Cells["F8:H8"].Merge = true;

                    ws1.Cells["I8"].Value = "Tên công việc";
                    ws1.Cells["I8:J8"].Merge = true;

                    ws1.Cells["K8"].Value = "Đơn vị";
                    ws1.Cells["K8:L8"].Merge = true;

                    ws1.Cells["M8"].Value = "Khối lượng";
                    ws1.Cells["M8:N8"].Merge = true;

                    ws1.Cells["O8"].Value = "Giá vật liệu";
                    ws1.Cells["O8:P8"].Merge = true;

                    ws1.Cells["Q8"].Value = "Giá nhân công";
                    ws1.Cells["Q8:R8"].Merge = true;

                    ws1.Cells["S8"].Value = "Giá máy thi công";
                    ws1.Cells["S8:T8"].Merge = true;


                    ws1.Cells["U8"].Value = "Thành tiền";
                    ws1.Cells["U8:V8"].Merge = true;
                    var r1 = 9;
                    foreach (var row in congviec)
                    {

                        var A = "A" + r1;
                        var B = "B" + r1;
                        var C = "C" + r1;
                        var D = "D" + r1;
                        var E = "E" + r1;
                        var F = "F" + r1;
                        var G = "G" + r1;
                        var H = "H" + r1;
                        var I = "I" + r1;
                        var K = "K" + r1;
                        var L = "L" + r1;
                        var N = "O" + r1;
                        var M = "M" + r1;
                        var O = "O" + r1;
                        var P = "P" + r1;
                        var Q = "Q" + r1;
                        var R = "R" + r1;
                        var S = "S" + r1;
                        var T = "T" + r1;
                        var U = "U" + r1;
                        var V = "V" + r1;



                        //  content

                        ws1.Cells[A].Value = row.MaHieuCV_User;
                        ws1.Cells[D].Value = row.MaHM;
                        ws1.Cells[F].Value = row.MaHieuCV_DM;
                        ws1.Cells[I].Value = row.TenCongViec;
                        ws1.Cells[K].Value = row.DonVi;
                        ws1.Cells[M].Value = row.KhoiLuong;
                        // chỉnh lại công thức excel
                        ws1.Cells[O].Value = row.GiaVL;
                        ws1.Cells[Q].Value = row.GiaNC;
                        ws1.Cells[S].Value = row.GiaMTC;
                        ws1.Cells[U].Formula = "+PRODUCT(SUM(O" + r1 + ":S" + r1 + "),M" + r1 + ")";
                        r1++;
                    }
                    //add dữ liệu cho sheet thành phần hao phí
                    // table 
                    ws2.Cells["I5"].Value = "DANH MỤC THÀNH PHẦN HAO PHÍ";
                    ws2.Cells["B8"].Value = "Mã hạng mục";
                    ws2.Cells["B8:D8"].Merge = true;

                    ws2.Cells["E8"].Value = "Mã hiệu công việc- user";
                    ws2.Cells["E8:G8"].Merge = true;

                    ws2.Cells["H8"].Value = "Tên thành phần";
                    ws2.Cells["H8:M8"].Merge = true;

                    ws2.Cells["N8"].Value = "Đơn vị";
                    ws2.Cells["N8:Q8"].Merge = true;

                    ws2.Cells["R8"].Value = "Đơn giá";
                    ws2.Cells["R8:T8"].Merge = true;

                    //
                    var r2 = 9;
                    foreach (var row in haophi)
                    {

                        var A = "A" + r2;
                        var B = "B" + r2;
                        var C = "C" + r2;
                        var D = "D" + r2;
                        var E = "E" + r2;
                        var F = "F" + r2;
                        var G = "G" + r2;
                        var H = "H" + r2;
                        var N = "N" + r2;
                        var R = "R" + r2;

                        //  content

                        ws2.Cells[B].Value = row.MaHP;
                        ws2.Cells[E].Value = row.MaHieuCV_User;
                        ws2.Cells[H].Value = row.Ten;
                        ws2.Cells[N].Value = row.DonVi;
                        ws2.Cells[R].Value = row.Gia;
                        r2++;
                    }


                    Byte[] fileBytes = pck.GetAsByteArray();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=DuToanXayDung.xlsx");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    StringWriter sw = new StringWriter();
                    Response.BinaryWrite(fileBytes);
                    Response.End();
                }

                return RedirectToAction("Index");

            }
            return View();
        }
    }
}