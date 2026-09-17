
using BanMayTinh_Admin.Models;

namespace BanMayTinh_Admin.Code
{
    public class UserModels
    {
        public NguoiDung nguoidung { get; set; }
        public TaiKhoan taikhoan { get; set; }
    }

    public class UserEditModels
    {
        public int MaNguoiDung { get; set; }
        public string? HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? AnhDaiDien { get; set; }
        public string? DiaChi { get; set; }
        public string? Email { get; set; }
        public string? DienThoai { get; set; }
        public bool? TrangThai { get; set; }
        public string? TaiKhoan1 { get; set; }
        public string? MatKhau { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string? LoaiQuyet { get; set; }
    }
}
