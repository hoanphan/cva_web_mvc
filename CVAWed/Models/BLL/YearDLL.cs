using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CVAWeb.Models.DAL;
namespace CVAWeb.Models.BLL
{
    public class YearBLL
    {
        static SchoolManagementDataContext DB = new SchoolManagementDataContext();
        public static string findIDNewYear()
        {
            return DB.DSNamHocs.Where(q => q.NamHienTai == true).Select(q => q.MaNamHoc).FirstOrDefault().ToString();
        } 
               
    }
}