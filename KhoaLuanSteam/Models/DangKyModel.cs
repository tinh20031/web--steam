using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KhoaLuanSteam.Models
{
    public class DangKyModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập tên")]
        [Display(Name = "Tên Khách hàng")]
        public string TenKH { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ")]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDN { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập ngày sinh")]
        [Display(Name = "Ngày sinh")]
        public Nullable<System.DateTime> NgaySinh { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập giới tính")]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }
        
    }
}