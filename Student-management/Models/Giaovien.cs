// File: GiaoVien.cs
namespace Student_Management.Models;

public partial class GiaoVien
{
    public int MaGiaoVien { get; set; }
    public string HoTen { get; set; } = null!;
    public DateTime? NgaySinh { get; set; }
    public string? GioiTinh { get; set; }
    public string? DiaChi { get; set; }
    public string? SoDienThoai { get; set; }
    public string? Email { get; set; }
    public int? MaMonHoc { get; set; }

    // Navigation Properties
    public virtual MonHoc? MonHoc { get; set; }
    public virtual ICollection<LichHoc> LichHocs { get; set; } = new List<LichHoc>();
    public virtual ICollection<Lop> LopHocs { get; set; } = new List<Lop>();
    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();
    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}