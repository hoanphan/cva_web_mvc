using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CVAWeb.Models.DAL;
using System.Data;
namespace CVAWeb.Models.BLL
{
    public class DSTuanBLL
    {
       static SchoolManagementDataContext DB = new SchoolManagementDataContext();
        public static DataTable LoadListWeek()
       {
           DataTable table = new DataTable();
           DataColumn STT = new DataColumn("MaTuan");
           DataColumn TenTuan = new DataColumn("TenTuan");
           DataColumn BatDauTuNgay = new DataColumn("BatDauTuNgay");
           DataColumn KetThucNgay = new DataColumn("KetThucNgay");
           table.Columns.Add(STT);
           table.Columns.Add(TenTuan);
           table.Columns.Add(BatDauTuNgay);
           table.Columns.Add(KetThucNgay);
           var listWeek = DB.DSTuans.Where(q => q.DSNamHoc.NamHienTai == true).OrderByDescending(q => q.MaTuan).ToList();
            for(int i=0;i<listWeek.Count;i++)
            {
                DataRow row = table.NewRow();
                row["MaTuan"] = listWeek[i].MaTuan;
                row["TenTuan"] = listWeek[i].TenTuan;
                row["BatDauTuNgay"] = SoLienLacDienTuCuaHocSinh.ChuyenDoiNgayThangNam(DateTime.Parse(listWeek[i].BatDauTuNgay.ToString()));
                row["KetThucNgay"] = SoLienLacDienTuCuaHocSinh.ChuyenDoiNgayThangNam(DateTime.Parse(listWeek[i].KetThucNgay.ToString()));
                table.Rows.Add(row);
            }
            return table;
       }
        public static string LayTuanGanNhat()
        {
            return DB.DSTuans.Where(q => q.DSNamHoc.NamHienTai == true).Max(q => q.MaTuan).ToString();


        }
    }

}