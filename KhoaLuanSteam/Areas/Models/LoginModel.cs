using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KhoaLuanSteam.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập tài khoản")]
        [Display(Name = "Tài khoản")]
        //public string TaiKhoan { get; set; }
        public string TenDN { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; }

        [Display(Name = "Ghi nhớ")]
        public bool? GhiNho { get; set; }

        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        [Display(Name = "Trạng thái")]
        public bool? TrangThai { get; set; }
    }
}