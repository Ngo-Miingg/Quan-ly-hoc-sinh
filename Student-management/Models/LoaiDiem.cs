using System;
using System.Collections.Generic;

namespace Student_Management.Models;
public partial class LoaiDiem
{
    public int MaLoaiDiem { get; set; }

    public string TenLoaiDiem { get; set; } = null!;

    public double HeSo { get; set; }

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();
}
