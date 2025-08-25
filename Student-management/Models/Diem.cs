using System;
using System.Collections.Generic;

namespace Student_management.Models;

public partial class Diem
{
    public int MaDiem { get; set; }

    public int MaHs { get; set; }

    public int MaMonHoc { get; set; }

    public int MaHocKy { get; set; }

    public int MaLoaiDiem { get; set; }

    public decimal DiemSo { get; set; }

    public int? LanThi { get; set; }

    public DateOnly? NgayKiemTra { get; set; }

    public int? NguoiNhap { get; set; }

    public virtual HocKy MaHocKyNavigation { get; set; } = null!;

    public virtual HocSinh MaHsNavigation { get; set; } = null!;

    public virtual LoaiDiem MaLoaiDiemNavigation { get; set; } = null!;

    public virtual MonHoc MaMonHocNavigation { get; set; } = null!;
}
