using System;
using System.Collections.Generic;

namespace Student_management.Models;

public partial class GiaoVien
{
    public int MaGv { get; set; }

    public int MaNguoiDung { get; set; }

    public string? TrinhDoChuyenMon { get; set; }

    public virtual ICollection<Lop> Lops { get; set; } = new List<Lop>();

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();
}
