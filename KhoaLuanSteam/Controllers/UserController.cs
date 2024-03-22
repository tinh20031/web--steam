using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KhoaLuanSteam.Models;
using KhoaLuanSteam.Models.Process;
namespace KhoaLuanSteam.Controllers
{
    public class UserController : Controller
    {
        //Khởi tạo biến dữ liệu : db
        QL_THIETBISTEAMEntities1 db = new QL_THIETBISTEAMEntities1();
        public static KHACHHANG khachhangstatic;
        //
        // GET: /User/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        //GET: /User/DangKy : đăng kí tài khoản thành viên
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        //POST: /User/DangKy : thực hiện lưu dữ liệu đăng ký tài khoản thành viên
        public ActionResult DangKy(KHACHHANG model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserProcess();

                var kh = new KHACHHANG();

                if (user.CheckUsername(model.TenDN, model.MatKhau) == 1)
                {
                    ModelState.AddModelError("", "Tài khoản đã tồn tại");
                }
                else if (user.CheckUsername(model.TenDN, model.MatKhau) == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đã tồn tại");
                }
                else
                {
                    kh.TenDN = model.TenDN;
                    kh.MatKhau = model.MatKhau;
                    kh.TenKH = model.TenKH;
                    kh.Email = model.Email;
                    kh.DiaChi = model.DiaChi;
                    kh.SDT = model.SDT;
                    kh.GioiTinh = model.GioiTinh;
                    kh.NgaySinh = model.NgaySinh;
                    kh.NgayTao = DateTime.Now;

                    var result = user.InsertUser(kh);
                    ViewBag.success = "Đã Đăng Ký Tài Khoản Thành Công";
                    khachhangstatic = kh;
                    Session["User"] = model.TenDN;
                    return RedirectToAction("ThongTinUser", "User");
                }


            }
            return View(model);
        }



        //GET : /User/Login : đăng nhập tài khoản
        //Parital View : Login

        public ActionResult Login()
        {
            return PartialView();
        }
        //POST : /User/Login : thực hiện đăng nhập
        [HttpPost]

        public ActionResult Login(Model_Login model)
        {
            //kiểm tra hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                //gọi hàm đăng nhập trong UserProcess và gán dữ liệu trong biến model
                var result = new UserProcess().Login(model.TenDN, model.MatKhau);

                //Nếu đúng
                if (result == 1)
                {
                    //gán Session["LoginAdmin"] bằng dữ liệu đã đăng nhập
                    Session["User"] = model.TenDN;
                    var kh = db.KHACHHANGs.Where(x => x.TenDN == model.TenDN).FirstOrDefault();
                    khachhangstatic = kh;
                    //trả về trang chủ
                    return RedirectToAction("TrangChu", "Home");
                }
                //nếu tài khoản không tồn tại
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                    return RedirectToAction("PageDangNhap", "User");
                }
                //nếu nhập sai tài khoản hoặc mật khẩu
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác");
                    return RedirectToAction("PageDangNhap", "User");
                }
            }
           
            return PartialView("");
        }
        //GET : /User/LoginPage : trang đăng nhập

        public ActionResult PageDangNhap()
        {
            return View();
        }

        //POST : /User/LoginPage : thực hiện đăng nhập
        [HttpPost]
        /*public ActionResult PageDangNhap(Model_Login model)
        {
            //kiểm tra hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                //gọi hàm đăng nhập trong AdminProcess và gán dữ liệu trong biến model
                var result = new UserProcess().Login(model.TenDN, model.MatKhau);
                //Nếu đúng
                if (result == 1)
                {
                    //gán Session["LoginAdmin"] bằng dữ liệu đã đăng nhập
                    Session["User"] = model.TenDN;
                    var kh = db.KHACHHANGs.Where(x => x.TenDN == model.TenDN).FirstOrDefault();
                    khachhangstatic = kh;
                    //trả về trang chủ
                    return RedirectToAction("TrangChu", "Home");
                }
                //nếu tài khoản không tồn tại
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                    return RedirectToAction("PageDangNhap", "User");
                }
                //nếu nhập sai tài khoản hoặc mật khẩu
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác");
                    return RedirectToAction("PageDangNhap", "User");
                }
            }

            return View();
        }*/

        public ActionResult PageDangNhap(Model_Login model)
        {
            // Code xác thực hợp lệ đã được bỏ qua để đơn giản hóa

            // Gọi hàm đăng nhập trong UserProcess
            var result = new UserProcess().Login(model.TenDN, model.MatKhau);
            // Xử lý kết quả đăng nhập
            if (result > 0) // Đăng nhập thành công
            {
                if (result == 1) // Nếu là admin
                {
                    //gán Session["LoginAdmin"] bằng dữ liệu đã đăng nhập
                    Session["LoginAdmin"] = model.TenDN;

                    var nv = db.NHANVIENs.Where(x => x.TenDN == model.TenDN).FirstOrDefault();
                    Session["CheckPQ"] = nv.ID_PhanQuyen;
                    Session["GetMaNV"] = nv.MaNV;

                    //trả về trang quản lý
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else if (result == 2) // Nếu là user
                {
                    Session["User"] = model.TenDN;
                    var kh = db.KHACHHANGs.Where(x => x.TenDN == model.TenDN).FirstOrDefault();
                    khachhangstatic = kh;
                    //trả về trang chủ
                    return RedirectToAction("TrangChu", "Home");
                }
            }
            else if (result < 0) // Sai thông tin đăng nhập
            {
                if (result == -1) // Sai thông tin đăng nhập cho admin
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác");
                }
                else if (result == -2) // Sai thông tin đăng nhập cho user
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác");
                }
            }
            else // Tài khoản không tồn tại
            {
                ModelState.AddModelError("", "Tài khoản không tồn tại.");
            }

            return View();
        }



        //GET : /User/Logout : đăng xuất tài khoản khách hàng
        [HttpGet]
        public ActionResult DangXuat()
        {
            Session["User"] = null;
            khachhangstatic = null;
                //hủy bỏ toàn bộ session
            Session.Abandon();
            return RedirectToAction("TrangChu", "Home");
        }

        //GET : /User/ThongTinUser : cập nhật thông tin khách hàng
        [HttpGet]
        public ActionResult ThongTinUser()
        {
            //lấy dữ liệu từ session
            var model = Session["User"];

            if (ModelState.IsValid)
            {
                //tìm tên tài khoản
                var result = db.KHACHHANGs.SingleOrDefault(x => x.TenDN == model);
                //trả về dữ liệu tương ứng
                return View(result);
            }

            return View();
        }

        //POST : /User/UpdateUser : thực hiện việc cập nhật thông tin khách hàng
        [HttpPost]
        public ActionResult ThongTinUser(KHACHHANG model)
        {
            var use = Session["User"];
            if (ModelState.IsValid)
            {
                //gọi hàm cập nhật thông tin khách hàng
                var result = new UserProcess().UpdateUser(model);

                //thực hiện kiểm tra
                if (result == 1)
                {
                    return RedirectToAction("ThongTinUser");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công.");
                }
            }

            return View(model);
        }
    }
}
