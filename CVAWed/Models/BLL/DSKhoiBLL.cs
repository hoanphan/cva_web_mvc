using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CVAWeb.Models.DAL;
using System.Collections;
namespace CVAWeb.Models.BLL
{
    public class DSKhoiBLL
    {
        static SchoolManagementDataContext DB = new SchoolManagementDataContext();
        public static List<DSLop> DsLop(string Khoi)
        {
            return DB.DSLops.Where(q=>q.MaKhoi==Khoi).ToList();
        }
        public static int LayRaViTriLop(string MaLop,string MaKhoi)
        {
            int vt = 0;
            for(int i=0;i<DsLop(MaKhoi).Count;i++)
            { 
                if(DsLop(MaKhoi)[i].MaLop==MaLop)
                {
                    vt = i;
                    break;
                }
            }
            return vt;

        }
        public static List<DSKhoi> DanhSachKhoiDuocPhanCong(string MaGV, string MaHocKi)
        {
            return DB.PhanCongGiangDays.Where(q => q.MaHocKy == MaHocKi && q.MaGiaoVien == MaGV).Select(q => q.DSLop.DSKhoi).Distinct().OrderBy(q => q.TenKhoi).ToList();
        }
        public static DSKhoi KhoiDauTien(string MaGV, string MaHocKi)
        {
            return DB.PhanCongGiangDays.Where(q => q.MaHocKy == MaHocKi && q.MaGiaoVien == MaGV).Select(q => q.DSLop.DSKhoi).Distinct().OrderBy(q => q.TenKhoi).FirstOrDefault();
        }
        public static int LayRaViTriKhoi(string MaKhoi,string MaGV, string MaHocKi)
        {
            int vt = 0;
            var khoi = DB.PhanCongGiangDays.Where(q => q.MaHocKy == MaHocKi && q.MaGiaoVien == MaGV).Select(q => q.DSLop.DSKhoi).Distinct().OrderBy(q => q.TenKhoi).ToList();
            for (int i=0;i<khoi.Count;i++)
            {
                if(khoi[i].MaKhoi==MaKhoi)
                {
                    vt = i;
                    break;
                }
            }
            return vt;

        }
        public static List<DSHocKy> dsHocKi()
        {
            return DB.DSHocKies.Where(q => q.TongHop == false).ToList();
        }
        public static int LayViTriHocKy(string MaHocKi)
        {
            int vt=0;
            var dl = DB.DSHocKies.Where(q => q.TongHop == false).ToList();
            for(int i=0;i<dl.Count;i++)
            {
                if(dl[i].MaHocKy==MaHocKi)
                {
                    vt = i;
                }
            }
            return vt;
        }
        public static bool ChoPhepKhoaSo(string MaHocKi)
        {
            var soLuongKhoaSo = DB.DSDiems.Where(q => q.MaHocKy == MaHocKi && q.MaNamHoc == YearBLL.findIDNewYear() && q.KhoaSo == true).Count();
            if (soLuongKhoaSo > 0)
                return true;
            return false;
        }
        public static string LayRaTenHocKi(string MaHK)
        {
            return DB.DSHocKies.Where(q => q.MaHocKy == MaHK).Select(q => q.TenHocKy).FirstOrDefault().ToString();
        }
    }
}