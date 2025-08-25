using System;
using System.Collections.Generic;

namespace Student_management.Models;

public partial class PhanCongGiangDay
{
    public int MaPhanCong { get; set; }

    public int MaGv { get; set; }

    public int MaMonHoc { get; set; }

    public int MaLop { get; set; }

    public int MaHocKy { get; set; }

    public virtual GiaoVien MaGvNavigation { get; set; } = null!;

    public virtual HocKy MaHocKyNavigation { get; set; } = null!;

    public virtual Lop MaLopNavigation { get; set; } = null!;

    public virtual MonHoc MaMonHocNavigation { get; set; } = null!;
}
