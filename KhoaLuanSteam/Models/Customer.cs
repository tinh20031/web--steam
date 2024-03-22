using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaLuanSteam.Models
{
    public class Customer
    {
        public int MaKH { get; set; }
        public string TenKH { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string TenDN { get; set; }
        public string MatKhau { get; set; }
    }
}