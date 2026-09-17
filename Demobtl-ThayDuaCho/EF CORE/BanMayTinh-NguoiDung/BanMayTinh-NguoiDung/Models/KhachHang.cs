using System;
using System.Collections.Generic;

namespace BanMayTinh_NguoiDung.Models
{
    public partial class KhachHang
    {
        public KhachHang()
        {
            HoaDonXuats = new HashSet<HoaDonXuat>();
        }

        public int MaKhachHang { get; set; }
        public string TenKhachHang { get; set; } = null!;
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }

        public virtual ICollection<HoaDonXuat> HoaDonXuats { get; set; }
    }
}
