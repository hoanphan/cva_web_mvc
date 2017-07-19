using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CVAWeb.Models.DAL;
namespace CVAWeb.Models.BLL
{
    public class PhanCongGiangDayBLL
    {
       static SchoolManagementDataContext DB = new SchoolManagementDataContext();
        public static List<DSLop> LayLopTheoMaHocKiMaKhoi(string maHocKi,string maKhoi,string MaGV)
       {
           return DB.PhanCongGiangDays.Where(q => q.MaHocKy == maHocKi && q.DSLop.DSKhoi.MaKhoi == maKhoi&&q.MaGiaoVien==MaGV).Select(q => q.DSLop).Distinct().OrderBy(q => q.TenLop).ToList();
       }
        public static bool KiemTraPhanCongGiangDayTrongHocKi(string MaHocKi,string MaGV)
        {
            try
            {
                PhanCongGiangDay Ds = DB.PhanCongGiangDays.Where(q => q.MaGiaoVien == MaGV && q.MaHocKy == MaHocKi && q.DSNamHoc.NamHienTai == true).First();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static DSLop LayLopDauTienChuaCoKhoi(string maHocKi, string MaGV)
        {
            return DB.PhanCongGiangDays.Where(q => q.MaHocKy == maHocKi && q.DSLop.DSKhoi.MaKhoi == DSKhoiBLL.KhoiDauTien(MaGV,maHocKi).MaKhoi && q.MaGiaoVien == MaGV).Select(q => q.DSLop).Distinct().OrderBy(q => q.TenLop).FirstOrDefault();
        }
        public static DSLop LayLopDauTienCoKhoi(string maHocKi, string MaGV,string Khoi)
        {
            return DB.PhanCongGiangDays.Where(q => q.MaHocKy == maHocKi && q.DSLop.DSKhoi.MaKhoi == Khoi && q.MaGiaoVien == MaGV).Select(q => q.DSLop).Distinct().OrderBy(q => q.TenLop).FirstOrDefault();
        }
        
        public static List<DSMonHoc> LayMonHocTheoLopMaHocKiMaGiaoVien(string maLop, string maHocKy, string maGiaoVien)
        {
            return DB.PhanCongGiangDays.Where(q => q.MaLop == maLop && q.MaHocKy == maHocKy && q.MaGiaoVien == maGiaoVien).Select(q => q.DSMonHoc).OrderBy(q => q.MaMonHoc).ToList();
        }
        public static DSMonHoc LayMonHocTheoLopMaHocKiMaGiaoVienDauTien(string maLop, string maHocKy, string maGiaoVien)
        {
            return DB.PhanCongGiangDays.Where(q => q.MaLop == maLop && q.MaHocKy == maHocKy && q.MaGiaoVien == maGiaoVien).Select(q => q.DSMonHoc).OrderBy(q => q.MaMonHoc).FirstOrDefault();
        }
        public static int LayRaViTriMonHoc(string maLop, string maHocKy, string maGiaoVien,string MaMonHoc)
        {
            int gt=0;
            var DsMonHoc = LayMonHocTheoLopMaHocKiMaGiaoVien(maLop, maHocKy, maGiaoVien);
            for(int i=0;i<DsMonHoc.Count;i++)
            {
                if (DsMonHoc[i].MaMonHoc == MaMonHoc)
                    gt = i;
            }
            return gt;
        }
    }
}