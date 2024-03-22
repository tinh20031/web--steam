using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KhoaLuanSteam.Models;
using PagedList;
using KhoaLuanSteam.Models.Process;
using System.IO;

namespace KhoaLuanSteam.Controllers
{
    //chinh
    public class HomeController : Controller
    {

        // GET: /Home/
        //Khởi tạo biến dữ liệu : db
        QL_THIETBISTEAMEntities1 db = new QL_THIETBISTEAMEntities1();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TrangChu()
        {
            return View();
        }
        //GET : /Book/All : hiển thị tất cả sách trong db
        public ActionResult SanPham(int? page)
        {
            // 1 int? the hiện null và kiểu int
            // 2 nếu page = null thì đặt lại là 1 lần gọi đầu tiên
            if (page == null) page = 1;
            //tạo biến số sản phẩm trên trang
            int pageSize = 9;
            //tạo biến số trang
            int pageNumber = (page ?? 1);
            var result = new ProductProcess().ShowAllProduct().ToPagedList(pageNumber, pageSize);

            return View(result);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult TinTuc()
        {
            return View();
        }
        //Trang quy định của web
        public ActionResult QuyDinh()
        {
            return View();
        }
        public ActionResult DoiTra()
        {
            return View();
        }
        public ActionResult Thanhtoan()
        {
            return View();
        }
        public ActionResult VanChuyen()
        {
            return View();
        }

        //GET : /Home/Contact : trang liên hệ, phản hồi của khách hàng
        [HttpGet]
        public ActionResult Contact()
        {
            if (Session["User"] == null || Session["User"].ToString() == "")
            {
                return RedirectToAction("PageDangNhap", "User");
            }

            if (Session["User"] != null) 
            { 
                var tenDN = Session["User"];
                var ketqua = db.KHACHHANGs.SingleOrDefault(x => x.TenDN == tenDN);
                int idKh = ketqua.MaKH;

                //lấy mã mà hiển thị tên
                //ViewBag.MaPhieuDH = new SelectList(db.PHIEUDATHANGs.ToList().Where(y => y.MaPhieuDH == idKh), "MaPhieuDH", "MaPhieuDH");
                ViewBag.MaPhieuDH = new SelectList(db.PHIEUDATHANGs.ToList().Where(p => p.MaKH == idKh).OrderByDescending(x => x.MaPhieuDH), "MaPhieuDH", "MaPhieuDH");
                //ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.TenNCC), "MaNCC", "TenNCC");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Contact(LIENHE model, HttpPostedFileBase fileUpload, HttpPostedFileBase fileUpload1) 
        {
            var home = new HomeProcess();
            var lh = new LIENHE();
            if (fileUpload1 == null)
            {
                ViewBag.Alert = "Vui lòng chọn ảnh sản phẩm";
                return View();
            }
            else
            {
                var fileName1 = Path.GetFileName(fileUpload1.FileName);
                //chuyển file đường dẫn và biên dịch vào /images
                var path = Path.Combine(Server.MapPath("/HinhAnhSach"), fileName1);

                //kiểm tra đường dẫn ảnh có tồn tại?
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Alert = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpload1.SaveAs(path);
                }

                lh.HinhAnhSP = fileName1;
            }

            if (fileUpload == null)
            {
                ViewBag.Alert = "Vui lòng chọn ảnh hóa đơn";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //lấy file đường dẫn
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //chuyển file đường dẫn và biên dịch vào /images
                    var path = Path.Combine(Server.MapPath("/HinhAnhSach"), fileName);

                    //kiểm tra đường dẫn ảnh có tồn tại?
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Alert = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    //var home = new HomeProcess();
                    //var lh = new LIENHE();

                    if (Session["User"] != null)
                    {
                        var tenDN = Session["User"];
                        var ketqua = db.KHACHHANGs.SingleOrDefault(x => x.TenDN == tenDN);
                        int idKh = ketqua.MaKH;

                        //lấy mã mà hiển thị tên
                        //ViewBag.MaPhieuDH = new SelectList(db.PHIEUDATHANGs.ToList().Where(y => y.MaPhieuDH == idKh), "MaPhieuDH", "MaPhieuDH");
                        ViewBag.MaPhieuDH = new SelectList(db.PHIEUDATHANGs.ToList().Where(p => p.MaKH == idKh).OrderByDescending(x => x.MaPhieuDH), "MaPhieuDH", "MaPhieuDH", model.MaPhieuDH);
                        //ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.TenNCC), "MaNCC", "TenNCC");
                    }

                    ViewBag.MaPhieuDH = new SelectList(db.PHIEUDATHANGs.ToList().OrderByDescending(x => x.MaPhieuDH), "MaPhieuDH", "MaPhieuDH", model.MaPhieuDH);


                    lh.HinhAnhHD = fileName;
                    lh.MaPhieuDH = model.MaPhieuDH;
                    //gán dữ liệu từ client vào model
                    lh.HoTen = model.HoTen;
                    //lh.Ho = model.Ho;
                    lh.Email = model.Email;
                    lh.DienThoai = model.DienThoai;
                    lh.NoiDung = model.NoiDung;
                    lh.NgayCapNhat = DateTime.Now;

                    //gọi hàm lưu thông tin phản hồi từ khách hàng
                    var result = home.InsertContact(lh);

                    if (result > 0)
                    {
                        ViewBag.success = "Đã ghi nhận phản hồi của bạn";
                        ModelState.Clear();
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Lỗi ghi nhận");
                    }
                }
            }

            return View(model);
        }
    }

}
