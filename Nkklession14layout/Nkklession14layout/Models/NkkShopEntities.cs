using System.ComponentModel.DataAnnotations;

namespace Nkklession14layout.Models;

public class NkkTaiKhoanModel
{
    [Key, StringLength(20)]
    public string MaTaiKhoan { get; set; } = "";

    [Required, StringLength(50)]
    public string TenDangNhap { get; set; } = "";

    [Required, StringLength(255)]
    public string MatKhau { get; set; } = "";

    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = "";

    public DateTime NgayTao { get; set; }

    [Required, StringLength(50)]
    public string PhanQuyen { get; set; } = "NhanVien";
}

public class NkkDienThoaiModel
{
    [Key, StringLength(20)]
    public string MaDienThoai { get; set; } = "";

    [Required, StringLength(100)]
    public string TenDienThoai { get; set; } = "";

    [StringLength(50)]
    public string? HangSanXuat { get; set; }

    [StringLength(50)]
    public string? MauSac { get; set; }

    [StringLength(20)]
    public string? Ram { get; set; }

    [StringLength(20)]
    public string? BoNho { get; set; }

    public decimal GiaNhap { get; set; }
    public decimal GiaBan { get; set; }
    public int SoLuong { get; set; }

    [StringLength(500)]
    public string? MoTa { get; set; }

    [StringLength(255)]
    public string? Anh { get; set; }

    public ICollection<NkkChiTietHoaDonModel> ChiTietHoaDons { get; set; } = new List<NkkChiTietHoaDonModel>();
    public ICollection<NkkChiTietPhieuNhapModel> ChiTietPhieuNhaps { get; set; } = new List<NkkChiTietPhieuNhapModel>();
}

public class NkkNhaCungCapModel
{
    [Key, StringLength(20)]
    public string MaNhaCungCap { get; set; } = "";

    [Required, StringLength(100)]
    public string TenNhaCungCap { get; set; } = "";

    [StringLength(15)]
    public string? SoDienThoai { get; set; }

    [EmailAddress, StringLength(100)]
    public string? Email { get; set; }

    [StringLength(255)]
    public string? DiaChi { get; set; }

    public ICollection<NkkPhieuNhapModel> PhieuNhaps { get; set; } = new List<NkkPhieuNhapModel>();
}

public class NkkKhachHangModel
{
    [Key, StringLength(20)]
    public string MaKhachHang { get; set; } = "";

    [Required, StringLength(100)]
    public string HoTen { get; set; } = "";

    [StringLength(10)]
    public string? GioiTinh { get; set; }

    [StringLength(15)]
    public string? SoDienThoai { get; set; }

    [EmailAddress, StringLength(100)]
    public string? Email { get; set; }

    [StringLength(255)]
    public string? DiaChi { get; set; }

    public ICollection<NkkHoaDonModel> HoaDons { get; set; } = new List<NkkHoaDonModel>();
}

public class NkkNhanVienModel
{
    [Key, StringLength(20)]
    public string MaNhanVien { get; set; } = "";

    [Required, StringLength(100)]
    public string HoTen { get; set; } = "";

    [StringLength(10)]
    public string? GioiTinh { get; set; }

    [StringLength(15)]
    public string? SoDienThoai { get; set; }

    [EmailAddress, StringLength(100)]
    public string? Email { get; set; }

    [StringLength(255)]
    public string? DiaChi { get; set; }

    [StringLength(50)]
    public string? ChucVu { get; set; }

    public DateTime? NgayVaoLam { get; set; }

    public ICollection<NkkHoaDonModel> HoaDons { get; set; } = new List<NkkHoaDonModel>();
    public ICollection<NkkPhieuNhapModel> PhieuNhaps { get; set; } = new List<NkkPhieuNhapModel>();
}

public class NkkHoaDonModel
{
    [Key, StringLength(20)]
    public string MaHoaDon { get; set; } = "";

    [Required, StringLength(20)]
    public string MaKhachHang { get; set; } = "";

    [Required, StringLength(20)]
    public string MaNhanVien { get; set; } = "";

    public DateTime NgayLap { get; set; }
    public decimal TongTien { get; set; }

    [StringLength(500)]
    public string? GhiChu { get; set; }

    public NkkKhachHangModel KhachHang { get; set; } = null!;
    public NkkNhanVienModel NhanVien { get; set; } = null!;
    public ICollection<NkkChiTietHoaDonModel> ChiTietHoaDons { get; set; } = new List<NkkChiTietHoaDonModel>();
}

public class NkkChiTietHoaDonModel
{
    [Required, StringLength(20)]
    public string MaHoaDon { get; set; } = "";

    [Required, StringLength(20)]
    public string MaDienThoai { get; set; } = "";

    public int SoLuong { get; set; }
    public decimal DonGia { get; set; }

    public NkkHoaDonModel HoaDon { get; set; } = null!;
    public NkkDienThoaiModel DienThoai { get; set; } = null!;
}

public class NkkPhieuNhapModel
{
    [Key, StringLength(20)]
    public string MaPhieuNhap { get; set; } = "";

    [Required, StringLength(20)]
    public string MaNhaCungCap { get; set; } = "";

    [Required, StringLength(20)]
    public string MaNhanVien { get; set; } = "";

    public DateTime NgayNhap { get; set; }
    public decimal TongTien { get; set; }

    public NkkNhaCungCapModel NhaCungCap { get; set; } = null!;
    public NkkNhanVienModel NhanVien { get; set; } = null!;
    public ICollection<NkkChiTietPhieuNhapModel> ChiTietPhieuNhaps { get; set; } = new List<NkkChiTietPhieuNhapModel>();
}

public class NkkChiTietPhieuNhapModel
{
    [Required, StringLength(20)]
    public string MaPhieuNhap { get; set; } = "";

    [Required, StringLength(20)]
    public string MaDienThoai { get; set; } = "";

    public int SoLuong { get; set; }
    public decimal DonGia { get; set; }

    public NkkPhieuNhapModel PhieuNhap { get; set; } = null!;
    public NkkDienThoaiModel DienThoai { get; set; } = null!;
}
