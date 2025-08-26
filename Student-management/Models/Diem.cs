// File: DiemSo.cs
namespace Student_Management.Models;

public partial class Diem
{
    public int MaDiemSo { get; set; }
    public int MaHocSinh { get; set; }
    public int MaMonHoc { get; set; }
    public int MaHocKy { get; set; }
    public double? DiemMieng { get; set; }
    public double? Diem15Phut { get; set; }
    public double? Diem1Tiet { get; set; }
    public double? DiemThi { get; set; }
    public double? DiemTrungBinh { get; set; }

    // Navigation Properties
    public virtual HocKy HocKy { get; set; } = null!;
    public virtual HocSinh HocSinh { get; set; } = null!;
    public virtual MonHoc MonHoc { get; set; } = null!;
}