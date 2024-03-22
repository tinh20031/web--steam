using KhoaLuanSteam.Models;
using KhoaLuanSteam.Models.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Hosting;
using KhoaLuanSteam.ViewModel;
using System.Configuration;
using KhoaLuanSteam.VNPay;

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using MoMo;

namespace KhoaLuanSteam.Controllers
{
    public class CartController : Controller
    {
        //Khởi tạo biến dữ liệu : db
        QL_THIETBISTEAMEntities1 db = new QL_THIETBISTEAMEntities1();

        //tạo 1 chuỗi hằng để gán session
        private const string GioHang = "GioHang";

        // GET: Cart/ : trang giỏ hàng
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = Session["User"];

            if (Session["User"] != null && Session["checkDC"] == null)
            {
                //tìm tên tài khoản
                var Ketqua = db.KHACHHANGs.SingleOrDefault(x => x.TenDN == model);

                //trả về dữ liệu tương ứng
                Session["DiaChiNhan"] = Ketqua.DiaChi;

                string endLocation = Ketqua.DiaChi;
                using (var client = new HttpClient())
                {
                    //string startLocation = distanceCalculation.StartLocation;
                    string startLocation = "140 Lê Trọng Tấn, Phường Tây Thạnh, Quận Tân Phú, TP.HCM";

                    string url = string.Format(API_URL, startLocation, endLocation);

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        var json = JsonConvert.DeserializeObject<dynamic>(result);
                        // Check if json object and its properties are not null before accessing them
                        if (json != null && json.rows != null && json.rows.Count > 0 &&
                            json.rows[0].elements != null && json.rows[0].elements.Count > 0 &&
                            json.rows[0].elements[0].distance != null && json.rows[0].elements[0].distance.value != null)
                        {
                            double soKilomet = (json.rows[0].elements[0].distance.value / 1000);
                            double phiShipHang = new GioHangProcess().tinhPhiShipHang(soKilomet);
                            Session["Kilomet"] = phiShipHang;
                        }
                        /*double soKilomet = (json.rows[0].elements[0].distance.value / 1000);
                        double phiShipHang = new GioHangProcess().tinhPhiShipHang(soKilomet);
                        Session["Kilomet"] = phiShipHang;*/
                        //Session["Kilomet"] = (json.rows[0].elements[0].distance.value / 1000) * 500;
                    }
                };
            }

            var cart = Session[GioHang];
            var list = new List<GioHang>();
            var sluong = 0;
            double? thanhtien = 0;
            if (cart != null)
            {
                list = (List<GioHang>)cart;
                sluong = list.Sum(x => x.iSoLuong);
                thanhtien = list.Sum(x => x.iThanhTien);
            }
            if (TempData["message"] != null)
            {
                ViewBag.Success = TempData["message"];
            }
            ViewBag.Quantity = sluong;
            if (Session["Kilomet"] != null)
            {
                object myObject = new Object();
                string myObjectString = Session["Kilomet"].ToString();
                double phiship = double.Parse(myObjectString);
                ViewBag.Total = thanhtien + phiship;
            }
            else
            {
                ViewBag.Total = thanhtien;
            }
                //ViewBag.Total = thanhtien;
            return View(list);
        }
        //GET : /Cart/CartIcon: đếm sổ sản phẩm trong giỏ hàng
        //PartialView : CartIcon
        public ActionResult CartIcon()
        {
            var cart = Session[GioHang];
            var list = new List<GioHang>();
            if (cart != null)
            {
                list = (List<GioHang>)cart;
            }

            return PartialView(list);
        }
        //GET : /Cart/ThemGioHang/?id=?&quantity=1 : thêm sản phẩm vào giỏ hàng
        public ActionResult ThemGioHang(int id, int soluong)
        {
            //lấy mã sách và gán đối tượng
            var sanpham = new GioHangProcess().LayMaSanPham(id);

            //lấy giỏ hàng từ session
            var cart = Session[GioHang];

            //nếu đã có sản phẩm trong giỏ hàng
            if (cart != null)
            {
                var list = (List<GioHang>)cart;
                //nếu tồn tại mã sản phẩm
                if (list.Exists(x => x.sanpham.MaSanPham == id))
                {

                    foreach (var item in list)
                    {
                        if (item.sanpham.MaSanPham == id)
                        {
                            item.iSoLuong += soluong;
                        }
                    }
                }
                //nếu chưa tồn tại khởi tạo giỏ hàng
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new GioHang();
                    item.sanpham = sanpham;
                    item.iSoLuong = soluong;
                    list.Add(item);
                }

                //Gán vào session
                Session[GioHang] = list;
            }
            //nếu chưa đã có sản phẩm trong giỏ hàng
            else
            {
                //tạo mới giỏ hàng
                var item = new GioHang();
                item.sanpham = sanpham;
                item.iSoLuong = soluong;
                var list = new List<GioHang>();
                list.Add(item);

                //gán vào session
                Session[GioHang] = list;
            }

            return RedirectToAction("Index");
        }
        //Xóa 1 sản phẩm trong giỏ hàng
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var sessionCart = (List<GioHang>)Session[GioHang];
            //xóa những giá trị mà có mã sp giống với id
            sessionCart.RemoveAll(x => x.sanpham.MaSanPham == id);
            //gán lại giá trị cho session
            Session[GioHang] = sessionCart;

            return Json(new
            {
                status = true
            });
        }
        //Xóa tất cả các sản phẩm trong giỏ hàng
        public JsonResult DeleteAll()
        {
            Session[GioHang] = null;
            return Json(new
            {
                status = true
            });
        }
        //Cập nhật giỏ hàng
        public JsonResult Update(string cartModel)
        {
            THONGTINSANPHAM sanphams = new THONGTINSANPHAM();

            //tạo 1 đối tượng dạng json
            var jsonCart = new JavaScriptSerializer().Deserialize<List<GioHang>>(cartModel);

            //ép kiểu từ session
            var sessionCart = (List<GioHang>)Session[GioHang];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.Single(x => x.sanpham.MaSanPham == item.sanpham.MaSanPham);
                sanphams = db.THONGTINSANPHAMs.Where(x => x.MaSanPham == item.sanpham.MaSanPham).FirstOrDefault();
                if (jsonItem != null)
                {
                    item.iSoLuong = jsonItem.iSoLuong;
                }
                if (item.iSoLuong > sanphams.SLTon)
                {
                    item.iSoLuong = (int)sanphams.SLTon;
                    TempData["message"] = "Sản phẩm: " + sanphams.TenSanPham + " chỉ còn  số lượng là : " + sanphams.SLTon;
                }
            }


            //cập nhật lại session
            Session[GioHang] = sessionCart;

            return Json(new
            {
                status = true
            });
        }


        //Thông tin khách hàng
        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult ThongTinKhachHang()
        {
            //lấy dữ liệu từ session
            var model = Session["User"];

            if (ModelState.IsValid)
            {
                //tìm tên tài khoản
                var result = db.KHACHHANGs.SingleOrDefault(x => x.TenDN == model);

                //trả về dữ liệu tương ứng
                return PartialView(result);
            }

            return PartialView();
        }
        [HttpGet]
        public ActionResult ThanhToan()
        {
            //kiểm tra đăng nhập
            if (Session["User"] == null || Session["User"].ToString() == "")
            {
                return RedirectToAction("PageDangNhap", "User");
            }
            else
            {

                var cart = Session[GioHang];
                var list = new List<GioHang>();
                var sl = 0;
                double? total = 0;
                if (cart != null)
                {
                    list = (List<GioHang>)cart;
                    sl = list.Sum(x => x.iSoLuong);
                    total = list.Sum(x => x.iThanhTien);
                }
                ViewBag.Quantity = sl;
                if (Session["Kilomet"] != null)
                {
                    object myObject = new Object();
                    string myObjectString = Session["Kilomet"].ToString();
                    double phiship = double.Parse(myObjectString);
                    ViewBag.Total = total + phiship;
                }
                else
                {
                    ViewBag.Total = total;
                }
                //ViewBag.Total = total;
                return View(list);
            }
        }

        public ActionResult ThanhToanVNPay(double? thanhTien)
        {
            // VNPay keys, thay đổi trong Web.config
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.1.0");
            pay.AddRequestData("vnp_Command", "pay");
            pay.AddRequestData("vnp_TmnCode", tmnCode);
            pay.AddRequestData("vnp_Amount", (thanhTien * 100).ToString());
            pay.AddRequestData("vnp_BankCode", "");
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress());
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang");
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", returnUrl);
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }

        public ActionResult TinhTrangThanhToanVNPay()
        {
            // Kiểm tra session datHang và giaoHang, nếu null trỏ về thằng ThanhToan
            // Lý do cần làm như này do có khả năng người dùng reload lại trang này (?) dẫn đến bug tại dòng 291
            if (Session["SS_DatHang"] == null || Session["SS_GiaoHang"] == null)
            {
                return RedirectToAction("ThanhToan", "Cart");
            }

            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"];
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode");
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"];

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret);

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        PHIEUDATHANG pdh = (PHIEUDATHANG)Session["SS_DatHang"];
                        PHIEUGIAOHANG pgh = (PHIEUGIAOHANG)Session["SS_GiaoHang"];

                        var rsDDH = new GioHangProcess().InsertDDH(pdh);
                        pgh.MaPhieuDH = rsDDH;
                        var rsPGH = new GioHangProcess().InsertPGH(pgh);
                        XacNhanEmail(rsDDH);
                        ViewBag.IsSuccess = "Yes";

                        // Message có thể thay đổi tùy ý theo tình huống, đây đang đặt mặc định trả về kết quả thực hiện giao dịch
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " Mã giao dịch: " + vnpayTranId;
                    }
                    else if (vnp_ResponseCode == "24")
                    {
                        ViewBag.IsSuccess = "Cancel";
                        ViewBag.Message = "Đã hủy thanh toán hóa đơn";
                    }
                    else
                    {
                        ViewBag.IsSuccess = "No";
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " Mã giao dịch: " + vnpayTranId;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }

                // Thực hiện được hay không giao dịch thì cũng phải remove cái session của datHang và giaoHang kia đi
                // Để các lần thực hiện sau không bị đè, lỗi hay trùng lặp dữ liệu. Tuyệt đối phải lưu ý tại điểm này
                Session.Remove("SS_DatHang");
                Session.Remove("SS_GiaoHang");
            }

            return View();
        }


        // thanh toán momo

        public ActionResult TinhTrangThanhToanMoMo()
        {
            string orderId = Request.QueryString["orderId"].ToString();
            string momoTranId = Request.QueryString["transId"].ToString();
             // Kiểm tra session datHang và giaoHang, nếu null trỏ về thằng ThanhToan
            // Lý do cần làm như này do có khả năng người dùng reload lại trang này (?) dẫn đến bug tại dòng 291
            //if (Session["SS_DatHang"] == null || Session["SS_GiaoHang"] == null)
            //{
            //    return RedirectToAction("ThanhToan", "Cart");
            //}
            string errorCode = Request.QueryString["errorCode"].ToString();
            if (int.Parse(errorCode) != 0)
            {
                ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                
            }
            else
            {
                PHIEUDATHANG pdh = (PHIEUDATHANG)Session["SS_DatHang1"];
                PHIEUGIAOHANG pgh = (PHIEUGIAOHANG)Session["SS_GiaoHang1"];

                var rsDDH = new GioHangProcess().InsertDDH(pdh);
                pgh.MaPhieuDH = rsDDH;
                var rsPGH = new GioHangProcess().InsertPGH(pgh);
                XacNhanEmail(rsDDH);
                ViewBag.IsSuccess = "Yes";

                        // Message có thể thay đổi tùy ý theo tình huống, đây đang đặt mặc định trả về kết quả thực hiện giao dịch
                ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " Mã giao dịch: " + momoTranId;
                
                
            }
            return RedirectToAction("TrangChu", "Home");
        }
        
           

        // momo
        public ActionResult ThanhToanMoMo(double? thanhTien)
        {
            //request params need to request to MoMo system
           
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "test";
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrlMoMo"];
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = thanhTien.ToString();
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());
        }




        // type: 1. Thanh toán khi nhận hàng, 2. Thanh toán bằng phương thức VNPay
        [HttpPost]
        public ActionResult ThanhToan(int MaKH, FormCollection f, int type)
        {
            if (!ModelState.IsValid)
                return View();

            var datHang = new PHIEUDATHANG();
            var giaoHang = new PHIEUGIAOHANG();

            var khachHang = db.KHACHHANGs.Where(x => x.MaKH == MaKH).FirstOrDefault();
            var gioHang = Session[GioHang];
            var lstGioHang = new List<GioHang>();

            int soLuong = 0;
            double? thanhTien = 0;
            string diaChi = Convert.ToString(f["DiaChi"]);
            string SDT = Convert.ToString(f["DienThoai"]);

            if (gioHang != null)
            {
                lstGioHang = (List<GioHang>)gioHang;
                soLuong = lstGioHang.Sum(x => x.iSoLuong);
                thanhTien = lstGioHang.Sum(x => x.iThanhTien);
            }
            double phiship = 0;
            if (Session["Kilomet"] != null)
            {
                object myObject = new Object();
                string myObjectString = Session["Kilomet"].ToString();
                phiship = double.Parse(myObjectString);
                thanhTien = thanhTien + phiship;
            }

            ViewBag.Quantity = soLuong;
            ViewBag.Total = thanhTien;

            datHang.NgayDat = DateTime.Now;
            datHang.TinhTrang = -1; // Chua xac nhan
            datHang.MaKH = MaKH;
            datHang.Tong_SL_Dat = soLuong;
            datHang.PhiShip = phiship;
            datHang.ThanhTien = thanhTien;

            giaoHang.TenKH = khachHang.TenKH;
            giaoHang.Email = khachHang.Email;
            giaoHang.DiaChi = diaChi;
            giaoHang.SDT = SDT;
            giaoHang.NgayTao = DateTime.Now;
            Session["DiaChi"] = diaChi;

            if (type == 1)
            {
                var rsDDH = new GioHangProcess().InsertDDH(datHang);
                giaoHang.MaPhieuDH = rsDDH;
                var rsPGH = new GioHangProcess().InsertPGH(giaoHang);
                ViewBag.MaPhieuDDH = rsDDH;

                BuildUserTemplate(ViewBag.MaPhieuDDH);
                if (rsDDH > 0)
                {
                    ModelState.Clear();
                    return RedirectToAction("KiemTraThongBaoKichHoat", "Cart");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng ký không thành công.");
                    return View();
                }
            }

            else if (type == 2)
            {
                // Lưu session của 2 đối tượng datHang và giaoHang để xử lý sau khi thanh toán VNPay
                // Chỉ khi có kết quả giao dịch trả về tại TinhTrangThanhToanVNPay mới thực hiện CRUD
                Session["SS_DatHang"] = datHang;
                Session["SS_GiaoHang"] = giaoHang;
                return RedirectToAction("ThanhToanVNPay", "Cart", new { thanhTien = thanhTien });
            }
            else
            {
                Session["SS_DatHang1"] = datHang;
                Session["SS_GiaoHang1"] = giaoHang;
                return RedirectToAction("ThanhToanMoMo", "Cart", new { thanhTien = thanhTien });
            }
        }


        public ActionResult XacNhan(int MaDH)
        {
            THONGTINSANPHAM sanphams = new THONGTINSANPHAM();
            //CT_PHIEUDATHANG ctpdh = new CT_PHIEUDATHANG();
            var list_sl = new List<CT_PHIEUDATHANG>();
            ViewBag.Madh = MaDH;
            var cart = Session[GioHang];
            var list = new List<GioHang>();
            var sl = 0;
            double? total = 0;
            if (cart != null)
            {
                list = (List<GioHang>)cart;
                sl = list.Sum(x => x.iSoLuong);
                total = list.Sum(x => x.iThanhTien);
            }

            ViewBag.Quantity = sl;

            if (Session["Kilomet"] != null)
            {
                object myObject = new Object();
                string myObjectString = Session["Kilomet"].ToString();
                double phiship = double.Parse(myObjectString);
                ViewBag.Total = total + phiship;
            }
            else
            {
                ViewBag.Total = total;
            }
            //ViewBag.Total = total;

            //Session["Kilomet"] = null;
            //Session["DiaChiNhan"] = null;
            //Session["checkDC"] = null;
            return View(list);

        }

        public JsonResult XacNhanEmail(int MaDH)
        {
            PHIEUDATHANG Data = db.PHIEUDATHANGs.Where(x => x.MaPhieuDH == MaDH).FirstOrDefault();
            Data.TinhTrang = 0;
            db.SaveChanges();
            var msg = "Mua hàng thành công.";
            //thêm dữ liệu vào đơn đặt hàng
            var order = new PHIEUDATHANG();
            var cart = (List<GioHang>)Session[GioHang];
            var result2 = new GioHangProcess();
            foreach (var item in cart)
            {

                var orderDetail = new CT_PHIEUDATHANG();
                orderDetail.MaPhieuDH = MaDH;
                orderDetail.MaSanPham = item.sanpham.MaSanPham;
                orderDetail.SoLuong = item.iSoLuong;

                int MaSP = item.sanpham.MaSanPham;  
                double? giaSanPham = new ProductProcess().GiaSanPham(MaSP);
                orderDetail.DonGia = giaSanPham;
                //orderDetail.DonGia = item.sanpham.GiaSanPham;
                result2.InsertCT_DDH(orderDetail);
            }
            Session[GioHang] = null;

            Session["Kilomet"] = null;
            Session["DiaChiNhan"] = null;
            Session["checkDC"] = null;
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteDonDatHang(int MaDH)
        {
            PHIEUDATHANG Data = db.PHIEUDATHANGs.Where(x => x.MaPhieuDH == MaDH).FirstOrDefault();
            Data.TinhTrang = -1;
            db.SaveChanges();
            var msg = "Hủy Đơn Hàng Thành Công";
            //thêm dữ liệu vào đơn đặt hàng         
            Session[GioHang] = null;
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        public void BuildUserTemplate(int MaDH)
        {

            //thân của email sẽ dc gửi
            string body =
                System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            var inforKH = db.PHIEUDATHANGs.Include("KHACHHANG").Where(x => x.MaPhieuDH == MaDH).First();
            //var inforDH = db.PHIEUDATHANGs.Include("PHIEUDATHANG").Where(x => x.MaPhieuDH == MaDH).First();
            var diachi = Session["DiaChi"];
            var cart = Session[GioHang];
            var list = new List<GioHang>();
            if (cart != null)
            {
                list = (List<GioHang>)cart;
            }

            string dsSP = "";

            foreach (var item in list)
            {
                dsSP += item.sanpham.TenSanPham + "<br>";
            }

            THONGTINSANPHAM tt = new THONGTINSANPHAM();



            //var tensp = from b in db.CT_PHIEUDATHANG 
            //            join c in db.THONGTINSANPHAMs on b.MaSanPham equals c.MaSanPham
            //            select new { c.TenSanPham };

            var url = "http://localhost:57161/" + "Cart/XacNhan?MaDH=" + MaDH;
            body = body.Replace("@ViewBag.MaDH", MaDH.ToString());
            body = body.Replace("@ViewBag.TenSanPham", dsSP);
            body = body.Replace("@ViewBag.LinkXacNhan", url);
            body = body.Replace("@ViewBag.TenUser", inforKH.KHACHHANG.TenKH);
            body = body.Replace("@ViewBag.NgayDat", inforKH.NgayDat.ToString());
            body = body.Replace("@ViewBag.TongSL", inforKH.Tong_SL_Dat.ToString());
            body = body.Replace("@ViewBag.DiaChi", diachi.ToString());
            body = body.Replace("@ViewBag.ThanhTien", inforKH.ThanhTien.ToString());



            body = body.ToString();
            //gọi hàm phía dưới và truyền tham số vào để tiến hành gửi email
            BuildEmailTemplate("Đơn Hàng Xác Nhận Thành Công", body, inforKH.KHACHHANG.Email);

        }

        public void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            //gmail của trang web
            from = "gapdaudomdo01@gmail.com";
            //gửi tến email kasch hàng
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }

            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, new ContentType("text/html")));
            SendEmail(mail);
        }

        public static void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            // Tạo SmtpClient kết nối đến smtp.gmail.com
            client.Host = "smtp.gmail.com";
            client.Port = 587; //gmail làm vc trên cổng này
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            // Tạo xác thực bằng địa chỉ gmail và password
            client.Credentials = new System.Net.NetworkCredential("thanhtung140723@gmail.com", "jmpiefjmevyynheb");
            try
            {
                client.Send(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }
        public ActionResult KiemTraThongBaoKichHoat()
        {
            return View();
        }

        public ActionResult DonHang_KH()
        {

            List<PHIEUDATHANG> donDatHang = db.PHIEUDATHANGs.Where(p => p.MaKH == UserController.khachhangstatic.MaKH).ToList();
            return View(donDatHang);

        }
        [HttpGet]
        public ActionResult DetailsDonDatHang(int id)
        {
            var tinhtrang = new GioHangProcess().GetDDHLoadCT_DDH(id);
            List<ChiTietDDHViewModel> lst = new List<ChiTietDDHViewModel>();
            List<CT_PHIEUDATHANG> lstCT = new GioHangProcess().DanhSachCT_DDH(id);

            //foreach (var item in lstCT)
            //{
            //    THONGTINSANPHAM sanphams = db.THONGTINSANPHAMs.Where(x => x.MaSanPham == item.MaSanPham).FirstOrDefault();
            //    lst.Add(new ChiTietDDHViewModel() {MaSanPham = sanphams.MaSanPham, HinhAnh = sanphams.HinhAnh, TenSanPham = sanphams.TenSanPham, Gia = sanphams.GiaSanPham, SoLuong = item.SoLuong, GiaGiam= sanphams.GiamGia });
            //}


            foreach (var item in lstCT)
            {
                THONGTINSANPHAM sanphams = db.THONGTINSANPHAMs.Where(x => x.MaSanPham == item.MaSanPham).FirstOrDefault();
                lst.Add(new ChiTietDDHViewModel() { MaSanPham = sanphams.MaSanPham, HinhAnh = sanphams.HinhAnh, TenSanPham = sanphams.TenSanPham, Gia = item.DonGia, SoLuong = item.SoLuong, GiaGiam = sanphams.GiamGia });
            }

            double? thanhtien = 0;
            thanhtien = tinhtrang.ThanhTien;

            ViewBag.PhiShip = tinhtrang.PhiShip;

            ViewBag.Total = thanhtien;
            if (tinhtrang.TinhTrang == 0)
            {
                ViewBag.TinhTrang = "Xử lý";
            }
            else if (tinhtrang.TinhTrang == 1)
            {
                ViewBag.TinhTrang = "Đã đóng gói";
            }
            else if (tinhtrang.TinhTrang == 2)
            {
                ViewBag.TinhTrang = "Đang giao hàng";
            }
            else
            {
                ViewBag.TinhTrang = "Giao hàng hoàn tất";
            }

            return View(lst);

        }

        //api chinh
        //private const string API_KEY = "AIzaSyAWOyX-d6CV4Z-58dGw1ujwVvMTctBykho";
        //private const string API_URL = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins={0}&destinations={1}&key=" + API_KEY;
        //api thuê đến 15/02/2023.
        //private const string API_URL = "https://api.distancematrix.ai/maps/api/distancematrix/json?origins={0}&destinations={1}&departure_time=now&key=0oFcQssi87L9DRl5QUnq3F3mfRsxu";
        //api thuê đến 21/02/2023.
        private const string API_URL = "https://api.distancematrix.ai/maps/api/distancematrix/json?origins={0}&destinations={1}&departure_time=now&key=FbMvE9zH5jdABVVxjzJrdkWc3lO1V";
        [HttpGet]
        public ActionResult CalculateDistance()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> CalculateDistance(DistanceCalculation distanceCalculation)
        {
            using (var client = new HttpClient())
            {
                var model = Session["User"];
                string endLocation = distanceCalculation.EndLocation;
                if (endLocation == null)
                {
                    var Ketqua = db.KHACHHANGs.SingleOrDefault(x => x.TenDN == model);
                    endLocation = Ketqua.DiaChi;
                }
                string startLocation = "140 Lê Trọng Tấn, Phường Tây Thạnh, Quận Tân Phú, TP.HCM";
                //string endLocation = distanceCalculation.EndLocation;
                Session["DiaChiNhan"] = endLocation;
                Session["checkDC"] = 1;

                string url = string.Format(API_URL, startLocation, endLocation);

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<dynamic>(result);
                    
                    //if(endLocation.Contains("TP.HCM") || endLocation.Contains("TPHCM") || endLocation.Contains("Thành Phố Hồ Chí Minh") || endLocation.Contains("Hồ Chí Minh"))

                    //Thành tiền phí Ship
                    double soKilomet = (json.rows[0].elements[0].distance.value / 1000);
                    double phiShipHang = new GioHangProcess().tinhPhiShipHang(soKilomet);
                    Session["Kilomet"] = phiShipHang;
                    //Session["Kilomet"] = (json.rows[0].elements[0].distance.value / 1000) * 500;
                }
            }
            return RedirectToAction("Index", "Cart");
            //return View();
            //return View(distanceCalculation);
        }
    }
}
