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

        // GET: /LopHoc - DANH SÁCH LỚP HỌC
        public async Task<IActionResult> Index()
        {
            var lopHocs = await _context.LopHocs
                .Include(l => l.NamHoc)
                .Include(l => l.GiaoVienChuNhiem)
                .Include(l => l.HocSinhs)
                .AsNoTracking()
                .ToListAsync();
            return View(lopHocs);
        }

        // GET: /LopHoc/Details/5 - TRUNG TÂM QUẢN LÝ LỚP HỌC
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var lopHoc = await _context.LopHocs
                .Include(l => l.NamHoc)
                .Include(l => l.GiaoVienChuNhiem)
                .Include(l => l.HocSinhs)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MaLopHoc == id);

            if (lopHoc == null) return NotFound();

            ViewBag.AvailableStudents = new SelectList(
                await _context.HocSinhs.Where(s => s.MaLopHoc == null || s.MaLopHoc != id).ToListAsync(),
                "MaHocSinh", "HoTen");

            return View(lopHoc);
        }

        // GET: /LopHoc/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenLopHoc,SiSo,MaNamHoc,MaGiaoVienChuNhiem")] Lop lopHoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lopHoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns(lopHoc.MaNamHoc, lopHoc.MaGiaoVienChuNhiem);
            return View(lopHoc);
        }

        // GET: /LopHoc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var lopHoc = await _context.LopHocs.FindAsync(id);
            if (lopHoc == null) return NotFound();
            await PopulateDropdowns(lopHoc.MaNamHoc, lopHoc.MaGiaoVienChuNhiem);
            return View(lopHoc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLopHoc,TenLopHoc,SiSo,MaNamHoc,MaGiaoVienChuNhiem")] Lop lopHoc)
        {
            if (id != lopHoc.MaLopHoc) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(lopHoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns(lopHoc.MaNamHoc, lopHoc.MaGiaoVienChuNhiem);
            return View(lopHoc);
        }

        // GET: /LopHoc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var lopHoc = await _context.LopHocs.Include(l => l.HocSinhs).FirstOrDefaultAsync(m => m.MaLopHoc == id);
            if (lopHoc == null) return NotFound();
            return View(lopHoc);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lopHoc = await _context.LopHocs.Include(l => l.HocSinhs).FirstOrDefaultAsync(l => l.MaLopHoc == id);
            if (lopHoc != null)
            {
                if (lopHoc.HocSinhs.Any())
                {
                    TempData["ErrorMessage"] = "Không thể xóa lớp học này vì vẫn còn học sinh trong lớp.";
                    return RedirectToAction(nameof(Index));
                }
                _context.LopHocs.Remove(lopHoc);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // --- CÁC ACTION XỬ LÝ CHỨC NĂNG CON BÊN TRONG MODULE ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudentToClass(int maLopHoc, int maHocSinh)
        {
            var student = await _context.HocSinhs.FindAsync(maHocSinh);
            if (student != null)
            {
                student.MaLopHoc = maLopHoc;
                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", new { id = maLopHoc });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveStudentFromClass(int maHocSinh, int maLopHoc)
        {
            var student = await _context.HocSinhs.FindAsync(maHocSinh);
            if (student != null)
            {
                student.MaLopHoc = null;
                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", new { id = maLopHoc });
        }

        private async Task PopulateDropdowns(object? selectedNamHoc = null, object? selectedGiaoVien = null)
        {
            ViewBag.NamHocList = new SelectList(await _context.NamHocs.AsNoTracking().ToListAsync(), "MaNamHoc", "TenNamHoc", selectedNamHoc);
            ViewBag.GiaoVienList = new SelectList(await _context.GiaoViens.AsNoTracking().ToListAsync(), "MaGiaoVien", "HoTen", selectedGiaoVien);
        }
    }
}