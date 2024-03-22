using KhoaLuanSteam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaLuanSteam.ViewModel
{
    public class SaleViewModel
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string HinhAnh { get; set; }

        public int GiamGia { get; set; }
        public double? Gia { get; set; }
        public int? SoLuong { get; set; }
        public int maSL { get; set; }

        //public double? iThanhTien
        //{
        //    get
        //    {
        //        if (sach.GiamGia <= 0)
        //        {
        //            return iSoLuong * sach.GiaSach;
        //        }
        //        else
        //        {
        //            return iSoLuong * (sach.GiaSach - (sach.GiaSach * sach.GiamGia));
        //        }
        //    }
        //}
    }
        public class ProductInSale
        {
        public THONGTINSANPHAM THONGTINSANPHAM { get; set; }
        public int GIAMGIA { get; set; }
    }
}
