using System.ComponentModel.DataAnnotations;

namespace Student_Management.Models;

public class ResetPasswordViewModel
{
    // Dùng để xác định người dùng, sẽ được truyền ẩn
    [Required]
    public string Email { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mã PIN.")]
    [Display(Name = "Mã PIN")]
    public string Pin { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu mới")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Xác nhận mật khẩu mới")]
    [Compare("NewPassword", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
    public string ConfirmPassword { get; set; }
}