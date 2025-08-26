using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_Management.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Student_Management.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // Sử dụng field private readonly để đảm bảo DbContext luôn sẵn có.
        private readonly QuanLyHocSinhContext _context;

        // Sử dụng constructor truyền thống để gán DbContext.
        public AdminController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // === DASHBOARD ===
        public async Task<IActionResult> Index()
        {
            ViewBag.TongHocSinh = await _context.HocSinhs.CountAsync();
            ViewBag.TongGiaoVien = await _context.GiaoViens.CountAsync();
            ViewBag.TongLop = await _context.LopHocs.CountAsync();
            ViewBag.TongTaiKhoan = await _context.TaiKhoans.CountAsync();
            return View();
        }

        // === QUẢN LÝ TÀI KHOẢN ===

        // GET: Hiển thị danh sách tài khoản
        public async Task<IActionResult> TaiKhoan()
        {
            var danhSachTaiKhoan = await _context.TaiKhoans
                .Include(t => t.HocSinh)
                .Include(t => t.GiaoVien)
                .ToListAsync();
            return View(danhSachTaiKhoan);
        }

        // GET: Admin/CreateUser - Form tạo người dùng hợp nhất
        public async Task<IActionResult> CreateUser()
        {
            ViewBag.LopHocList = new SelectList(await _context.LopHocs.ToListAsync(), "MaLopHoc", "TenLopHoc");
            return View();
        }

        // POST: Admin/CreateUser - Xử lý tạo người dùng hợp nhất
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra Tên đăng nhập và Email đã tồn tại chưa
                if (await _context.TaiKhoans.AnyAsync(t => t.TenDangNhap == model.TenDangNhap))
                {
                    ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại.");
                }
                bool emailExists = await _context.HocSinhs.AnyAsync(hs => hs.Email == model.Email) ||
                                   await _context.GiaoViens.AnyAsync(gv => gv.Email == model.Email);
                if (emailExists)
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.LopHocList = new SelectList(await _context.LopHocs.ToListAsync(), "MaLopHoc", "TenLopHoc", model.MaLopHoc);
                    return View(model);
                }

                if (model.VaiTro == "HocSinh")
                {
                    var newHocSinh = new HocSinh
                    {
                        HoTen = model.HoTen,
                        NgaySinh = model.NgaySinh,
                        GioiTinh = model.GioiTinh,
                        DiaChi = model.DiaChi,
                        SoDienThoai = model.SoDienThoai,
                        Email = model.Email,
                        MaLopHoc = model.MaLopHoc.Value
                    };
                    _context.HocSinhs.Add(newHocSinh);
                    await _context.SaveChangesAsync();

                    var newAccount = new TaiKhoan
                    {
                        TenDangNhap = model.TenDangNhap,
                        MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhau),
                        VaiTro = "HocSinh",
                        MaHocSinh = newHocSinh.MaHocSinh
                    };
                    _context.TaiKhoans.Add(newAccount);
                }
                else if (model.VaiTro == "GiaoVien")
                {
                    var newGiaoVien = new GiaoVien
                    {
                        HoTen = model.HoTen,
                        NgaySinh = model.NgaySinh,
                        GioiTinh = model.GioiTinh,
                        DiaChi = model.DiaChi,
                        SoDienThoai = model.SoDienThoai,
                        Email = model.Email
                    };
                    _context.GiaoViens.Add(newGiaoVien);
                    await _context.SaveChangesAsync();

                    var newAccount = new TaiKhoan
                    {
                        TenDangNhap = model.TenDangNhap,
                        MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhau),
                        VaiTro = "GiaoVien",
                        MaGiaoVien = newGiaoVien.MaGiaoVien
                    };
                    _context.TaiKhoans.Add(newAccount);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TaiKhoan));
            }

            ViewBag.LopHocList = new SelectList(await _context.LopHocs.ToListAsync(), "MaLopHoc", "TenLopHoc", model.MaLopHoc);
            return View(model);
        }

        // GET: Xóa tài khoản
        public async Task<IActionResult> XoaTaiKhoan(int? id)
        {
            if (id == null) return NotFound();
            var tk = await _context.TaiKhoans
                .Include(t => t.HocSinh)
                .Include(t => t.GiaoVien)
                .FirstOrDefaultAsync(m => m.MaTaiKhoan == id);
            if (tk == null) return NotFound();
            return View(tk);
        }

        // POST: Xác nhận xóa tài khoản
        [HttpPost, ActionName("XoaTaiKhoan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XacNhanXoaTaiKhoan(int id)
        {
            var tk = await _context.TaiKhoans.FindAsync(id);
            if (tk != null)
            {
                _context.TaiKhoans.Remove(tk);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(TaiKhoan));
        }

        // === QUẢN LÝ NHÂN SỰ ===

        // GET: Danh sách giáo viên
        public async Task<IActionResult> DanhSachGiaoVien()
        {
            var gv = await _context.GiaoViens.Include(g => g.MonHoc).ToListAsync();
            return View(gv);
        }

        // GET: Danh sách học sinh
        public async Task<IActionResult> DanhSachHocSinh()
        {
            var hs = await _context.HocSinhs.Include(h => h.LopHoc).ToListAsync();
            return View(hs);
        }

        // === QUẢN LÝ LỚP HỌC (phát triển sau) ===

        public async Task<IActionResult> DanhSachLop()
        {
            var lop = await _context.LopHocs
                .Include(l => l.GiaoVienChuNhiem)
                .Include(l => l.NamHoc)
                .ToListAsync();
            return View(lop);
        }
    }
}