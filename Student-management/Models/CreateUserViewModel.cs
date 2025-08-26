using System.ComponentModel.DataAnnotations;
namespace Student_Management.Models;

public class CreateUserViewModel
{
    [Required(ErrorMessage = "Họ tên là bắt buộc.")]
    [Display(Name = "Họ và tên")]
    public string HoTen { get; set; }

    [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày sinh")]
    public DateTime NgaySinh { get; set; }

    [Required(ErrorMessage = "Giới tính là bắt buộc.")]
    [Display(Name = "Giới tính")]
    public string GioiTinh { get; set; }

    [Required(ErrorMessage = "Email là bắt buộc.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public string Email { get; set; }

    [Display(Name = "Số điện thoại")]
    public string? SoDienThoai { get; set; }

    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }

    [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; }

    [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string MatKhau { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
    [Display(Name = "Vai trò")]
    public string VaiTro { get; set; }

    [Display(Name = "Lớp học")]
    public int? MaLopHoc { get; set; }
}