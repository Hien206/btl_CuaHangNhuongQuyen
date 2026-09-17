using System;
using System.Collections.Generic;

namespace BanMayTinh_Admin.Models
{
    public partial class HoaDonNhap
    {
        public HoaDonNhap()
        {
            ChiTietHoaDonNhaps = new HashSet<ChiTietHoaDonNhap>();
        }

        public int MaHoaDonNhap { get; set; }
        public string SoHoaDon { get; set; } = null!;
        public DateTime NgayNhap { get; set; }
        public int MaNguoiDung { get; set; }
        public int MaNhaCungCap { get; set; }

        public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
        public virtual NhaCungCap MaNhaCungCapNavigation { get; set; } = null!;
        public virtual ICollection<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; }
    }
}
