using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaLuanSteam.Models
{
    public class GioHang
    {
        QL_THIETBISTEAMEntities1 db = new QL_THIETBISTEAMEntities1();

        public THONGTINSANPHAM sanpham { get; set; }
        public int iSoLuong { get; set; }

        public string tensp { get; set; }
        public int iGiamGia
        {
            get
            {
                var x = (from s in db.SALEs
                         join sps in db.SPSALEs on s.MASL equals sps.MASL
                         where sps.MaSanPham == sanpham.MaSanPham && (DateTime.Now > s.NGAYBATDAU && DateTime.Now < s.NGAYKETTHUC)
                         select sps.GIAMGIA).SingleOrDefault();
                if (x == null || x <= 0)
                {
                    return 0;
                }
                return (int)x;
            }
        }
        public double? iThanhTien
        {
            get
            {
                if (iGiamGia == null || iGiamGia <= 0)
                {
                    return iSoLuong * sanpham.GiaSanPham;
                }
                else
                {
                    double sl = (double)iSoLuong;
                    double discount = (double)(100 - iGiamGia) /(double)100;
                    double? totalprice = sl * sanpham.GiaSanPham * discount;
                    return totalprice;
                }
            }
        }
    }
}