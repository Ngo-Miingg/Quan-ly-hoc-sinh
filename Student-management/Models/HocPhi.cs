using System;
using System.Collections.Generic;

namespace Student_management.Models;

public partial class HocPhi
{
    public int MaHocPhi { get; set; }

    public int MaHs { get; set; }

    public decimal SoTien { get; set; }

    public DateOnly? NgayDong { get; set; }

    public string? TrangThai { get; set; }

    public int MaHocKy { get; set; }

    public string? GhiChu { get; set; }

    public virtual HocKy MaHocKyNavigation { get; set; } = null!;

    public virtual HocSinh MaHsNavigation { get; set; } = null!;
}
