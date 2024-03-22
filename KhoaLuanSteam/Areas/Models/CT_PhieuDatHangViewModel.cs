using KhoaLuanSteam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaLuanSteam.Areas.Models
{
    public class CT_PhieuDatHangViewModel
    {
        public int MaPhieuDH { get; set; }
        public int MaSanPham { get; set; }
        public Nullable<int> SoLuong { get; set; }
        public Nullable<double> DonGia { get; set; }

        public virtual PHIEUDATHANG PHIEUDATHANG { get; set; }
        public virtual THONGTINSANPHAM THONGTINSANPHAM { get; set; }
    }
}