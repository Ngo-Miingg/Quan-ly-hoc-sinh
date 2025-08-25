using System;
using System.Collections.Generic;

namespace Student_management.Models;

public partial class NguoiDung
{
    public int MaNguoiDung { get; set; }

    public string HoTen { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    public string? DiaChi { get; set; }

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public string? AnhDaiDien { get; set; }

    public virtual GiaoVien? GiaoVien { get; set; }

    public virtual HocSinh? HocSinh { get; set; }

    public virtual TaiKhoan? TaiKhoan { get; set; }
}
