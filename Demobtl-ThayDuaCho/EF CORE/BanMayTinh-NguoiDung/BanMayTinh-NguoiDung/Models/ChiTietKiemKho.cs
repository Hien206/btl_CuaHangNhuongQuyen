using System;
using System.Collections.Generic;

namespace BanMayTinh_NguoiDung.Models
{
    public partial class ChiTietKiemKho
    {
        public int MaChiTietKiemKho { get; set; }
        public int MaSanPham { get; set; }
        public int MaKiemKho { get; set; }
        public int? SoLuongDemDuoc { get; set; }
        public int? SoLuongTinhToan { get; set; }
        public int? SoLuongThayDoi { get; set; }

        public virtual KiemKho MaKiemKhoNavigation { get; set; } = null!;
        public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
    }
}
