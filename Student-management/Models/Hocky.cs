// File: HocKy.cs
namespace Student_Management.Models;

public partial class HocKy
{
    public int MaHocKy { get; set; }
    public string TenHocKy { get; set; } = null!;
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    public int MaNamHoc { get; set; }

    // Navigation Properties
    public virtual NamHoc NamHoc { get; set; } = null!;
    public virtual ICollection<Diem> DiemSos { get; set; } = new List<Diem>();
    public virtual ICollection<HocPhi> HocPhis { get; set; } = new List<HocPhi>();
    public virtual ICollection<LichHoc> LichHocs { get; set; } = new List<LichHoc>();
    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();
}