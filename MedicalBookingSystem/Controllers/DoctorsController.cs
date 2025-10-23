using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalBookingSystem.Data;
using MedicalBookingSystem.Models;

namespace MedicalBookingSystem.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Doctors
        public IActionResult Index()
        {
            var doctors = _context.Doctors
                .Include(d => d.Specialty)
                .Where(d => d.IsActive)
                .ToList();

            return View(doctors);
        }

        // GET: /Doctors/Details/1
        public IActionResult Details(int id)
        {
            var doctor = _context.Doctors
                .Include(d => d.Specialty)
                .FirstOrDefault(d => d.Id == id);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }
    }
}