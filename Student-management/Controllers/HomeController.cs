using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Student_Management.Models; // Namespace cho ErrorViewModel

namespace Student_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: /Home/Index (Trang chủ)
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Privacy (Trang chính sách bảo mật)
        public IActionResult Privacy()
        {
            return View();
        }

        // Action xử lý lỗi
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}