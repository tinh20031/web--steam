using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaLuanSteam.Models
{
    public class Order
    {
        public int MaPhieuDH { get; set; }
        public Nullable<int> MaKH { get; set; }
        public Nullable<System.DateTime> NgayDat { get; set; }
        public Nullable<int> Tong_SL_Dat { get; set; }
        public Nullable<double> ThanhTien { get; set; }
        public Nullable<int> TinhTrang { get; set; }
        public Nullable<double> PhiShip { get; set; }
    }
}