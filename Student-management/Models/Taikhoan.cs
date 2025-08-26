// File: TaiKhoan.cs
namespace Student_Management.Models;

public partial class TaiKhoan
{
    public int MaTaiKhoan { get; set; }
    public string TenDangNhap { get; set; } = null!;
    public string MatKhau { get; set; } = null!;
    public string? VaiTro { get; set; }
    public int? MaGiaoVien { get; set; }
    public int? MaHocSinh { get; set; }

    // Navigation Properties
    public virtual GiaoVien? GiaoVien { get; set; }
    public virtual HocSinh? HocSinh { get; set; }
}