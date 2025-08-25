using System;
using System.Collections.Generic;

namespace Student_management.Models;

public partial class MonHoc
{
    public int MaMonHoc { get; set; }

    public string TenMonHoc { get; set; } = null!;

    public int? SoTiet { get; set; }

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();
}
