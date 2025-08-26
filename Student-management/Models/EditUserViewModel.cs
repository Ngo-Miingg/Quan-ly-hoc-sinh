using System.ComponentModel.DataAnnotations;

namespace Student_Management.Models;

public class EditUserViewModel
{
    // ID để biết chúng ta đang sửa tài khoản nào
    public int MaTaiKhoan { get; set; }

    [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; } = string.Empty;

    [Display(Name = "Mật khẩu mới (Để trống nếu không đổi)")]
    [DataType(DataType.Password)]
    public string? MatKhauMoi { get; set; }

    // Thông tin cá nhân
    [Required(ErrorMessage = "Họ tên là bắt buộc.")]
    [Display(Name = "Họ và tên")]
    public string HoTen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày sinh")]
    public DateTime NgaySinh { get; set; }

    [Required(ErrorMessage = "Email là bắt buộc.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? SoDienThoai { get; set; }
    public string? DiaChi { get; set; }
    public string? GioiTinh { get; set; }

    // Dành riêng cho học sinh
    [Display(Name = "Lớp học")]
    public int? MaLopHoc { get; set; }

    // Giữ lại vai trò để biết đang sửa ai
    public string VaiTro { get; set; } = string.Empty;
}