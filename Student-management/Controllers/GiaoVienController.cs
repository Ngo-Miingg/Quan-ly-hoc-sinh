using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Management.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Student_Management.Controllers
{
    // [Authorize(Roles = "Admin")] // Thông thường Admin sẽ quản lý giáo viên
    public class GiaoVienController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public GiaoVienController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: GiaoVien
        public async Task<IActionResult> Index()
        {
            // Sử dụng DbSet và navigation property đã chuẩn hóa
            var giaoViens = await _context.GiaoViens
                .Include(g => g.MonHoc)
                .Include(g => g.LopHocs) // Các lớp mà giáo viên này chủ nhiệm
                .ToListAsync();

            return View(giaoViens);
        }

        // GET: GiaoVien/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var giaoVien = await _context.GiaoViens
                .Include(g => g.MonHoc)
                .Include(g => g.LopHocs)
                .Include(g => g.LichHocs).ThenInclude(l => l.MonHoc)
                .Include(g => g.LichHocs).ThenInclude(l => l.LopHoc)
                .Include(g => g.PhanCongGiangDays).ThenInclude(p => p.LopHoc)
                .FirstOrDefaultAsync(m => m.MaGiaoVien == id); // Sửa tên PK

            if (giaoVien == null) return NotFound();

            return View(giaoVien);
        }

        // GET: GiaoVien/Create
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        // POST: GiaoVien/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GiaoVien giaoVien, int? maLopHocChuNhiem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giaoVien);
                await _context.SaveChangesAsync(); // Lưu để lấy được MaGiaoVien mới

                if (maLopHocChuNhiem.HasValue)
                {
                    var lopHoc = await _context.LopHocs.FindAsync(maLopHocChuNhiem.Value);
                    if (lopHoc != null)
                    {
                        lopHoc.MaGiaoVienChuNhiem = giaoVien.MaGiaoVien;
                        _context.Update(lopHoc);
                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            LoadDropdowns();
            return View(giaoVien);
        }

        // GET: GiaoVien/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var giaoVien = await _context.GiaoViens
                .Include(g => g.LopHocs)
                .FirstOrDefaultAsync(g => g.MaGiaoVien == id);

            if (giaoVien == null) return NotFound();

            var lopChuNhiem = await _context.LopHocs.FirstOrDefaultAsync(l => l.MaGiaoVienChuNhiem == giaoVien.MaGiaoVien);
            ViewBag.MaLopHocChuNhiem = lopChuNhiem?.MaLopHoc;

            LoadDropdowns();
            return View(giaoVien);
        }

        // POST: GiaoVien/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GiaoVien giaoVien, int? maLopHocChuNhiem)
        {
            if (id != giaoVien.MaGiaoVien) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Cập nhật thông tin cơ bản của giáo viên
                    _context.Update(giaoVien);

                    // 2. Xóa vai trò chủ nhiệm ở lớp cũ (nếu có)
                    var lopChuNhiemCu = await _context.LopHocs
                        .FirstOrDefaultAsync(l => l.MaGiaoVienChuNhiem == giaoVien.MaGiaoVien);
                    if (lopChuNhiemCu != null)
                    {
                        lopChuNhiemCu.MaGiaoVienChuNhiem = null;
                    }

                    // 3. Gán vai trò chủ nhiệm cho lớp mới (nếu có chọn)
                    if (maLopHocChuNhiem.HasValue)
                    {
                        var lopHocMoi = await _context.LopHocs.FindAsync(maLopHocChuNhiem.Value);
                        if (lopHocMoi != null)
                        {
                            lopHocMoi.MaGiaoVienChuNhiem = giaoVien.MaGiaoVien;
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.GiaoViens.Any(e => e.MaGiaoVien == giaoVien.MaGiaoVien))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            LoadDropdowns();
            return View(giaoVien);
        }

        // GET: GiaoVien/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var giaoVien = await _context.GiaoViens
                .Include(g => g.MonHoc)
                .Include(g => g.LopHocs)
                .FirstOrDefaultAsync(m => m.MaGiaoVien == id);

            if (giaoVien == null) return NotFound();

            return View(giaoVien);
        }

        // POST: GiaoVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var giaoVien = await _context.GiaoViens.FindAsync(id);

            if (giaoVien != null)
            {
                // Vì DbContext có ràng buộc Restrict, ta phải xử lý các phụ thuộc trước khi xóa
                // 1. Bỏ liên kết lớp chủ nhiệm
                var lopChuNhiem = await _context.LopHocs.FirstOrDefaultAsync(l => l.MaGiaoVienChuNhiem == id);
                if (lopChuNhiem != null)
                {
                    lopChuNhiem.MaGiaoVienChuNhiem = null;
                }

                // 2. Xóa các lịch học và phân công giảng dạy liên quan
                var lichHocs = await _context.LichHocs.Where(l => l.MaGiaoVien == id).ToListAsync();
                _context.LichHocs.RemoveRange(lichHocs);

                var phanCongs = await _context.PhanCongGiangDays.Where(p => p.MaGiaoVien == id).ToListAsync();
                _context.PhanCongGiangDays.RemoveRange(phanCongs);

                // Lưu các thay đổi phụ thuộc
                await _context.SaveChangesAsync();

                // 3. Xóa giáo viên
                _context.GiaoViens.Remove(giaoVien);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns()
        {
            ViewBag.MonHocList = _context.MonHocs.ToList();
            ViewBag.LopHocList = _context.LopHocs.ToList();
        }
    }
}