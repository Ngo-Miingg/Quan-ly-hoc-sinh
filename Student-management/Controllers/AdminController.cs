using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(QuanLyHocSinhContext context) : Controller
{
    private readonly QuanLyHocSinhContext _context = context;

    public IActionResult Index()
    {
        return View();
    }

    // GET: /Admin/Classroom
    public async Task<IActionResult> Classroom()
    {
        var classes = await _context.Lops
                                    .Include(l => l.MaNamHocNavigation)
                                    .Include(l => l.MaGvcnNavigation)
                                    .ThenInclude(gv => gv.MaNguoiDungNavigation)
                                    .ToListAsync();
        return View(classes);
    }

    // GET: /Admin/CreateClassroom
    [HttpGet]
    public async Task<IActionResult> CreateClassroom()
    {
        ViewBag.NamHocs = await _context.NamHocs.ToListAsync();
        ViewBag.GiaoViens = await _context.GiaoViens
                                         .Include(gv => gv.MaNguoiDungNavigation)
                                         .ToListAsync();
        return View();
    }

    // POST: /Admin/CreateClassroom
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateClassroom(Lop lop)
    {
        lop.SiSo = 0;
        _context.Lops.Add(lop);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Lớp học đã được tạo thành công.";
        return RedirectToAction("Classroom");
    }
    [HttpGet]
    public async Task<IActionResult> EditUser(int? id)
    {
        if (id == null) return NotFound();

        var user = await _context.NguoiDungs
                                 .Include(n => n.TaiKhoan)
                                 .FirstOrDefaultAsync(n => n.MaNguoiDung == id);
        if (user == null) return NotFound();

        ViewBag.Roles = await _context.VaiTros.ToListAsync();
        return View(user);

    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(int id, [Bind("MaNguoiDung,HoTen,NgaySinh,GioiTinh,DiaChi,Sdt,Email")] NguoiDung nguoiDung)
    {
        if (id != nguoiDung.MaNguoiDung) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(nguoiDung);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thông tin người dùng đã được cập nhật thành công.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.NguoiDungs.Any(e => e.MaNguoiDung == nguoiDung.MaNguoiDung))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(ListUsers));
        }
        ViewBag.Roles = await _context.VaiTros.ToListAsync();
        return View(nguoiDung);
    }
    [HttpGet]
    public async Task<IActionResult> DeleteUser(int? id)
    {
        if (id == null) return NotFound();

        var user = await _context.NguoiDungs
                                 .Include(n => n.TaiKhoan)
                                 .ThenInclude(t => t.MaVaiTroNavigation)
                                 .FirstOrDefaultAsync(m => m.MaNguoiDung == id);
        if (user == null) return NotFound();

        return View(user);
    }
    // Trong Controllers/AdminController.cs
    [HttpPost, ActionName("DeleteUser")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var nguoiDung = await _context.NguoiDungs.FindAsync(id);
        if (nguoiDung != null)
        {
            // 1. Tìm và xóa bản ghi Tài khoản liên quan
            var taiKhoan = await _context.TaiKhoans.FirstOrDefaultAsync(t => t.MaNguoiDung == id);
            if (taiKhoan != null)
            {
                _context.TaiKhoans.Remove(taiKhoan);
            }

            // 2. Tìm và xóa bản ghi Học sinh liên quan (nếu có)
            var hocSinh = await _context.HocSinhs.FirstOrDefaultAsync(hs => hs.MaNguoiDung == id);
            if (hocSinh != null)
            {
                // Nếu là học sinh, giảm sĩ số lớp học trước khi xóa
                var lop = await _context.Lops.FindAsync(hocSinh.MaLop);
                if (lop != null)
                {
                    lop.SiSo--;
                    _context.Lops.Update(lop);
                }
                _context.HocSinhs.Remove(hocSinh);
            }

            // 3. Tìm và xóa bản ghi Giáo viên liên quan (nếu có)
            var giaoVien = await _context.GiaoViens.FirstOrDefaultAsync(gv => gv.MaNguoiDung == id);
            if (giaoVien != null)
            {
                _context.GiaoViens.Remove(giaoVien);
            }

            // 4. Cuối cùng, xóa bản ghi Người dùng
            _context.NguoiDungs.Remove(nguoiDung);

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Người dùng và các dữ liệu liên quan đã được xóa thành công.";
        }
        return RedirectToAction(nameof(ListUsers));
    }
    // GET: /Admin/EditClassroom
    [HttpGet]
    public async Task<IActionResult> EditClassroom(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lop = await _context.Lops.FindAsync(id);
        if (lop == null)
        {
            return NotFound();
        }

        ViewBag.NamHocs = await _context.NamHocs.ToListAsync();
        ViewBag.GiaoViens = await _context.GiaoViens
                                         .Include(gv => gv.MaNguoiDungNavigation)
                                         .ToListAsync();

        return View(lop);
    }

    // POST: /Admin/EditClassroom
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditClassroom(int id, [Bind("MaLop,TenLop,SiSo,MaNamHoc,MaGvcn")] Lop lop)
    {
        if (id != lop.MaLop)
        {
            return NotFound();
        }

        try
        {
            _context.Update(lop);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Lớp học đã được cập nhật thành công.";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Lops.Any(e => e.MaLop == lop.MaLop))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return RedirectToAction(nameof(Classroom));
    }

    // GET: /Admin/DeleteClassroom
    [HttpGet]
    public async Task<IActionResult> DeleteClassroom(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lop = await _context.Lops
                                .Include(l => l.MaNamHocNavigation)
                                .Include(l => l.MaGvcnNavigation)
                                .ThenInclude(gv => gv.MaNguoiDungNavigation)
                                .FirstOrDefaultAsync(m => m.MaLop == id);
        if (lop == null)
        {
            return NotFound();
        }

        return View(lop);
    }

    

    // GET: /Admin/ListUsers
    public async Task<IActionResult> ListUsers()
    {
        var users = await _context.NguoiDungs
                                  .Include(n => n.TaiKhoan)
                                  .ThenInclude(t => t.MaVaiTroNavigation)
                                  .ToListAsync();
        return View(users);
    }

    // GET: /Admin/CreateUser
    [HttpGet]
    public async Task<IActionResult> CreateUser()
    {
        ViewBag.Roles = await _context.VaiTros.ToListAsync();
        ViewBag.Lops = await _context.Lops.ToListAsync();
        return View(new CreateUserViewModel());
    }

    // POST: /Admin/CreateUser
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (await _context.TaiKhoans.AnyAsync(t => t.TenDangNhap == model.TenDangNhap))
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại.");
                ViewBag.Roles = await _context.VaiTros.ToListAsync();
                ViewBag.Lops = await _context.Lops.ToListAsync();
                return View(model);
            }

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
            await _context.SaveChangesAsync();

            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = model.TenDangNhap,
                MatKhauHash = model.MatKhau,
                MaNguoiDung = nguoiDung.MaNguoiDung,
                MaVaiTro = model.MaVaiTro
            };

            _context.TaiKhoans.Add(taiKhoan);

            var vaiTro = await _context.VaiTros.FirstOrDefaultAsync(v => v.MaVaiTro == model.MaVaiTro);
            if (vaiTro?.TenVaiTro == "HocSinh")
            {
                var hocSinh = new HocSinh
                {
                    MaNguoiDung = nguoiDung.MaNguoiDung,
                    MaLop = model.MaLop ?? 0
                };
                _context.HocSinhs.Add(hocSinh);

                var lop = await _context.Lops.FindAsync(model.MaLop);
                if (lop != null)
                {
                    lop.SiSo++;
                    _context.Update(lop);
                }
            }
            else if (vaiTro?.TenVaiTro == "GiaoVien")
            {
                var giaoVien = new GiaoVien
                {
                    MaNguoiDung = nguoiDung.MaNguoiDung
                };
                _context.GiaoViens.Add(giaoVien);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Tài khoản đã được tạo thành công.";
            return RedirectToAction("ListUsers");
        }

        ViewBag.Roles = await _context.VaiTros.ToListAsync();
        ViewBag.Lops = await _context.Lops.ToListAsync();
        return View(new CreateUserViewModel()); // Thêm dòng này để truyền model rỗng vào view
        
    }

}
