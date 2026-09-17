using System;
using System.Collections.Generic;

namespace BanMayTinh_NguoiDung.Models
{
    public partial class ChiTietHoaDonXuat
    {
        public int MaChiTietHoaDonXuat { get; set; }
        public int MaHoaDonXuat { get; set; }
        public int MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public double GiaBan { get; set; }
        public double? ChietKhau { get; set; }
        public int? TraLai { get; set; }

        public virtual HoaDonXuat MaHoaDonXuatNavigation { get; set; } = null!;
        public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
    }
}
