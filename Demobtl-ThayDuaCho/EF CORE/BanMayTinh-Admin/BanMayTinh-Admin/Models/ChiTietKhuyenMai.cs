using System;
using System.Collections.Generic;

namespace BanMayTinh_Admin.Models
{
    public partial class ChiTietKhuyenMai
    {
        public int MaChiTietKhuyenMai { get; set; }
        public int MaSanPham { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public int MaKhuyenMai { get; set; }
        public bool TrangThai { get; set; }

        public virtual KhuyenMai MaKhuyenMaiNavigation { get; set; } = null!;
        public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
    }
}
