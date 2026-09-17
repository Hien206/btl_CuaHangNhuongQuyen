using System;
using System.Collections.Generic;

namespace BanMayTinh_NguoiDung.Models
{
    public partial class GiaSanPham
    {
        public int MaGiaSanPham { get; set; }
        public int MaSanPham { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public double? Gia { get; set; }
    }
}
