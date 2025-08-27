// File: LopHoc.cs
namespace Student_Management.Models;

public partial class Lop
{
    public int MaLopHoc { get; set; }
    public string TenLopHoc { get; set; } = null!; // Bỏ từ khóa 'required'
    public int? SiSo { get; set; }
    public int MaNamHoc { get; set; }
    public int? MaGiaoVienChuNhiem { get; set; } // Thay đổi từ 'int' sang 'int?'

    // Navigation Properties

    public virtual GiaoVien? GiaoVienChuNhiem { get; set; }
    public virtual NamHoc? NamHoc { get; set; } // Thay null! thành nullable
    public virtual ICollection<HocSinh> HocSinhs { get; set; } = new List<HocSinh>();
    public virtual ICollection<LichHoc> LichHocs { get; set; } = new List<LichHoc>();
    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();
}