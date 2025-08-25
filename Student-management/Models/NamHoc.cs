using System;
using System.Collections.Generic;

namespace Student_management.Models;

public partial class NamHoc
{
    public int MaNamHoc { get; set; }

    public string TenNamHoc { get; set; } = null!;

    public DateOnly NgayBatDau { get; set; }

    public DateOnly NgayKetThuc { get; set; }

    public virtual ICollection<HocKy> HocKies { get; set; } = new List<HocKy>();

    public virtual ICollection<Lop> Lops { get; set; } = new List<Lop>();
}
