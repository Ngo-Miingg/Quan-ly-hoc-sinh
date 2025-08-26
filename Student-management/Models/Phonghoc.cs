// File: PhongHoc.cs
namespace Student_Management.Models;

public partial class PhongHoc
{
    public int MaPhongHoc { get; set; }
    public string TenPhongHoc { get; set; } = null!;
    public int? SucChua { get; set; }
    public string? ViTri { get; set; }

    // Navigation Properties
    public virtual ICollection<LichHoc> LichHocs { get; set; } = new List<LichHoc>();
}