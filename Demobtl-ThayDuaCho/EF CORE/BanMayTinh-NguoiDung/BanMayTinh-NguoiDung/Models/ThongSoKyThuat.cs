using System;
using System.Collections.Generic;

namespace BanMayTinh_NguoiDung.Models
{
    public partial class ThongSoKyThuat
    {
        public int MaThongSo { get; set; }
        public int? MaSanPham { get; set; }
        public string? TenThongSo { get; set; }
        public string? MoTa { get; set; }

        public virtual SanPham? MaSanPhamNavigation { get; set; }
    }
}
