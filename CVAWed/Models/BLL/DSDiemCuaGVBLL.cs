using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CVAWed.Models.DAL;
using System.Data;
using DevExpress.Web.Mvc;
using CVAWeb.Models.DAL;
using System.Collections;
namespace CVAWeb.Models.BLL
{
    public class DSDiemCuaGVBLL
    {
        static SchoolManagementDataContext DB = new SchoolManagementDataContext();

        private static string XacDinhMaHocKi()
        {
            DateTime dateTimeNow = DateTime.Now;
            string datetimeInSemester = DB.DSThangs.Where(q => q.ThoiGianGui >= dateTimeNow).Min(q => q.ThoiGianGui).ToString();
            string MaHK = DB.DSThangs.Where(q => q.ThoiGianGui == DateTime.Parse(datetimeInSemester)).Select(q => q.MaHocKy).FirstOrDefault().ToString();
            if (MaHK == "K2")
                MaHK = "K1";
            return MaHK;
        }
        public static IList LayListTenMonHocTheoLop(string maGiaoVien,string maHocKi,string menLop)
        {
            return (from phanCongGiangDay in DB.PhanCongGiangDays
                    join dsMonHoc in DB.DSMonHocs on phanCongGiangDay.MaMonHoc equals dsMonHoc.MaMonHoc
                    select new
                    {
                        MaGiaoVien = phanCongGiangDay.MaGiaoVien,
                        MaHocKi = phanCongGiangDay.MaHocKy,
                        TenMonHoc = dsMonHoc.TenMonHoc,
                        TenLop = phanCongGiangDay.DSLop.MaLop,
                        MaNamHoc = phanCongGiangDay.DSNamHoc.NamHienTai,
                        MaMonHoc=dsMonHoc.MaMonHoc
                    }
                   ).Where(q=>q.MaNamHoc==true).Where(q=>q.MaGiaoVien==maGiaoVien).Where(q=>q.TenLop==menLop).Where(q=>q.MaHocKi==maHocKi).ToList();
        }
        
        public static string TenHocKi()
        {
            return DB.DSHocKies.Where(q => q.MaHocKy == XacDinhMaHocKi()).Select(q => q.TenHocKy).FirstOrDefault().ToString();
        }
        private static DataTable TaoBang()
        {
            DataTable table = new DataTable();
            DataColumn collumMaHS = new DataColumn("MaHocSinh",typeof(string));
            DataColumn collumTenHS = new DataColumn("TenHocSinh", typeof(string));
            table.Columns.Add(collumMaHS);
            table.Columns.Add(collumTenHS);
            List<DSLoaiDiem> lisDsLoaiDiem = DB.DSLoaiDiems.ToList();
            foreach (DSLoaiDiem _LoaiDiem in lisDsLoaiDiem)
            {
                if (_LoaiDiem.SoDiemToiDa > 1)
                {
                    for (int i = 1; i <= _LoaiDiem.SoDiemToiDa; i++)
                    {
                        DataColumn collum = new DataColumn( _LoaiDiem.MaLoaiDiem + "_" + i.ToString(),typeof(string));
                        table.Columns.Add(collum);
                    }
                }
                else
                {
                    DataColumn collum = new DataColumn(_LoaiDiem.MaLoaiDiem + "_" + (1).ToString(),typeof(string));
                  
                    table.Columns.Add(collum);
                }
            }
            return table;
        }
        public static DataTable LoadDuLieuVaoBang(string MaLop, string MaMonHoc,string MaHocKi)
        {
            DataTable table = TaoBang();
            List<DSHocSinhTheoLop> dsHocSinhTheoLop = DB.DSHocSinhTheoLops.Where(q => q.MaLop == MaLop).Where(q => q.DSLop.DSNamHoc.NamHienTai == true).OrderBy(q=>q.STT).ToList();
            foreach (DSHocSinhTheoLop hs in dsHocSinhTheoLop)
            {
                DataRow row = table.NewRow();
                row["MaHocSinh"] = hs.MaHocSinh;
                row["TenHocSinh"] = hs.DSHocSinh.HoDem + " " + hs.DSHocSinh.Ten;
                List<DSDiem> listDiemCuaHS = DB.DSDiems.Where(q => q.MaHocSinh == hs.MaHocSinh).Where(q => q.MaMonHoc == MaMonHoc).Where(q => q.MaNamHoc == YearBLL.findIDNewYear()).Where(q => q.MaHocKy == MaHocKi).ToList();
                foreach (DSDiem diem in listDiemCuaHS)
                {
                    if (diem.Diem > -1)
                    {

                        row[diem.MaLoaiDiem + "_" + diem.STTDiem] = diem.Diem;

                    }
                    else
                        if (diem.Diem == -1)
                            row[diem.MaLoaiDiem + "_" + diem.STTDiem] = "-";
                        else
                            if (diem.Diem == -2)
                                row[diem.MaLoaiDiem + "_" + diem.STTDiem] = "Đạt";
                            else
                                if (diem.Diem == -3)
                                    row[diem.MaLoaiDiem + "_" + diem.STTDiem] = "Chưa đạt";

                }
                table.Rows.Add(row);
            }
            return table;
        }
        public static DSDiem TruyVanTheoMa(DSDiem Diem)
        {
            string MaNamHoc = YearBLL.findIDNewYear();
            return DB.DSDiems.Where(q => q.MaHocSinh == Diem.MaHocSinh && q.MaHocKy == Diem.MaHocKy && q.MaNamHoc == MaNamHoc && q.MaMonHoc == Diem.MaMonHoc && q.MaLoaiDiem == Diem.MaLoaiDiem && q.STTDiem == Diem.STTDiem).First();
        }
        public static bool KiemTraLaDiem(string Diem)
        {
            double tt;
            return double.TryParse(Diem, out tt);
        }
        public static double LayDiem(string Diem,string MaMonHoc)
        {
            double gt=-1;
            if((bool)LaChoDiem(MaMonHoc))
            {
                if (KiemTraLaDiem(Diem))
                    gt = double.Parse(Diem);
            }
            else
            {
                if (Diem == "Đ")
                    gt = -2;
                else
                    if (Diem == "CĐ")
                        gt = -3;
            }
            return gt;
        }
        public static void CapNhatNhieuDiem(DiemDAL listDiem,string maHocKi,string maMonHoc)
        {
            #region[Điểm miệng]
            DSDiem DiemMieng1 = new DSDiem
            {
                MaHocSinh=listDiem.MaHocSinh,
                MaHocKy=maHocKi,
                MaMonHoc=maMonHoc,
                MaLoaiDiem="LD1",
                Diem=LayDiem(listDiem.LD1_1,maMonHoc),
                STTDiem=1
            };
           
            CapNhatDiem(DiemMieng1);
            DSDiem DiemMieng2 = new DSDiem
            {
                MaHocSinh = listDiem.MaHocSinh,
                MaHocKy = maHocKi,
                MaMonHoc = maMonHoc,
                MaLoaiDiem = "LD1",
                Diem = LayDiem(listDiem.LD1_2, maMonHoc),
                STTDiem = 2
            };
           
            CapNhatDiem(DiemMieng2);



            DSDiem DiemMieng3 = new DSDiem
            {
                MaHocSinh = listDiem.MaHocSinh,
                MaHocKy = maHocKi,
                MaMonHoc = maMonHoc,
                MaLoaiDiem = "LD1",
                Diem = LayDiem(listDiem.LD1_3, maMonHoc),
                STTDiem = 3
            };
            


            CapNhatDiem(DiemMieng3);


            DSDiem DiemMieng4 = new DSDiem
            {
                MaHocSinh = listDiem.MaHocSinh,
                MaHocKy = maHocKi,
                MaMonHoc = maMonHoc,
                MaLoaiDiem = "LD1",
                Diem = LayDiem(listDiem.LD1_4, maMonHoc),
                STTDiem = 4
               
            };

           
            CapNhatDiem(DiemMieng4);
            #endregion
            #region[Điểm 15']
            DSDiem Diem15_1 = new DSDiem
            {
                MaHocSinh = listDiem.MaHocSinh,
                MaHocKy = maHocKi,
                MaMonHoc = maMonHoc,
                MaLoaiDiem = "LD2",
                Diem = LayDiem(listDiem.LD2_1, maMonHoc),
                STTDiem = 1,
                
            };
            CapNhatDiem(Diem15_1);

            DSDiem Diem15_2 = new DSDiem
            {
                MaHocSinh = listDiem.MaHocSinh,
                MaHocKy = maHocKi,
                MaMonHoc = maMonHoc,
                MaLoaiDiem = "LD2",
                Diem = LayDiem(listDiem.LD2_2, maMonHoc),
                STTDiem = 2,
            };
            CapNhatDiem(Diem15_2);
            DSDiem Diem15_3 = new DSDiem
            {
                MaHocSinh = listDiem.MaHocSinh,
                MaHocKy = maHocKi,
                MaMonHoc = maMonHoc,
                MaLoaiDiem = "LD2",
                Diem = LayDiem(listDiem.LD2_3, maMonHoc),
                STTDiem = 3,
                
            };
           

            CapNhatDiem(Diem15_3);
            DSDiem Diem15_4 = new DSDiem
            {
                MaHocSinh = listDiem.MaHocSinh,
                MaHocKy = maHocKi,
                MaMonHoc = maMonHoc,
                MaLoaiDiem = "LD2",
                Diem = LayDiem(listDiem.LD2_4, maMonHoc),
                STTDiem = 4,
               
            };
          
            CapNhatDiem(Diem15_4);
            #endregion
            #region[1 tiết]
            DSDiem Diem1Tiet_1 = new DSDiem
            {
                MaHocSinh = listDiem.MaHocSinh,
                MaHocKy = maHocKi,
                MaMonHoc = maMonHoc,
                MaLoaiDiem = "LD3",
                Diem = LayDiem(listDiem.LD3_1, maMonHoc),
                STTDiem = 1

            };
            
            CapNhatDiem(Diem1Tiet_1);

            DSDiem Diem1Tiet_2 = new DSDiem
            {
                MaHocSinh = listDiem.MaHocSinh,
                MaHocKy = maHocKi,
                MaMonHoc = maMonHoc,
                MaLoaiDiem = "LD3",
                Diem = LayDiem(listDiem.LD3_2, maMonHoc),
                STTDiem = 2
            };
           
            CapNhatDiem(Diem1Tiet_2);
            DSDiem Diem1Tiet_3 = new DSDiem
            {
                MaHocSinh = listDiem.MaHocSinh,
                MaHocKy = maHocKi,
                MaMonHoc = maMonHoc,
                MaLoaiDiem = "LD3",
                Diem = LayDiem(listDiem.LD3_3, maMonHoc),
                STTDiem = 3
            };
          
            CapNhatDiem(Diem1Tiet_3);
            DSDiem Diem1Tiet_4 = new DSDiem
            {
                MaHocSinh = listDiem.MaHocSinh,
                MaHocKy = maHocKi,
                MaMonHoc = maMonHoc,
                MaLoaiDiem = "LD3",
                Diem = LayDiem(listDiem.LD3_4, maMonHoc),
                STTDiem = 4
                           };
         
            CapNhatDiem(Diem1Tiet_4);
            DSDiem Diem1Tiet_5 = new DSDiem
            {
                MaHocSinh = listDiem.MaHocSinh,
                MaHocKy = maHocKi,
                MaMonHoc = maMonHoc,
                MaLoaiDiem = "LD3",
                Diem = LayDiem(listDiem.LD3_5, maMonHoc),
                STTDiem = 5,
                NgayChinhSua = DateTime.Now
            };
           
            CapNhatDiem(Diem1Tiet_5);
            #endregion
            #region[HK]
            DSDiem HK = new DSDiem
            {
                MaHocSinh = listDiem.MaHocSinh,
                MaHocKy = maHocKi,
                MaMonHoc = maMonHoc,
                MaLoaiDiem = "LD4",
                Diem = LayDiem(listDiem.LD4_1, maMonHoc),
                STTDiem = 1,
             
            };
           
            CapNhatDiem(HK);
            #endregion
            
        }
        public static string TimMaLopTheoMaHocSinh(string MaHocSinh)
        {
            return DB.DSHocSinhTheoLops.Where(q => q.MaHocSinh == MaHocSinh && q.DSLop.DSNamHoc.NamHienTai == true).Select(q => q.MaLop).ToString();
        }
        public static void TinhDiemTrungBinh(string maHocKy, string maLop, string maMonHoc)
        {
             string maNamHoc = YearBLL.findIDNewYear();
            List<DSLoaiDiem> LoaiDiems = DB.DSLoaiDiems.ToList();
            string[] maHocSinhTheoLops = maHocSinhTheoLop(maLop);
            if (LaChoDiem(maMonHoc) == true)
            {
                foreach (string maHocSinh in maHocSinhTheoLops)
                {
                   double? TongDiem = 0;
                    int? TongHeSo = 0;
                    string diem;
                    foreach (DSLoaiDiem LoaiDiem in LoaiDiems)
                    {
                       if (LoaiDiem.TinhToan == true)
                       {
                            double?[] Diems = LoadDSDiem(maHocKy, maMonHoc, maHocSinh, LoaiDiem.MaLoaiDiem);
                            TongDiem += Diems.Sum() * LoaiDiem.HeSo;
                            TongHeSo += Diems.Count() * LoaiDiem.HeSo;
                        }
                    }
                    string maLoaiDiemTongHop = LoadLoaiDiemTongHop();
                    DSDiem Diem = DB.DSDiems.Where(q => q.MaNamHoc == maNamHoc && q.MaHocKy == maHocKy && q.MaMonHoc == maMonHoc && q.MaLoaiDiem == maLoaiDiemTongHop && q.MaHocSinh == maHocSinh).First();
                    diem = Math.Round((double)(TongDiem / TongHeSo), 1, MidpointRounding.AwayFromZero).ToString();
                    Diem.NgayCapNhat = DateTime.Now;
                    if(KiemTraLaDiem(diem)&&diem!="NaN")
                    Diem.Diem = double.Parse(diem);
                    DB.SubmitChanges();
                }
            }
            else
            {
                foreach (string maHocSinh in maHocSinhTheoLops)
                {
                    int TongSoDat = 0;
                    int TongSoBaiKT = 0;
                    foreach (DSLoaiDiem LoaiDiem in LoaiDiems)
                    {
                        if (LoaiDiem.TinhToan == true)
                        {
                            double?[] Diems = LoadDSDiem(maHocKy, maMonHoc, maHocSinh, LoaiDiem.MaLoaiDiem);
                            foreach (double? diem in Diems)
                            {
                                if (diem == -2)
                                {
                                    TongSoDat++;
                                }
                            }
                            TongSoBaiKT += Diems.Count();
                        }
                    }
                    DSDiem DiemHK = DB.DSDiems.Where(q => q.MaNamHoc == maNamHoc && q.MaHocKy == maHocKy && q.MaMonHoc == maMonHoc && q.MaLoaiDiem==MaDiemHk() && q.MaHocSinh == maHocSinh).FirstOrDefault();
                    string maLoaiDiemTongHop = LoadLoaiDiemTongHop();
                    DSDiem Diem = DB.DSDiems.Where(q => q.MaNamHoc == maNamHoc && q.MaHocKy == maHocKy && q.MaMonHoc == maMonHoc && q.MaLoaiDiem == maLoaiDiemTongHop && q.MaHocSinh == maHocSinh).FirstOrDefault();
                    Diem.NgayCapNhat = DateTime.Now;
                    if (((float)TongSoDat / TongSoBaiKT) >= (0.6666) && (DiemHK.Diem == -2))
                        Diem.Diem = -2;
                    else
                        Diem.Diem = -3;
                   
                }
              
            }
            
           
        }
        public static string MaDiemHk()
        {
            return DB.DSLoaiDiems.Where(q => q.LaHocKy == true).Select(q => q.MaLoaiDiem).FirstOrDefault().ToString();
        }
        public static string[] maHocSinhTheoLop(string maLop)
        {
            return DB.DSHocSinhTheoLops.Where(q => q.MaLop == maLop).Select(q => q.MaHocSinh).ToArray();
        }

        public static double?[] LoadDSDiem(string maHocKy, string maMonHoc, string maHocSinh, string maLoaiDiem)
        {
            string maNamHoc = YearBLL.findIDNewYear();
            return DB.DSDiems.Where(q => q.MaNamHoc == maNamHoc && q.MaHocKy == maHocKy && q.MaMonHoc == maMonHoc && q.MaHocSinh == maHocSinh && q.MaLoaiDiem == maLoaiDiem && q.Diem != -1).Select(q => q.Diem).ToArray();
        }
        public static string LoadLoaiDiemTongHop()
        {
            return DB.DSLoaiDiems.Where(q => q.TinhToan == false).Select(q => q.MaLoaiDiem).FirstOrDefault().ToString();
        }
        public static void CapNhatDiem(DSDiem _Diem)
        {
            DSDiem diem = TruyVanTheoMa(_Diem);
           
            if (diem.Diem== -1&&_Diem.Diem!=-1)
                diem.NgayCapNhat =DateTime.Now;
                if(diem.Diem!=-1&&diem.Diem!=_Diem.Diem)
                diem.NgayChinhSua = DateTime.Now;
                if (_Diem.Diem >= 0 && _Diem.Diem <= 10 || _Diem.Diem == -1 || _Diem.Diem == -2 || _Diem.Diem == -3)
                {
                    diem.Diem = _Diem.Diem;
                    DB.SubmitChanges();

                }
             
        }
        public static bool? LaChoDiem(string maMonHoc)
        {
            return DB.DSMonHocs.Where(q => q.MaMonHoc == maMonHoc).Select(q => q.DSHinhThucDanhGia.TinhDiem).First();
        }
        public static int SoLuongHocSinh(string maLop)
        {
            return DB.DSHocSinhTheoLops.Where(q => q.MaLop == maLop).Count();
        }
        public static string CatMaLoaiDiem(string tenTruong)
        {
            byte i = 0;
            for (; i < tenTruong.Length; i++)
            {
                if (tenTruong[i] == '_')
                    break;
            }
            return tenTruong.Substring(0, i);
        }

        public static byte CatSoTT(string tenTruong)
        {
            byte i = 0;
            for (; i < tenTruong.Length; i++)
                if (tenTruong[i] == '_')
                    break;
            return byte.Parse(tenTruong.Substring(i + 1, tenTruong.Length - i - 1));
        }
        
        //public static string CapNhatDiem(MVCxGridView gvDiem)
        //{
        //    string maNamHoc = YearBLL.findIDNewYear();
        //    return gvDiem.GetRowValues(1, gvDiem.Columns[3].Name.ToString()).ToString();
        //    //float tt;
        //    //if (LaChoDiem(maMonHoc) == true)
        //    //{
        //    //    for (int i = 0; i<SoLuongHocSinh(maLop); i++)
        //    //    {

        //    //        //string maHocSinh = gvDiem.GetRowCellValue(i, gvDiem.Columns[1]).ToString();
        //    //        string maHocSinh=gvDiem.GetRowValues(i,gvDiem.Columns[1].Name).ToString();
        //    //        for (byte j = 4; j < gvDiem.Columns.Count; j++)
        //    //        {
        //    //            //string cellValue = gvDiem.GetRowCellValue(i, gvDiem.Columns[j]).ToString();
        //    //            string cellValue = gvDiem.GetRowValues(i, gvDiem.Columns[j].Name).ToString();
        //    //            string maLoaiDiem = CatMaLoaiDiem(gvDiem.Columns[j].Name);
        //    //            byte STT = CatSoTT(gvDiem.Columns[j].Name);
        //    //            DSDiem Diem = DiemCon(maHocSinh, maNamHoc, maHocKi, maMonHoc, maLoaiDiem, STT);
        //    //            float diemCon = -1;
        //    //            if (cellValue == "-")
        //    //                diemCon = -1;
        //    //            else
        //    //                if (float.TryParse(cellValue, out tt))
        //    //                    diemCon = float.Parse(cellValue);
        //    //            if ((Diem.Diem == -1) && (diemCon != -1))
        //    //            {
        //    //                Diem.NgayCapNhat = DateTime.Today;
        //    //                //Diem.NgayChinhSua = DateTime.Today;
        //    //            }
        //    //            if (diemCon != Diem.Diem)
        //    //            {
        //    //                Diem.Diem = Math.Round(diemCon, 2);
        //    //                Diem.NgayChinhSua = DateTime.Today;
        //    //            }
        //    //            DB.SubmitChanges();
        //    //            //UTL.Ultils.ThongBao(Diem.MaHocSinh);

        //    //        }
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    for (int i = 0; i < SoLuongHocSinh(maLop); i++)
        //    //    {
        //    //        string maHocSinh = gvDiem.GetRowValues(i, gvDiem.Columns[1].Name).ToString();
        //    //        for (byte j = 4; j < gvDiem.Columns.Count; j++)
        //    //        {
        //    //            string cellValue = gvDiem.GetRowValues(i, gvDiem.Columns[j].Name).ToString();
        //    //            float diemCon = -1;
        //    //            if (cellValue == "Đ")
        //    //                diemCon = -2;
        //    //            if (cellValue == "CĐ")
        //    //                diemCon = -3;
        //    //            string maLoaiDiem = CatMaLoaiDiem(gvDiem.Columns[j].Name);
        //    //            byte STT = CatSoTT(gvDiem.Columns[j].Name);
        //    //            DSDiem Diem = DiemCon(maHocSinh, maNamHoc, maHocKi, maMonHoc, maLoaiDiem, STT);

        //    //            if (Diem.Diem == -1)
        //    //            {
        //    //                Diem.NgayCapNhat = DateTime.Today;
        //    //                //Diem.NgayChinhSua = DateTime.Today;
        //    //            }
        //    //            if (diemCon != Diem.Diem)
        //    //            {
        //    //                Diem.Diem = diemCon;
        //    //                Diem.NgayChinhSua = DateTime.Today;
        //    //            }
        //    //            DB.SubmitChanges();
        //    //            //UTL.Ultils.ThongBao(Diem.MaHocSinh);                        
        //    //        }
        //    //    }
        //    //}
        //}
    }
}