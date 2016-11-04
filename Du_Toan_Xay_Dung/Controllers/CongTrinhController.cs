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

            ViewData["List_CongTrinh"] = _db.Buildings.Where(i => i.Email.Equals(SessionHandler.User.Email)).Select(i => new BuildingViewModel(i)).ToList();
            //ViewData["List_CongTrinh_Null"] = _db.Buildings.Where(i => i.Email.Equals(SessionHandler.User.Email) && !i.BuildingItems.Any(o => o.ID.Equals(i.ID))).Select(i => i.ID).ToList();
            //ViewData["list_hinhanh"] = _db.Images_Urls.Select(i => new Images_CongTrinhViewModel(i)).ToList();
            return View();
        }
        [PageLogin]
        public JsonResult get_datafile(string ID)
        {
            var data = _db.Images_Urls.Where(i => i.Building_ID.Equals(ID)).Select(i => i.Url).ToList();
            return Json(data);
        }

        [PageLogin]
        public ActionResult BuildingItem()
        {
            ViewData["List_CongTrinh"] = _db.Buildings.Where(i => i.Email.Equals(SessionHandler.User.Email)).Select(i => new BuildingViewModel(i)).ToList();
            ViewData["List_HangMuc"] = _db.BuildingItems.Select(i => new HangMucViewModel(i)).ToList();

            return View();
        }
        public JsonResult Get_AllHangMuc()
        {
            var list = _db.Buildings.Join(_db.BuildingItems, bid => bid.ID, biid => biid.Building_ID, (bid, biid) => new
            {
                Building = bid,
                BuildingItem = biid
            }).Where(i => i.Building.Email.Equals(SessionHandler.User.Email)).Select(i => new
            {
                ID = i.BuildingItem.ID,
                Building_ID = i.BuildingItem.Building_ID,
                Name = i.BuildingItem.Name,
                Description = i.BuildingItem.Description,
                Sum = i.BuildingItem.Sum
            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_Allinf()
        {
            var list = _db.Buildings.Where(i => i.Email.Equals(SessionHandler.User.Email)).Select(i => new BuildingViewModel(i)).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [PageLogin]
        [HttpPost]
        public JsonResult Post_ThemCongTrinh(BuildingViewModel obj)
        {

            try
            {
                Building congtrinh = new Building();
                congtrinh.Email = SessionHandler.User.Email;
                congtrinh.Name = obj.Name;
                congtrinh.Description = obj.Description;
                congtrinh.Address = obj.Address;
                congtrinh.Sum = 0;
                _db.Buildings.InsertOnSubmit(congtrinh);
                _db.SubmitChanges();

                if (obj.img_congtrinh != null)
                {
                    foreach (var file in obj.img_congtrinh)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            //them image vao database
                            Images_Url image = new Images_Url();
                            image.Building_ID = congtrinh.ID;
                            image.Url = "/Images/CongTrinh/Building" + congtrinh.ID + "/" + file.FileName;
                            congtrinh.Images_Urls.Add(image);
                        }
                    }
                }

                _db.SubmitChanges();

                var building_id = congtrinh.ID;

                if (obj.img_congtrinh != null)
                {
                    if (obj.img_congtrinh.Count() > 0)
                    {
                        string url_location = Server.MapPath(@"~/Images/CongTrinh");
                        if (Directory.Exists(url_location))
                        {
                            foreach (var file in obj.img_congtrinh)
                            {
                                if (file != null && file.ContentLength > 0)
                                {
                                    string directory = Server.MapPath(@"~/Images/CongTrinh/Building" + Convert.ToString(building_id) + "/");
                                    string fileLocation = directory + file.FileName;
                                    Directory.CreateDirectory(directory);
                                    file.SaveAs(fileLocation);
                                }
                            }
                        }
                    }
                }

                return Json("ok");
            }
            catch (Exception e)
            {
                return Json("error");
            }
        }

        [PageLogin]
        public ActionResult UpdateCongTrinh(string ID)
        {
            if (ID == null)
            {
                ID = "";
            }
            ViewData["CongTrinh_Update"] = _db.Buildings.Where(i => i.ID.Equals(ID)).Select(i => new BuildingViewModel(i)).FirstOrDefault();
            ViewData["image_congtrinh"] = _db.Images_Urls.Where(a => a.Building_ID.Equals(ID)).Select(i => new Images_CongTrinhViewModel(i)).ToList();
            return View();
        }

        [PageLogin]
        [HttpPost]
        public JsonResult post_updatecongtrinh(BuildingViewModel obj)
        {
            try
            {
                var congtrinh = _db.Buildings.Where(i => i.ID.Equals(obj.ID)).FirstOrDefault();

                if (congtrinh != null)
                {
                    if (obj.img_old != null)
                    {
                        if (obj.img_old.Count() > 0)
                        {
                            foreach (var item in obj.img_old)
                            {
                                var image_old = _db.Images_Urls.Where(i => i.ID.Equals(item)).FirstOrDefault();
                                _db.Images_Urls.DeleteOnSubmit(image_old);
                            }
                        }
                    }

                    if (obj.img_congtrinh != null)
                    {
                        if (obj.img_congtrinh.Count() > 0)
                        {
                            string directory = Server.MapPath(@"~/Images/CongTrinh/Building" + Convert.ToString(obj.ID) + "/");
                            if (Directory.Exists(directory))
                            {
                                foreach (var file in obj.img_congtrinh)
                                {
                                    if (file != null && file.ContentLength > 0)
                                    {
                                        string fileLocation = directory + file.FileName;
                                        file.SaveAs(fileLocation);

                                        //them image vao database
                                        var image = new Images_Url();
                                        image.Building_ID = obj.ID;
                                        image.Url = "/Images/CongTrinh/Building" + congtrinh.ID + "/" + file.FileName;
                                        _db.Images_Urls.InsertOnSubmit(image);
                                    }
                                }
                            }
                        }
                    }

                    congtrinh.ID = obj.ID;
                    congtrinh.Name = obj.Name;
                    congtrinh.Description = obj.Description;
                    congtrinh.Address = obj.Address;
                    congtrinh.Sum = obj.Sum;

                    _db.SubmitChanges();
                }
                return Json("ok");
            }
            catch (Exception e)
            {
                return Json("error");
            }
        }

        [PageLogin]
        public JsonResult Delete_CongTrinh(string building_id)
        {
            try
            {
                var congtrinh = _db.Buildings.FirstOrDefault(i => i.ID.Equals(building_id));
                if (congtrinh != null)
                {
                    var img_congtrinh = _db.Images_Urls.Where(i => i.Building_ID.Equals(building_id)).ToList();
                    var hangmuc = _db.BuildingItems.Where(i => i.Building_ID.Equals(building_id)).ToList();
                    _db.Images_Urls.DeleteAllOnSubmit(img_congtrinh);

                    string location = Server.MapPath(@"~/Images/CongTrinh/Building" + Convert.ToString(building_id) + "/");
                    DirectoryInfo directory = new DirectoryInfo(location);
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        file.Delete();
                    }
                    directory.Delete(true);

                    if (hangmuc.Count != 0)
                    {
                        foreach (var item in hangmuc)
                        {
                            var userworks = _db.UserWorks.Where(i => i.BuildingItem_ID.Equals(item.ID)).ToList();
                            var userwork_resources = _db.UserWork_Resources.Where(i => i.BuildingItem_ID.Equals(item.ID)).ToList();
                            _db.UserWorks.DeleteAllOnSubmit(userworks);
                            _db.UserWork_Resources.DeleteAllOnSubmit(userwork_resources);
                        }
                    }
                    _db.BuildingItems.DeleteAllOnSubmit(hangmuc);
                    _db.Buildings.DeleteOnSubmit(congtrinh);

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



        [PageLogin]
        [HttpPost]
        public JsonResult post_updatehangmuc(HangMucViewModel obj)
        {
            try
            {
                var hangmuc = _db.BuildingItems.Where(i => i.ID.Equals(obj.ID)).FirstOrDefault();
                hangmuc.Building_ID = obj.Building_ID;
                hangmuc.ID = obj.ID;
                hangmuc.Description = obj.Description;
                hangmuc.Name = obj.Name;
                hangmuc.Sum = Convert.ToDecimal(obj.Sum);
                _db.SubmitChanges();
                return Json("ok");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        [PageLogin]
        public JsonResult Delete_HangMuc(HangMucViewModel obj)
        {
            //try
            //{
            var hangmuc = _db.BuildingItems.Single(i => i.ID.Equals(obj.ID));
                if (hangmuc != null)
                {
                    var userwork_resources = _db.UserWork_Resources.Where(i => i.BuildingItem_ID.Equals(obj.ID));
                    var userwork = _db.UserWorks.Where(i => i.BuildingItem_ID.Equals(obj.ID));
                    if (userwork_resources != null)
                        _db.UserWork_Resources.DeleteAllOnSubmit(userwork_resources);
                    if (userwork != null)
                        _db.UserWorks.DeleteAllOnSubmit(userwork);
                    _db.BuildingItems.DeleteOnSubmit(hangmuc);
                }
                _db.SubmitChanges();
                return Json("ok");
            //}
            //catch (Exception)
            //{
               // return Json("error");
            //}
        }

        [PageLogin]
        [HttpPost]
        public JsonResult post_themhangmuc(HangMucViewModel obj)
        {
            try
            {

                BuildingItem hm = new BuildingItem();
                hm.Building_ID = obj.Building_ID;
                hm.Name = obj.Name;
                hm.Description = obj.Description;
                hm.Sum = 0;
                _db.BuildingItems.InsertOnSubmit(hm);
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
                var congtrinh = _db.Buildings.Where(i => i.ID.Equals(ID)).Select(i => new BuildingViewModel(i)).FirstOrDefault();
                var hangmuc = _db.BuildingItems.Where(i => i.Building_ID.Equals(ID)).Select(i => new HangMucViewModel(i)).ToList();
                //var mahangmucs = _db.BuildingItems.Where(i => i.Building_ID.Equals(ID)).Select(i => i.ID).ToList();
                List<long> mahangmucs1 = _db.BuildingItems.Where(i => i.Building_ID.Equals(ID)).Select(i => i.ID).ToList();
                var congviec = _db.UserWorks.Where(i => mahangmucs1.Contains(i.BuildingItem_ID)).Select(i => new UserWorkViewModel(i)).ToList();
                List<string> macongviecs = _db.UserWorks.Where(i => mahangmucs1.Contains(i.BuildingItem_ID)).Select(i => i.NormWork_ID).ToList();
                var haophi = _db.NormDetails.Where(i => macongviecs.Contains(i.NormWork_ID)).Select(i => new DonGiaChiTiet_DM_ViewModel(i)).ToList();
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
                    ws.Cells["J7"].Value = "Tên công trình;" + "   " + congtrinh.Name;

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

                        ws.Cells[B].Value = row.ID;
                        ws.Cells[E].Value = row.Building_ID;
                        ws.Cells[H].Value = row.Name;
                        ws.Cells[N].Value = row.Description;
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

                        ws1.Cells[A].Value = row.ID;
                        ws1.Cells[D].Value = row.BuildingItem_ID;
                        ws1.Cells[F].Value = row.NormWork_ID;
                        ws1.Cells[I].Value = row.Name;
                        ws1.Cells[K].Value = row.Unit;
                        ws1.Cells[M].Value = row.Number;
                        // chỉnh lại công thức excel
                        ws1.Cells[O].Value = row.SumMaterial;
                        ws1.Cells[Q].Value = row.SumLabor;
                        ws1.Cells[S].Value = row.SumMachine;
                        ws1.Cells[U].Formula = "+PRODUCT(SUM(O" + r1 + ":S" + r1 + "),M" + r1 + ")";
                        r1++;
                    }
                    //add dữ liệu cho sheet thành phần hao phí
                    // table 
                    ws2.Cells["I5"].Value = "DANH MỤC THÀNH PHẦN HAO PHÍ";

                    ws2.Cells["E8"].Value = "Mã hiệu công việc- user";
                    ws2.Cells["E8:G8"].Merge = true;

                    ws2.Cells["H8"].Value = "Mã thành phần";
                    ws2.Cells["H8:M8"].Merge = true;

                    ws2.Cells["N8"].Value = "Số lượng";
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


                        ws2.Cells[E].Value = row.NormWork_ID;
                        ws2.Cells[H].Value = row.ID;
                        ws2.Cells[N].Value = row.Numbers;
                        ws2.Cells[R].Value = row.UnitPrice_ID;
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