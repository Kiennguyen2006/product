using Microsoft.EntityFrameworkCore;
using Nkklession14layout.Models;

namespace Nkklession14layout.Data;

public class NkkApplicationDbContext(DbContextOptions<NkkApplicationDbContext> options) : DbContext(options)
{
    public DbSet<NkkTaiKhoanModel> TaiKhoans => Set<NkkTaiKhoanModel>();
    public DbSet<NkkDienThoaiModel> DienThoais => Set<NkkDienThoaiModel>();
    public DbSet<NkkNhaCungCapModel> NhaCungCaps => Set<NkkNhaCungCapModel>();
    public DbSet<NkkKhachHangModel> KhachHangs => Set<NkkKhachHangModel>();
    public DbSet<NkkNhanVienModel> NhanViens => Set<NkkNhanVienModel>();
    public DbSet<NkkHoaDonModel> HoaDons => Set<NkkHoaDonModel>();
    public DbSet<NkkChiTietHoaDonModel> ChiTietHoaDons => Set<NkkChiTietHoaDonModel>();
    public DbSet<NkkPhieuNhapModel> PhieuNhaps => Set<NkkPhieuNhapModel>();
    public DbSet<NkkChiTietPhieuNhapModel> ChiTietPhieuNhaps => Set<NkkChiTietPhieuNhapModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NkkTaiKhoanModel>(entity =>
        {
            entity.ToTable("TaiKhoan");
            entity.HasKey(x => x.MaTaiKhoan).HasName("PK_TaiKhoan");
            entity.HasIndex(x => x.TenDangNhap).IsUnique().HasDatabaseName("UQ_TaiKhoan_TenDangNhap");
            entity.HasIndex(x => x.Email).IsUnique().HasDatabaseName("UQ_TaiKhoan_Email");
            entity.Property(x => x.MaTaiKhoan).HasColumnType("varchar(20)");
            entity.Property(x => x.TenDangNhap).HasMaxLength(50).IsRequired();
            entity.Property(x => x.MatKhau).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(100).IsRequired();
            entity.Property(x => x.NgayTao).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(x => x.PhanQuyen).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<NkkDienThoaiModel>(entity =>
        {
            entity.ToTable("DienThoai");
            entity.HasKey(x => x.MaDienThoai).HasName("PK_DienThoai");
            entity.Property(x => x.MaDienThoai).HasColumnType("varchar(20)");
            entity.Property(x => x.TenDienThoai).HasMaxLength(100).IsRequired();
            entity.Property(x => x.HangSanXuat).HasMaxLength(50);
            entity.Property(x => x.MauSac).HasMaxLength(50);
            entity.Property(x => x.Ram).HasMaxLength(20);
            entity.Property(x => x.BoNho).HasMaxLength(20);
            entity.Property(x => x.GiaNhap).HasPrecision(18, 2);
            entity.Property(x => x.GiaBan).HasPrecision(18, 2);
            entity.Property(x => x.SoLuong).HasDefaultValue(0);
            entity.Property(x => x.MoTa).HasMaxLength(500);
            entity.Property(x => x.Anh).HasMaxLength(255);
        });

        modelBuilder.Entity<NkkNhaCungCapModel>(entity =>
        {
            entity.ToTable("NhaCungCap");
            entity.HasKey(x => x.MaNhaCungCap).HasName("PK_NhaCungCap");
            entity.Property(x => x.MaNhaCungCap).HasColumnType("varchar(20)");
            entity.Property(x => x.TenNhaCungCap).HasMaxLength(100).IsRequired();
            entity.Property(x => x.SoDienThoai).HasColumnType("varchar(15)");
            entity.Property(x => x.Email).HasMaxLength(100);
            entity.Property(x => x.DiaChi).HasMaxLength(255);
        });

        modelBuilder.Entity<NkkKhachHangModel>(entity =>
        {
            entity.ToTable("KhachHang");
            entity.HasKey(x => x.MaKhachHang).HasName("PK_KhachHang");
            entity.Property(x => x.MaKhachHang).HasColumnType("varchar(20)");
            entity.Property(x => x.HoTen).HasMaxLength(100).IsRequired();
            entity.Property(x => x.GioiTinh).HasMaxLength(10);
            entity.Property(x => x.SoDienThoai).HasColumnType("varchar(15)");
            entity.Property(x => x.Email).HasMaxLength(100);
            entity.Property(x => x.DiaChi).HasMaxLength(255);
        });

        modelBuilder.Entity<NkkNhanVienModel>(entity =>
        {
            entity.ToTable("NhanVien");
            entity.HasKey(x => x.MaNhanVien).HasName("PK_NhanVien");
            entity.Property(x => x.MaNhanVien).HasColumnType("varchar(20)");
            entity.Property(x => x.HoTen).HasMaxLength(100).IsRequired();
            entity.Property(x => x.GioiTinh).HasMaxLength(10);
            entity.Property(x => x.SoDienThoai).HasColumnType("varchar(15)");
            entity.Property(x => x.Email).HasMaxLength(100);
            entity.Property(x => x.DiaChi).HasMaxLength(255);
            entity.Property(x => x.ChucVu).HasMaxLength(50);
            entity.Property(x => x.NgayVaoLam).HasColumnType("date");
        });

        modelBuilder.Entity<NkkHoaDonModel>(entity =>
        {
            entity.ToTable("HoaDon");
            entity.HasKey(x => x.MaHoaDon).HasName("PK_HoaDon");
            entity.Property(x => x.MaHoaDon).HasColumnType("varchar(20)");
            entity.Property(x => x.MaKhachHang).HasColumnType("varchar(20)").IsRequired();
            entity.Property(x => x.MaNhanVien).HasColumnType("varchar(20)").IsRequired();
            entity.Property(x => x.NgayLap).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(x => x.TongTien).HasPrecision(18, 2).HasDefaultValue(0);
            entity.Property(x => x.GhiChu).HasMaxLength(500);
            entity.HasOne(x => x.KhachHang).WithMany(x => x.HoaDons).HasForeignKey(x => x.MaKhachHang)
                .HasConstraintName("FK_HoaDon_KhachHang").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.NhanVien).WithMany(x => x.HoaDons).HasForeignKey(x => x.MaNhanVien)
                .HasConstraintName("FK_HoaDon_NhanVien").OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<NkkChiTietHoaDonModel>(entity =>
        {
            entity.ToTable("ChiTietHoaDon");
            entity.HasKey(x => new { x.MaHoaDon, x.MaDienThoai }).HasName("PK_ChiTietHoaDon");
            entity.Property(x => x.MaHoaDon).HasColumnType("varchar(20)");
            entity.Property(x => x.MaDienThoai).HasColumnType("varchar(20)");
            entity.Property(x => x.DonGia).HasPrecision(18, 2);
            entity.HasOne(x => x.HoaDon).WithMany(x => x.ChiTietHoaDons).HasForeignKey(x => x.MaHoaDon)
                .HasConstraintName("FK_ChiTietHoaDon_HoaDon").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.DienThoai).WithMany(x => x.ChiTietHoaDons).HasForeignKey(x => x.MaDienThoai)
                .HasConstraintName("FK_ChiTietHoaDon_DienThoai").OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<NkkPhieuNhapModel>(entity =>
        {
            entity.ToTable("PhieuNhap");
            entity.HasKey(x => x.MaPhieuNhap).HasName("PK_PhieuNhap");
            entity.Property(x => x.MaPhieuNhap).HasColumnType("varchar(20)");
            entity.Property(x => x.MaNhaCungCap).HasColumnType("varchar(20)").IsRequired();
            entity.Property(x => x.MaNhanVien).HasColumnType("varchar(20)").IsRequired();
            entity.Property(x => x.NgayNhap).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(x => x.TongTien).HasPrecision(18, 2).HasDefaultValue(0);
            entity.HasOne(x => x.NhaCungCap).WithMany(x => x.PhieuNhaps).HasForeignKey(x => x.MaNhaCungCap)
                .HasConstraintName("FK_PhieuNhap_NhaCungCap").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.NhanVien).WithMany(x => x.PhieuNhaps).HasForeignKey(x => x.MaNhanVien)
                .HasConstraintName("FK_PhieuNhap_NhanVien").OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<NkkChiTietPhieuNhapModel>(entity =>
        {
            entity.ToTable("ChiTietPhieuNhap");
            entity.HasKey(x => new { x.MaPhieuNhap, x.MaDienThoai }).HasName("PK_ChiTietPhieuNhap");
            entity.Property(x => x.MaPhieuNhap).HasColumnType("varchar(20)");
            entity.Property(x => x.MaDienThoai).HasColumnType("varchar(20)");
            entity.Property(x => x.DonGia).HasPrecision(18, 2);
            entity.HasOne(x => x.PhieuNhap).WithMany(x => x.ChiTietPhieuNhaps).HasForeignKey(x => x.MaPhieuNhap)
                .HasConstraintName("FK_ChiTietPhieuNhap_PhieuNhap").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.DienThoai).WithMany(x => x.ChiTietPhieuNhaps).HasForeignKey(x => x.MaDienThoai)
                .HasConstraintName("FK_ChiTietPhieuNhap_DienThoai").OnDelete(DeleteBehavior.NoAction);
        });
    }
}
