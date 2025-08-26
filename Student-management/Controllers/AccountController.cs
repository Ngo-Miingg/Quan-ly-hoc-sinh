using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_Management.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Student_Management.Controllers
{
    public class AccountController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public AccountController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // --- Các chức năng xác thực chung ---
        [HttpGet]
        public IActionResult Login() { /* ... Logic đăng nhập ... */ }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model) { /* ... Logic xử lý đăng nhập ... */ }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() { /* ... Logic đăng xuất ... */ }

        // --- Các chức năng quản trị tài khoản (chỉ Admin) ---

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ListUser() { /* ... Logic lấy danh sách tài khoản ... */ }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> CreateUser() { /* ... Logic hiển thị form tạo user ... */ }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model) { /* ... Logic xử lý tạo user ... */ }

        // ... Các action Sửa, Xóa tài khoản khác ...

        // --- Hàm tiện ích ---
        private async Task PopulateLopHocDropDownList(object? selectedLop = null)
        {
            ViewBag.LopHocList = new SelectList(await _context.LopHocs.AsNoTracking().ToListAsync(), "MaLopHoc", "TenLopHoc", selectedLop);
        }

        private IActionResult RedirectToRole(string? role) => RedirectToAction("Index", role switch { "Admin" => "Admin", _ => "Home" });
    }
}