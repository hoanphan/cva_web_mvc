using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CVAWeb.Models.DAL;
namespace CVAWeb.Models.BLL
{
    public class SuaThongTinBLL
    {
       static SchoolManagementDataContext DB = new SchoolManagementDataContext();
        public static bool KiemTraTonTai(string TaiKhoan,string MatKhau)
        {
            string matkhau = LoginBLL.Encrypt(MatKhau, "CVA");
            if (DB.DSGiaoViens.Where(q => q.MaGiaoVien == TaiKhoan).Where(q => q.MatKhau == matkhau).FirstOrDefault() != null)
                return true;
            else
            {
                if (DB.DSHocSinhs.Where(q => q.MaHocSinh == TaiKhoan).Where(q => q.MatKhau == matkhau).FirstOrDefault() != null)
                    return true;
                else
                    return false;
            }
        }
        public static void ThayDoiMatKhau(string TaiKhoan,string _MatKhauMoi)
        {
            string MatKhauMoi = LoginBLL.Encrypt(_MatKhauMoi, "CVA");
            if(DB.DSGiaoViens.Where(q=>q.MaGiaoVien==TaiKhoan).FirstOrDefault()!=null)
            {
                DSGiaoVien Ds = DB.DSGiaoViens.Where(q => q.MaGiaoVien == TaiKhoan).FirstOrDefault();
                Ds.MatKhau = MatKhauMoi;
                DB.SubmitChanges();
            }
            else
                if(DB.DSHocSinhs.Where(q => q.MaHocSinh == TaiKhoan).FirstOrDefault()!=null)
            {
                DSHocSinh Ds = DB.DSHocSinhs.Where(q => q.MaHocSinh == TaiKhoan).FirstOrDefault();
                Ds.MatKhau = MatKhauMoi;
                DB.SubmitChanges();
            }
        }
    }
}