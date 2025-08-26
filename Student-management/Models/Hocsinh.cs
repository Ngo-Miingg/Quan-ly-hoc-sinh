// File: HocSinh.cs
// Tên class và các thuộc tính đã được chuẩn hóa PascalCase
namespace Student_Management.Models;

public partial class HocSinh
{
    public int MaHocSinh { get; set; }
    public string HoTen { get; set; } = null!;
    public DateTime? NgaySinh { get; set; }
    public string? GioiTinh { get; set; }
    public string? Email { get; set; }
    public string? SoDienThoai { get; set; }
    public string? TrangThai { get; set; }
    public string? DiaChi { get; set; }
    public int MaLopHoc { get; set; }

    // Navigation Properties
    public virtual Lop LopHoc { get; set; } = null!;
    public virtual ICollection<Diem> DiemSos { get; set; } = new List<Diem>();
    public virtual ICollection<HocPhi> HocPhis { get; set; } = new List<HocPhi>();
    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}