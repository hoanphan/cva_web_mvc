using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CVAWeb.Models.DAL;
namespace CVAWeb.Models.BLL
{
    public class ElectricBookBLL
    {
        static SchoolManagementDataContext DB = new SchoolManagementDataContext();
        public static string TimMaLopGiaoVienChuNhiem(string MaGV)
        {
            return DB.DSLops.Where(q => q.MaGVCN == MaGV).Select(q => q.MaLop).FirstOrDefault();
        }
        public static bool chectExitsElectricBook(string idStudent, int idWeek)
        {
            SoLienLacDienTu DS = DB.SoLienLacDienTus.Where(q => q.MaHocSinh == idStudent).Where(q => q.DSTuan.DSNamHoc.NamHienTai == true).Where(q => q.MaTuan == idWeek).FirstOrDefault();
            if (DS != null)
                return true;
            else return false;
        }
         public static SoLienLacDienTu TruyVanSoLienLacDienTuTheoMa(string _idStudent, int _idWeek)
        {
            return DB.SoLienLacDienTus.Where(q => q.MaHocSinh == _idStudent).Where(q => q.DSTuan.DSNamHoc.NamHienTai==true).Where(q => q.MaTuan == _idWeek).FirstOrDefault();
        }
        public static void ThemSoLienLacDienTu(string maHocSinh, int maTuan,string noidung)
        {
            SoLienLacDienTu DS = new SoLienLacDienTu
            {
                MaHocSinh = maHocSinh,
                MaTuan = maTuan,
                NoiDung=noidung,
                MaNamHoc =YearBLL.findIDNewYear()
            };
            DB.SoLienLacDienTus.InsertOnSubmit(DS);
            DB.SubmitChanges();
        }
        public static string ThongTinCuaSoLienLacDienTu(string MaHocSinh, int MaTuan)
        {
            SoLienLacDienTu Ds = DB.SoLienLacDienTus.Where(q => q.MaHocSinh == MaHocSinh).Where(q => q.DSTuan.DSNamHoc.NamHienTai == true).Where(q => q.MaTuan == MaTuan).FirstOrDefault();
            if (Ds != null)
                return Ds.NoiDung;
            else
                return null;
        }
        public static void SuaSoLienLac(SoLienLacDienTu Ds)
        {
            SoLienLacDienTu DS = DB.SoLienLacDienTus.Where(q => q.MaHocSinh == Ds.MaHocSinh).Where(q => q.DSTuan.DSNamHoc.NamHienTai==true).Where(q => q.MaTuan == Ds.MaTuan).FirstOrDefault();
            DS = Ds;
            DB.SubmitChanges();
        }
        public static void XoaSoLienLac(string MaHS, int MaTuan)
        {
            SoLienLacDienTu DS = TruyVanSoLienLacDienTuTheoMa(MaHS,MaTuan);
            DB.SoLienLacDienTus.DeleteOnSubmit(DS);
            DB.SubmitChanges();
        }
        public static bool KiemTraDuocSuaDoi(string MaGV, string MaHS)
        {
            string MaLop = DB.DSHocSinhTheoLops.Where(q => q.MaHocSinh == MaHS).Where(q => q.DSLop.DSNamHoc.NamHienTai == true).Select(q => q.MaLop).FirstOrDefault();
            if (DB.DSLops.Where(q => q.MaGVCN == MaGV).Where(q => q.MaLop == MaLop).FirstOrDefault() != null)
                return true;
            else
                return false;
        }
    }
    }
