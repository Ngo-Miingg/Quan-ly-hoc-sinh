using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Student_management.Models;

public class CreateUserViewModel
{
    [Required]
    [Display(Name = "Họ và tên")]
    public string HoTen { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Ngày sinh")]
    [DataType(DataType.Date)]
    public DateTime NgaySinh { get; set; }

    [Required]
    [Display(Name = "Giới tính")]
    public string GioiTinh { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Địa chỉ")]
    public string DiaChi { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Số điện thoại")]
    [RegularExpression(@"^(\d{10})$", ErrorMessage = "Số điện thoại không hợp lệ.")]
    public string Sdt { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string MatKhau { get; set; } = string.Empty;

    [Required]
    [DisplayName("Vai trò")]
    public int MaVaiTro { get; set; }

    // Thông tin dành riêng cho Học sinh
    [DisplayName("Lớp học")]
    public int? MaLop { get; set; }
}