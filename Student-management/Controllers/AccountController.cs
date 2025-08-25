// Controllers/AccountController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers
{
    public class AccountController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public AccountController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // Action hiển thị form đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Action xử lý đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = await _context.TaiKhoans
                    .Include(t => t.MaVaiTroNavigation)
                    .Include(t => t.MaNguoiDungNavigation)
                    .FirstOrDefaultAsync(t => t.TenDangNhap == model.TenDangNhap);

                if (taiKhoan != null)
                {
                    // So sánh mật khẩu (sử dụng BCrypt hoặc một thư viện hashing khác)
                    // Đối với ví dụ này, chúng ta sẽ so sánh mật khẩu plain-text đơn giản.
                    // Trong thực tế, bạn PHẢI sử dụng hashing!
                    if (taiKhoan.MatKhauHash == model.MatKhau)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, taiKhoan.TenDangNhap),
                            new Claim(ClaimTypes.Role, taiKhoan.MaVaiTroNavigation.TenVaiTro)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties();

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        // Chuyển hướng người dùng đến trang phù hợp với vai trò
                        if (taiKhoan.MaVaiTroNavigation.TenVaiTro == "Admin")
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
            }
            return View(model);
        }

        // Action xử lý đăng xuất
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}