// File: NamHoc.cs
namespace Student_Management.Models;

public partial class NamHoc
{
    public int MaNamHoc { get; set; }
    public string TenNamHoc { get; set; } = null!;
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }

    // Navigation Properties
    public virtual ICollection<HocKy> HocKys { get; set; } = new List<HocKy>();
    public virtual ICollection<Lop> LopHocs { get; set; } = new List<Lop>();
}