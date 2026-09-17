using System;
using System.Collections.Generic;

namespace BanMayTinh_NguoiDung.Models
{
    public partial class Menu
    {
        public int MaMenu { get; set; }
        public string TenMenu { get; set; } = null!;
        public bool TrangThai { get; set; }
    }
}
