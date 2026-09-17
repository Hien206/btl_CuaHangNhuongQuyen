using System;
using System.Collections.Generic;

namespace BanMayTinh_Admin.Models
{
    public partial class ChiTietDonHang
    {
        public int MaChiTietDonHang { get; set; }
        public int MaDonHang { get; set; }
        public int MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public double GiaMua { get; set; }

        public virtual DonHang MaDonHangNavigation { get; set; } = null!;
    }
}
