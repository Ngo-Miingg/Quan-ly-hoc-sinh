// File: HocSinhViewModel.cs
namespace Student_Management.Models.ViewModels
{
    public class HocSinhViewModel
    {
        public int MaHocSinh { get; set; }
        public string HoTen { get; set; } = null!;
        public DateTime? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? Email { get; set; }
        public string? SoDienThoai { get; set; }
        public string? TrangThai { get; set; }
        public string? DiaChi { get; set; }
        public int MaLopHoc { get; set; }
        public string? TenLopHoc { get; set; }

        // Danh sách lớp để hiển thị trong dropdown, cần dùng LopHoc
        public List<Lop> LopHocList { get; set; } = new List<Lop>();
    }
}