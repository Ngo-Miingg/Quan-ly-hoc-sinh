using System;
using System.Collections.Generic;

namespace Student_management.Models;

public partial class HocSinh
{
    public int MaHs { get; set; }

    public int MaNguoiDung { get; set; }

    public int MaLop { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    public virtual ICollection<HocPhi> HocPhis { get; set; } = new List<HocPhi>();

    public virtual Lop MaLopNavigation { get; set; } = null!;

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
}
