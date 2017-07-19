using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CVAWeb.Models.DAL;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;


namespace CVAWeb.Models.BLL
{
    public class LoginBLL
    {
        private const int keysize = 256;
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");
        

        public static string TenChuongTrinh = "Chương trình Quản lý trường phổ thông";
        static SchoolManagementDataContext DB = new SchoolManagementDataContext();
        private static string OperationSystem()
        {
            OperatingSystem  os=Environment.OSVersion;
            return os.VersionString;
        }
        public static int KiemTraDangNhap(string TaiKhoan,string MatKhau,string IP,string Name)
        {
            int Kt = 0;
            string quyenDangNhap;
            bool thanhCong;
            MatKhau = Encrypt(MatKhau,"CVA");
            TaiKhoan = TaiKhoan.ToUpper();
            DSHocSinh hs = DB.DSHocSinhs.Where(q => q.MaHocSinh == TaiKhoan).Where(q=>q.MatKhau==MatKhau).FirstOrDefault();
            DSGiaoVien gv = DB.DSGiaoViens.Where(q => q.MaGiaoVien == TaiKhoan).Where(q => q.MatKhau == MatKhau).FirstOrDefault();
            if (gv != null)
            {
                quyenDangNhap = "Giáo viên";
                thanhCong = true;
               
                //Management_Login_Web _management = new Management_Login_Web
                //{
                //    TaiKhoan = TaiKhoan,
                //    QuyenDangNhap = "Giáo viên",
                //    ThanhCong = true,
                //    ThoiGianDangNhap = DateTime.Now,
                //    TinhTrang = true,//van dang dang nhap
                //    IP_LAN = LanIP(),
                //    IP_Public = IP,
                //    Mac_Address = MacAddress(),
                //};
                
                if (DB.DSLops.Where(q => q.MaGVCN == gv.MaGiaoVien).FirstOrDefault() != null)//kiểm tra có phải giáo viên chủ nhiệm không
                    Kt = 1;
                else
                    Kt = 4;
            }
            else
                if (hs != null)
                {
                    if (hs.DangKyDichVu == true)
                    {
                        Kt = 2;
                        quyenDangNhap = "Học sinh đã đăng kí dịch vụ";
                        thanhCong = true;
                        //Management_Login_Web _management = new Management_Login_Web
                        //{
                        //    TaiKhoan = TaiKhoan,
                        //    QuyenDangNhap = "Học sinh đã đăng kí dịch vụ",
                        //    ThanhCong = true,
                        //    ThoiGianDangNhap = DateTime.Now,
                        //    TinhTrang = true,//van dang dang nhap
                        //    IP_LAN = LanIP(),
                        //    IP_Public = PublicIP(),
                        //    Mac_Address = MacAddress(),
                        //};
                        //ThemLogin(_management);
                    }
                    else
                    {
                        Kt = 3;

                        
                            quyenDangNhap = "Học sinh chưa đăng kí dịch vụ";
                            thanhCong = false;
                      
                    }
                }
                else
                {

                    quyenDangNhap = "Đăng nhập thất bại";
                        thanhCong = false;
                        
                   
                }
            Login log = new Login
            {
                TaiKhoan = TaiKhoan,
                QuyenDangNhap = quyenDangNhap,
                ThanhCong = thanhCong,
                IP_LAN = IP,
                Mac_Address = MacAddress(),
                User_Computer=Name,
                Operating_System = OperationSystem(),
                IP_Public = PublicIP(),
                ThoiGianDangNhap = DateTime.Now
            };
            ThemLogin(log);
            return Kt;
                
        }
        public static string Encrypt(string plainText, string passPhrase)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] cipherTextBytes = memoryStream.ToArray();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }
        
        public static string PublicIP()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return System.Web.HttpContext.Current.Request.UserHostAddress;;
        }
        public static string MacAddress()
        {
            string macAddresses = "";

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }
       
       
        public static void ThemLogin(Login log)
        {
            DB.Logins.InsertOnSubmit(log);
            DB.SubmitChanges();
        }
    }
}