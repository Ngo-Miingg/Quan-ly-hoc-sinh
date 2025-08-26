// File: MonHoc.cs
namespace Student_Management.Models;

public partial class MonHoc
{
    public int MaMonHoc { get; set; }
    public string TenMonHoc { get; set; } = null!;
    public int? SoTiet { get; set; }
    public double? HeSo { get; set; }

    // Navigation Properties
    public virtual ICollection<Diem> DiemSos { get; set; } = new List<Diem>();
    public virtual ICollection<GiaoVien> GiaoViens { get; set; } = new List<GiaoVien>();
    public virtual ICollection<LichHoc> LichHocs { get; set; } = new List<LichHoc>();
    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();
}