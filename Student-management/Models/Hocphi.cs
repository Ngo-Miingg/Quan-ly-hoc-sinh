// File: HocPhi.cs
namespace Student_Management.Models;

public partial class HocPhi
{
    public int MaHocPhi { get; set; }
    public int MaHocSinh { get; set; }
    public decimal? SoTien { get; set; }
    public DateTime? NgayDong { get; set; }
    public string? TrangThai { get; set; }
    public int MaHocKy { get; set; }

    // Navigation Properties
    public virtual HocKy HocKy { get; set; } = null!;
    public virtual HocSinh HocSinh { get; set; } = null!;
}