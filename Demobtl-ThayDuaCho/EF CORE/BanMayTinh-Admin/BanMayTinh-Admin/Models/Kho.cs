using System;
using System.Collections.Generic;

namespace BanMayTinh_Admin.Models
{
    public partial class Kho
    {
        public Kho()
        {
            ChiTietKhos = new HashSet<ChiTietKho>();
            KiemKhos = new HashSet<KiemKho>();
        }

        public int MaKho { get; set; }
        public string TenKho { get; set; } = null!;
        public string DiaChi { get; set; } = null!;

        public virtual ICollection<ChiTietKho> ChiTietKhos { get; set; }
        public virtual ICollection<KiemKho> KiemKhos { get; set; }
    }
}
