using System;
using System.Collections.Generic;

namespace Student_management.Models;

public partial class HocKy
{
    public int MaHocKy { get; set; }

    public string TenHocKy { get; set; } = null!;

    public int HeSo { get; set; }

    public int MaNamHoc { get; set; }

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    public virtual ICollection<HocPhi> HocPhis { get; set; } = new List<HocPhi>();

    public virtual NamHoc MaNamHocNavigation { get; set; } = null!;

    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();
}
