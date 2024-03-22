using KhoaLuanSteam.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaLuanSteam.Models.Process
{
    public class ProductProcess
    {
        //Khởi tạo biến dữ liệu : db
        QL_THIETBISTEAMEntities1 db = null;

        //constructor :  khởi tạo đối tượng
        public ProductProcess()
        {
            db = new QL_THIETBISTEAMEntities1();
        }

        /// <summary>
        /// lay 8 san pham moi
        /// </summary>
        /// <param name="count">int</param>
        /// <returns>List</returns>
        public List<THONGTINSANPHAM> NewDateProduct()
        {
            var a = db.THONGTINSANPHAMs.Take(8).OrderBy(x => x.MaSanPham).ToList();
            foreach (var item in a)
            {
                var x = (from s in db.SPSALEs
                         join sps in db.SALEs on s.MASL equals sps.MASL
                         join tts in db.THONGTINSANPHAMs on s.MaSanPham equals tts.MaSanPham
                         where DateTime.Now > sps.NGAYBATDAU && DateTime.Now < sps.NGAYKETTHUC && s.MaSanPham == item.MaSanPham
                         select s.GIAMGIA).FirstOrDefault();
                if (x == null || x <= 0)
                {
                    item.GiamGia = 0;
                }
                else
                {
                    item.GiamGia = x;
                }
            }
            return a;
        }

        public List<THONGTINSANPHAM> LatestProduct()
        {
            //return db.THONGTINSANPHAMs.Take(3).OrderByDescending(x => x.MaSanPham).ToList();
            var a = db.THONGTINSANPHAMs.Where(x => x.GiamGia == 0).OrderByDescending(x => x.MaSanPham).Take(3).ToList();
            foreach (var item in a)
            {
                var x = (from s in db.SPSALEs
                         join sps in db.SALEs on s.MASL equals sps.MASL
                         join tts in db.THONGTINSANPHAMs on s.MaSanPham equals tts.MaSanPham
                         where DateTime.Now > sps.NGAYBATDAU && DateTime.Now < sps.NGAYKETTHUC && s.MaSanPham == item.MaSanPham
                         select s.GIAMGIA).FirstOrDefault();
                if (x == null || x <= 0)
                {
                    item.GiamGia = 0;
                }
                else
                {
                    item.GiamGia = x;
                }
            }
            return a;
        }
     

        //public object SanPhamGiamGia()
        //{
        //    var ketqua = (from product in db.THONGTINSANPHAMs
        //                 where product.GiamGia > 0
        //                 select product).OrderByDescending(x => x.GiamGia).Take(8);

        //    return ketqua;
        //}
        public List<ProductInSale> getSaleProduct()
        {
            //var spsale = (from sach in db.THONGTINSACHes join sale in db.SPSALEs on sach.MaSach equals sale.MaSach where sach.MaSach == sale.MaSach select new { sach.MaSach, sach.MaLoai, sach.MaTG, sach.MaNXB, sach.TenSach, sach.GiaSach, sach.MoTa, sach.HinhAnh, sale.GIAMGIA, sach.SLTon }).ToList();
            ////return db.THONGTINSACHes.OrderByDescending(x => x.MaSach).ToList();
            var x = (from s in db.SPSALEs
                     join sps in db.SALEs on s.MASL equals sps.MASL
                     join tts in db.THONGTINSANPHAMs on s.MaSanPham equals tts.MaSanPham
                     where DateTime.Now > sps.NGAYBATDAU && DateTime.Now < sps.NGAYKETTHUC
                     select new ProductInSale
                     {
                         THONGTINSANPHAM = tts,
                         GIAMGIA = (int)s.GIAMGIA
                     }).Take(3).ToList();
            return x;
        }

        public List<ProductInSale> getAllSaleProduct()
        {
            //var spsale = (from sach in db.THONGTINSACHes join sale in db.SPSALEs on sach.MaSach equals sale.MaSach where sach.MaSach == sale.MaSach select new { sach.MaSach, sach.MaLoai, sach.MaTG, sach.MaNXB, sach.TenSach, sach.GiaSach, sach.MoTa, sach.HinhAnh, sale.GIAMGIA, sach.SLTon }).ToList();
            ////return db.THONGTINSACHes.OrderByDescending(x => x.MaSach).ToList();
            var x = (from s in db.SPSALEs
                     join sps in db.SALEs on s.MASL equals sps.MASL
                     join tts in db.THONGTINSANPHAMs on s.MaSanPham equals tts.MaSanPham
                     where DateTime.Now > sps.NGAYBATDAU && DateTime.Now < sps.NGAYKETTHUC
                     select new ProductInSale
                     {
                         THONGTINSANPHAM = tts,
                         GIAMGIA = (int)s.GIAMGIA
                     }).ToList();
            return x;
        }

        public double GiaSanPham(int masanpham)
        {
            THONGTINSANPHAM sanpham = db.THONGTINSANPHAMs.Single(s => s.MaSanPham == masanpham);
            var x = (from s in db.SALEs
                     join sps in db.SPSALEs on s.MASL equals sps.MASL
                     where sps.MaSanPham == masanpham && (DateTime.Now > s.NGAYBATDAU && DateTime.Now < s.NGAYKETTHUC)
                     select sps.GIAMGIA).SingleOrDefault();
            if (x == null || x <= 0)
                return (double)sanpham.GiaSanPham;
            else
            {
                double discount = (double)(100 - x) / (double)100;
                double? totalprice =  sanpham.GiaSanPham * discount;
                return (double)totalprice;
            }

        }
        /// <summary>
        /// lay 3  sản phẩm ban chay
        /// </summary>
        /// <param name="count">int</param>
        /// <returns>List</returns>
        //public List<THONGTINSANPHAM> TakeProduct()
        //{
        //    return db.THONGTINSANPHAMs.Take(3).OrderBy(x => x.MaSanPham).ToList();
        //}

        public object TakeProduct()
        {
            List<THONGTINSANPHAM> tHONGTINSANPHAMs = new List<THONGTINSANPHAM>();
            List<int> ListTopMaSP;

            using (var ctx = new QL_THIETBISTEAMEntities1())
            {
                //ListTopMaSP = ctx.Database.SqlQuery<int>("select TOP(3) MaSanPham from CT_PHIEUDATHANG Group by MaSanPham ORDER BY SUM(CT_PHIEUDATHANG.SoLuong) DESC").ToList();
                ListTopMaSP = ctx.Database.SqlQuery<int>("select TOP(3) ISNULL(MaSanPham, 1) from CT_PHIEUDATHANG Group by MaSanPham ORDER BY SUM(CT_PHIEUDATHANG.SoLuong) DESC").ToList();
            }
            foreach(var item in ListTopMaSP)
            {              
                var sp = GetIdSanPham(item);
                var x = (from s in db.SALEs
                         join sps in db.SPSALEs on s.MASL equals sps.MASL
                         where sps.MaSanPham == item && (DateTime.Now > s.NGAYBATDAU && DateTime.Now < s.NGAYKETTHUC)
                         select sps.GIAMGIA).SingleOrDefault();
                if (x == null || x <= 0)
                {
                  sp.GiamGia = 0 ;
                }
                else
                {                   
                    sp.GiamGia = x ;                    
                }       
                tHONGTINSANPHAMs.Add(sp);
            }
            return tHONGTINSANPHAMs;
        }


        /// <summary>
        /// lay 4  csp lien quan toi ma loai duoc truyen vao
        /// </summary>
        /// <param name="count">int</param>
        /// <returns>List</returns>
        public List<THONGTINSANPHAM> SanPhamLienQuan(int LoaiSanPham, int MaSanPham)
        {
            var a = db.THONGTINSANPHAMs.Where(x => x.MaLoai == LoaiSanPham).Where(x => x.MaSanPham != MaSanPham).Take(4).ToList();
            foreach (var item in a)
            {
                var x = (from s in db.SPSALEs
                         join sps in db.SALEs on s.MASL equals sps.MASL
                         join tts in db.THONGTINSANPHAMs on s.MaSanPham equals tts.MaSanPham
                         where DateTime.Now > sps.NGAYBATDAU && DateTime.Now < sps.NGAYKETTHUC && s.MaSanPham == item.MaSanPham
                         select s.GIAMGIA).FirstOrDefault();
                if (x == null || x <= 0)
                {
                    item.GiamGia = 0;
                }
                else
                {
                    item.GiamGia = x;
                }
            }
            return a;
        }

        /// <summary>
        /// hàm xuất danh sách loại sp
        /// </summary>
        /// <returns></returns>
        public List<LOAISANPHAM> ListLoaiSanPham()
        {
            return db.LOAISANPHAMs.OrderBy(x => x.MaLoai).ToList();
        }


        ///// <summary>
        ///// hàm xuất danh sách NXB
        ///// </summary>
        ///// <returns></returns>
        public List<NHACUNGCAP> ListNCC()
        {
            return db.NHACUNGCAPs.OrderBy(x => x.MaNCC).ToList();
        }



        /// <summary>
        /// Xem tất cả sản phẩm
        /// </summary>
        /// <returns>List</returns>
        public List<THONGTINSANPHAM> ShowAllProduct()
        {
          
            var a = db.THONGTINSANPHAMs.OrderByDescending(x => x.MaSanPham).ToList();

            foreach (var item in a)
            {
                var x = (from s in db.SPSALEs
                         join sps in db.SALEs on s.MASL equals sps.MASL
                         join tts in db.THONGTINSANPHAMs on s.MaSanPham equals tts.MaSanPham
                         where DateTime.Now > sps.NGAYBATDAU && DateTime.Now < sps.NGAYKETTHUC && s.MaSanPham == item.MaSanPham
                         select s.GIAMGIA).FirstOrDefault();
                if (x == null || x <= 0)
                {
                    item.GiamGia = 0;
                }
                else
                {
                    item.GiamGia = x;
                }
            }
            return a;
        }
        /// <summary>
        /// hàm lấy mã loại sp
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>TheLoai</returns>
        public LOAISANPHAM LaymaloaiSP(int maSP)
        {
            return db.LOAISANPHAMs.Find(maSP);
        }
        /// <summary>
        /// lọc sách theo chủ đề
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>List</returns>
        public List<THONGTINSANPHAM> SanPhamtheoCD(int maCD)
        {

            var a = db.THONGTINSANPHAMs.Where(x => x.MaLoai == maCD).ToList();
            foreach (var item in a)
            {
                var x = (from s in db.SPSALEs
                         join sps in db.SALEs on s.MASL equals sps.MASL
                         join tts in db.THONGTINSANPHAMs on s.MaSanPham equals tts.MaSanPham
                         where DateTime.Now > sps.NGAYBATDAU && DateTime.Now < sps.NGAYKETTHUC && s.MaSanPham == item.MaSanPham
                         select s.GIAMGIA).FirstOrDefault();
                if (x == null || x <= 0)
                {
                    item.GiamGia = 0;
                }
                else
                {
                    item.GiamGia = x;
                }
            }
            return a;
        }


        ///// <summary>
        ///// hàm lấy mã loại sp
        ///// </summary>
        ///// <param name="id">int</param>
        ///// <returns>TheLoai</returns>
        public NHACUNGCAP LaymaloaiNCC(string maNCC)
        {
            return db.NHACUNGCAPs.Find(maNCC);
        }
        /// <summary>
        /// lọc sách theo chủ đề
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>List</returns>
        public List<THONGTINSANPHAM> SanPhamtheoNCC(string maNCC)
        {

            var a = db.THONGTINSANPHAMs.Where(x => x.MaNCC == maNCC).ToList();
            foreach (var item in a)
            {
                var x = (from s in db.SPSALEs
                         join sps in db.SALEs on s.MASL equals sps.MASL
                         join tts in db.THONGTINSANPHAMs on s.MaSanPham equals tts.MaSanPham
                         where DateTime.Now > sps.NGAYBATDAU && DateTime.Now < sps.NGAYKETTHUC && s.MaSanPham == item.MaSanPham
                         select s.GIAMGIA).FirstOrDefault();
                if (x == null || x <= 0)
                {
                    item.GiamGia = 0;
                }
                else
                {
                    item.GiamGia = x;
                }
            }
            return a;
        }
        /// <summary>
        /// hàm tìm kiếm tên sp
        /// </summary>
        /// <param name="key">string</param>
        /// <returns>List</returns>
        public List<THONGTINSANPHAM> Search(string txt_Search)
        {
            var a = db.THONGTINSANPHAMs.Where(x => x.TenSanPham.Contains(txt_Search)).OrderBy(x => x.TenSanPham).ToList();
            foreach (var item in a)
            {
                var x = (from s in db.SPSALEs
                         join sps in db.SALEs on s.MASL equals sps.MASL
                         join tts in db.THONGTINSANPHAMs on s.MaSanPham equals tts.MaSanPham
                         where DateTime.Now > sps.NGAYBATDAU && DateTime.Now < sps.NGAYKETTHUC && s.MaSanPham == item.MaSanPham
                         select s.GIAMGIA).FirstOrDefault();
                if (x == null || x <= 0)
                {
                    item.GiamGia = 0;
                }
                else
                {
                    item.GiamGia = x;
                }
            }
            return a;
        }

        /// <summary>
        /// hàm lấy mã sp
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Sach</returns>
        public THONGTINSANPHAM GetIdSanPham(int id)
        {
            return db.THONGTINSANPHAMs.Find(id);
        }
    }
}