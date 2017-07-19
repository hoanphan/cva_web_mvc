using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using CVAWeb.Models;
using CVAWeb.Models.BLL;
using CVAWeb.Models.DAL;
namespace CVAWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            ViewBag.SuDungMenu = "";
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection f)
        {

            string TaiKhoan = f["txtTaiKhoan"].ToString();
            string MatKhau = f["txtMatKhau"].ToString();
            string IP = this.Request.UserHostAddress;
            string Name = this.Request.UserHostName;
            int gt = LoginBLL.KiemTraDangNhap(TaiKhoan, MatKhau, IP,Name);
            if (gt == 1)//Là giáo viên chủ nhiệm
            {
                Session["MaDangNhap"] = TaiKhoan.ToUpper();
                Session["SuDungMenu"] = 1;//sử dụng menu của gv chủ nhiệm
                return RedirectToAction("NhapDiem", "GiaoVien", null);
            }
            else
                if (gt == 2)//là học sinh đăng kí dịch vụ
                {
                    Session["MaDangNhap"] = TaiKhoan.ToUpper();
                    Session["SuDungMenu"] = 2;
                    return RedirectToAction("DiemCuaHocSinh", "HocSinh", null);
                }
                else
                    if (gt == 3)//là học sinh không đăng kí dịch vụ
                    {
                        ViewBag.KTLogin = 1;
                        ViewBag.SuDungMenu = "";
                        return View();
                    }
                    else
                        if (gt == 4)//là giáo viên không chủ nhiệm
                        {
                            Session["MaDangNhap"] = TaiKhoan.ToUpper();
                            Session["SuDungMenu"] = 3;//sử dung menu của giáo viên thông thường
                            return RedirectToAction("NhapDiem", "GiaoVien", null);
                        }
                        else//đăng nhập không thành công
                        {
                            ViewBag.KTLogin = 2;
                            ViewBag.SuDungMenu = "";
                            return View();
                        }
        }
        public ActionResult Logoff()
        {

            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index","Home",null);
        }
        public ActionResult SuaThongTin()
        {
            SchoolManagementDataContext DB=new SchoolManagementDataContext();
            ViewBag.SuDungMenu=Session["SuDungMenu"].ToString();
            string MaTaiKhoan = Session["MaDangNhap"].ToString();
            if(DB.DSGiaoViens.Where(q=>q.MaGiaoVien==MaTaiKhoan).FirstOrDefault()!=null)
            {
                DSGiaoVien GV = DB.DSGiaoViens.Where(a => a.MaGiaoVien == MaTaiKhoan).FirstOrDefault();
               ViewBag.HoTenGiaoVien = GV.HoDem + " " + GV.Ten;
            }
            else
            {
                DSHocSinh HS = DB.DSHocSinhs.Where(q => q.MaHocSinh == MaTaiKhoan).FirstOrDefault();
                ViewBag.HoTenHocSinh = HS.HoDem + " " + HS.Ten;
            }
            if (Session["Kt2"] != null)
                ViewBag.Kt2 = Session["Kt2"].ToString();
            Session["Kt2"] = "3";
            return View();
        }
        [HttpPost]
        public ActionResult SuaThongTin(FormCollection f)
        {
            string MatKhaucu = f["txtMatKhau"];
            string MatKhauMoi = f["txtMatKhauMoi"];
            string NhapLaiMatKhau = f["txtNhapLaiMatKhau"];
            if (MatKhaucu != "" && MatKhauMoi != "" && NhapLaiMatKhau != "")
                return RedirectToAction("SuaThongTin2", "Home", new { TaiKhoan = Session["MaDangNhap"].ToString(), MatKhauCu = MatKhaucu, MatKhauMoi = MatKhauMoi, NhapLaiMatKhau = NhapLaiMatKhau });
            else
                return View();
        }
        public ActionResult SuaThongTin2(string TaiKhoan, string MatKhauCu, string MatKhauMoi, string NhapLaiMatKhau)
        {
            if (MatKhauMoi != NhapLaiMatKhau)
            {
                Session["Kt2"] = 2;
                return RedirectToAction("SuaThongTin", "Home", null);
            }
            else
            {

                if (SuaThongTinBLL.KiemTraTonTai(TaiKhoan, MatKhauCu) == true)
                {
                    SuaThongTinBLL.ThayDoiMatKhau(TaiKhoan, MatKhauMoi);
                    Session["Kt2"] = 1;
                    return RedirectToAction("SuaThongTin", "Home", null);
                }
                else
                {
                    Session["Kt2"] = 2;
                    return RedirectToAction("SuaThongTin", "Home", null);
                }
            }
        }
        public ActionResult XoaSoLienLacDienTu(string MaHocSinh, string MaTuan)
        {
            ElectricBookBLL.XoaSoLienLac(MaHocSinh, int.Parse(MaTuan));
            return RedirectToAction("Index", "Home", new { MaTuan = MaTuan });
        }
        public ActionResult LoiHeThong()
        {

            Session.Clear();
            Session.Abandon();
            return View();
        }

    }

}