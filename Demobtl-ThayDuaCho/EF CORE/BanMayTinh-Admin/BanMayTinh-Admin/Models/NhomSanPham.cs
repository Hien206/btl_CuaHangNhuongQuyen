using System;
using System.Collections.Generic;

namespace BanMayTinh_Admin.Models
{
    public partial class NhomSanPham
    {
        public NhomSanPham()
        {
            ChiTietNhoms = new HashSet<ChiTietNhom>();
        }

        public int MaNhomSanPham { get; set; }
        public string? TenNhom { get; set; }
        public bool? TrangThai { get; set; }

        public virtual ICollection<ChiTietNhom> ChiTietNhoms { get; set; }
    }
}
