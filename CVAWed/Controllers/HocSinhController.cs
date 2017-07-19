using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using CVAWeb.Models.DAL;
using CVAWeb.Models.BLL;


    public class HocSinhController : Controller
    {
        //
        // GET: /Diem/
        #region[ điểm của học sinh]
        SchoolManagementDataContext DB = new SchoolManagementDataContext();
        public ActionResult DiemCuaHocSinh()
        {
            string MaHocSinh = Session["MaDangNhap"].ToString();
            ViewBag.SuDungMenu = Session["SuDungMenu"].ToString();
            DSHocSinh HS = DB.DSHocSinhs.Where(q => q.MaHocSinh == MaHocSinh).FirstOrDefault();
            ViewBag.HoTenHocSinh = HS.HoDem + " " + HS.Ten;
            XepLoaiCuaHocSinh(MaHocSinh);
            var model = DB.DSLoaiDiems.ToList();
            ViewData["DuLieu"] = LoadDiemBLL.LoadDiem(MaHocSinh);
          

            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {

            string MaHocSinh = Session["MaDangNhap"].ToString();
           
            var model = DB.DSLoaiDiems.ToList();
            ViewData["DuLieu"] = LoadDiemBLL.LoadDiem(MaHocSinh);
            return PartialView("_GridViewPartial",model);

        }


        [ValidateInput(false)]
        public ActionResult XepLoaiPatial()
        {
            return PartialView();
        }
       protected void XepLoaiCuaHocSinh(string MaHocSinh)
        {

           
            var DsHk = DB.DSHocKies.OrderByDescending(q => q.MaHocKy).ToList();
           foreach(DSHocKy ds in DsHk)
           {
               DSTongKetTheoKy DS = DB.DSTongKetTheoKies.Where(q => q.MaHocSinh == MaHocSinh).Where(q => q.DSNamHoc.NamHienTai == true).Where(q=>q.MaHocKy==ds.MaHocKy).FirstOrDefault();
               if(DS.TrungBinhChung!=null&&DS.MaHanhKiem!=null)
               {
                   ViewBag.TenHocKi = DS.DSHocKy.TenHocKy;
                   if(DS.TrungBinhChung!=null)
                        ViewBag.HocLuc = DS.TrungBinhChung;
                   if(DS.MaHanhKiem!=null)
                         ViewBag.HanhKiem = DS.DMHanhKiem.TenHanhKiem;
                   if(DS.MaDanhHieu!=null)
                         ViewBag.DanhHieu = DS.DMDanhHieu.TenDanhHieu;
                   break;
               }
           }
        }
        #endregion
        #region[sổ liên lạc của học sinh]
       public ActionResult SoLienLacCuaHocSinh()
       {
           string MaHocSinh = Session["MaDangNhap"].ToString();
           DSHocSinh HS = DB.DSHocSinhs.Where(q => q.MaHocSinh == MaHocSinh).FirstOrDefault();
           ViewBag.HoTenHocSinh = HS.HoDem + " " + HS.Ten;
           
           ViewBag.SuDungMenu = Session["SuDungMenu"].ToString();
           var model = SoLienLacDienTuCuaHocSinh.LoadSoLineLacDienTu(MaHocSinh);
           return View(model);
       }
       [ValidateInput(false)]
       public ActionResult GridSoLienLacDienTu()
       {
           string MaHocSinh = Session["MaDangNhap"].ToString();
           var model = SoLienLacDienTuCuaHocSinh.LoadSoLineLacDienTu(MaHocSinh);
           return PartialView("GridViewSoLienLacDienTu",model);

       }
        #endregion
    }
