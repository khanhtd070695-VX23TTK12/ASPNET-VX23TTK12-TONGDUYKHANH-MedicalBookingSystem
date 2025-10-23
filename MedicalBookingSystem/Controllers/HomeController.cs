using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalBookingSystem.Data;
using MedicalBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace MedicalBookingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                // FIX NULLABLE: Thêm điều kiện kiểm tra null
                var featuredDoctors = _context.Doctors?
                    .Include(d => d.Specialty)
                    .Where(d => d.IsActive && d.IsAvailable)
                    .Take(4)
                    .ToList() ?? new List<Doctor>();

                // Lấy thống kê với kiểm tra null
                var totalDoctors = _context.Doctors?.Count(d => d.IsActive) ?? 0;
                var totalSpecialties = _context.Specialties?.Count(s => s.IsActive) ?? 0;
                var totalAppointments = _context.Appointments?.Count() ?? 0;

                ViewBag.FeaturedDoctors = featuredDoctors;
                ViewBag.TotalDoctors = totalDoctors;
                ViewBag.TotalSpecialties = totalSpecialties;
                ViewBag.TotalAppointments = totalAppointments;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải trang chủ");

                // TRẢ VỀ DANH SÁCH RỖNG NẾU CÓ LỖI
                ViewBag.FeaturedDoctors = new List<Doctor>();
                ViewBag.TotalDoctors = 0;
                ViewBag.TotalSpecialties = 0;
                ViewBag.TotalAppointments = 0;
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Profile()
        {
            // FIX NULLABLE: Kiểm tra User.Identity có thể null
            var userName = User.Identity?.Name ?? "Khách";
            ViewBag.UserName = userName;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}