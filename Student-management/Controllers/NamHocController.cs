using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Management.Models;
using System;
using System.Threading.Tasks;

namespace Student_Management.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NamHocController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public NamHocController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: Hiển thị form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Xử lý dữ liệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NamHoc namHoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(namHoc);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Admin"); // Chuyển về Dashboard
            }
            return View(namHoc);
        }
    }
}