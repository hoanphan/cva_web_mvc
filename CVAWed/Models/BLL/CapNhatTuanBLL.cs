using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CVAWeb.Models.DAL;
namespace CVAWeb.Models.BLL
{
    public class CapNhatTuanBLL
    {
        static SchoolManagementDataContext DB = new SchoolManagementDataContext();
        private static bool KiemTraSuTonTaiCuaTuan(int MaTuan)
        {
            if (DB.DSTuans.Where(q => q.MaTuan == MaTuan).Where(q => q.DSNamHoc.NamHienTai == true).FirstOrDefault() == null)
                return true;
            else
                return false;
        }
        public static DSTuan TruyVanTheoMa(int MaTuan)
        {
            return DB.DSTuans.Where(q => q.MaTuan == MaTuan).Where(q => q.DSNamHoc.NamHienTai == true).FirstOrDefault();
        }
        public static void CapNhatLaiTuan()
        {
            DateTime NgayThangNamHienTai = DateTime.Now;
            DateTime NgayBatDauCuaNamHoc = (DateTime)DB.DSTuans.Where(q => q.DSNamHoc.NamHienTai == true).Where(q => q.MaTuan == 1).Select(q => q.BatDauTuNgay).First();
            int count = 2;
            DateTime NgayCuoiTuan;
            //CapNhatLaiNgay
            if (NgayBatDauCuaNamHoc.DayOfWeek == DayOfWeek.Monday)
            {
                NgayCuoiTuan = NgayBatDauCuaNamHoc.AddDays(5);
                DSTuan DS = TruyVanTheoMa(1);
                DS.KetThucNgay = (DateTime)NgayCuoiTuan;
                DB.SubmitChanges();
                NgayBatDauCuaNamHoc = NgayBatDauCuaNamHoc.AddDays(7);
                while (NgayBatDauCuaNamHoc < NgayThangNamHienTai)
                {
                    if (KiemTraSuTonTaiCuaTuan(count))
                    {
                        NgayCuoiTuan = NgayBatDauCuaNamHoc.AddDays(5);
                        DSTuan DS2 = new DSTuan
                        {
                            MaTuan = count,
                            TenTuan = ("Tuần " + count).Trim().ToString(),
                            BatDauTuNgay = NgayBatDauCuaNamHoc,
                            KetThucNgay = NgayCuoiTuan,
                            MaNamHoc = YearBLL.findIDNewYear()
                        };
                        DB.DSTuans.InsertOnSubmit(DS2);
                        DB.SubmitChanges();

                    }
                    NgayBatDauCuaNamHoc = NgayBatDauCuaNamHoc.AddDays(7);
                    count++;
                }
            }
            else
            {
                for (int i = 1; i < 5; i++)
                {
                    if (NgayBatDauCuaNamHoc.AddDays(i).DayOfWeek == DayOfWeek.Saturday)
                    {
                        NgayCuoiTuan = NgayBatDauCuaNamHoc.AddDays(i);

                        DSTuan DS = TruyVanTheoMa(1);
                        DS.KetThucNgay = (DateTime)NgayCuoiTuan;
                        DB.SubmitChanges();
                        NgayBatDauCuaNamHoc = NgayBatDauCuaNamHoc.AddDays(i + 2);
                    }
                }
                while (NgayBatDauCuaNamHoc < NgayThangNamHienTai)
                {
                    if (KiemTraSuTonTaiCuaTuan(count))
                    {
                        NgayCuoiTuan = NgayBatDauCuaNamHoc.AddDays(5);
                        DSTuan DS2 = new DSTuan
                        {
                            MaTuan = count,
                            TenTuan = ("Tuần " + count).Trim().ToString(),
                            BatDauTuNgay = NgayBatDauCuaNamHoc,
                            KetThucNgay = NgayCuoiTuan,
                            MaNamHoc = YearBLL.findIDNewYear()
                        };
                        DB.DSTuans.InsertOnSubmit(DS2);
                        DB.SubmitChanges();
                    }
                    NgayBatDauCuaNamHoc = NgayBatDauCuaNamHoc.AddDays(7);
                    count++;
                }
            }
        }
    }
}