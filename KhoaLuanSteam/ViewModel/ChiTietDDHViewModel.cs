using KhoaLuanSteam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//namespace DoAn_MonHoc.ViewModels
namespace KhoaLuanSteam.ViewModel
{
    public class ChiTietDDHViewModel
    {
        public int MaSanPham { get; set; }
        public string HinhAnh { get; set; }
        public string TenSanPham { get; set; }
        public double? Gia { get; set; }
        public int? SoLuong { get; set; }

        public double? GiaGiam { get; set; }
    }
}