using System;
using System.Collections.Generic;

namespace BanMayTinh_Admin.Models
{
    public partial class GiamGium
    {
        public int MaGiamGia { get; set; }
        public int? MaSanPham { get; set; }
        public double? PhanTram { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public bool? TrangThai { get; set; }

        public virtual SanPham? MaSanPhamNavigation { get; set; }
    }
}
