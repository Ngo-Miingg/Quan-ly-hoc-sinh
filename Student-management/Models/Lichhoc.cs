// File: LichHoc.cs
namespace Student_Management.Models;

public partial class LichHoc
{
    public int MaLichHoc { get; set; }
    public int MaLopHoc { get; set; }
    public int MaMonHoc { get; set; }
    public int MaGiaoVien { get; set; }
    public int MaPhongHoc { get; set; }
    public int MaHocKy { get; set; }
    public string? ThuTrongTuan { get; set; }
    public int? TietHoc { get; set; }

    // Navigation Properties
    public virtual GiaoVien GiaoVien { get; set; } = null!;
    public virtual HocKy HocKy { get; set; } = null!;
    public virtual Lop LopHoc { get; set; } = null!;
    public virtual MonHoc MonHoc { get; set; } = null!;
    public virtual PhongHoc PhongHoc { get; set; } = null!;
}