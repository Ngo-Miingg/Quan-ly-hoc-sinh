using System.ComponentModel.DataAnnotations;

namespace Student_Management.Models;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập email của bạn.")]
    [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
    [Display(Name = "Địa chỉ Email")]
    public string Email { get; set; }
}