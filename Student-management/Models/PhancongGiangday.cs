// File: PhanCongGiangDay.cs
namespace Student_Management.Models;

public partial class PhanCongGiangDay
{
    public int MaPhanCong { get; set; }
    public int MaGiaoVien { get; set; }
    public int MaMonHoc { get; set; }
    public int MaLopHoc { get; set; }
    public int MaHocKy { get; set; }

    // Navigation Properties
    public virtual GiaoVien GiaoVien { get; set; } = null!;
    public virtual HocKy HocKy { get; set; } = null!;
    public virtual Lop LopHoc { get; set; } = null!;
    public virtual MonHoc MonHoc { get; set; } = null!;
}