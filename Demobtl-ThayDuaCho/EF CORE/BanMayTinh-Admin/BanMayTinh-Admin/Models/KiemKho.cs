using System;
using System.Collections.Generic;

namespace BanMayTinh_Admin.Models
{
    public partial class KiemKho
    {
        public KiemKho()
        {
            ChiTietKiemKhos = new HashSet<ChiTietKiemKho>();
        }

        public int MaKiemKho { get; set; }
        public int? MaNguoiDung { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public int? TrangThaiKho { get; set; }
        public int? MaKho { get; set; }
        public string? MoTa { get; set; }

        public virtual Kho? MaKhoNavigation { get; set; }
        public virtual ICollection<ChiTietKiemKho> ChiTietKiemKhos { get; set; }
    }
}
