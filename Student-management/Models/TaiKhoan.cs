using System;
using System.Collections.Generic;

namespace Student_management.Models;

public partial class TaiKhoan
{
    public int MaTaiKhoan { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhauHash { get; set; } = null!;

    public int MaNguoiDung { get; set; }

    public int MaVaiTro { get; set; }

    public bool? TrangThai { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual VaiTro MaVaiTroNavigation { get; set; } = null!;
}
