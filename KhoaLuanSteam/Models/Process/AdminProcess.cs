using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

//using System.Data.SqlClient;
//using System.Data;

namespace KhoaLuanSteam.Models.Process
{
    public class AdminProcess
    {

        ///HÀM QUẢN LÝ CUA ADMIN
        ///
        //Khởi tạo biến dữ liệu : db
        QL_THIETBISTEAMEntities1 db = null;

        //constructor :  khởi tạo đối tượng
        public AdminProcess()
        {
            db = new QL_THIETBISTEAMEntities1();
        }

        /// <summary>
        /// Hàm đăng nhập
        /// </summary>
        /// <param name="username">string</param>
        /// <param name="password">string</param>
        /// <returns>int</returns>
        public int Login(string username, string password)
        {
            //var result = db.Admins.Where(x => x.TaiKhoan == username).FirstOrDefault();
            var result = db.NHANVIENs.Where(x => x.TenDN == username).FirstOrDefault();
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.MatKhau == password)
                {

                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        #region Phan Hoi Khach Hang
        public int InsertDoiTra(DOITRA entity)
        {
            db.DOITRAs.Add(entity);
            db.SaveChanges();
            return entity.MaDT;
        }
        #endregion

        //Xử Lý Thông Tin Sản Phẩm
        #region Xử Lý Thông Tin Sản Phẩm
        /// <summary>
        /// hàm lấy mã sách
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Sach</returns>
        public THONGTINSANPHAM GetIdSanPham(int id)
        {
            return db.THONGTINSANPHAMs.Find(id);
        }

        //Books : sách

        /// <summary>
        /// hàm xuất danh sách Sách
        /// </summary>
        /// <returns>List</returns>
        public List<THONGTINSANPHAM> Ad_ThongTinSanPham()
        {
            return db.THONGTINSANPHAMs.OrderBy(x => x.MaSanPham).ToList();
        }

        /// <summary>
        /// hàm xóa 1 sp
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>bool</returns>
        public bool DeleteSanPham(int id)
        {
            try
            {
                var sanpham = db.THONGTINSANPHAMs.SingleOrDefault(x => x.MaSanPham == id);
                db.THONGTINSANPHAMs.Remove(sanpham);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// hàm cập nhật sản phẩm
        /// </summary>
        /// <param name="entity">Sách</param>
        /// <returns>int</returns>
        public int UpdateSanPham(THONGTINSANPHAM entity)
        {
            try
            {
                var sanpham = db.THONGTINSANPHAMs.Find(entity.MaSanPham);
                sanpham.MaLoai = entity.MaLoai;
                sanpham.MaNCC = entity.MaNCC;
                sanpham.TenSanPham = entity.TenSanPham;
                sanpham.GiaSanPham = entity.GiaSanPham;
                sanpham.MoTa = entity.MoTa;
                sanpham.HinhAnh = entity.HinhAnh;
                sanpham.GiamGia = entity.GiamGia;
                sanpham.SLTon = entity.SLTon;
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// hàm thêm sản phẩm
        /// </summary>
        /// <param name="entity">Sach</param>
        /// <returns>int</returns>
        public int InsertSanPham(THONGTINSANPHAM entity)
        {
            db.THONGTINSANPHAMs.Add(entity);
            db.SaveChanges();
            return entity.MaSanPham;
        }

        //------------------------------ start
        /// <summary>
        /// hàm thêm CT_Don nhap hang
        /// </summary>
        /// <param name="entity">CT_PHIEUNHAPHANG</param>
        /// <returns>string</returns>
        public int InsertCT_PhieuNhapHang(CT_PHIEUNHAPHANG entity)
        {
            db.CT_PHIEUNHAPHANG.Add(entity);
            db.SaveChanges();
            return 1;
        }



        #endregion



        // thêm tk nhân viên:
        public int InsertNhanVien(NHANVIEN entity)
        {
            db.NHANVIENs.Add(entity);
            db.SaveChanges();
            return entity.MaNV;
        }



        //Liên hệ từ khách hàng

        #region phản hồi khách hàng
        /// <summary>
        /// hàm lấy mã liên hệ
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>LienHe</returns>
        public LIENHE GetIdContact(int id)
        {
            return db.LIENHEs.Find(id);
        }

        /// <summary>
        /// hàm lấy danh sách những phản hồi từ khách hàng
        /// </summary>
        /// <returns>List</returns>
        public List<LIENHE> ShowListContact()
        {
            return db.LIENHEs.OrderBy(x => x.MaLH).ToList();
        }

        /// <summary>
        /// hàm xóa thông tin phản hồi khách hàng
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>bool</returns>
        public bool deleteContact(int id)
        {
            try
            {
                var contact = db.LIENHEs.Find(id);
                db.LIENHEs.Remove(contact);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion


        //Quản lý người dùng kh
        #region quản lý người dùng
        /// <summary>
        /// Hàm lấy mã khách hàng tham quan
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>KhachHang</returns>
        public KHACHHANG GetIdKH(int id)
        {
            return db.KHACHHANGs.Find(id);
        }
        /// <summary>
        /// hàm xuất danh sách người dùng
        /// </summary>
        /// <returns>List</returns>
        public List<KHACHHANG> ListUser()
        {
            return db.KHACHHANGs.OrderBy(x => x.MaKH).ToList();
        }

        /// <summary>
        /// hàm xóa người dùng
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>bool</returns>
        public bool DeleteUser(int id)
        {
            try
            {
                var user = db.KHACHHANGs.Find(id);
                db.KHACHHANGs.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        //Quản lý người nhân viên
        #region quản lý người nhân viên
        /// <summary>
        /// Hàm lấy mã nhân viên tham quan
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>NhanVien </returns>
        public NHANVIEN GetIdNV(int id)
        {
            return db.NHANVIENs.Find(id);
        }

        /// <summary>
        /// hàm xuất danh sách nhân viên
        /// </summary>
        /// <returns>List</returns>
        public List<NHANVIEN> ListUserNV()
        {
            return db.NHANVIENs.OrderBy(x => x.MaNV).ToList();
        }


        /// <summary>
        /// hàm xóa người dùng nhân viên
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>bool</returns>
        public bool DeleteUserNV(int id)
        {
            try
            {
                var user = db.NHANVIENs.Find(id);
                db.NHANVIENs.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// hàm cập nhật sản phẩm
        /// </summary>
        /// <param name="entity">Sách</param>
        /// <returns>int</returns>
        public int UpdateNhanVien(NHANVIEN entity)
        {
            try
            {
                var nhanvien = db.NHANVIENs.Find(entity.MaNV);
                nhanvien.TenNV = entity.TenNV;
                nhanvien.NgaySinh = entity.NgaySinh;
                nhanvien.GioiTinh = entity.GioiTinh;
                nhanvien.Email = entity.Email;
                nhanvien.SoDT = entity.SoDT;
                //nhanvien.HinhAnh = entity.HinhAnh;
                nhanvien.TenDN = entity.TenDN;
                nhanvien.MatKhau = entity.MatKhau;
                nhanvien.ID_PhanQuyen = entity.ID_PhanQuyen;
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        #endregion


        //Xu ly thong tin Loai SanPham
        #region Xu Ly Thong Tin Loai San Pham

        /// <summary>
        /// hàm lấy mã thể loại
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>TheLoai</returns>
        public LOAISANPHAM GetIdLoaiSanPham(int id)
        {
            return db.LOAISANPHAMs.Find(id);
        }
        /// <summary>
        /// hàm xuất danh sách loại sp
        /// </summary>
        /// <returns>List</returns>
        public List<LOAISANPHAM> Ad_ThongTinLoaiSanPham()
        {
            return db.LOAISANPHAMs.OrderBy(x => x.MaLoai).ToList();
        }

        /// <summary>
        /// hàm thêm  loại sp
        /// </summary>
        /// <param name="entity">TheLoai</param>
        /// <returns>int</returns>
        public int InsertLoaiSanPham(LOAISANPHAM entity)
        {
            db.LOAISANPHAMs.Add(entity);
            db.SaveChanges();
            return entity.MaLoai;
        }

        /// <summary>
        /// hàm cập nhật  loại sp
        /// </summary>
        /// <param name="entity">TheLoai</param>
        /// <returns>int</returns>
        public int UpdateLoaiSanPham(LOAISANPHAM entity)
        {
            try
            {
                var tl = db.LOAISANPHAMs.Find(entity.MaLoai);
                tl.TenLoai = entity.TenLoai;
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// hàm xóa loại sp
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>bool</returns>
        public bool DeleteLoaiSanPham(int id)
        {
            try
            {
                var tl = db.LOAISANPHAMs.Find(id);
                db.LOAISANPHAMs.Remove(tl);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion





        //Xu ly thong tin Sale
        #region Xu ly SALE
        /// <summary>
        /// hàm lấy mã Sale
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>TacGia</returns>
        public SALE GetIdSale(int id)
        {
            return db.SALEs.Find(id);
        }


        /// <summary>
        /// hàm xuất danh sách tác giả
        /// </summary>
        /// <returns>List</returns>
        public List<SALE> AD_ShowAllSale()
        {
            return db.SALEs.OrderBy(x => x.MASL).ToList();
        }

        /// <summary>
        /// hàm thêm tác giả
        /// </summary>
        /// <param name="entity">TacGia</param>
        /// <returns></returns>
        public int InsertSale(SALE entity)
        {
            db.SALEs.Add(entity);
            db.SaveChanges();
            return entity.MASL;
        }

        /// <summary>
        /// hàm cập nhật tác giả
        /// </summary>
        /// <param name="entity">TacGia</param>
        /// <returns>int</returns>
        public int UpdateSale(SALE entity)
        {
            try
            {
                var sl = db.SALEs.Find(entity.MASL);
                sl.TENSL = entity.TENSL;
                sl.NGAYBATDAU = entity.NGAYBATDAU;
                sl.NGAYKETTHUC = entity.NGAYKETTHUC;
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// hàm xóa tác giả
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>int</returns>
        public bool DeleteSale(int id)
        {
            try
            {
                var sl = db.SALEs.Find(id);
                db.SALEs.Remove(sl);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        //Xu ly san pham Sale
        #region Xu ly SPSALE
        public SPSALE GetIdSPSale(int id)
        {
            return db.SPSALEs.Find(id);
        }


        /// <summary>
        /// hàm xuất danh sách tác giả
        /// </summary>
        /// <returns>List</returns>
        public List<SPSALE> AD_ShowAllSPSale()
        {
            return db.SPSALEs.OrderBy(x => x.MASPSALE).ToList();
        }

        public List<SPSALE> DanhSachSP_Sale(int id)
        {
            return db.SPSALEs.Where(x => x.MASL == id).ToList();
        }

        /// <summary>
        /// hàm thêm tác giả
        /// </summary>
        /// <param name="entity">TacGia</param>
        /// <returns></returns>
        public int InsertSPSale(SPSALE entity)
        {
            SPSALE SP = db.SPSALEs.Where(x => (x.MaSanPham == entity.MaSanPham) && (x.MASL == entity.MASL)).FirstOrDefault();

            if (SP != null)
            {
                SP.GIAMGIA = entity.GIAMGIA;
                db.SPSALEs.Attach(SP);
                db.Entry(SP).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                return SP.MaSanPham;
            }
            else
            {
                db.SPSALEs.Add(entity);
                db.SaveChanges();
                return entity.MaSanPham;
            }
            return 0;
        }

        /// <summary>
        /// hàm cập nhật tác giả
        /// </summary>
        /// <param name="entity">TacGia</param>
        /// <returns>int</returns>
        public int UpdateSPSale(SPSALE entity)
        {
            try
            {
                var sp = db.SPSALEs.Find(entity.MASL);
                sp.MASL = entity.MASL;
                sp.MaSanPham = entity.MaSanPham;
                sp.GIAMGIA = entity.GIAMGIA;
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// hàm xóa tác giả
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>int</returns>
        public bool DeleteSPSale(int masl, int id)
        {
            try
            {
                var sl = db.SPSALEs.Where(x => x.MASL == masl && x.MaSanPham == id).FirstOrDefault();
                db.SPSALEs.Remove(sl);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion


        //Xu ly Thong Tin Phieu Dat Hang

        #region Xu ly Thong Tin PhieuDatHang

        public PHIEUDATHANG GetIdPDH(int id)
        {
            return db.PHIEUDATHANGs.Find(id);
        }
        /// <summary>
        /// hàm xuất danh sách đơn đặt hàng
        /// </summary>
        /// <returns>List</returns>
        public List<PHIEUDATHANG> AD_ShowAllphieudathang()
        {
            return db.PHIEUDATHANGs.OrderBy(x => x.MaPhieuDH).ToList();
        }

        /// <summary>
        /// Xem chi tiết đơn hàng
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>List</returns>
        public List<CT_PHIEUDATHANG> detailsCT_PDDH(int id)
        {
            return db.CT_PHIEUDATHANG.Where(x => x.MaPhieuDH == id).OrderBy(x => x.MaPhieuDH).ToList();
        }

        public int UpdatePdh(PHIEUDATHANG entity)
        {
            try
            {
                var tg = db.PHIEUDATHANGs.Find(entity.MaPhieuDH);
                tg.TinhTrang = entity.TinhTrang;
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion


        //Xu ly Thong Tin Nha Cung Cap
        #region
        /// <summary>
        /// hàm lấy mã nhà cung câp
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>NHACUNGCAP</returns>
        public NHACUNGCAP GetIdNCC(string id)
        {
            return db.NHACUNGCAPs.Find(id.TrimEnd());
        }
        /// <summary>
        /// hàm xuất danh sách nhà cung cấp
        /// </summary>
        /// <returns>List</returns>
        public List<NHACUNGCAP> AD_ShowNhaCungcap()
        {
            return db.NHACUNGCAPs.OrderBy(x => x.MaNCC.TrimEnd()).ToList();
        }
        /// <summary>
        /// hàm thêm nhà cung cấp
        /// </summary>
        /// <param name="entity">NhaXuatBan</param>
        /// <returns>string</returns>
        public int InsertNcc(NHACUNGCAP entity)
        {
            db.NHACUNGCAPs.Add(entity);
            db.SaveChanges();
            return 1;
        }
        /// <summary>
        /// hàm cập nhật nhà cung cấp
        /// </summary>
        /// <param name="entity">TacGia</param>
        /// <returns>string</returns>
        /// 
        public int UpdateNcc(NHACUNGCAP entity)
        {
            try
            {
                var tg = db.NHACUNGCAPs.Find(entity.MaNCC);
                tg.TenNCC = entity.TenNCC;
                tg.DiaChi = entity.DiaChi;
                tg.DienThoai = entity.DienThoai;
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// hàm xóa nhà cung câp
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>string</returns>
        public bool DeleteNcc(string id)
        {
            try
            {
                var tg = db.NHACUNGCAPs.Find(id.TrimEnd());
                db.NHACUNGCAPs.Remove(tg);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion


        public List<DonDatHangNCC> AD_ShowAlldondathangNCC()
        {
            return db.DonDatHangNCCs.OrderBy(x => x.MaDonDatHangNCC).ToList();
        }


        //Xu ly Thong Tin Phieu Nhập hàng
        #region Xu ly Thong Tin PhieuNhapHang
        /// <summary>
        /// hàm xuất danh sách phiếu nhập hàng
        /// </summary>
        /// <returns>List</returns>
        public List<PHIEUNHAPHANG> AD_ShowAllphieunhaphang()
        {
            return db.PHIEUNHAPHANGs.OrderBy(x => x.MaPhieuNhapHang).ToList();
        }

        /// <summary>
        /// Xem chi tiết đơn nhập  hàng
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>List</returns>
        //public List<CT_PHIEUNHAPHANG> detailsCT_PNhaphang(string id)
        //{
        //    return db.CT_PHIEUNHAPHANG.Where(x => x.MaPhieuNhapHang == id.Trim()).OrderBy(x => x.MaPhieuNhapHang).ToList();
        //}
        public List<CT_PHIEUNHAPHANG> detailsCT_PNhaphang(int id)
        {
            return db.CT_PHIEUNHAPHANG.Where(x => x.MaPhieuNhapHang == id).OrderBy(x => x.MaPhieuNhapHang).ToList();
        }
        /// <summary>
        /// hàm thêm phiếu nhập hàng
        /// </summary>
        /// <param name="entity">Sach</param>
        /// <returns>string</returns>
        public int Insertphieunhaphang(PHIEUNHAPHANG entity)
        {
            db.PHIEUNHAPHANGs.Add(entity);
            db.SaveChanges();
            return 1;
        }
        /// hàm lấy mã chi tiết phiếu nhập hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CT_PHIEUDATHANG GetIdCT_PNH(string id)
        {
            return db.CT_PHIEUDATHANG.Find(id.Trim());
        }

        /// <summary>
        /// hàm thêm sản phẩm vào đơn đặt hàng
        /// </summary>
        /// <param name="detail">ChiTietDDH</param>
        /// <returns>bool</returns>
        public bool InsertCT_PNH(CT_PHIEUNHAPHANG detail)
        {
            try
            {
                db.CT_PHIEUNHAPHANG.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;

            }
        }

        #endregion


        public int Insertdondathangncc(DonDatHangNCC entity)
        {
            db.DonDatHangNCCs.Add(entity);
            db.SaveChanges();
            return 1;
        }

        public int InsertCT_DonDatHangNCC(CT_DonDatHangNCC entity)
        {
            db.CT_DonDatHangNCC.Add(entity);
            db.SaveChanges();
            return 1;
        }

        public List<CT_DonDatHangNCC> detailsCT_DatHangTuNCCid(int id)
        {
            return db.CT_DonDatHangNCC.Where(x => x.MaDonDatHangNCC == id).OrderBy(x => x.MaCT_DonDatHangNCC).ToList();
        }

        public List<CT_DonDatHangNCC> detailsCT_DonDatHangNCC(int id)
        {
            return db.CT_DonDatHangNCC.Where(x => x.MaDonDatHangNCC == id).OrderBy(x => x.MaDonDatHangNCC).ToList();
        }
    }
}