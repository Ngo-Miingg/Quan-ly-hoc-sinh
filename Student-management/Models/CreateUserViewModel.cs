using System.ComponentModel.DataAnnotations;

namespace Student_Management.Models;

public class CreateUserViewModel
{
    // Khởi tạo giá trị mặc định để loại bỏ cảnh báo null
    [Required(ErrorMessage = "Họ tên là bắt buộc.")]
    [Display(Name = "Họ và tên")]
    public string HoTen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày sinh")]
    public DateTime NgaySinh { get; set; }

    [Required(ErrorMessage = "Giới tính là bắt buộc.")]
    [Display(Name = "Giới tính")]
    public string GioiTinh { get; set; } = string.Empty;

    // ... làm tương tự cho các thuộc tính string khác ...
    [Required(ErrorMessage = "Email là bắt buộc.")]
    public string Email { get; set; } = string.Empty;
    public string? SoDienThoai { get; set; }
    public string? DiaChi { get; set; }
    public string TenDangNhap { get; set; } = string.Empty;
    public string MatKhau { get; set; } = string.Empty;
    public string VaiTro { get; set; } = string.Empty;

    [Display(Name = "Lớp học")]
    public int? MaLopHoc { get; set; }
}