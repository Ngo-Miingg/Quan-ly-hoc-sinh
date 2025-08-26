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

        // === 1. DANH SÁCH LỚP HỌC (TRANG CHÍNH) ===
        // GET: /LopHoc
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

        // === 2. THÔNG TIN CHI TIẾT LỚP HỌC ===
        // GET: /LopHoc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lopHoc = await _context.LopHocs
                .Include(l => l.NamHoc)
                .Include(l => l.GiaoVienChuNhiem)
                .Include(l => l.HocSinhs)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MaNamHoc == id);

            if (lopHoc == null)
            {
                return NotFound();
            }

            return View(lopHoc);
        }

        // === 3. TẠO LỚP HỌC MỚI ===
        // GET: /LopHoc/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View(new Lop()); // Trả về một đối tượng mới để tránh lỗi NullReferenceException
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

        // === 4. SỬA THÔNG TIN LỚP HỌC ===
        // GET: /LopHoc/Edit/5
        // GET: /LopHoc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Tìm lớp học
            var lopHoc = await _context.LopHocs.FindAsync(id);

            if (lopHoc == null)
            {
                // Nếu không tìm thấy, trả về trang 404
                return NotFound();
            }

            // Nếu tìm thấy, tiếp tục xử lý
            await PopulateDropdowns(lopHoc.MaNamHoc, lopHoc.MaGiaoVienChuNhiem);
            return View(lopHoc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLopHoc,TenLopHoc,SiSo,MaNamHoc,MaGiaoVienChuNhiem")] Lop lopHoc)
        {
            if (id != lopHoc.MaLopHoc)
            {
                return NotFound();
            }

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
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns(lopHoc.MaNamHoc, lopHoc.MaGiaoVienChuNhiem);
            return View(lopHoc);
        }

        // === 5. XÓA LỚP HỌC ===
        // GET: /LopHoc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var lopHoc = await _context.LopHocs
                .Include(l => l.NamHoc)
                .Include(l => l.GiaoVienChuNhiem)
                .Include(l => l.HocSinhs)
                .FirstOrDefaultAsync(m => m.MaLopHoc == id);
            if (lopHoc == null)
            {
                return NotFound();
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
                _context.LopHocs.Remove(lopHoc);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // === 6. HÀM TIỆN ÍCH CHO DROPDOWN ===
        private async Task PopulateDropdowns(object? selectedNamHoc = null, object? selectedGiaoVien = null)
        {
            ViewBag.NamHocList = new SelectList(await _context.NamHocs.AsNoTracking().ToListAsync(), "MaNamHoc", "TenNamHoc", selectedNamHoc);
            ViewBag.GiaoVienList = new SelectList(await _context.GiaoViens.AsNoTracking().ToListAsync(), "MaGiaoVien", "TenGiaoVien", selectedGiaoVien);
        }
    }
}