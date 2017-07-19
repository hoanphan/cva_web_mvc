using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CVAWeb.Models.DAL;
using CVAWeb.Models.BLL;
using DevExpress.Web.Mvc;
using System.Collections;
using System.Data;
using CVAWed.Models.DAL;
using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;


namespace CVAWeb.Controllers
{
    public class GiaoVienController : Controller
    {
        //
        // GET: /GiaoVien/
        #region[Nhập điểm của giá0 viên]
        SchoolManagementDataContext DB = new SchoolManagementDataContext();
          #region[combobox thay doi]
        public ActionResult HocKi(string HocKi)
        {            
            Session["Lop"] = null;
            Session["Mon"] = null;
            Session["HocKi"] = HocKi;
            return RedirectToAction("NhapDiem");
        }
        public ActionResult Khoi(string Khoi)
        {
            Session["TenKhoi"] = Khoi;
            Session["Lop"] = null;
            Session["Mon"] = null;
            return RedirectToAction("NhapDiem");
        }
        public ActionResult Lop(string Lop)
        {
            Session["Lop"] = Lop;
            Session["Mon"] = null;
            return RedirectToAction("NhapDiem");
        }
        public ActionResult Mon(string Mon)
        {
            Session["Mon"] = Mon;
           
            return RedirectToAction("NhapDiem");
        }
        #endregion
        public ActionResult NhapDiem()
        {
               ViewBag.SuDungMenu = Session["SuDungMenu"].ToString();
                string MaGV = Session["MaDangNhap"].ToString();
                ViewBag.MaGV = MaGV;
            if(Session["Loi"]!=null)
                ViewBag.Loi = Session["Loi"].ToString();
                Session["Loi"] = null;
                try
                {
                    #region[Khoi tao hoc ki]
                    DSGiaoVien Gv = DB.DSGiaoViens.Where(q => q.MaGiaoVien == MaGV).FirstOrDefault();
                ViewBag.HoTenGiaoVien = Gv.HoDem + " " + Gv.Ten;
                
                ViewData["DsHocKy"] = DSKhoiBLL.dsHocKi();

                if (Session["HocKi"] != null)//Kiem Tra Hoc Ki
                {
                    ViewBag.ChoPhepChinhSua = DSKhoiBLL.ChoPhepKhoaSo(Session["HocKi"].ToString());
                    ViewBag.VTHocKy = DSKhoiBLL.LayViTriHocKy(Session["HocKi"].ToString());
                }
                else
                {
                    if (PhanCongGiangDayBLL.KiemTraPhanCongGiangDayTrongHocKi("K1", MaGV))
                        Session["HocKi"] = "K1";
                    else
                        Session["HocKi"] = "K2";
                    ViewBag.VTHocKy = DSKhoiBLL.LayViTriHocKy(Session["HocKi"].ToString());
                }

                

             #endregion
                    #region[Kiem Tra Khoi]
                    ViewData["dsKhoi"] = DSKhoiBLL.DanhSachKhoiDuocPhanCong(MaGV, Session["HocKi"].ToString());
                    if (Session["TenKhoi"] != null)
                    {
                        ViewBag.VTKhoi = DSKhoiBLL.LayRaViTriKhoi(Session["TenKhoi"].ToString(),MaGV,Session["HocKi"].ToString());
                        ViewData["dsLop"] = PhanCongGiangDayBLL.LayLopTheoMaHocKiMaKhoi(Session["HocKi"].ToString(), Session["TenKhoi"].ToString(), MaGV);
                    }
                    else
                    {
                        if (DSKhoiBLL.KhoiDauTien(MaGV, Session["HocKi"].ToString()) != null)
                        {
                            List<DSLop> listLop = PhanCongGiangDayBLL.LayLopTheoMaHocKiMaKhoi(Session["HocKi"].ToString(), DSKhoiBLL.KhoiDauTien(MaGV, Session["HocKi"].ToString()).MaKhoi, MaGV);
                            if (listLop != null)
                                ViewData["dsLop"] = listLop;

                        }
                        else
                            ViewData["dsLop"] = null;
                    }

                    #endregion
                    
                    #region[Kiem Tra Lop]
                    if (Session["Lop"] != null)
                    {
                        ViewData["dsMonHoc"] = PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVien(Session["Lop"].ToString(), Session["HocKi"].ToString(), MaGV);
                       
                        if (Session["TenKhoi"] != null)
                            ViewBag.VTLop = DSKhoiBLL.LayRaViTriLop(Session["Lop"].ToString(), Session["TenKhoi"].ToString());
                        else
                            ViewBag.VTLop = DSKhoiBLL.LayRaViTriLop(Session["Lop"].ToString(), DSKhoiBLL.KhoiDauTien(MaGV, Session["HocKi"].ToString()).MaKhoi);

                    }
                    else
                    {
                        if (Session["TenKhoi"] != null)
                        {
                            string MaKhoi = Session["TenKhoi"].ToString();
                            string TenLop = PhanCongGiangDayBLL.LayLopDauTienCoKhoi(Session["HocKi"].ToString(), MaGV, Session["TenKhoi"].ToString()).TenLop;
                            ViewData["dsMonHoc"] = PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVien(PhanCongGiangDayBLL.LayLopDauTienCoKhoi(Session["HocKi"].ToString(), MaGV,Session["TenKhoi"].ToString()).MaLop, Session["HocKi"].ToString(), MaGV);
                        }
                        else
                            ViewData["dsMonHoc"] = PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVien(PhanCongGiangDayBLL.LayLopDauTienChuaCoKhoi(Session["HocKi"].ToString(), MaGV).MaLop, Session["HocKi"].ToString(), MaGV);
                    }
                    #endregion
                    #region[Kiem tra mon hoc]
                    if (Session["Mon"]!=null)
                    {
                        if (Session["Lop"] != null)
                            ViewBag.VTMonHoc = PhanCongGiangDayBLL.LayRaViTriMonHoc(Session["Lop"].ToString(), Session["HocKi"].ToString(), MaGV, Session["Mon"].ToString());
                        else
                        {
                            if (Session["TenKhoi"] != null)
                            {
                                int vt = PhanCongGiangDayBLL.LayRaViTriMonHoc(PhanCongGiangDayBLL.LayLopDauTienCoKhoi(Session["HocKi"].ToString(), MaGV, Session["TenKhoi"].ToString()).MaLop, Session["HocKi"].ToString(), MaGV, Session["Mon"].ToString());
                                ViewBag.VTMonHoc = PhanCongGiangDayBLL.LayRaViTriMonHoc(PhanCongGiangDayBLL.LayLopDauTienCoKhoi(Session["HocKi"].ToString(), MaGV, Session["TenKhoi"].ToString()).MaLop, Session["HocKi"].ToString(), MaGV, Session["Mon"].ToString());
                            }
                            else
                            {
                                ViewBag.VTMonHoc = PhanCongGiangDayBLL.LayRaViTriMonHoc(PhanCongGiangDayBLL.LayLopDauTienChuaCoKhoi(Session["HocKi"].ToString(), MaGV).MaLop, Session["HocKi"].ToString(), MaGV, Session["Mon"].ToString());
                            }
                        }

                    #endregion


                    }
                   ViewBag.KhoaSo = DSKhoiBLL.ChoPhepKhoaSo(Session["HocKi"].ToString());
                    var model = DB.DSLoaiDiems.ToList();
                
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "Home", null);
            }
        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            try
            {
                
                string MaGV = Session["MaDangNhap"].ToString();
                if (Session["Mon"] != null && Session["Lop"] == null)
                {

                    if (Session["TenKhoi"] == null)
                        ViewData["dsDiem"] = DSDiemCuaGVBLL.LoadDuLieuVaoBang(PhanCongGiangDayBLL.LayLopDauTienChuaCoKhoi(Session["HocKi"].ToString(), MaGV).ToString(), Session["Mon"].ToString(), Session["HocKi"].ToString());
                    else
                    {
                        string TenKhoi = Session["TenKhoi"].ToString();
                        
                        ViewData["dsDiem"] = DSDiemCuaGVBLL.LoadDuLieuVaoBang(PhanCongGiangDayBLL.LayLopDauTienCoKhoi(Session["HocKi"].ToString(), MaGV, Session["TenKhoi"].ToString()).MaLop, Session["Mon"].ToString(), Session["HocKi"].ToString());
                    }
                }

                else
                {
                    if (Session["Mon"] != null && Session["Lop"] != null)
                        ViewData["dsDiem"] = DSDiemCuaGVBLL.LoadDuLieuVaoBang(Session["Lop"].ToString(), Session["Mon"].ToString(), Session["HocKi"].ToString());
                    else
                    {
                        //hoc ki va khoi  lop chua co gi tri
                        if (Session["HocKi"] != null && Session["TenKhoi"] == null && Session["Lop"] == null)
                        {
                            string maHocKi = Session["HocKi"].ToString();
                            string khoi = DSKhoiBLL.KhoiDauTien(MaGV, maHocKi).MaKhoi;
                            if (khoi != null)
                            {
                                string MaLop = PhanCongGiangDayBLL.LayLopDauTienCoKhoi(maHocKi, MaGV, khoi).MaLop;
                                ViewData["dsDiem"] = DSDiemCuaGVBLL.LoadDuLieuVaoBang(MaLop, PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(MaLop, maHocKi, MaGV).MaMonHoc, maHocKi);
                            }
                            else
                                ViewData["dsDiem"] = null;
                        }
                        if (Session["TenKhoi"] == null && Session["Lop"] == null)
                            ViewData["dsDiem"] = DSDiemCuaGVBLL.LoadDuLieuVaoBang(PhanCongGiangDayBLL.LayLopDauTienChuaCoKhoi(Session["HocKi"].ToString(), MaGV).MaLop, PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(PhanCongGiangDayBLL.LayLopDauTienChuaCoKhoi(Session["HocKi"].ToString(), MaGV).MaLop, Session["HocKi"].ToString(), MaGV).MaMonHoc, Session["HocKi"].ToString());
                        else
                            if (Session["Lop"] != null)
                                ViewData["dsDiem"] = DSDiemCuaGVBLL.LoadDuLieuVaoBang(Session["Lop"].ToString(), PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(Session["Lop"].ToString(), Session["HocKi"].ToString(), MaGV).MaMonHoc, Session["HocKi"].ToString());
                            else
                                if (Session["TenKhoi"] != null)
                                {
                                    string MaLop = PhanCongGiangDayBLL.LayLopDauTienCoKhoi(Session["HocKi"].ToString(), MaGV, Session["TenKhoi"].ToString()).MaLop;
                                    ViewData["dsDiem"] = DSDiemCuaGVBLL.LoadDuLieuVaoBang(MaLop, PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(MaLop, Session["HocKi"].ToString(), MaGV).MaMonHoc, Session["HocKi"].ToString());
                                }
                    }
                }
                ViewBag.KhoaSo = DSKhoiBLL.ChoPhepKhoaSo(Session["HocKi"].ToString());
                var model = DB.DSLoaiDiems.ToList();
                return PartialView("_GridViewPartial", model);
            }
            catch
            {
                return RedirectToAction("LoiHeThong", "Home", null);
            }
        }
        
       
      
        [ValidateInput(false)]
        public ActionResult BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<DiemDAL, int> updateValues)
        {
            try
            {
                foreach (var product in updateValues.Update)
                {

                    if (updateValues.IsValid(product))
                        UpdateProduct(product, updateValues);
                }
                #region[Tinh diem trung binh]
                //Tinh Diem TRung binh 
                string MaHocKi = Session["HocKi"].ToString();
                string MaGV = Session["MaDangNhap"].ToString();
                string MaMon = "";
                string MaLop = null;
                if (Session["Loi"] != null)
                ViewBag.Loi = Session["Loi"].ToString();
                Session["Loi"] = null;
                if (Session["TenKhoi"] == null && Session["Lop"] == null && Session["Mon"] == null)
                {
                    MaLop = PhanCongGiangDayBLL.LayLopDauTienChuaCoKhoi(MaHocKi, MaGV).MaLop;
                    MaMon = PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(MaLop, MaHocKi, MaGV).MaMonHoc;
                }
                if (Session["TenKhoi"] != null && Session["Lop"] == null)
                {
                    MaLop = PhanCongGiangDayBLL.LayLopDauTienCoKhoi(MaHocKi, MaGV, Session["TenKhoi"].ToString()).MaLop;
                    MaMon = PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(MaLop, MaHocKi, MaGV).MaMonHoc;
                }
                if (Session["TenKhoi"] == null && Session["Lop"] != null && Session["Mon"] == null)
                {
                    MaMon = PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(Session["Lop"].ToString(), MaHocKi, MaGV).MaMonHoc;
                }
                if (Session["TenKhoi"] == null && Session["Lop"] == null && Session["Mon"] != null)
                {
                    MaLop = PhanCongGiangDayBLL.LayLopDauTienChuaCoKhoi(MaHocKi, MaGV).MaLop;
                    MaMon = Session["Mon"].ToString();
                }
                if (Session["Lop"] != null && Session["Mon"] == null)
                {
                    MaLop = Session["Lop"].ToString();
                    MaMon = PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(MaLop, MaHocKi, MaGV).MaMonHoc;
                }
                if (Session["Lop"] != null && Session["Mon"] != null)
                {
                    MaLop = Session["Lop"].ToString();
                    MaMon = Session["Mon"].ToString();
                }
                DSDiemCuaGVBLL.TinhDiemTrungBinh(MaHocKi, MaLop, MaMon);
                //ket thuc
                #endregion

                //can than

                if (Session["Mon"] != null && Session["Lop"] == null)
                {

                    if (Session["TenKhoi"] == null)
                        ViewData["dsDiem"] = DSDiemCuaGVBLL.LoadDuLieuVaoBang(PhanCongGiangDayBLL.LayLopDauTienChuaCoKhoi(Session["HocKi"].ToString(), MaGV).ToString(), Session["Mon"].ToString(), Session["HocKi"].ToString());
                    else
                    {
                        string TenKhoi = Session["TenKhoi"].ToString();

                        ViewData["dsDiem"] = DSDiemCuaGVBLL.LoadDuLieuVaoBang(PhanCongGiangDayBLL.LayLopDauTienCoKhoi(Session["HocKi"].ToString(), MaGV, Session["TenKhoi"].ToString()).MaLop, Session["Mon"].ToString(), Session["HocKi"].ToString());
                    }
                }
                else
                {
                    if (Session["Mon"] != null && Session["Lop"] != null)
                        ViewData["dsDiem"] = DSDiemCuaGVBLL.LoadDuLieuVaoBang(Session["Lop"].ToString(), Session["Mon"].ToString(), Session["HocKi"].ToString());
                    else
                    {
                        if (Session["HocKi"] != null && Session["TenKhoi"] == null && Session["Lop"] == null)
                        {
                            string maHocKi = Session["HocKi"].ToString();
                            string khoi = DSKhoiBLL.KhoiDauTien(MaGV, maHocKi).MaKhoi;
                            if (khoi != null)
                            {
                                string maLop = PhanCongGiangDayBLL.LayLopDauTienCoKhoi(maHocKi, MaGV, khoi).MaLop;
                                ViewData["dsDiem"] = DSDiemCuaGVBLL.LoadDuLieuVaoBang(maLop, PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(MaLop, maHocKi, MaGV).MaMonHoc, maHocKi);
                            }
                            else
                                ViewData["dsDiem"] = null;
                        }
                        if (Session["TenKhoi"] == null && Session["Lop"] == null)
                            ViewData["dsDiem"] = DSDiemCuaGVBLL.LoadDuLieuVaoBang(PhanCongGiangDayBLL.LayLopDauTienChuaCoKhoi(Session["HocKi"].ToString(), MaGV).MaLop, PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(PhanCongGiangDayBLL.LayLopDauTienChuaCoKhoi(Session["HocKi"].ToString(), MaGV).MaLop, Session["HocKi"].ToString(), MaGV).MaMonHoc, Session["HocKi"].ToString());
                        else
                            if (Session["Lop"] != null)
                                ViewData["dsDiem"] = DSDiemCuaGVBLL.LoadDuLieuVaoBang(Session["Lop"].ToString(), PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(Session["Lop"].ToString(), Session["HocKi"].ToString(), MaGV).MaMonHoc, Session["HocKi"].ToString());
                            else
                                if (Session["TenKhoi"] != null)
                                {
                                    string maLop = PhanCongGiangDayBLL.LayLopDauTienCoKhoi(Session["HocKi"].ToString(), MaGV, Session["TenKhoi"].ToString()).MaLop;
                                    ViewData["dsDiem"] = DSDiemCuaGVBLL.LoadDuLieuVaoBang(maLop, PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(MaLop, Session["HocKi"].ToString(), MaGV).MaMonHoc, Session["HocKi"].ToString());
                                }
                    }
                }
                ViewBag.KhoaSo = DSKhoiBLL.ChoPhepKhoaSo(Session["HocKi"].ToString());
                
                var model = DB.DSLoaiDiems.ToList();
               
                return PartialView("_GridViewPartial",model);
            }
            catch
            {
                Session["Loi"] = true;
                return RedirectToAction("NhapDiem", "GiaoVien", null);
            }
        }
        protected void UpdateProduct(DiemDAL row, MVCxGridViewBatchUpdateValues<DiemDAL, int> updateValues)
        {
            string MaHocKi = Session["HocKi"].ToString();
            string MaGV = Session["MaDangNhap"].ToString();
            string MaMon = "";
            string MaLop = null;
            
            if (Session["TenKhoi"] == null && Session["Lop"] == null && Session["Mon"] == null)
            {
                MaLop = PhanCongGiangDayBLL.LayLopDauTienChuaCoKhoi(MaHocKi, MaGV).MaLop;
                MaMon = PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(MaLop, MaHocKi, MaGV).MaMonHoc;
            }
            if (Session["TenKhoi"] != null && Session["Lop"] == null)
            {
                MaLop = PhanCongGiangDayBLL.LayLopDauTienCoKhoi(MaHocKi, MaGV, Session["TenKhoi"].ToString()).MaLop;
                MaMon = PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(MaLop, MaHocKi, MaGV).MaMonHoc;
            }
            if (Session["TenKhoi"] == null && Session["Lop"] != null && Session["Mon"] == null)
            {
                MaMon = PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(Session["Lop"].ToString(), MaHocKi, MaGV).MaMonHoc;
            }
            if (Session["TenKhoi"] == null && Session["Lop"] == null && Session["Mon"] != null)
            {
                MaLop = PhanCongGiangDayBLL.LayLopDauTienChuaCoKhoi(MaHocKi, MaGV).MaLop;
                MaMon = Session["Mon"].ToString();
            }
            if (Session["Lop"] != null && Session["Mon"] == null)
            {
                MaLop = Session["Lop"].ToString();
                MaMon = PhanCongGiangDayBLL.LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(MaLop, MaHocKi, MaGV).MaMonHoc;
            }
            if (Session["Mon"] != null)
            {
                MaMon = Session["Mon"].ToString();
            }
            DSDiemCuaGVBLL.CapNhatNhieuDiem(row, MaHocKi, MaMon);
           
        }
       #endregion
        #region[Sổ liên lạc điện tử]
        public ActionResult TuanThayDoi(string MaTuan)
        {
            Session["MaTuan"]=MaTuan;
            return RedirectToAction("SoLienLacDienTuCuaGV", "GiaoVien", null);
        }
        public ActionResult SoLienLacDienTuCuaGV()
        {
            try
            {
                ViewBag.SuDungMenu = Session["SuDungMenu"].ToString();
                string MaGV = Session["MaDangNhap"].ToString();
                DSGiaoVien Gv = DB.DSGiaoViens.Where(q => q.MaGiaoVien == MaGV).FirstOrDefault();
                ViewBag.HoTenGiaoVien = Gv.HoDem + " " + Gv.Ten;

                if (Session["MaTuan"] == null)
                    ViewBag.MaTuan =DB.DSTuans.Where(q=>q.DSNamHoc.NamHienTai==true).Max(q=>q.MaTuan);
                else
                    ViewBag.MaTuan = int.Parse(Session["MaTuan"].ToString());
                CapNhatTuanBLL.CapNhatLaiTuan();
                string MaLopCN = ElectricBookBLL.TimMaLopGiaoVienChuNhiem(MaGV);
                var model = DB.DSHocSinhs.Where(q => q.DSHocSinhTheoLop.MaLop == MaLopCN).ToList();
                ViewData["ListTuan"] = DSTuanBLL.LoadListWeek();
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "Home", null);
            }
        }
        public ActionResult GridViewSoLienLacDienCuaHocSinh()
        {
            try
            {
                string MaGV = Session["MaDangNhap"].ToString();
                if (Session["MaTuan"] == null)
                    ViewBag.MaTuan = "";
                else
                    ViewBag.MaTuan = int.Parse(Session["MaTuan"].ToString());
                CapNhatTuanBLL.CapNhatLaiTuan();
                string MaLopCN = ElectricBookBLL.TimMaLopGiaoVienChuNhiem(MaGV);
                var model = DB.DSHocSinhs.Where(q => q.DSHocSinhTheoLop.MaLop == MaLopCN).ToList();
                ViewData["ListTuan"] = DSTuanBLL.LoadListWeek();

                return PartialView("SoLienLacDienTuPatial", model);
            }
            catch
            {
                return RedirectToAction("Index", "Home", null);
            }
        }

        
        public ActionResult SoLienLacDienCuaHocSinhAddNew(string MaHocSinh,int MaTuan,bool LaThem)
        {
            Session["MaHocSinh"] = MaHocSinh;
            Session["LaThem"] = LaThem;
            Session["MaTuan"] = MaTuan;
            return RedirectToAction("SuaSoLienDienDienTu", "GiaoVien", null);
        }
     
        public ActionResult SoLienLacDienCuaHocSinhUpdate(string MaHocSinh,int MaTuan,bool LaThem)
        {
            Session["MaHocSinh"] = MaHocSinh;
            Session["LaThem"] = LaThem;
            Session["MaTuan"] = MaTuan;
            return RedirectToAction("SuaSoLienDienDienTu", "GiaoVien", null);
        }
        public ActionResult SoLienLacDienCuaHocSinhDelete(string MaHocSinh, int MaTuan)
        {
            ElectricBookBLL.XoaSoLienLac(MaHocSinh, MaTuan);
            return RedirectToAction("SuaSoLienDienDienTu", "GiaoVien", null);
        }
        
        public ActionResult SuaSoLienDienDienTu()
        {
            try
            {
                ViewBag.SuDungMenu = Session["SuDungMenu"].ToString();
                bool LaThem = (bool)Session["LaThem"];
                ViewBag.MaHocSinh = Session["MaHocSinh"].ToString();
                ViewBag.MaTuan = Session["MaTuan"].ToString();
                DSGiaoVien GV = DB.DSGiaoViens.Where(q => q.MaGiaoVien == Session["MaDangNhap"].ToString()).FirstOrDefault();
                ViewBag.HoTenGiaoVien = GV.HoDem + " " + GV.Ten;

                ViewBag.MaHocSinh = Session["MaHocSinh"].ToString();
                DSHocSinh HS = DB.DSHocSinhs.Where(q => q.MaHocSinh == Session["MaHocSinh"].ToString()).FirstOrDefault();
                ViewBag.HoTen = HS.HoDem + " " + HS.Ten;
                ViewBag.Matuan = Session["MaTuan"].ToString();
                string MaTuan = Session["MaTuan"].ToString();
                var DsTuan = DB.DSTuans.Where(q => q.MaTuan == int.Parse(MaTuan)).Where(q => q.DSNamHoc.NamHienTai == true).FirstOrDefault();
                ViewBag.NgayBatDau = DsTuan.BatDauTuNgay;
                ViewBag.DenNgay = DsTuan.KetThucNgay;

                if (ElectricBookBLL.KiemTraDuocSuaDoi(Session["MaDangNhap"].ToString(), Session["MaHocSinh"].ToString()))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("SoLienLacDienTuCuaGV", "GiaoVien", null);
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home", null);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSoLienDienDienTu(FormCollection f)
        {
            try
            {
                SchoolManagementDataContext DB = new SchoolManagementDataContext();
                int _MaTuan = int.Parse(Session["MaTuan"].ToString());
                string MaHocSinh = Session["MaHocSinh"].ToString();
                bool LaThem = (bool)Session["LaThem"];
                string NoiDung = HtmlEditorExtension.GetHtml("HtmlEditor");
                if (NoiDung != "")
                {
                    if (!LaThem)
                    {
                        SoLienLacDienTu Ds = ElectricBookBLL.TruyVanSoLienLacDienTuTheoMa(MaHocSinh, _MaTuan);
                        Ds.NoiDung = NoiDung;
                        ElectricBookBLL.SuaSoLienLac(Ds);
                    }
                    else
                    {
                        ElectricBookBLL.ThemSoLienLacDienTu(MaHocSinh, _MaTuan, NoiDung);
                    }
                    return RedirectToAction("TuanThayDoi", "GiaoVien", new { MaTuan = _MaTuan });
                }

                else
                {
                    if (!LaThem)
                        return RedirectToAction("SoLienLacDienCuaHocSinhAddNew", "GiaoVien", new { MaHocSinh = MaHocSinh, MaTuan = _MaTuan, LaThem = false });
                    else
                        return RedirectToAction("SoLienLacDienCuaHocSinhUpdate", "GiaoVien", new { MaHocSinh = MaHocSinh, MaTuan = _MaTuan, LaThem = true });
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home", null);
            }
        }
        #endregion
       
        
        

        public ActionResult HtmlEditorPartial()
        {
            var model = DB.SoLienLacDienTus.Where(q => q.MaHocSinh == Session["MaHocSinh"].ToString()).Where(q => q.MaTuan == int.Parse(Session["MaTuan"].ToString())).Where(q => q.DSTuan.DSNamHoc.NamHienTai == true).Select(q => q.NoiDung).FirstOrDefault();
            
            return PartialView("_HtmlEditorPartial",model);
        }
        public ActionResult FeaturesImageUpload() {
            HtmlEditorExtension.SaveUploadedFile("HtmlEditor", HtmlEditorFeaturesDemosHelper.ImageUploadValidationSettings, HtmlEditorFeaturesDemosHelper.UploadDirectory);
            return null;
        }
        public ActionResult FeaturesImageSelectorUpload() {
            HtmlEditorExtension.SaveUploadedImage("HtmlEditor", HtmlEditorFeaturesDemosHelper.ImageSelectorSettings);
            return null;
        }
    }
   public class HtmlEditorFeaturesDemosHelper {
        public const string ImagesDirectory = "~/Images/";
        public const string ThumbnailsDirectory = "~/Images/";
        public const string UploadDirectory = "~/Content/HtmlEditor/UploadFiles/";
        public const string HtmlLocation = "~/Content/HtmlEditor/DemoHtml/";

        public static readonly UploadControlValidationSettings ImageUploadValidationSettings = new UploadControlValidationSettings {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 4000000
        };

        static HtmlEditorValidationSettings validationSettings;
        public static HtmlEditorValidationSettings ValidationSettings {
            get {
                if(validationSettings == null) {
                    validationSettings = new HtmlEditorValidationSettings();
                    validationSettings.RequiredField.IsRequired = true;
                }
                return validationSettings;
            }
        }

        static MVCxHtmlEditorImageSelectorSettings imageSelectorSettings;
        public static HtmlEditorImageSelectorSettings ImageSelectorSettings {
            get {
                if(imageSelectorSettings == null) {
                    imageSelectorSettings = new MVCxHtmlEditorImageSelectorSettings();
                    SetHtmlEditorImageSelectorSettings(imageSelectorSettings);
                }
                return imageSelectorSettings;
            }
        }
        public static MVCxHtmlEditorImageSelectorSettings SetHtmlEditorImageSelectorSettings(MVCxHtmlEditorImageSelectorSettings settingsImageSelector) {
            settingsImageSelector.UploadCallbackRouteValues = new { Controller = "GiaoVien", Action = "FeaturesImageSelectorUpload" };
            settingsImageSelector.Enabled = true;
            settingsImageSelector.CommonSettings.RootFolder = ImagesDirectory;
            settingsImageSelector.CommonSettings.ThumbnailFolder = ThumbnailsDirectory;
            settingsImageSelector.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" };
            settingsImageSelector.EditingSettings.AllowCreate = true;
            settingsImageSelector.EditingSettings.AllowDelete = true;
            settingsImageSelector.EditingSettings.AllowMove = true;
            settingsImageSelector.EditingSettings.AllowRename = true;
            settingsImageSelector.UploadSettings.Enabled = true;
            settingsImageSelector.FoldersSettings.ShowLockedFolderIcons = true;

            settingsImageSelector.PermissionSettings.AccessRules.Add(
                new FileManagerFolderAccessRule {
                    Path = "",
                    Upload = Rights.Deny
                });
            return settingsImageSelector;
        }

        public static string GeHtmlContentByFileName(string fileName) {
            return System.IO.File.ReadAllText(System.Web.HttpContext.Current.Request.MapPath(string.Format("{0}{1}", HtmlLocation, fileName)));
        }
        public static string GeHtmlContentByFileName(string fileName, bool demoPageIsInRoot) {
            string result = GeHtmlContentByFileName(fileName);
            return demoPageIsInRoot ? result : result.Replace("Content/", "../Content/");
        }
    }
}

