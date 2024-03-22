using KhoaLuanSteam.Areas.Admin.Models;
using KhoaLuanSteam.Models;
using KhoaLuanSteam.Models.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KhoaLuanSteam.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        //Khởi tạo biến dữ liệu : db
        QL_THIETBISTEAMEntities1 db = new QL_THIETBISTEAMEntities1();
        //
        // GET: /Admin/Login/

        public ActionResult LoginAdmin()
        {
            return View();
        }
        //POST : /Admin/Login/Index : thực hiện việc đăng nhập người quản lý
        [HttpPost]
        public ActionResult LoginAdmin(LoginModel model)
        {
            //kiểm tra hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                //gọi hàm đăng nhập trong AdminProcess và gán dữ liệu trong biến model
                var result = new AdminProcess().Login(model.TenDN, model.MatKhau);
                //Nếu đúng
                if (result == 1)
                {
                    //gán Session["LoginAdmin"] bằng dữ liệu đã đăng nhập
                    Session["LoginAdmin"] = model.TenDN;

                    var nv = db.NHANVIENs.Where(x => x.TenDN == model.TenDN).FirstOrDefault();
                    Session["CheckPQ"] = nv.ID_PhanQuyen;
                    Session["GetMaNV"] = nv.MaNV;

                    //trả về trang quản lý
                    return RedirectToAction("Index", "Home");
                }
                //nếu tài khoản không tồn tại 
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                //nếu nhập sai tài khoản hoặc mật khẩu
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác");
                }
            }

            return View();
        }


        //GET : /Admin/Login/AdminProfile : button xem thông tin về người quản lý
        //Partial View : AdminProfile
        public ActionResult AdminProfile()
        {
            return PartialView();
        }
        //GET : /Admin/Login/Logout :  trang đăng xuất tài khoản người quản lý
        public ActionResult Logout()
        {
            //gán session bằng null
            Session["LoginAdmin"] = null;

            //trả về trang đăng nhập
            return Redirect("/Admin/Login");
            return Redirect("/");
        }
        //GET : /Admin/Login/AdminInfo : trang xem thông tin người quản lý
        public ActionResult AdminInfo()
        {
            //lấy dữ liệu session
            var model = Session["LoginAdmin"];

            //kiểm tra tính hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                if (Session["LoginAdmin"] != null)
                {
                    //so sánh và tìm tên tài khoản
                    var result = db.NHANVIENs.SingleOrDefault(x => x.TenDN == model);
                    //trả về dữ liệu tương ứng trong View
                    return View(result);
                }
            }

            return View();
        }

    }
}
