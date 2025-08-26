using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Management.Models;

namespace Student_Management.Controllers
{
    public class AuthController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public AuthController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var account = await _context.TaiKhoans
                .FirstOrDefaultAsync(t => t.TenDangNhap == model.TenDangNhap);

            // Kiểm tra tài khoản và xác thực mật khẩu bằng BCrypt
            if (account == null || !BCrypt.Net.BCrypt.Verify(model.MatKhau, account.MatKhau))
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return View(model);
            }

            // Lưu thông tin vào Session
            HttpContext.Session.SetString("Username", account.TenDangNhap);
            HttpContext.Session.SetString("Role", account.VaiTro ?? "Default");
            HttpContext.Session.SetInt32("UserId", account.MaTaiKhoan);

            // Điều hướng dựa trên vai trò
            return RedirectToRole(account.VaiTro);
        }

        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string username, string password, string confirmPassword, string role = "HocSinh")
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Tên đăng nhập và mật khẩu không được để trống.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp.";
                return View();
            }

            bool exists = await _context.TaiKhoans.AnyAsync(t => t.TenDangNhap == username);
            if (exists)
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại.";
                return View();
            }

            var newAccount = new TaiKhoan
            {
                TenDangNhap = username,
                // Băm mật khẩu bằng BCrypt trước khi lưu
                MatKhau = BCrypt.Net.BCrypt.HashPassword(password),
                VaiTro = role
            };

            _context.Add(newAccount);
            await _context.SaveChangesAsync();

            ViewBag.Success = "Đăng ký thành công. Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        // GET: /Auth/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // Hàm điều hướng tiện ích
        private IActionResult RedirectToRole(string? role)
        {
            return RedirectToAction("Index", role switch
            {
                "Admin" => "Admin",
                "GiaoVien" => "GiaoVien",
                "HocSinh" => "HocSinh",
                _ => "Home"
            });
        }
    }
}