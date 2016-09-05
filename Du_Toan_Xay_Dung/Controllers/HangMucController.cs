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
        //
        // GET: /TT_D_KLCongViec/

        public ActionResult Estimate_Work()
        {
            return View();
        }


        /*
        public ActionResult Index_2(string ID)
        {

            if (ID != null)
            {
                int d = ID.IndexOf(',');
                if (d == -1)
                {
                    ViewData["CongTrinh"] = _db.Buildings.Where(i => i.ID.Equals(ID)).Select(i => new BuildingViewModel(i)).FirstOrDefault();
                }
                else
                {
                    var Arr_ID = ID.Split(',');
                    ViewData["CongTrinh"] = _db.Buildings.Where(i => i.MaCT.Equals(Arr_ID[0])).Select(i => new BuildingViewModel(i)).FirstOrDefault();
                    ViewData["HangMuc_ChiTiet"] = _db.CongViecs.Where(i => i.MaHM.Equals(Arr_ID[1])).Select(i => new CongViec_User_ViewModel(i)).ToList();
                    ViewData["HangMuc"] = _db.HangMucs.Where(i => i.MaHM.Equals(Arr_ID[1])).Select(i => new HangMucViewModel(i)).FirstOrDefault();
                }
            }

            if (SessionHandler.User != null)
            {
                ViewData["DSCongTrinh"] = _db.CongTrinhs.Where(i => i.Email.Equals(SessionHandler.User.Email)).Select(i => new BuildingViewModel(i)).ToList();
            }


            ViewData["DSDinhMuc"] = _db.DinhMucs.Select(i => new DinhMucViewModel(i)).ToList();
            ViewData["DSDonGia"] = _db.DonGias.Select(i => new DonGiaViewModel(i)).ToList();

            return View();
        }

        //[PageLogin]
        public ActionResult Index(string ID)
        {
            if (ID != null)
            {
                int d = ID.IndexOf(',');
                if (d == -1)
                {
                    ViewData["CongTrinh"] = _db.CongTrinhs.Where(i => i.MaCT.Equals(ID)).Select(i => new BuildingViewModel(i)).FirstOrDefault();
                }
                else
                {
                    var Arr_ID = ID.Split(',');
                    ViewData["CongTrinh"] = _db.CongTrinhs.Where(i => i.MaCT.Equals(Arr_ID[0])).Select(i => new BuildingViewModel(i)).FirstOrDefault();
                    ViewData["CongViec_User"] = _db.CongViecs.Where(i => i.MaHM.Equals(Arr_ID[1])).Select(i => new CongViec_User_ViewModel(i)).ToList();
                    ViewData["HangMuc"] = _db.HangMucs.Where(i => i.MaHM.Equals(Arr_ID[1])).Select(i => new HangMucViewModel(i)).FirstOrDefault();
                }
            }

            if (SessionHandler.User != null)
            {
                ViewData["DSCongTrinh"] = _db.CongTrinhs.Where(i => i.Email.Equals(SessionHandler.User.Email)).Select(i => new BuildingViewModel(i)).ToList();
            }


            ViewData["DSDinhMuc"] = _db.DinhMucs.Select(i => new DinhMucViewModel(i)).ToList();
            //ViewData["DSDonGia"] = _db.DonGias.Select(i => new DonGiaViewModel(i)).Take(10).ToList();
            ViewData["KhuVucDG"] = _db.DonGias.GroupBy(i => i.MaKV).Select(g => g.First().MaKV).ToList();

            return View();
        }
         */
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
            var list = _db.NormDetails.Join(_db.UnitPrices, nd => nd.UnitPrice_ID, up => up.ID, (nd, up) => new {
                NormDetails = nd, UnitPrices =up})
                .Join(_db.UnitPrice_Areas, up=>up.UnitPrices.ID, upa=>upa.UnitPrice_ID, (up,upa) => new {
                    UnitPrices = up, UnitPrice_Areas = upa})
                    .Select( s => new {
                        Key_NormWork = s.UnitPrices.NormDetails.NormWork_ID,
                        Key_Material = s.UnitPrices.UnitPrices.ID,
                        Number = s.UnitPrices.NormDetails.Numbers,
                        Unit = s.UnitPrices.UnitPrices.Unit,
                        Name_Material = s.UnitPrices.UnitPrices.Name,
                        Price = s.UnitPrice_Areas.Price
                    });

            

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDSCongViec(string mahangmuc)
        {
            var congviecs = _db.CongViecs.Where(i => i.MaHM.Equals(mahangmuc)).Select(i => i.MaHieuCV_User).ToList();
            var list = _db.ThanhPhanHaoPhis.Where(i => congviecs.Contains(i.MaHieuCV_User)).Select(i => new HaoPhi_DM_ViewModel()
            {
                MaHP = i.MaHP,
                MaHieuCV_User = i.MaHieuCV_User,
                MaHieuCV_DM = i.CongViec.MaHieuCV_DM,
                Ten = i.Ten,
                DonVi = i.DonVi,
                SoLuong_DM = i.SoLuong_DM,
                Gia = i.Gia
            }).ToList();
            return Json(list);
        }

        /*
        [HttpPost]
        public JsonResult getPrice_Norm(string idDinhMuc)
        {
            var Arr_id = idDinhMuc.Split(',');
            List<All_PriceMaterialViewModel> list = new List<All_PriceMaterialViewModel>();

            foreach(var id in Arr_id)
            {
                decimal totalGiaVL = 0;
                decimal totalGiaNC = 0;
                decimal totalGiaMay = 0;

                var giaVL = _db.ChiTiet_DinhMucs
                    .Where(i => i.MaHieuCV_DM.Equals(id) && i.MaVL_NC_MTC.Contains("V")).ToList();
                foreach (var item in giaVL)
                {
                    var _temGia = _db.DonGias.Where(i => i.MaVL_NC_MTC.Equals(item.MaVL_NC_MTC)).FirstOrDefault();
                    totalGiaVL += _temGia.Gia * item.SoLuong;
                }
                var giaNC = _db.ChiTiet_DinhMucs.Where(i => i.MaHieuCV_DM.Equals(id) && i.MaVL_NC_MTC.Contains("N")).ToList();
                foreach (var item in giaNC)
                {
                    var _temGia = _db.DonGias.Where(i => i.MaVL_NC_MTC.Equals(item.MaVL_NC_MTC)).FirstOrDefault();
                    totalGiaNC += _temGia.Gia * item.SoLuong;
                }

                var giaMay = _db.ChiTiet_DinhMucs.Where(i => i.MaHieuCV_DM.Equals(id) && i.MaVL_NC_MTC.Contains("M")).ToList();
                foreach (var item in giaMay)
                {
                    var _temGia = _db.DonGias.Where(i => i.MaVL_NC_MTC.Equals(item.MaVL_NC_MTC)).FirstOrDefault();
                    totalGiaMay += _temGia.Gia * item.SoLuong;
                }
                list.Add(new All_PriceMaterialViewModel(id,totalGiaVL,totalGiaNC,totalGiaMay));

            }
            /*
            var list_haophi = _db.ChiTiet_DinhMucs.Where(i => i.MaHieuCV_DM.Equals(idDinhMuc)).Select(i => new HaoPhi_DM_ViewModel()
            {
                MaHP = i.MaVL_NC_MTC,
                MaHieuCV_DM = i.MaHieuCV_DM,
                Ten = i.DonGia.Ten,
                DonVi = i.DonGia.DonVi,
                SoLuong_DM = i.SoLuong,
                Gia = i.DonGia.Gia
            }).ToList();
            
            return Json(list);
        }
    */
        public ActionResult DonGiaChiTiet(string ID)
        {
            if (ID != null)
            {
                var Arr_ID = ID.Split(',');
                ViewData["List_DinhMuc"] = _db.DinhMucs.Where(i => Arr_ID.Contains(i.MaHieuCV_DM)).Select(i => new DinhMucViewModel(i)).ToList();
                ViewData["DonGiaVL"] = _db.ChiTiet_DinhMucs.Where(i => Arr_ID.Contains(i.MaHieuCV_DM) && i.MaVL_NC_MTC.Contains("V"))
                                                .Select(i => new DonGiaChiTiet_DM_ViewModel(i)).ToList();

                ViewData["DonGiaNC"] = _db.ChiTiet_DinhMucs.Where(i => Arr_ID.Contains(i.MaHieuCV_DM) && i.MaVL_NC_MTC.Contains("N"))
                                                .Select(i => new DonGiaChiTiet_DM_ViewModel(i)).ToList();
                ViewData["DonGiaMTC"] = _db.ChiTiet_DinhMucs.Where(i => Arr_ID.Contains(i.MaHieuCV_DM) && i.MaVL_NC_MTC.Contains("M"))
                                                .Select(i => new DonGiaChiTiet_DM_ViewModel(i)).ToList();
            }
            return View();
        }

        //co the thay doi
        public ActionResult ChiTiet_VL_NC_MTC(string ID)
        {
            if (ID != null)
            {
                string temp = ID.Substring(0, 3);
                if (temp != "CV_")
                {
                    ViewData["List_VL"] = _db.ChiTiet_DinhMucs.Where(c => c.MaHieuCV_DM.Equals(ID) && c.MaVL_NC_MTC.Contains("V")).Select(c => new HaoPhi_DM_ViewModel()
                    {
                        MaHP = c.MaVL_NC_MTC,
                        Ten = c.DonGia.Ten,
                        DonVi = c.DonGia.DonVi,
                        Gia = c.DonGia.Gia * c.SoLuong
                    }).ToList();

                    ViewData["List_NC"] = _db.ChiTiet_DinhMucs.Where(c => c.MaHieuCV_DM.Equals(ID) && c.MaVL_NC_MTC.Contains("N")).Select(c => new HaoPhi_DM_ViewModel()
                    {
                        MaHP = c.MaVL_NC_MTC,
                        Ten = c.DonGia.Ten,
                        DonVi = c.DonGia.DonVi,
                        Gia = c.DonGia.Gia * c.SoLuong
                    }).ToList();

                    ViewData["List_MTC"] = _db.ChiTiet_DinhMucs.Where(c => c.MaHieuCV_DM.Equals(ID) && c.MaVL_NC_MTC.Contains("M")).Select(c => new HaoPhi_DM_ViewModel()
                    {
                        MaHP = c.MaVL_NC_MTC,
                        Ten = c.DonGia.Ten,
                        DonVi = c.DonGia.DonVi,
                        Gia = c.DonGia.Gia * c.SoLuong
                    }).ToList();
                }
                else
                {
                    ViewData["List_VL"] = _db.ThanhPhanHaoPhis.Where(i => i.MaHieuCV_User.Equals(ID)).Where(i => i.MaHP.Contains("V_HP")).Select(i => new HaoPhi_User_ViewModel(i)).ToList();

                    ViewData["List_NC"] = _db.ThanhPhanHaoPhis.Where(i => i.MaHieuCV_User.Equals(ID)).Where(i => i.MaHP.Contains("N_HP")).Select(i => new HaoPhi_User_ViewModel(i)).ToList();

                    ViewData["List_MTC"] = _db.ThanhPhanHaoPhis.Where(i => i.MaHieuCV_User.Equals(ID)).Where(i => i.MaHP.Contains("M_HP")).Select(i => new HaoPhi_User_ViewModel(i)).ToList();
                }

                ViewData["ID_MaHieuCV"] = ID;

            }
            return View();
        }


        [HttpPost]
        public JsonResult Post_ChiTiet_VL_NC_MTC(Update_HaoPhi_User model)
        {
            try
            {

                var Arr_mahp = model.MaHP.Split(',');
                var Arr_gia = model.Gia.Split(',');
                decimal tong_gia_vl = 0;
                decimal tong_gia_nc = 0;
                decimal tong_gia_mtc = 0;
                string mahieucv_user = "";
                string mahm = "";
                for (int j = 0; j < Arr_mahp.Length; j++)
                {
                    var haophi = _db.ThanhPhanHaoPhis.Single(i => i.MaHP.Equals(Arr_mahp[j]));
                    if (haophi != null)
                    {
                        haophi.Gia = Convert.ToDecimal(Arr_gia[j]);
                        if (haophi.MaHP.ToString().Substring(0, 1) == "V")
                        {
                            tong_gia_vl = tong_gia_vl + Convert.ToDecimal(Arr_gia[j]);
                        }
                        if (haophi.MaHP.ToString().Substring(0, 1) == "N")
                        {
                            tong_gia_nc = tong_gia_nc + Convert.ToDecimal(Arr_gia[j]);
                        }
                        if (haophi.MaHP.ToString().Substring(0, 1) == "M")
                        {
                            tong_gia_mtc = tong_gia_mtc + Convert.ToDecimal(Arr_gia[j]);
                        }
                        mahieucv_user = haophi.MaHieuCV_User;
                    }
                }

                decimal gia_cv_last = 0;                //get gia cong viec cu
                var congviec = _db.CongViecs.Single(i => i.MaHieuCV_User.Equals(mahieucv_user));
                if (congviec != null)
                {
                    gia_cv_last = congviec.ThanhTien;
                    congviec.GiaVL = tong_gia_vl;
                    congviec.GiaNC = tong_gia_nc;
                    congviec.GiaMTC = tong_gia_mtc;
                    congviec.ThanhTien = tong_gia_vl + tong_gia_nc + tong_gia_mtc;
                    mahm = congviec.MaHM;
                }

                string mact = "";
                decimal gia_hm_last = 0;
                decimal gia_hm_now = 0;
                var hangmuc = _db.HangMucs.Single(i => i.MaHM.Equals(mahm));
                if (hangmuc != null)
                {
                    gia_hm_last = hangmuc.Gia;
                    hangmuc.Gia = (hangmuc.Gia - gia_cv_last) + tong_gia_vl + tong_gia_nc + tong_gia_mtc;
                    mact = hangmuc.MaCT;
                    gia_hm_now = hangmuc.Gia;
                }


                var congtrinh = _db.CongTrinhs.Single(i => i.MaCT.Equals(mact));
                if (congtrinh != null)
                {
                    congtrinh.Gia = (congtrinh.Gia - gia_hm_last) + gia_hm_now;
                }

                _db.SubmitChanges();
                return Json("ok");
            }
            catch (Exception e)
            {
                return Json("error");
            }

        }



        [HttpPost]
        public JsonResult Post_CongViec(Submit_CongViec_User_ViewModel model)
        {
            return Json("ok");
        }

        [HttpPost]
        public ActionResult FormSubmit(FormCollection form)
        {

            if (SessionHandler.User == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var txtmacongtrinh = form["txt_ma_congtrinh"];
            var txtmahangmuc = form["txt_ma_hangmuc"];
            var txttenhangmuc = form["txt_ten_hangmuc"];
            var txtmahieucv_dmArr = form["txtmahieucv_dm[]"].Split(',');
            var txttencvArr = form["txttencv[]"].Split(',');
            var txtdonviArr = form["txtdonvi[]"].Split(',');
            var txtkhoiluongArr = form["txtkhoiluong[]"].Split(',');
            var txtgiavlArr = form["txtgiavl[]"].Split(',');
            var txtgiancArr = form["txtgianc[]"].Split(',');
            var txtgiamtcArr = form["txtgiamtc[]"].Split(',');
            var txtthanhtiencArr = form["txtthanhtien[]"].Split(',');
            var txttongtien = form["txt_tongtien"];


            if (txtmacongtrinh != "")
            {
                if (txtmahangmuc == "")                                     //tao hang muc, bao gom cong viec moi
                {
                    //lay dong cuoi cung bang hangmuc
                    var idhm_last = _db.HangMucs.OrderByDescending(i => i.Id).Select(i => i.Id).FirstOrDefault();
                    idhm_last = idhm_last + 1;
                    HangMuc hm = new HangMuc();
                    hm.Id = idhm_last;
                    hm.MaHM = "HM" + idhm_last.ToString();
                    hm.MaCT = txtmacongtrinh;
                    hm.TenHM = txttenhangmuc;
                    hm.MoTa = "";
                    hm.Gia = Convert.ToDecimal(txttongtien);

                    _db.HangMucs.InsertOnSubmit(hm);

                    //lay dong cuoi cung bang congviec
                    var idcv_last = _db.CongViecs.OrderByDescending(i => i.Id).Select(i => i.Id).FirstOrDefault();
                    idcv_last = idcv_last + 1;

                    //lay dong cuoi cung bang thanhphanhaophi
                    var idhp_last = _db.ThanhPhanHaoPhis.OrderByDescending(o => o.Id).Select(o => o.Id).FirstOrDefault();
                    idhp_last = idhp_last + 1;
                    var mahp_now = (dynamic)null;

                    for (int i = 0; i < txtmahieucv_dmArr.Length; i++)
                    {
                        CongViec cv_user = new CongViec();
                        cv_user.Id = idcv_last;
                        cv_user.MaHM = "HM" + idhm_last.ToString();
                        cv_user.MaHieuCV_User = "CV_" + idcv_last.ToString();
                        cv_user.MaHieuCV_DM = txtmahieucv_dmArr[i];
                        cv_user.TenCongViec = txttencvArr[i];
                        cv_user.DonVi = txtdonviArr[i];
                        cv_user.KhoiLuong = Convert.ToDecimal(txtkhoiluongArr[i]);
                        cv_user.GiaVL = Convert.ToDecimal(txtgiavlArr[i]);
                        cv_user.GiaNC = Convert.ToDecimal(txtgiancArr[i]);
                        cv_user.GiaMTC = Convert.ToDecimal(txtgiamtcArr[i]);
                        cv_user.ThanhTien = Convert.ToDecimal(txtthanhtiencArr[i]);

                        //insert thanh phan hao phi
                        var list_haophi = _db.ChiTiet_DinhMucs.Where(c => c.MaHieuCV_DM.Equals(cv_user.MaHieuCV_DM)).Select(c => new HaoPhi_DM_ViewModel()
                        {
                            MaHP = c.MaVL_NC_MTC,
                            MaHieuCV_DM = c.MaHieuCV_DM,
                            Ten = c.DonGia.Ten,
                            DonVi = c.DonGia.DonVi,
                            SoLuong_DM = c.SoLuong,
                            Gia = c.DonGia.Gia
                        }).ToList();

                        foreach (var k in list_haophi)
                        {
                            if (k.MaHP.ToString().Substring(0, 1) == "V")
                            {
                                mahp_now = "V_HP" + idhp_last.ToString();
                            }
                            if (k.MaHP.ToString().Substring(0, 1) == "N")
                            {
                                mahp_now = "N_HP" + idhp_last.ToString();
                            }
                            if (k.MaHP.ToString().Substring(0, 1) == "M")
                            {
                                mahp_now = "M_HP" + idhp_last.ToString();
                            }
                            ThanhPhanHaoPhi hp = new ThanhPhanHaoPhi();
                            hp.Id = idhp_last;
                            hp.MaHP = mahp_now;
                            hp.MaHieuCV_User = "CV_" + idcv_last.ToString();
                            hp.Ten = k.Ten;
                            hp.DonVi = k.DonVi;
                            hp.SoLuong_DM = k.SoLuong_DM;
                            hp.Gia = k.Gia;

                            idhp_last = idhp_last + 1;

                            _db.ThanhPhanHaoPhis.InsertOnSubmit(hp);
                        }

                        idcv_last = idcv_last + 1;
                        _db.CongViecs.InsertOnSubmit(cv_user);
                    }
                }
                else
                {
                    //delete cong viec trong hang muc
                    var congviecs = _db.CongViecs.Where(i => i.MaHM.Equals(txtmahangmuc));
                    _db.CongViecs.DeleteAllOnSubmit(congviecs);

                    var macongviecs = _db.CongViecs.Where(i => i.MaHM.Equals(txtmahangmuc)).Select(i => i.MaHieuCV_User).ToList();
                    var haophis = _db.ThanhPhanHaoPhis.Where(i => macongviecs.Contains(i.MaHieuCV_User));
                    _db.ThanhPhanHaoPhis.DeleteAllOnSubmit(haophis);
                    _db.SubmitChanges();


                    //lay dong cuoi cung bang congviec
                    var idcv_last = _db.CongViecs.OrderByDescending(i => i.Id).Select(i => i.Id).FirstOrDefault();
                    idcv_last = idcv_last + 1;

                    //lay dong cuoi cung bang thanhphanhaophi
                    var idhp_last = _db.ThanhPhanHaoPhis.OrderByDescending(o => o.Id).Select(o => o.Id).FirstOrDefault();
                    idhp_last = idhp_last + 1;
                    var mahp_now = (dynamic)null;

                    for (int i = 0; i < txtmahieucv_dmArr.Length; i++)
                    {
                        CongViec cv_user = new CongViec();
                        cv_user.Id = idcv_last;
                        cv_user.MaHM = txtmahangmuc;
                        cv_user.MaHieuCV_User = "CV_" + idcv_last.ToString();
                        cv_user.MaHieuCV_DM = txtmahieucv_dmArr[i];
                        cv_user.TenCongViec = txttencvArr[i];
                        cv_user.DonVi = txtdonviArr[i];
                        cv_user.KhoiLuong = Convert.ToDecimal(txtkhoiluongArr[i]);
                        cv_user.GiaVL = Convert.ToDecimal(txtgiavlArr[i]);
                        cv_user.GiaNC = Convert.ToDecimal(txtgiancArr[i]);
                        cv_user.GiaMTC = Convert.ToDecimal(txtgiamtcArr[i]);
                        cv_user.ThanhTien = Convert.ToDecimal(txtthanhtiencArr[i]);

                        //insert thanh phan hao phi
                        var list_haophi = _db.ChiTiet_DinhMucs.Where(c => c.MaHieuCV_DM.Equals(cv_user.MaHieuCV_DM)).Select(c => new HaoPhi_DM_ViewModel()
                        {
                            MaHP = c.MaVL_NC_MTC,
                            MaHieuCV_DM = c.MaHieuCV_DM,
                            Ten = c.DonGia.Ten,
                            DonVi = c.DonGia.DonVi,
                            SoLuong_DM = c.SoLuong,
                            Gia = c.DonGia.Gia
                        }).ToList();

                        foreach (var k in list_haophi)
                        {
                            if (k.MaHP.ToString().Substring(0, 1) == "V")
                            {
                                mahp_now = "V_HP" + idhp_last.ToString();
                            }
                            if (k.MaHP.ToString().Substring(0, 1) == "N")
                            {
                                mahp_now = "N_HP" + idhp_last.ToString();
                            }
                            if (k.MaHP.ToString().Substring(0, 1) == "M")
                            {
                                mahp_now = "M_HP" + idhp_last.ToString();
                            }
                            ThanhPhanHaoPhi hp = new ThanhPhanHaoPhi();
                            hp.Id = idhp_last;
                            hp.MaHP = mahp_now;
                            hp.MaHieuCV_User = "CV_" + idcv_last.ToString();
                            hp.Ten = k.Ten;
                            hp.DonVi = k.DonVi;
                            hp.SoLuong_DM = k.SoLuong_DM;
                            hp.Gia = k.Gia;

                            idhp_last = idhp_last + 1;

                            _db.ThanhPhanHaoPhis.InsertOnSubmit(hp);
                        }

                        idcv_last = idcv_last + 1;
                        _db.CongViecs.InsertOnSubmit(cv_user);
                    }
                }
            }
            _db.SubmitChanges();
            return RedirectToAction("Index", "CongTrinh");
        }

    }
}
