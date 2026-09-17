using System;
using System.Collections.Generic;

namespace BanMayTinh_NguoiDung.Models
{
    public partial class NhaSanXuat
    {
        public int MaNhaSanXuat { get; set; }
        public string TenNhaSanXuat { get; set; } = null!;
        public string? MoTa { get; set; }
    }
}
