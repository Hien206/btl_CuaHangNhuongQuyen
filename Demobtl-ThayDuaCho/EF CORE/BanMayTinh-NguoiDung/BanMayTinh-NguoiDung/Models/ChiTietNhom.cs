using System;
using System.Collections.Generic;

namespace BanMayTinh_NguoiDung.Models
{
    public partial class ChiTietNhom
    {
        public int MaChiTietNhom { get; set; }
        public int MaNhomSanPham { get; set; }
        public int MaSanPham { get; set; }

        public virtual NhomSanPham MaNhomSanPhamNavigation { get; set; } = null!;
        public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
    }
}
