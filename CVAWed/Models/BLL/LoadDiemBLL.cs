using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CVAWeb.Models.DAL;
using System.Data;
namespace CVAWeb.Models.BLL
{
    public class LoadDiemBLL
    {
        static SchoolManagementDataContext DB = new SchoolManagementDataContext();
        public static DSHocSinh MaLop(string MaHocSinh)
        {
            return DB.DSHocSinhs.Where(q => q.MaHocSinh == MaHocSinh).Where(q => q.DSHocSinhTheoLop.DSLop.DSNamHoc.NamHienTai == true).FirstOrDefault();
        }
        public static DataTable TaoBang()
        {
            DataTable table = new DataTable();
          
            DataColumn Collum1 = new DataColumn("TenMonHoc");
            DataColumn CollumSTT = new DataColumn("STT");
            table.Columns.Add(CollumSTT);
            table.Columns.Add(Collum1);
            var dsLoaiDiem = DB.DSLoaiDiems.ToList();
            foreach (DSLoaiDiem _dsLoaiDiem in dsLoaiDiem)
            {
                if (_dsLoaiDiem.SoDiemToiDa > 1)
                    for (int i = 1; i <= _dsLoaiDiem.SoDiemToiDa; i++)
                    {
                        DataColumn Collum = new DataColumn(_dsLoaiDiem.MaLoaiDiem + i.ToString());
                        table.Columns.Add(Collum);
                    }
                else
                    if (_dsLoaiDiem.SoDiemToiDa == 1)
                    {
                        DataColumn Collum = new DataColumn(_dsLoaiDiem.MaLoaiDiem + _dsLoaiDiem.SoDiemToiDa.ToString());
                        table.Columns.Add(Collum);
                    }
            }
            DataColumn Collum2 = new DataColumn("HocKi");
            table.Columns.Add(Collum2);
            return table;
        }
        public static DataTable LoadDiem(string MaHocSinh)
        {
            DataTable table = TaoBang();
                int stt = 1;
                var DsHocKi = DB.DSHocKies.ToList();
                string MaLop = DB.DSHocSinhTheoLops.Where(q => q.MaHocSinh == MaHocSinh).Select(q => q.MaLop).FirstOrDefault().ToString();
               
                foreach (DSHocKy dsHK in DsHocKi)
                {
                        var dsMonHoc = DB.DSMonHocTheoLops.Where(q => q.MaLop == MaLop).Where(q=>q.MaHocKy==dsHK.MaHocKy).ToList();
                            foreach (DSMonHocTheoLop _dsMonHoc in dsMonHoc)
                            {
                                DataRow row = table.NewRow();
                                row["TenMonHoc"] = _dsMonHoc.DSMonHoc.TenMonHoc.ToString();
                                row["STT"] = stt;
                                var dsDiem = DB.DSDiems.Where(q => q.MaHocSinh == MaHocSinh&&q.MaNamHoc==YearBLL.findIDNewYear()).Where(q => q.MaHocKy == dsHK.MaHocKy).Where(q=>q.MaMonHoc==_dsMonHoc.MaMonHoc).ToList();
                                foreach(DSDiem _dsDiem in dsDiem)
                                if (_dsDiem != null)
                                {
                                   
                                    if (_dsDiem.Diem > -1)
                                        row[_dsDiem.MaLoaiDiem + _dsDiem.STTDiem.ToString()] = _dsDiem.Diem;
                                    else
                                        if (_dsDiem.Diem == -1)
                                            row[_dsDiem.MaLoaiDiem + _dsDiem.STTDiem.ToString()] = "-";
                                        else
                                            if (_dsDiem.Diem == -2)
                                                row[_dsDiem.MaLoaiDiem + _dsDiem.STTDiem.ToString()] = "Đạt";
                                            else
                                                row[_dsDiem.MaLoaiDiem + _dsDiem.STTDiem.ToString()] = "Chưa đạt";
                                    row["HocKi"] = DSKhoiBLL.LayRaTenHocKi(_dsDiem.MaHocKy);
                                    stt++;
                                }
                                table.Rows.Add(row);
                          
                    }
                }
            //Load Ca Nam
                var dsMonHoc2 = DB.DSMonHocTheoLops.Where(q => q.MaLop == MaLop).Where(q => q.MaHocKy == "K1").ToList();
                foreach (DSMonHocTheoLop _dsMonHoc in dsMonHoc2)
                {
                    DataRow row = table.NewRow();
                    row["TenMonHoc"] = _dsMonHoc.DSMonHoc.TenMonHoc.ToString();
                    row["STT"] = stt;
                    var dsDiem = DB.DSDiems.Where(q => q.MaHocSinh == MaHocSinh && q.MaNamHoc == YearBLL.findIDNewYear()).Where(q => q.MaHocKy == "K3").Where(q => q.MaMonHoc == _dsMonHoc.MaMonHoc).ToList();
                    foreach (DSDiem _dsDiem in dsDiem)
                        if (_dsDiem != null)
                        {

                            if (_dsDiem.Diem > -1)
                                row[_dsDiem.MaLoaiDiem + _dsDiem.STTDiem.ToString()] = _dsDiem.Diem;
                            else
                                if (_dsDiem.Diem == -1)
                                    row[_dsDiem.MaLoaiDiem + _dsDiem.STTDiem.ToString()] = "-";
                                else
                                    if (_dsDiem.Diem == -2)
                                        row[_dsDiem.MaLoaiDiem + _dsDiem.STTDiem.ToString()] = "Đạt";
                                    else
                                        row[_dsDiem.MaLoaiDiem + _dsDiem.STTDiem.ToString()] = "Chưa đạt";
                            row["HocKi"] = DSKhoiBLL.LayRaTenHocKi(_dsDiem.MaHocKy);
                            stt++;
                        }
                    table.Rows.Add(row);

                }
            return table;
        }
        public static DataRow row()
        {
            DataTable table = TaoBang();
            return table.NewRow();
        }
    }
}