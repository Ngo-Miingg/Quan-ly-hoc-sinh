using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_Management.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Student_Management.Controllers
{
    // [Authorize(Roles = "HocSinh")] <-- Phân quyền này không hợp lý
    // Thay đổi: Chỉ Admin hoặc Giáo viên mới có quyền quản lý danh sách học sinh.
    [Authorize(Roles = "Admin, GiaoVien")]
    public class HocSinhController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public HocSinhController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: HocSinh (Hiển thị danh sách các lớp học)
        public async Task<IActionResult> Index()
        {
            var dsLopHoc = await _context.LopHocs
                .Include(l => l.GiaoVienChuNhiem) // Sửa navigation property
                .Include(l => l.NamHoc)           // Sửa navigation property
                .ToListAsync();

            return View(dsLopHoc);
        }

        // GET: HocSinh/ByClass/5 (Hiển thị danh sách học sinh theo lớp)
        public async Task<IActionResult> ByClass(int? id)
        {
            if (id == null) return NotFound();

            var lopHoc = await _context.LopHocs
                .Include(l => l.HocSinhs)           // Sửa navigation property
                .Include(l => l.GiaoVienChuNhiem) // Sửa navigation property
                .FirstOrDefaultAsync(l => l.MaLopHoc == id); // Sửa tên PK

            if (lopHoc == null) return NotFound();

            return View(lopHoc);
        }

        // GET: HocSinh/Create
        public IActionResult Create(int? lopId)
        {
            // Sửa DbSet và các thuộc tính khóa
            ViewBag.LopHocList = new SelectList(_context.LopHocs, "MaLopHoc", "TenLopHoc", lopId);
            return View();
        }

        // POST: HocSinh/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HocSinh hocSinh) // Sửa tên Model
        {
            if (ModelState.IsValid)
            {
                _context.Add(hocSinh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ByClass), new { id = hocSinh.MaLopHoc });
            }

            ViewBag.LopHocList = new SelectList(_context.LopHocs, "MaLopHoc", "TenLopHoc", hocSinh.MaLopHoc);
            return View(hocSinh);
        }

        // GET: HocSinh/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var hocSinh = await _context.HocSinhs.FindAsync(id);
            if (hocSinh == null) return NotFound();

            ViewBag.LopHocList = new SelectList(_context.LopHocs, "MaLopHoc", "TenLopHoc", hocSinh.MaLopHoc);
            return View(hocSinh);
        }

        // POST: HocSinh/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HocSinh hocSinh)
        {
            if (id != hocSinh.MaHocSinh) return NotFound(); // Sửa tên PK

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hocSinh);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ByClass), new { id = hocSinh.MaLopHoc });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.HocSinhs.Any(e => e.MaHocSinh == hocSinh.MaHocSinh)) return NotFound();
                    else throw;
                }
            }
            ViewBag.LopHocList = new SelectList(_context.LopHocs, "MaLopHoc", "TenLopHoc", hocSinh.MaLopHoc);
            return View(hocSinh);
        }

        // GET: HocSinh/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var hocSinh = await _context.HocSinhs
                .Include(h => h.LopHoc) // Sửa navigation property
                .FirstOrDefaultAsync(m => m.MaHocSinh == id); // Sửa tên PK

            if (hocSinh == null) return NotFound();
            return View(hocSinh);
        }

        // GET: HocSinh/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var hocSinh = await _context.HocSinhs
                .Include(h => h.LopHoc) // Sửa navigation property
                .FirstOrDefaultAsync(m => m.MaHocSinh == id); // Sửa tên PK

            if (hocSinh == null) return NotFound();
            return View(hocSinh);
        }

        // POST: HocSinh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hocSinh = await _context.HocSinhs.FindAsync(id);
            if (hocSinh != null)
            {
                int maLopHoc = hocSinh.MaLopHoc;
                _context.HocSinhs.Remove(hocSinh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ByClass), new { id = maLopHoc });
            }
            return NotFound();
        }
    }
}