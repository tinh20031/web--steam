//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KhoaLuanSteam.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class THONGTINSANPHAM
    {
        public THONGTINSANPHAM()
        {
            this.CT_DonDatHangNCC = new HashSet<CT_DonDatHangNCC>();
            this.CT_PHIEUDATHANG = new HashSet<CT_PHIEUDATHANG>();
            this.CT_PHIEUNHAPHANG = new HashSet<CT_PHIEUNHAPHANG>();
            this.SPSALEs = new HashSet<SPSALE>();
        }
    
        public int MaSanPham { get; set; }
        public Nullable<int> MaLoai { get; set; }
        public string MaNCC { get; set; }
        public string TenSanPham { get; set; }
        public Nullable<double> GiaSanPham { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public Nullable<double> GiamGia { get; set; }
        public Nullable<int> SLTon { get; set; }
    
        public virtual ICollection<CT_DonDatHangNCC> CT_DonDatHangNCC { get; set; }
        public virtual ICollection<CT_PHIEUDATHANG> CT_PHIEUDATHANG { get; set; }
        public virtual ICollection<CT_PHIEUNHAPHANG> CT_PHIEUNHAPHANG { get; set; }
        public virtual LOAISANPHAM LOAISANPHAM { get; set; }
        public virtual NHACUNGCAP NHACUNGCAP { get; set; }
        public virtual ICollection<SPSALE> SPSALEs { get; set; }
    }
}