using System.ComponentModel.DataAnnotations;

namespace Student_management.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [Display(Name = "Tên đăng nhập")]
        public required string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public required string MatKhau { get; set; }
    }
}