using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers;

[Authorize(Roles = "Admin")]
public class HocSinhController(QuanLyHocSinhContext context) : Controller
{
    private readonly QuanLyHocSinhContext _context = context;

    // GET: /HocSinh
    public async Task<IActionResult> Index()
    {
        var hocSinhes = await _context.HocSinhs
            .Include(hs => hs.MaNguoiDungNavigation)
            .Include(hs => hs.MaLopNavigation)
            .ToListAsync();
        return View(hocSinhes);
    }

    // GET: /HocSinh/Create
    public IActionResult Create()
    {
        ViewData["MaLop"] = new SelectList(_context.Lops, "MaLop", "TenLop");
        return View();
    }

    // POST: /HocSinh/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var nguoiDung = new NguoiDung
            {
                HoTen = model.HoTen,
                NgaySinh = DateOnly.FromDateTime(model.NgaySinh),
                GioiTinh = model.GioiTinh,
                DiaChi = model.DiaChi,
                Sdt = model.Sdt,
                Email = model.Email
            };
            _context.NguoiDungs.Add(nguoiDung);
            await _context.SaveChangesAsync(); // Lưu NguoiDung để có MaNguoiDung

            var hocSinh = new HocSinh
            {
                MaNguoiDung = nguoiDung.MaNguoiDung,
                MaLop = model.MaLop ?? 0, // Giả sử lớp là bắt buộc
            };
            _context.HocSinhs.Add(hocSinh);

            var lop = await _context.Lops.FindAsync(model.MaLop);
            if (lop != null)
            {
                lop.SiSo++;
                _context.Update(lop);
            }

            await _context.SaveChangesAsync(); // Lưu các thay đổi cuối cùng
            TempData["SuccessMessage"] = "Học sinh đã được thêm thành công.";
            return RedirectToAction(nameof(Index));
        }

        ViewData["MaLop"] = new SelectList(_context.Lops, "MaLop", "TenLop", model.MaLop);
        return View(model);
    }

    // GET: /HocSinh/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var hocSinh = await _context.HocSinhs
                                    .Include(hs => hs.MaNguoiDungNavigation)
                                    .FirstOrDefaultAsync(hs => hs.MaHs == id);
        if (hocSinh == null) return NotFound();

        ViewData["MaLop"] = new SelectList(_context.Lops, "MaLop", "TenLop", hocSinh.MaLop);
        return View(hocSinh);
    }

    // POST: /HocSinh/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MaHs,MaNguoiDung,MaLop")] HocSinh hocSinh)
    {
        if (id != hocSinh.MaHs) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(hocSinh);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thông tin học sinh đã được cập nhật thành công.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HocSinhExists(hocSinh.MaHs)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["MaLop"] = new SelectList(_context.Lops, "MaLop", "TenLop", hocSinh.MaLop);
        ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "HoTen", hocSinh.MaNguoiDung);
        return View(hocSinh);
    }

    // GET: /HocSinh/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var hocSinh = await _context.HocSinhs
                                    .Include(hs => hs.MaNguoiDungNavigation)
                                    .Include(hs => hs.MaLopNavigation)
                                    .FirstOrDefaultAsync(m => m.MaHs == id);
        if (hocSinh == null) return NotFound();

        return View(hocSinh);
    }

    // POST: /HocSinh/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var hocSinh = await _context.HocSinhs.FindAsync(id);
        if (hocSinh != null)
        {
            var diemHocSinh = await _context.Diems.Where(d => d.MaHs == id).ToListAsync();
            if (diemHocSinh.Any())
            {
                _context.Diems.RemoveRange(diemHocSinh);
            }
            _context.HocSinhs.Remove(hocSinh);
        }

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Học sinh đã được xóa thành công.";
        return RedirectToAction(nameof(Index));
    }

    private bool HocSinhExists(int id)
    {
        return _context.HocSinhs.Any(e => e.MaHs == id);
    }
}