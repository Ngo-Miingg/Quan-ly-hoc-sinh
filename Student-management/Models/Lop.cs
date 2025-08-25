using System;
using System.Collections.Generic;

namespace Student_management.Models;

public partial class Lop
{
    public int MaLop { get; set; }

    public string TenLop { get; set; } = null!;

    public int? SiSo { get; set; }

    public int MaNamHoc { get; set; }

    public int? MaGvcn { get; set; }

    public virtual ICollection<HocSinh> HocSinhs { get; set; } = new List<HocSinh>();

    public virtual GiaoVien? MaGvcnNavigation { get; set; }

    public virtual NamHoc MaNamHocNavigation { get; set; } = null!;

    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();
}
