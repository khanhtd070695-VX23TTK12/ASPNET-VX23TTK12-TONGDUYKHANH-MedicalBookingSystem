using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalBookingSystem.Data;
using MedicalBookingSystem.Models;

namespace MedicalBookingSystem.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Appointments
        public IActionResult Index()
        {
            var appointments = _context.Appointments
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();

            return View(appointments);
        }

        // GET: /Appointments/Create
        public IActionResult Create()
        {
            ViewBag.Doctors = _context.Doctors
                .Where(d => d.IsActive && d.IsAvailable)
                .ToList();

            return View();
        }

        // POST: /Appointments/Create
        [HttpPost]
        public IActionResult Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Appointments.Add(appointment);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Đặt lịch thành công! Chúng tôi sẽ liên hệ xác nhận.";
                return RedirectToAction("Index");
            }

            ViewBag.Doctors = _context.Doctors
                .Where(d => d.IsActive && d.IsAvailable)
                .ToList();

            return View(appointment);
        }
    }
}