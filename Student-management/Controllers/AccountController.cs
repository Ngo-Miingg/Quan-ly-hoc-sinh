using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_Management.Models;
using System;
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

        // === ĐĂNG NHẬP, ĐĂNG XUẤT, TRUY CẬP ===

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity is { IsAuthenticated: true })
            {
                return RedirectToRole(User.FindFirstValue(ClaimTypes.Role));
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = await _context.TaiKhoans.FirstOrDefaultAsync(t => t.TenDangNhap == model.TenDangNhap);
                if (taiKhoan != null && BCrypt.Net.BCrypt.Verify(model.MatKhau, taiKhoan.MatKhau))
                {
                    var userRole = taiKhoan.VaiTro ?? string.Empty;
                    var claims = new List<Claim> {
                        new(ClaimTypes.Name, taiKhoan.TenDangNhap),
                        new(ClaimTypes.NameIdentifier, taiKhoan.MaTaiKhoan.ToString()),
                        new(ClaimTypes.Role, userRole)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties { IsPersistent = true });
                    return RedirectToRole(userRole);
                }
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();


        // === CHỨC NĂNG QUẢN TRỊ (CHỈ DÀNH CHO ADMIN) ===

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ListUser()
        {
            var users = await _context.TaiKhoans.Include(t => t.HocSinh).Include(t => t.GiaoVien).AsNoTracking().ToListAsync();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            await PopulateLopHocDropDownList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.VaiTro == "HocSinh" && !model.MaLopHoc.HasValue)
                    ModelState.AddModelError("MaLopHoc", "Vui lòng chọn lớp học cho học sinh.");

                if (await _context.TaiKhoans.AnyAsync(t => t.TenDangNhap == model.TenDangNhap))
                    ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại.");

                if (await _context.HocSinhs.AnyAsync(hs => hs.Email == model.Email) || await _context.GiaoViens.AnyAsync(gv => gv.Email == model.Email))
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");

                if (ModelState.IsValid)
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        if (model.VaiTro == "HocSinh")
                        {
                            var newHocSinh = new HocSinh { HoTen = model.HoTen, NgaySinh = model.NgaySinh, GioiTinh = model.GioiTinh, DiaChi = model.DiaChi, SoDienThoai = model.SoDienThoai, Email = model.Email, MaLopHoc = model.MaLopHoc.Value };
                            _context.HocSinhs.Add(newHocSinh);
                            await _context.SaveChangesAsync();
                            var newAccount = new TaiKhoan { TenDangNhap = model.TenDangNhap, MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhau), VaiTro = "HocSinh", MaHocSinh = newHocSinh.MaHocSinh };
                            _context.TaiKhoans.Add(newAccount);
                        }
                        else if (model.VaiTro == "GiaoVien")
                        {
                            var newGiaoVien = new GiaoVien { HoTen = model.HoTen, NgaySinh = model.NgaySinh, GioiTinh = model.GioiTinh, DiaChi = model.DiaChi, SoDienThoai = model.SoDienThoai, Email = model.Email };
                            _context.GiaoViens.Add(newGiaoVien);
                            await _context.SaveChangesAsync();
                            var newAccount = new TaiKhoan { TenDangNhap = model.TenDangNhap, MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhau), VaiTro = "GiaoVien", MaGiaoVien = newGiaoVien.MaGiaoVien };
                            _context.TaiKhoans.Add(newAccount);
                        }
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        TempData["SuccessMessage"] = "Tạo người dùng mới thành công!";
                        return RedirectToAction(nameof(ListUser));
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi lưu vào database: " + ex.Message);
                    }
                }
            }
            await PopulateLopHocDropDownList(model.MaLopHoc);
            return View(model);
        }

        // ... Các action Sửa, Xóa sẽ được thêm vào đây sau khi chức năng này ổn định ...
        // Dán 2 Action này vào trong file AccountController.cs
        // Dán 2 Action này vào trong file AccountController.cs

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null) return NotFound();
            var taiKhoan = await _context.TaiKhoans
                .Include(t => t.HocSinh)
                .Include(t => t.GiaoVien)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MaTaiKhoan == id);
            if (taiKhoan == null) return NotFound();
            return View(taiKhoan);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(int id)
        {
            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan != null)
            {
                // Quan trọng: Phải xóa người dùng (HocSinh/GiaoVien) liên quan trước
                // nếu database có ràng buộc khóa ngoại chặt chẽ.
                // Tuy nhiên, nếu chỉ xóa tài khoản thì chỉ cần xóa tài khoản.
                _context.TaiKhoans.Remove(taiKhoan);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa tài khoản thành công!";
            }
            return RedirectToAction(nameof(ListUser));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null) return NotFound();

            var taiKhoan = await _context.TaiKhoans
                .Include(t => t.HocSinh)
                .Include(t => t.GiaoVien)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.MaTaiKhoan == id);

            if (taiKhoan == null) return NotFound();

            // Map dữ liệu từ database sang ViewModel để hiển thị lên form
            var model = new EditUserViewModel
            {
                MaTaiKhoan = taiKhoan.MaTaiKhoan,
                TenDangNhap = taiKhoan.TenDangNhap,
                VaiTro = taiKhoan.VaiTro
            };

            if (taiKhoan.HocSinh != null)
            {
                var hs = taiKhoan.HocSinh;
                model.HoTen = hs.HoTen;
                model.NgaySinh = hs.NgaySinh.GetValueOrDefault();
                model.GioiTinh = hs.GioiTinh;
                model.Email = hs.Email;
                model.SoDienThoai = hs.SoDienThoai;
                model.DiaChi = hs.DiaChi;
                model.MaLopHoc = hs.MaLopHoc;
            }
            else if (taiKhoan.GiaoVien != null)
            {
                var gv = taiKhoan.GiaoVien;
                model.HoTen = gv.HoTen;
                model.NgaySinh = gv.NgaySinh.GetValueOrDefault();
                model.GioiTinh = gv.GioiTinh;
                model.Email = gv.Email;
                model.SoDienThoai = gv.SoDienThoai;
                model.DiaChi = gv.DiaChi;
            }

            await PopulateLopHocDropDownList(model.MaLopHoc);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, EditUserViewModel model)
        {
            if (id != model.MaTaiKhoan) return NotFound();

            if (ModelState.IsValid)
            {
                var taiKhoanToUpdate = await _context.TaiKhoans
                    .Include(t => t.HocSinh)
                    .Include(t => t.GiaoVien)
                    .FirstOrDefaultAsync(t => t.MaTaiKhoan == id);

                if (taiKhoanToUpdate == null) return NotFound();

                // Cập nhật thông tin tài khoản
                taiKhoanToUpdate.TenDangNhap = model.TenDangNhap;
                if (!string.IsNullOrEmpty(model.MatKhauMoi))
                {
                    taiKhoanToUpdate.MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhauMoi);
                }

                // Cập nhật thông tin cá nhân
                if (taiKhoanToUpdate.HocSinh != null)
                {
                    taiKhoanToUpdate.HocSinh.HoTen = model.HoTen;
                    // ... cập nhật các trường còn lại cho học sinh
                }
                else if (taiKhoanToUpdate.GiaoVien != null)
                {
                    taiKhoanToUpdate.GiaoVien.HoTen = model.HoTen;
                    // ... cập nhật các trường còn lại cho giáo viên
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                return RedirectToAction(nameof(ListUser));
            }

            await PopulateLopHocDropDownList(model.MaLopHoc);
            return View(model);
        }
        // HÀM TIỆN ÍCH
        private async Task PopulateLopHocDropDownList(object? selectedLop = null)
        {
            ViewBag.LopHocList = new SelectList(await _context.LopHocs.AsNoTracking().ToListAsync(), "MaLopHoc", "TenLopHoc", selectedLop);
        }

        private IActionResult RedirectToRole(string? role) => RedirectToAction("Index", role switch { "Admin" => "Admin", _ => "Home" });
    }
}