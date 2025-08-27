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
    public class LopHocController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public LopHocController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lopHocs = await _context.LopHocs
                .Include(l => l.NamHoc)
                .Include(l => l.GiaoVienChuNhiem)
                .ToListAsync();
            return View(lopHocs);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var lopHoc = await _context.LopHocs
                .Include(l => l.NamHoc)
                .Include(l => l.GiaoVienChuNhiem)
                .Include(l => l.HocSinhs)
                .FirstOrDefaultAsync(m => m.MaLopHoc == id);
            if (lopHoc == null) return NotFound();
            return View(lopHoc);
        }

        // GET: LopHoc/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View(new Lop()); // Model trống
        }

        // POST: LopHoc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lop lopHoc)
        {
            if (ModelState.IsValid)
            {
                _context.LopHocs.Add(lopHoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi validate, reload dropdown
            await PopulateDropdowns(lopHoc.MaNamHoc, lopHoc.MaGiaoVienChuNhiem);
            return View(lopHoc);
        }


        // GET: LopHoc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var lopHoc = await _context.LopHocs.FindAsync(id);
            if (lopHoc == null) return NotFound();

            await PopulateDropdowns(lopHoc.MaNamHoc, lopHoc.MaGiaoVienChuNhiem);
            return View(lopHoc);
        }

        // POST: LopHoc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Lop lopHoc)
        {
            if (id != lopHoc.MaLopHoc) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lopHoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.LopHocs.Any(e => e.MaLopHoc == lopHoc.MaLopHoc))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // Nếu lỗi validate, reload dropdown
            await PopulateDropdowns(lopHoc.MaNamHoc, lopHoc.MaGiaoVienChuNhiem);
            return View(lopHoc);
        }


        // --- Xóa lớp học ---
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var lopHoc = await _context.LopHocs
                .Include(l => l.NamHoc)
                .Include(l => l.GiaoVienChuNhiem)
                .FirstOrDefaultAsync(m => m.MaLopHoc == id);
            if (lopHoc == null) return NotFound();

            var hasStudents = await _context.HocSinhs.AnyAsync(s => s.MaLopHoc == id);
            if (hasStudents)
            {
                TempData["ErrorMessage"] = "Không thể xóa lớp học này vì vẫn còn học sinh trong lớp.";
                return RedirectToAction(nameof(Index));
            }

            return View(lopHoc);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lopHoc = await _context.LopHocs.FindAsync(id);
            if (lopHoc != null)
            {
                var hasStudents = await _context.HocSinhs.AnyAsync(s => s.MaLopHoc == id);
                if (hasStudents)
                {
                    TempData["ErrorMessage"] = "Không thể xóa lớp học này vì vẫn còn học sinh trong lớp.";
                    return RedirectToAction(nameof(Index));
                }

                _context.LopHocs.Remove(lopHoc);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Hàm tiện ích tạo dropdown
        private async Task PopulateDropdowns(object? selectedNamHoc = null, object? selectedGiaoVien = null)
        {
            var namHocs = await _context.NamHocs.ToListAsync() ?? new List<NamHoc>();
            var giaoViens = await _context.GiaoViens.ToListAsync() ?? new List<GiaoVien>();

            ViewBag.NamHocList = new SelectList(namHocs, "MaNamHoc", "TenNamHoc", selectedNamHoc);
            ViewBag.GiaoVienList = new SelectList(giaoViens, "MaGiaoVien", "HoTen", selectedGiaoVien);
        }

    }
}