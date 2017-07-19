using System;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;

namespace UTL
{
    public class Ultils
    {
        private const int keysize = 256;
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");
        public static string strKey = UTL.Ultils.strKey;

        public static string TenChuongTrinh = "Chương trình Quản lý trường phổ thông";
        /// <summary>
        /// Phương thức xác thực việc xóa một bản ghi khỏi cơ sở dữ liệu
        /// </summary>
        /// <param name="doiTuong">
        /// Chuỗi chứa tên Tiếng Việt của đối tượng cần xóa để hỏi đáp
        /// </param>
        /// <returns>
        /// true: nếu đồng ý xóa
        /// false: nếu không đồng ý xóa hoặc hủy lệnh xóa
        /// </returns>
        public static bool XacThucXoa(string doiTuong)
        {
            string thongDiep = "Bạn có chắc chắn muốn xóa " + doiTuong + " đã chọn không?";
            DialogResult question = MessageBox.Show(thongDiep, Params.Title, MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
            if (question == DialogResult.Yes)
                return true;
            else
                return false;
        }

        public static bool XacThucNhapDuThongTin()
        {
            string thongDiep = "Hãy chắc chắn rằng bạn đã nhập đủ các thông tin như: Phân công môn học theo lớp, Phân công giảng dạy. " +
                             "Bạn có chắc chắn rằng mình đã nhập đủ các thông tin trên.";
            DialogResult question = MessageBox.Show(thongDiep, Params.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (question == DialogResult.Yes)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Phương thức xuất dữ liệu từ GridControl ra tệp tin excel, pdf, text, html
        /// </summary>
        /// <param name="dgvGrid"></param>
        /// Tham số là GridControl cần xuất dữ liệu
        public static void XuatRaExcel(GridControl dgvGrid)
        {
            try
            {
                string fileName = "";
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Reset();
                saveFileDialog.Title = "Xuất dữ liệu";
                saveFileDialog.Filter = "Excel file(*.xls)|*.xls|Excel file(*.xlsx)|*.xls|Pdf files (*.pdf)|*.pdf|Html files (*.html)|*.html|Text files (*.txt)|*.txt";
                saveFileDialog.ShowDialog();
                fileName = saveFileDialog.FileName;
                if (!string.IsNullOrEmpty(fileName))
                {
                    string str3 = fileName.Substring(fileName.IndexOf(".") + 1);
                    if (str3 != null)
                    {
                        if (!(str3 == "xls"))
                        {
                            if (str3 == "xlsx")
                            {
                                goto Label_00E7;
                            }
                            if (str3 == "pdf")
                            {
                                goto Label_0115;
                            }
                            if (str3 == "html")
                            {
                                goto Label_0140;
                            }
                            if (str3 == "txt")
                            {
                                goto Label_016B;
                            }
                        }
                        else if (!saveFileDialog.CheckFileExists)
                        {
                            dgvGrid.ExportToXls(fileName);
                        }
                        else if (saveFileDialog.OverwritePrompt)
                        {
                            dgvGrid.ExportToXls(fileName);
                        }
                    }
                }
                return;
            Label_00E7:
                if (!saveFileDialog.CheckFileExists)
                {
                    dgvGrid.ExportToXlsx(fileName);
                }
                else if (saveFileDialog.OverwritePrompt)
                {
                    dgvGrid.ExportToXlsx(fileName);
                }
                return;
            Label_0115:
                if (!saveFileDialog.CheckFileExists)
                {
                    dgvGrid.ExportToPdf(fileName);
                }
                else if (saveFileDialog.OverwritePrompt)
                {
                    dgvGrid.ExportToPdf(fileName);
                }
                return;
            Label_0140:
                if (!saveFileDialog.CheckFileExists)
                {
                    dgvGrid.ExportToHtml(fileName);
                }
                else if (saveFileDialog.OverwritePrompt)
                {
                    dgvGrid.ExportToHtml(fileName);
                }
                return;
            Label_016B:
                if (!saveFileDialog.CheckFileExists)
                {
                    dgvGrid.ExportToText(fileName);
                }
                else if (saveFileDialog.OverwritePrompt)
                {
                    dgvGrid.ExportToText(fileName);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi trong qu\x00e1 tr\x00ecnh xuất dữ liệu" + exception.ToString());
                throw;
            }

        }

        ///
        ///
        ///
        public static string CatHoDem(string hoVaTen)
        {
            hoVaTen = ChuanHoa(hoVaTen);
            int vt = 0;
            for (int i = hoVaTen.Length - 2; i > 0; i--)
                if (hoVaTen[i] == ' ')
                {
                    vt = i;
                    break;
                }
            return hoVaTen.Substring(0, vt);
        }

        public static string CatTen(string hoVaTen)
        {
            hoVaTen = ChuanHoa(hoVaTen);
            int vt = 0;
            for (int i = hoVaTen.Length - 2; i > 0; i--)
                if (hoVaTen[i] == ' ')
                {
                    vt = i;
                    break;
                }
            return hoVaTen.Substring(vt + 1, hoVaTen.Length - vt - 1);
        }

        public static string ChuanHoa(string str)
        {            
            str = str.Trim();
            while (str.IndexOf("\t") >= 0)
            {
                str = str.Replace("\t", " ");
            }
            while (str.IndexOf("  ") >= 0)
            {
                str = str.Replace("  ", " ");
            }
            string[] arrStr = str.Split(' ');
            string s = "";
            foreach (string item in arrStr)
            {
                s += item.Substring(0, 1).ToUpper() + item.Substring(1).ToLower() + " ";

            }
            return s;
        }

        public static byte CatKhoi(string maKhoi)
        {
            return byte.Parse(maKhoi.Substring(1, 2));
        }

        public static string CatMaLoaiDiem(string tenTruong)
        {
            byte i = 0;
            for(; i < tenTruong.Length; i++)
            {
                if (tenTruong[i] == '_')
                    break;
            }
            return tenTruong.Substring(0,i);
        }

        public static byte CatSoTT(string tenTruong)
        {
            byte i = 0;
            for (; i < tenTruong.Length; i++)
                if (tenTruong[i] == '_')
                    break;
            return byte.Parse(tenTruong.Substring(i+1, tenTruong.Length - i-1));
        }

        public static void NhapDuLieuError()
        {
            MessageBox.Show("Còn lỗi nhập dữ liệu. Kiểm tra lại trước khi lưu.", TenChuongTrinh);
        }

        public static void ThongBao(string thongDiep, MessageBoxIcon icon)
        {
            MessageBox.Show(thongDiep, TenChuongTrinh, MessageBoxButtons.OK, icon);
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

        public static string Decrypt(string cipherText, string passPhrase)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        public static string GetConnectionString()
        {
            string ConnectionString = "";
            string strAppDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string fullPath = Path.Combine(strAppDir, "Setting.dat");
            if (File.Exists(fullPath))
            {
                string strServerName = "", strDatabaseName = "", strUserSQL = "", strPassSQL = "";
                try
                {
                    using (StreamReader sr = new StreamReader(fullPath))
                    {
                        string str = sr.ReadLine();
                        strServerName = UTL.Ultils.Decrypt(str, "CVA");
                        str = sr.ReadLine();
                        strDatabaseName = UTL.Ultils.Decrypt(str, "CVA");
                        str = sr.ReadLine();
                        strUserSQL = UTL.Ultils.Decrypt(str, "CVA");
                        str = sr.ReadLine();
                        strPassSQL = UTL.Ultils.Decrypt(str, "CVA");
                    }
                }
                finally
                {

                }
                ConnectionString = "Data Source=" + strServerName + ";Initial Catalog=" + strDatabaseName + ";Persist Security Info=True;User ID=" + strUserSQL + ";Password=" + strPassSQL;                
            }
            return ConnectionString;
        }

        /// <summary>
        /// Phương thức chuyển đổi chữ Tiếng Việt có dấu sang không dấu
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ConvertToUnSign(string text)
        {
            for (int i = 33; i < 48; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 58; i < 65; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 91; i < 97; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            for (int i = 123; i < 127; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            text = text.Replace(" ", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
            text = text.Replace("-", " ");
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static void SendMail(string nguoiNhan, string tieuDe, string noiDung)
        {
            int SoNguoiNhan = 0;
            string[] ListNguoiNhan;
            if (!nguoiNhan.Contains(","))
            {
                ListNguoiNhan = new string[1];
                ListNguoiNhan[0] = nguoiNhan;                
            }                
            else
            {
                ListNguoiNhan = nguoiNhan.Split(',');
            }
            SoNguoiNhan = ListNguoiNhan.Length;
            if (SoNguoiNhan > 0)
            {
                NetworkCredential cred = new NetworkCredential("truongcvadhtb@gmail.com", "cva999999999");
                MailMessage msg = new MailMessage();
                msg.To.Add(ListNguoiNhan[0]);
                if (SoNguoiNhan > 1)
                    for (int i = 1; i < SoNguoiNhan; i++)
                        msg.CC.Add(ListNguoiNhan[i]);
                msg.Subject = tieuDe;
                msg.IsBodyHtml = true;
                msg.Body = noiDung;
                msg.From = new MailAddress("truongcvadhtb@gmail.com"); // Your Email Id
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                //SmtpClient client1 = new SmtpClient("smtp.mail.yahoo.com", 465);
                client.Credentials = cred;
                client.EnableSsl = true;
                client.Send(msg);
            }            
        }

    }
}
