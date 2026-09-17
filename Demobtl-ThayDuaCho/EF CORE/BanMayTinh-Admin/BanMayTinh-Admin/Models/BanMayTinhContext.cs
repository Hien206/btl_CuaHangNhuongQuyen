using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BanMayTinh_Admin.Models
{
    public partial class BanMayTinhContext : DbContext
    {
        private string connectstring;
        public BanMayTinhContext(IConfiguration configuration)
        {
            connectstring = configuration["ConnectionStrings:DefaultConnection"].ToString();
        }

        public BanMayTinhContext(DbContextOptions<BanMayTinhContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiTietAnhSanPham> ChiTietAnhSanPhams { get; set; } = null!;
        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; } = null!;
        public virtual DbSet<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; } = null!;
        public virtual DbSet<ChiTietHoaDonXuat> ChiTietHoaDonXuats { get; set; } = null!;
        public virtual DbSet<ChiTietKho> ChiTietKhos { get; set; } = null!;
        public virtual DbSet<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; } = null!;
        public virtual DbSet<ChiTietKiemKho> ChiTietKiemKhos { get; set; } = null!;
        public virtual DbSet<ChiTietNhom> ChiTietNhoms { get; set; } = null!;
        public virtual DbSet<DanhMuc> DanhMucs { get; set; } = null!;
        public virtual DbSet<DonHang> DonHangs { get; set; } = null!;
        public virtual DbSet<DonViTinh> DonViTinhs { get; set; } = null!;
        public virtual DbSet<GiaSanPham> GiaSanPhams { get; set; } = null!;
        public virtual DbSet<GiamGium> GiamGia { get; set; } = null!;
        public virtual DbSet<HoaDonNhap> HoaDonNhaps { get; set; } = null!;
        public virtual DbSet<HoaDonXuat> HoaDonXuats { get; set; } = null!;
        public virtual DbSet<KhachHang> KhachHangs { get; set; } = null!;
        public virtual DbSet<Kho> Khos { get; set; } = null!;
        public virtual DbSet<KhuyenMai> KhuyenMais { get; set; } = null!;
        public virtual DbSet<KiemKho> KiemKhos { get; set; } = null!;
        public virtual DbSet<LichSuGiaBan> LichSuGiaBans { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; } = null!;
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; } = null!;
        public virtual DbSet<NhaSanXuat> NhaSanXuats { get; set; } = null!;
        public virtual DbSet<NhomSanPham> NhomSanPhams { get; set; } = null!;
        public virtual DbSet<SanPham> SanPhams { get; set; } = null!;
        public virtual DbSet<Silde> Sildes { get; set; } = null!;
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; } = null!;
        public virtual DbSet<ThongSoKyThuat> ThongSoKyThuats { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(connectstring);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietAnhSanPham>(entity =>
            {
                entity.HasKey(e => e.MaAnhChitiet);

                entity.ToTable("ChiTietAnhSanPham");

                entity.Property(e => e.Anh)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.ChiTietAnhSanPhams)
                    .HasForeignKey(d => d.MaSanPham)
                    .HasConstraintName("FK_ChiTietAnhSanPham_SanPham");
            });

            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.HasKey(e => e.MaChiTietDonHang);

                entity.ToTable("ChiTietDonHang");

                entity.HasOne(d => d.MaDonHangNavigation)
                    .WithMany(p => p.ChiTietDonHangs)
                    .HasForeignKey(d => d.MaDonHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietDonHang_DonHang");
            });

            modelBuilder.Entity<ChiTietHoaDonNhap>(entity =>
            {
                entity.HasKey(e => e.MaChiTiet);

                entity.ToTable("ChiTietHoaDonNhap");

                entity.Property(e => e.MaChiTiet).ValueGeneratedNever();

                entity.HasOne(d => d.MaHoaDonNhapNavigation)
                    .WithMany(p => p.ChiTietHoaDonNhaps)
                    .HasForeignKey(d => d.MaHoaDonNhap)
                    .HasConstraintName("FK_ChiTietHoaDonNhap_HoaDonNhap");

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.ChiTietHoaDonNhaps)
                    .HasForeignKey(d => d.MaSanPham)
                    .HasConstraintName("FK_ChiTietHoaDonNhap_SanPham");
            });

            modelBuilder.Entity<ChiTietHoaDonXuat>(entity =>
            {
                entity.HasKey(e => e.MaChiTietHoaDonXuat);

                entity.ToTable("ChiTietHoaDonXuat");

                entity.HasOne(d => d.MaHoaDonXuatNavigation)
                    .WithMany(p => p.ChiTietHoaDonXuats)
                    .HasForeignKey(d => d.MaHoaDonXuat)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietHoaDonXuat_HoaDonXuat");

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.ChiTietHoaDonXuats)
                    .HasForeignKey(d => d.MaSanPham)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietHoaDonXuat_SanPham");
            });

            modelBuilder.Entity<ChiTietKho>(entity =>
            {
                entity.HasKey(e => e.MaChiTietKho);

                entity.ToTable("ChiTietKho");

                entity.Property(e => e.KhayKhe).HasMaxLength(1500);

                entity.HasOne(d => d.MaKhoNavigation)
                    .WithMany(p => p.ChiTietKhos)
                    .HasForeignKey(d => d.MaKho)
                    .HasConstraintName("FK_ChiTietKho_Kho");

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.ChiTietKhos)
                    .HasForeignKey(d => d.MaSanPham)
                    .HasConstraintName("FK_ChiTietKho_SanPham");
            });

            modelBuilder.Entity<ChiTietKhuyenMai>(entity =>
            {
                entity.HasKey(e => e.MaChiTietKhuyenMai);

                entity.ToTable("ChiTietKhuyenMai");

                entity.Property(e => e.MaChiTietKhuyenMai).ValueGeneratedNever();

                entity.Property(e => e.NgayBatDau).HasColumnType("datetime");

                entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");

                entity.HasOne(d => d.MaKhuyenMaiNavigation)
                    .WithMany(p => p.ChiTietKhuyenMais)
                    .HasForeignKey(d => d.MaKhuyenMai)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietKhuyenMai_KhuyenMai");

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.ChiTietKhuyenMais)
                    .HasForeignKey(d => d.MaSanPham)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietKhuyenMai_SanPham");
            });

            modelBuilder.Entity<ChiTietKiemKho>(entity =>
            {
                entity.HasKey(e => e.MaChiTietKiemKho);

                entity.ToTable("ChiTietKiemKho");

                entity.Property(e => e.MaChiTietKiemKho).ValueGeneratedNever();

                entity.HasOne(d => d.MaKiemKhoNavigation)
                    .WithMany(p => p.ChiTietKiemKhos)
                    .HasForeignKey(d => d.MaKiemKho)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietKiemKho_KiemKho");

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.ChiTietKiemKhos)
                    .HasForeignKey(d => d.MaSanPham)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietKiemKho_SanPham");
            });

            modelBuilder.Entity<ChiTietNhom>(entity =>
            {
                entity.HasKey(e => e.MaChiTietNhom);

                entity.ToTable("ChiTietNhom");

                entity.HasOne(d => d.MaNhomSanPhamNavigation)
                    .WithMany(p => p.ChiTietNhoms)
                    .HasForeignKey(d => d.MaNhomSanPham)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietNhom_NhomSanPham");

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.ChiTietNhoms)
                    .HasForeignKey(d => d.MaSanPham)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietNhom_SanPham");
            });

            modelBuilder.Entity<DanhMuc>(entity =>
            {
                entity.HasKey(e => e.MaDanhMuc);

                entity.ToTable("DanhMuc");

                entity.Property(e => e.Stt).HasColumnName("STT");

                entity.Property(e => e.TenDanhMuc).HasMaxLength(250);
            });

            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.HasKey(e => e.MaDonHang);

                entity.ToTable("DonHang");

                entity.Property(e => e.NgayDat).HasColumnType("datetime");
            });

            modelBuilder.Entity<DonViTinh>(entity =>
            {
                entity.HasKey(e => e.MaDonViTinh);

                entity.ToTable("DonViTinh");

                entity.Property(e => e.TenDonViTinh).HasMaxLength(100);
            });

            modelBuilder.Entity<GiaSanPham>(entity =>
            {
                entity.HasKey(e => e.MaGiaSanPham)
                    .HasName("PK_GiaSanPham_1");

                entity.ToTable("GiaSanPham");

                entity.Property(e => e.NgayBatDau).HasColumnType("datetime");

                entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");
            });

            modelBuilder.Entity<GiamGium>(entity =>
            {
                entity.HasKey(e => e.MaGiamGia);

                entity.Property(e => e.MaGiamGia).ValueGeneratedNever();

                entity.Property(e => e.ThoiGianBatDau).HasColumnType("datetime");

                entity.Property(e => e.ThoiGianKetThuc).HasColumnType("datetime");

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.GiamGia)
                    .HasForeignKey(d => d.MaSanPham)
                    .HasConstraintName("FK_GiamGia_SanPham");
            });

            modelBuilder.Entity<HoaDonNhap>(entity =>
            {
                entity.HasKey(e => e.MaHoaDonNhap);

                entity.ToTable("HoaDonNhap");

                entity.Property(e => e.MaHoaDonNhap).ValueGeneratedNever();

                entity.Property(e => e.NgayNhap).HasColumnType("datetime");

                entity.Property(e => e.SoHoaDon)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.HoaDonNhaps)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HoaDonNhap_NguoiDung");

                entity.HasOne(d => d.MaNhaCungCapNavigation)
                    .WithMany(p => p.HoaDonNhaps)
                    .HasForeignKey(d => d.MaNhaCungCap)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HoaDonNhap_NhaCungCap");
            });

            modelBuilder.Entity<HoaDonXuat>(entity =>
            {
                entity.HasKey(e => e.MaHoaDonXuat);

                entity.ToTable("HoaDonXuat");

                entity.Property(e => e.NgayXuat).HasColumnType("datetime");

                entity.Property(e => e.SoHoaDon)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.MaKhachHangNavigation)
                    .WithMany(p => p.HoaDonXuats)
                    .HasForeignKey(d => d.MaKhachHang)
                    .HasConstraintName("FK_HoaDonXuat_KhachHang");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.MaKhachHang);

                entity.ToTable("KhachHang");

                entity.Property(e => e.DiaChi).HasMaxLength(1500);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TenKhachHang).HasMaxLength(250);
            });

            modelBuilder.Entity<Kho>(entity =>
            {
                entity.HasKey(e => e.MaKho);

                entity.ToTable("Kho");

                entity.Property(e => e.DiaChi).HasMaxLength(500);

                entity.Property(e => e.TenKho).HasMaxLength(250);
            });

            modelBuilder.Entity<KhuyenMai>(entity =>
            {
                entity.HasKey(e => e.MaKhuyenMai);

                entity.ToTable("KhuyenMai");

                entity.Property(e => e.MaKhuyenMai).ValueGeneratedNever();

                entity.Property(e => e.MoTa).HasColumnType("ntext");

                entity.Property(e => e.TenKhuyenMai).HasMaxLength(250);
            });

            modelBuilder.Entity<KiemKho>(entity =>
            {
                entity.HasKey(e => e.MaKiemKho);

                entity.ToTable("KiemKho");

                entity.Property(e => e.MaKiemKho).ValueGeneratedNever();

                entity.Property(e => e.MoTa).HasColumnType("ntext");

                entity.Property(e => e.ThoiGianBatDau).HasColumnType("datetime");

                entity.Property(e => e.ThoiGianKetThuc).HasColumnType("datetime");

                entity.HasOne(d => d.MaKhoNavigation)
                    .WithMany(p => p.KiemKhos)
                    .HasForeignKey(d => d.MaKho)
                    .HasConstraintName("FK_KiemKho_Kho");
            });

            modelBuilder.Entity<LichSuGiaBan>(entity =>
            {
                entity.HasKey(e => e.MaGiaBan);

                entity.ToTable("LichSuGiaBan");

                entity.Property(e => e.MaGiaBan).ValueGeneratedNever();

                entity.Property(e => e.NgayBatDau).HasColumnType("datetime");

                entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.LichSuGiaBans)
                    .HasForeignKey(d => d.MaSanPham)
                    .HasConstraintName("FK_LichSuGiaBan_SanPham");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.MaMenu);

                entity.ToTable("Menu");

                entity.Property(e => e.MaMenu).ValueGeneratedNever();

                entity.Property(e => e.TenMenu).HasMaxLength(250);
            });

            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.HasKey(e => e.MaNguoiDung);

                entity.ToTable("NguoiDung");

                entity.Property(e => e.AnhDaiDien)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DiaChi).HasMaxLength(1500);

                entity.Property(e => e.DienThoai)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.GioiTinh).HasMaxLength(20);

                entity.Property(e => e.HoTen).HasMaxLength(250);

                entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            });

            modelBuilder.Entity<NhaCungCap>(entity =>
            {
                entity.HasKey(e => e.MaNhaCungCap);

                entity.ToTable("NhaCungCap");

                entity.Property(e => e.MaNhaCungCap).ValueGeneratedNever();

                entity.Property(e => e.DiaChi).HasMaxLength(500);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TenNhaCungCap).HasMaxLength(250);
            });

            modelBuilder.Entity<NhaSanXuat>(entity =>
            {
                entity.HasKey(e => e.MaNhaSanXuat);

                entity.ToTable("NhaSanXuat");

                entity.Property(e => e.MoTa).HasColumnType("ntext");

                entity.Property(e => e.TenNhaSanXuat).HasMaxLength(250);
            });

            modelBuilder.Entity<NhomSanPham>(entity =>
            {
                entity.HasKey(e => e.MaNhomSanPham);

                entity.ToTable("NhomSanPham");

                entity.Property(e => e.TenNhom).HasMaxLength(250);
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasKey(e => e.MaSanPham);

                entity.ToTable("SanPham");

                entity.Property(e => e.AnhDaiDien)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.MoTaSanPham).HasColumnType("ntext");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.TenSanPham).HasMaxLength(250);
            });

            modelBuilder.Entity<Silde>(entity =>
            {
                entity.HasKey(e => e.MaSilde);

                entity.ToTable("Silde");

                entity.Property(e => e.Anh)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Link)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasKey(e => e.MaTaiKhoan);

                entity.ToTable("TaiKhoan");

                entity.Property(e => e.LoaiQuyet)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MatKhau)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NgayBatDau).HasColumnType("datetime");

                entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");

                entity.Property(e => e.TaiKhoan1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TaiKhoan");

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.TaiKhoans)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .HasConstraintName("FK_TaiKhoan_NguoiDung");
            });

            modelBuilder.Entity<ThongSoKyThuat>(entity =>
            {
                entity.HasKey(e => e.MaThongSo);

                entity.ToTable("ThongSoKyThuat");

                entity.Property(e => e.MaThongSo).ValueGeneratedNever();

                entity.Property(e => e.MoTa).HasMaxLength(500);

                entity.Property(e => e.TenThongSo).HasMaxLength(150);

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.ThongSoKyThuats)
                    .HasForeignKey(d => d.MaSanPham)
                    .HasConstraintName("FK_ThongSoKyThuat_SanPham");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
