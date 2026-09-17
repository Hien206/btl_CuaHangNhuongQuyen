using System;
using System.Collections.Generic;

namespace BanMayTinh_Admin.Models
{
    public partial class SanPham
    {
        public SanPham()
        {
            ChiTietAnhSanPhams = new HashSet<ChiTietAnhSanPham>();
            ChiTietHoaDonNhaps = new HashSet<ChiTietHoaDonNhap>();
            ChiTietHoaDonXuats = new HashSet<ChiTietHoaDonXuat>();
            ChiTietKhos = new HashSet<ChiTietKho>();
            ChiTietKhuyenMais = new HashSet<ChiTietKhuyenMai>();
            ChiTietKiemKhos = new HashSet<ChiTietKiemKho>();
            ChiTietNhoms = new HashSet<ChiTietNhom>();
            GiamGia = new HashSet<GiamGium>();
            LichSuGiaBans = new HashSet<LichSuGiaBan>();
            ThongSoKyThuats = new HashSet<ThongSoKyThuat>();
        }

        public int MaSanPham { get; set; }
        public int MaDanhMuc { get; set; }
        public string TenSanPham { get; set; } = null!;
        public string MoTaSanPham { get; set; } = null!;
        public string AnhDaiDien { get; set; } = null!;
        public int MaNhaSanXuat { get; set; }
        public int MaDonViTinh { get; set; }
        public DateTime NgayTao { get; set; }

        public virtual ICollection<ChiTietAnhSanPham> ChiTietAnhSanPhams { get; set; }
        public virtual ICollection<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; }
        public virtual ICollection<ChiTietHoaDonXuat> ChiTietHoaDonXuats { get; set; }
        public virtual ICollection<ChiTietKho> ChiTietKhos { get; set; }
        public virtual ICollection<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; }
        public virtual ICollection<ChiTietKiemKho> ChiTietKiemKhos { get; set; }
        public virtual ICollection<ChiTietNhom> ChiTietNhoms { get; set; }
        public virtual ICollection<GiamGium> GiamGia { get; set; }
        public virtual ICollection<LichSuGiaBan> LichSuGiaBans { get; set; }
        public virtual ICollection<ThongSoKyThuat> ThongSoKyThuats { get; set; }
    }
}
