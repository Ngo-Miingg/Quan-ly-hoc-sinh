using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Student_Management.Models;

public class EditUserViewModel
{
    // ID để biết chúng ta đang sửa tài khoản nào
    public int MaTaiKhoan { get; set; }

    [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; }

    [Display(Name = "Mật khẩu mới (để trống nếu không đổi)")]
    [DataType(DataType.Password)]
    public string? MatKhauMoi { get; set; }

    [Required]
    [Display(Name = "Vai trò")]
    public string VaiTro { get; set; }

    // Thông tin cá nhân
    [Required]
    [Display(Name = "Họ và tên")]
    public string HoTen { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày sinh")]
    public DateTime NgaySinh { get; set; }

    [Required]
    [Display(Name = "Giới tính")]
    public string GioiTinh { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Display(Name = "Số điện thoại")]
    public string? SoDienThoai { get; set; }

    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }

    [Display(Name = "Lớp học")]
    public int? MaLopHoc { get; set; }
}