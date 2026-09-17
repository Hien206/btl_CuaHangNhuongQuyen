using System;
using System.Collections.Generic;

namespace BanMayTinh_NguoiDung.Models
{
    public partial class NguoiDung
    {
        public NguoiDung()
        {
            HoaDonNhaps = new HashSet<HoaDonNhap>();
            TaiKhoans = new HashSet<TaiKhoan>();
        }

        public int MaNguoiDung { get; set; }
        public string? HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? AnhDaiDien { get; set; }
        public string? DiaChi { get; set; }
        public string? Email { get; set; }
        public string? DienThoai { get; set; }
        public bool? TrangThai { get; set; }

        public virtual ICollection<HoaDonNhap> HoaDonNhaps { get; set; }
        public virtual ICollection<TaiKhoan> TaiKhoans { get; set; }
    }
}
