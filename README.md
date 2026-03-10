# 🏥 MedicalBookingSystem

> **Website đặt lịch hẹn khám bệnh ngoài giờ**  
> Đồ án môn Chuyên đề ASP.NET — Trường Đại học Trà Vinh

![.NET](https://img.shields.io/badge/.NET-6.0-blue?logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC-purple)
![EF Core](https://img.shields.io/badge/Entity_Framework-Core_6-green)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.1.3-blueviolet)
![SQL Server](https://img.shields.io/badge/SQL_Server-LocalDB-red)

---

## 📋 Mục lục

- [Giới thiệu](#-giới-thiệu)
- [Công nghệ sử dụng](#-công-nghệ-sử-dụng)
- [Cấu trúc dự án](#-cấu-trúc-dự-án)
- [Cơ sở dữ liệu](#-cơ-sở-dữ-liệu)
- [Hướng dẫn cài đặt](#-hướng-dẫn-cài-đặt)
- [Tài khoản mặc định](#-tài-khoản-mặc-định)
- [Mã nguồn chính](#-mã-nguồn-chính)
  - [Models](#models)
  - [Controllers](#controllers)
  - [Views](#views)
  - [Cấu hình hệ thống](#cấu-hình-hệ-thống)
- [Chức năng hệ thống](#-chức-năng-hệ-thống)
- [Giao diện](#-giao-diện)

---

## 📖 Giới thiệu

**MedicalBookingSystem** là hệ thống website đặt lịch hẹn khám bệnh trực tuyến, cho phép:

- **Bệnh nhân** tìm kiếm bác sĩ theo chuyên khoa, xem thông tin chi tiết và đặt lịch hẹn khám bệnh
- **Bác sĩ** quản lý lịch làm việc và lịch hẹn của bệnh nhân
- **Quản trị viên** quản lý toàn bộ hệ thống: bác sĩ, chuyên khoa, người dùng

---

## 🛠 Công nghệ sử dụng

| Thành phần | Công nghệ |
|---|---|
| Backend Framework | ASP.NET Core 6.0 MVC |
| ORM | Entity Framework Core 6.0 |
| Database | SQL Server LocalDB |
| Authentication | ASP.NET Core Identity |
| Frontend | Bootstrap 5.1.3 + Font Awesome 6 |
| IDE | Visual Studio 2022 |

---

## 📁 Cấu trúc dự án

```
MedicalBookingSystem/
│
├── Controllers/
│   ├── HomeController.cs          # Trang chủ, hồ sơ người dùng
│   ├── DoctorsController.cs       # Danh sách & chi tiết bác sĩ
│   ├── AppointmentsController.cs  # Đặt lịch & quản lý lịch hẹn
│   └── AccountController.cs       # Đăng ký, đăng nhập, đăng xuất
│
├── Models/
│   ├── ApplicationUser.cs         # Model người dùng (kế thừa IdentityUser)
│   ├── Doctor.cs                  # Model bác sĩ
│   ├── Specialty.cs               # Model chuyên khoa
│   ├── Appointment.cs             # Model lịch hẹn
│   └── Schedule.cs                # Model lịch làm việc bác sĩ
│
├── Data/
│   └── ApplicationDbContext.cs    # DbContext + cấu hình quan hệ bảng
│
├── Views/
│   ├── Shared/
│   │   └── _Layout.cshtml         # Layout chung (navbar, footer)
│   ├── Home/
│   │   └── Index.cshtml           # Trang chủ
│   ├── Doctors/
│   │   ├── Index.cshtml           # Danh sách bác sĩ
│   │   └── Details.cshtml         # Chi tiết bác sĩ
│   ├── Appointments/
│   │   ├── Index.cshtml           # Danh sách lịch hẹn
│   │   └── Create.cshtml          # Form đặt lịch
│   └── Account/
│       ├── Register.cshtml        # Form đăng ký
│       └── Login.cshtml           # Form đăng nhập
│
├── appsettings.json               # Cấu hình kết nối database
└── Program.cs                     # Cấu hình dịch vụ + seed data
```

---

## 🗄 Cơ sở dữ liệu

### Bảng `Specialties` — Chuyên khoa

| Cột | Kiểu | Mô tả |
|---|---|---|
| `Id` | int (PK) | Khóa chính |
| `Name` | nvarchar(100) | Tên chuyên khoa |
| `Description` | nvarchar(MAX) | Mô tả |
| `IsActive` | bit | Trạng thái hoạt động |
| `SortOrder` | int | Thứ tự hiển thị |
| `CreatedDate` | datetime | Ngày tạo |

### Bảng `Doctors` — Bác sĩ

| Cột | Kiểu | Mô tả |
|---|---|---|
| `Id` | int (PK) | Khóa chính |
| `Name` | nvarchar(100) | Tên bác sĩ |
| `UserId` | nvarchar (FK) | Liên kết tài khoản Identity |
| `SpecialtyId` | int (FK) | Chuyên khoa |
| `MedicalLicense` | nvarchar(50) | Số giấy phép hành nghề |
| `Qualifications` | nvarchar(MAX) | Bằng cấp |
| `Experience` | int | Số năm kinh nghiệm |
| `ConsultationFee` | decimal(18,2) | Phí khám |
| `AverageRating` | decimal | Điểm đánh giá trung bình |
| `TotalRatings` | int | Tổng lượt đánh giá |
| `IsAvailable` | bit | Đang tiếp nhận bệnh nhân |
| `IsActive` | bit | Trạng thái hoạt động |

### Bảng `Appointments` — Lịch hẹn

| Cột | Kiểu | Mô tả |
|---|---|---|
| `Id` | int (PK) | Khóa chính |
| `DoctorId` | int (FK) | Bác sĩ được đặt |
| `PatientName` | nvarchar(100) | Họ tên bệnh nhân |
| `PatientEmail` | nvarchar(100) | Email bệnh nhân |
| `PatientPhone` | nvarchar(15) | Số điện thoại |
| `AppointmentDate` | datetime | Ngày khám |
| `AppointmentTime` | nvarchar | Giờ khám |
| `Symptoms` | nvarchar(MAX) | Triệu chứng |
| `Status` | nvarchar(50) | Trạng thái (`Chờ xác nhận` / `Đã xác nhận` / `Đã hoàn thành`) |
| `CreatedDate` | datetime | Ngày tạo |

### Bảng `Schedules` — Lịch làm việc bác sĩ

| Cột | Kiểu | Mô tả |
|---|---|---|
| `Id` | int (PK) | Khóa chính |
| `DoctorId` | int (FK) | Bác sĩ |
| `WorkDate` | datetime | Ngày làm việc |
| `StartTime` | TimeSpan | Giờ bắt đầu |
| `EndTime` | TimeSpan | Giờ kết thúc |
| `MaxAppointments` | int | Số lượt đặt tối đa |
| `IsActive` | bit | Trạng thái |

---

## ⚙️ Hướng dẫn cài đặt

### Yêu cầu hệ thống

- Visual Studio 2022
- .NET 6.0 SDK
- SQL Server LocalDB (đi kèm Visual Studio)

### Các bước cài đặt

**Bước 1:** Clone hoặc tải về dự án

```bash
git clone https://github.com/your-username/MedicalBookingSystem.git
cd MedicalBookingSystem
```

**Bước 2:** Kiểm tra connection string trong `appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MedicalBookingSystem;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
}
```

**Bước 3:** Chạy migration để tạo database

```bash
# Trong Package Manager Console (Visual Studio)
Add-Migration InitialCreate
Update-Database
```

**Bước 4:** Chạy ứng dụng

```bash
dotnet run
# hoặc nhấn F5 trong Visual Studio
```

> ✅ Khi khởi động lần đầu, hệ thống tự động seed dữ liệu mẫu (chuyên khoa, bác sĩ, lịch hẹn, tài khoản admin).

---

## 🔑 Tài khoản mặc định

| Vai trò | Email | Mật khẩu |
|---|---|---|
| Admin | `admin@medical.com` | `Admin123!` |
| Bác sĩ (mẫu) | `doctor@medical.com` | `Doctor123!` |

---

## 💻 Mã nguồn chính

### Models

#### `Models/Specialty.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace MedicalBookingSystem.Models
{
    public class Specialty
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên chuyên khoa")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Hoạt động")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Thứ tự")]
        public int SortOrder { get; set; } = 0;

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
```

#### `Models/Doctor.cs`

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalBookingSystem.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên bác sĩ")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Chuyên khoa")]
        public int SpecialtyId { get; set; }

        [Required(ErrorMessage = "Số giấy phép hành nghề là bắt buộc")]
        [StringLength(50)]
        [Display(Name = "Số giấy phép")]
        public string MedicalLicense { get; set; } = string.Empty;

        [Display(Name = "Bằng cấp")]
        public string? Qualifications { get; set; }

        [Required]
        [Display(Name = "Kinh nghiệm")]
        public int Experience { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Phí khám")]
        public decimal ConsultationFee { get; set; }

        [Display(Name = "Đánh giá")]
        public decimal AverageRating { get; set; } = 0;

        [Display(Name = "Số đánh giá")]
        public int TotalRatings { get; set; } = 0;

        [Display(Name = "Có sẵn")]
        public bool IsAvailable { get; set; } = true;

        [Display(Name = "Hoạt động")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("SpecialtyId")]
        public virtual Specialty? Specialty { get; set; }
    }
}
```

#### `Models/Appointment.cs`

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalBookingSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        [Display(Name = "Họ tên bệnh nhân")]
        public string PatientName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string PatientEmail { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Số điện thoại")]
        public string PatientPhone { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Ngày khám")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "Giờ khám")]
        public string AppointmentTime { get; set; } = string.Empty;

        [Display(Name = "Triệu chứng")]
        public string? Symptoms { get; set; }

        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Chờ xác nhận";

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("DoctorId")]
        public virtual Doctor? Doctor { get; set; }
    }
}
```

#### `Models/Schedule.cs`

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalBookingSystem.Models
{
    public class Schedule
    {
        public int Id { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        [Display(Name = "Ngày làm việc")]
        [DataType(DataType.Date)]
        public DateTime WorkDate { get; set; }

        [Required]
        [Display(Name = "Giờ bắt đầu")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Display(Name = "Giờ kết thúc")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "Số lượt đặt tối đa")]
        public int MaxAppointments { get; set; } = 10;

        [Display(Name = "Hoạt động")]
        public bool IsActive { get; set; } = true;

        [ForeignKey("DoctorId")]
        public virtual Doctor? Doctor { get; set; }
    }
}
```

#### `Models/ApplicationUser.cs`

```csharp
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MedicalBookingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Giới tính")]
        [StringLength(10)]
        public string? Gender { get; set; }

        [Display(Name = "Địa chỉ")]
        [StringLength(200)]
        public string? Address { get; set; }

        [Required]
        [Display(Name = "Vai trò")]
        [StringLength(20)]
        public string Role { get; set; } = "Patient";

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Ngày cập nhật")]
        public DateTime? LastModifiedDate { get; set; }

        // Navigation properties
        public virtual ICollection<Appointment> PatientAppointments { get; set; } = new List<Appointment>();
        public virtual Doctor? Doctor { get; set; }
    }
}
```

---

### Controllers

#### `Controllers/HomeController.cs`

```csharp
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
                var featuredDoctors = _context.Doctors?
                    .Include(d => d.Specialty)
                    .Where(d => d.IsActive && d.IsAvailable)
                    .Take(4)
                    .ToList() ?? new List<Doctor>();

                ViewBag.FeaturedDoctors = featuredDoctors;
                ViewBag.TotalDoctors = _context.Doctors?.Count(d => d.IsActive) ?? 0;
                ViewBag.TotalSpecialties = _context.Specialties?.Count(s => s.IsActive) ?? 0;
                ViewBag.TotalAppointments = _context.Appointments?.Count() ?? 0;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải trang chủ");
                ViewBag.FeaturedDoctors = new List<Doctor>();
                ViewBag.TotalDoctors = 0;
                ViewBag.TotalSpecialties = 0;
                ViewBag.TotalAppointments = 0;
                return View();
            }
        }

        [Authorize]
        public IActionResult Profile()
        {
            var userName = User.Identity?.Name ?? "Khách";
            ViewBag.UserName = userName;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View();
    }
}
```

#### `Controllers/DoctorsController.cs`

```csharp
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

            if (doctor == null) return NotFound();
            return View(doctor);
        }
    }
}
```

#### `Controllers/AppointmentsController.cs`

```csharp
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
```

#### `Controllers/AccountController.cs` — ViewModels

```csharp
public class RegisterViewModel
{
    [Required(ErrorMessage = "Họ tên là bắt buộc")]
    [Display(Name = "Họ và tên")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    [Phone]
    [StringLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    public string Gender { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; } = DateTime.Now.AddYears(-18);

    [Required]
    [StringLength(200)]
    public string Address { get; set; } = string.Empty;

    [Range(typeof(bool), "true", "true", ErrorMessage = "Bạn phải đồng ý với điều khoản sử dụng")]
    public bool AcceptTerms { get; set; }
}

public class LoginViewModel
{
    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Ghi nhớ đăng nhập")]
    public bool RememberMe { get; set; }
}
```

---

### Cấu hình hệ thống

#### `Data/ApplicationDbContext.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using MedicalBookingSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MedicalBookingSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<Specialty> Specialties { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MedicalLicense).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ConsultationFee).HasColumnType("decimal(18,2)");
                entity.HasOne(d => d.Specialty)
                    .WithMany(s => s.Doctors)
                    .HasForeignKey(d => d.SpecialtyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PatientName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PatientEmail).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PatientPhone).IsRequired().HasMaxLength(15);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.HasOne(a => a.Doctor)
                    .WithMany()
                    .HasForeignKey(a => a.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
```

#### `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MedicalBookingSystem;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.AspNetCore.Identity": "Information"
    }
  },
  "AllowedHosts": "*",
  "ApplicationSettings": {
    "AppName": "Medical Booking System",
    "Version": "1.0.0",
    "AdminEmail": "admin@medical.com",
    "SupportEmail": "support@medical.com"
  }
}
```

#### `Program.cs` — Cấu hình Identity & Seed Data

```csharp
using Microsoft.EntityFrameworkCore;
using MedicalBookingSystem.Data;
using MedicalBookingSystem.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Cấu hình DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Cấu hình Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;

    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cấu hình Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Seed Data
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    context.Database.EnsureCreated();

    // Tạo Roles
    string[] roleNames = { "Admin", "Doctor", "Patient" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
            await roleManager.CreateAsync(new IdentityRole(roleName));
    }

    // Seed Specialties
    if (!context.Specialties.Any())
    {
        context.Specialties.AddRange(
            new Specialty { Name = "Tim mạch",      Description = "Chuyên khoa tim mạch",      SortOrder = 1, IsActive = true },
            new Specialty { Name = "Thần kinh",     Description = "Chuyên khoa thần kinh",     SortOrder = 2, IsActive = true },
            new Specialty { Name = "Tiêu hóa",      Description = "Chuyên khoa tiêu hóa",      SortOrder = 3, IsActive = true },
            new Specialty { Name = "Da liễu",       Description = "Chuyên khoa da liễu",       SortOrder = 4, IsActive = true },
            new Specialty { Name = "Nhi khoa",      Description = "Chuyên khoa nhi",           SortOrder = 5, IsActive = true },
            new Specialty { Name = "Sản phụ khoa",  Description = "Chuyên khoa sản phụ khoa", SortOrder = 6, IsActive = true }
        );
        await context.SaveChangesAsync();
    }

    // Seed Doctors
    if (!context.Doctors.Any())
    {
        var specialties = context.Specialties.ToList();
        var doctorUser = new IdentityUser
        {
            UserName = "doctor@medical.com",
            Email = "doctor@medical.com",
            EmailConfirmed = true,
            PhoneNumber = "0912345678"
        };
        var result = await userManager.CreateAsync(doctorUser, "Doctor123!");
        if (result.Succeeded)
            await userManager.AddToRoleAsync(doctorUser, "Doctor");

        context.Doctors.AddRange(
            new Doctor
            {
                Name = "BS. Nguyễn Văn A", UserId = doctorUser.Id,
                SpecialtyId = specialties[0].Id, MedicalLicense = "GL001",
                Qualifications = "Tiến sĩ Y khoa, Chuyên khoa Tim mạch - Bệnh viện Bạch Mai",
                Experience = 15, ConsultationFee = 500000,
                Description = "Bác sĩ chuyên khoa Tim mạch với 15 năm kinh nghiệm.",
                AverageRating = 4.8m, TotalRatings = 124, IsAvailable = true, IsActive = true
            },
            new Doctor
            {
                Name = "BS. Trần Thị B", UserId = doctorUser.Id,
                SpecialtyId = specialties[1].Id, MedicalLicense = "GL002",
                Qualifications = "Thạc sĩ Thần kinh học - Đại học Y Hà Nội",
                Experience = 12, ConsultationFee = 450000,
                Description = "Chuyên gia Thần kinh với 12 năm kinh nghiệm.",
                AverageRating = 4.7m, TotalRatings = 89, IsAvailable = true, IsActive = true
            }
        );
        await context.SaveChangesAsync();
    }

    // Tạo Admin
    var adminUser = await userManager.FindByEmailAsync("admin@medical.com");
    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = "admin@medical.com",
            Email = "admin@medical.com",
            EmailConfirmed = true
        };
        var adminResult = await userManager.CreateAsync(adminUser, "Admin123!");
        if (adminResult.Succeeded)
            await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Seed data error: {ex.Message}");
}

app.Run();
```

---

### Views

#### `Views/Shared/_Layout.cshtml`

```html
<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - MedicalBookingSystem</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">
</head>
<body>
<header>
    <nav class="navbar navbar-expand-sm navbar-light bg-white border-bottom box-shadow mb-3">
        <div class="container">
            <a class="navbar-brand" href="/">
                <i class="fas fa-hospital-alt me-2"></i>MedicalBooking
            </a>
            <div class="navbar-collapse collapse">
                <ul class="navbar-nav flex-grow-1">
                    <li class="nav-item">
                        <a class="nav-link text-dark" href="/"><i class="fas fa-home me-1"></i>Trang Chủ</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" href="/Doctors"><i class="fas fa-user-md me-1"></i>Bác Sĩ</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" href="/Appointments"><i class="fas fa-calendar-check me-1"></i>Đặt Lịch</a>
                    </li>
                </ul>
                <ul class="navbar-nav">
                    @* Kiểm tra trạng thái đăng nhập *@
                    @if (User?.Identity?.IsAuthenticated == true)
                    {
                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle text-dark" href="#" data-bs-toggle="dropdown">
                                <i class="fas fa-user me-1"></i>@User.Identity.Name
                            </a>
                            <ul class="dropdown-menu">
                                <li><a class="dropdown-item" href="/Home/Profile">Hồ sơ</a></li>
                                <li><hr class="dropdown-divider"></li>
                                <li>
                                    <form asp-controller="Account" asp-action="Logout" method="post">
                                        <button type="submit" class="dropdown-item">Đăng xuất</button>
                                    </form>
                                </li>
                            </ul>
                        </li>
                    }
                    else
                    {
                        <li class="nav-item">
                            <a class="nav-link text-dark" href="/Account/Register">Đăng ký</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" href="/Account/Login">Đăng nhập</a>
                        </li>
                    }
                </ul>
            </div>
        </div>
    </nav>
</header>
<div class="container">
    <main role="main" class="pb-3">@RenderBody()</main>
</div>
<footer class="border-top footer text-muted">
    <div class="container text-center">
        &copy; 2024 - MedicalBookingSystem -
        <a href="/Home/Privacy">Chính sách bảo mật</a>
    </div>
</footer>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
@await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

#### `Views/Home/Index.cshtml` — Trang chủ

```html
@{
    ViewData["Title"] = "Trang Chủ - Hệ thống đặt lịch khám bệnh";
}

<!-- Hero Section -->
<section class="hero-section bg-primary text-white py-5">
    <div class="container">
        <div class="row align-items-center">
            <div class="col-lg-6">
                <h1 class="display-4 fw-bold mb-4">Chăm sóc sức khỏe của bạn một cách thông minh</h1>
                <p class="lead mb-4">Đặt lịch khám với bác sĩ chuyên khoa nhanh chóng và thuận tiện.</p>
                <a href="/Doctors" class="btn btn-light btn-lg">
                    <i class="fas fa-search me-2"></i>Tìm Bác Sĩ
                </a>
                <a href="/Appointments/Create" class="btn btn-outline-light btn-lg ms-2">
                    <i class="fas fa-calendar-plus me-2"></i>Đặt Lịch Ngay
                </a>
            </div>
        </div>
    </div>
</section>

<!-- Thống kê -->
<section class="py-5">
    <div class="container">
        <div class="row text-center">
            <div class="col-md-4">
                <i class="fas fa-user-md fa-3x text-primary mb-3"></i>
                <h3 class="text-primary">@ViewBag.TotalDoctors+</h3>
                <p class="text-muted">Bác Sĩ Chuyên Khoa</p>
            </div>
            <div class="col-md-4">
                <i class="fas fa-stethoscope fa-3x text-success mb-3"></i>
                <h3 class="text-success">@ViewBag.TotalSpecialties+</h3>
                <p class="text-muted">Chuyên Khoa</p>
            </div>
            <div class="col-md-4">
                <i class="fas fa-calendar-check fa-3x text-warning mb-3"></i>
                <h3 class="text-warning">@ViewBag.TotalAppointments+</h3>
                <p class="text-muted">Lượt Đặt Lịch</p>
            </div>
        </div>
    </div>
</section>

<!-- Bác sĩ nổi bật -->
<section class="py-5 bg-light">
    <div class="container">
        <h2 class="text-center mb-5 fw-bold">Bác Sĩ Nổi Bật</h2>
        <div class="row">
            @if (ViewBag.FeaturedDoctors != null && ViewBag.FeaturedDoctors.Count > 0)
            {
                @foreach (var doctor in ViewBag.FeaturedDoctors)
                {
                    <div class="col-lg-3 col-md-6 mb-4">
                        <div class="card doctor-card h-100 shadow-sm border-0">
                            <div class="card-body text-center p-4">
                                <h5 class="card-title fw-bold">@doctor.Name</h5>
                                @if (doctor.Specialty != null)
                                {
                                    <p class="text-primary fw-bold">@doctor.Specialty.Name</p>
                                }
                                <p><i class="fas fa-briefcase me-1"></i>@doctor.Experience năm kinh nghiệm</p>
                                <p><i class="fas fa-money-bill-wave me-1"></i>@doctor.ConsultationFee.ToString("N0") VNĐ</p>
                                <a href="/Doctors/Details/@doctor.Id" class="btn btn-outline-primary btn-sm">Chi Tiết</a>
                                <a href="/Appointments/Create?doctorId=@doctor.Id" class="btn btn-primary btn-sm">Đặt Lịch</a>
                            </div>
                        </div>
                    </div>
                }
            }
        </div>
    </div>
</section>

<style>
    .hero-section { background: linear-gradient(135deg, #007bff 0%, #0056b3 100%); }
    .doctor-card { transition: transform 0.3s ease; }
    .doctor-card:hover { transform: translateY(-5px); box-shadow: 0 10px 30px rgba(0,0,0,0.1); }
</style>
```

#### `Views/Doctors/Index.cshtml` — Danh sách bác sĩ

```html
@model IEnumerable<MedicalBookingSystem.Models.Doctor>
@{
    ViewData["Title"] = "Danh Sách Bác Sĩ";
}

<div class="container mt-4">
    <h1 class="text-center mb-4">Danh Sách Bác Sĩ</h1>
    <div class="row">
        @if (Model != null && Model.Any())
        {
            @foreach (var doctor in Model)
            {
                <div class="col-lg-4 col-md-6 mb-4">
                    <div class="card h-100 shadow-sm">
                        <div class="card-body text-center">
                            <h5 class="card-title">@doctor.Name</h5>
                            @if (doctor.Specialty != null)
                            {
                                <p class="text-primary fw-bold">@doctor.Specialty.Name</p>
                            }
                            <p><small><i class="fas fa-briefcase me-2"></i>@doctor.Experience năm kinh nghiệm</small></p>
                            <p><small><i class="fas fa-money-bill-wave me-2"></i>@doctor.ConsultationFee.ToString("N0") VNĐ</small></p>
                            <a href="/Doctors/Details/@doctor.Id" class="btn btn-primary">
                                <i class="fas fa-info-circle me-2"></i>Xem Chi Tiết
                            </a>
                        </div>
                    </div>
                </div>
            }
        }
        else
        {
            <div class="col-12 text-center py-5">
                <h4 class="text-muted">Chưa có bác sĩ trong hệ thống</h4>
            </div>
        }
    </div>
</div>
```

#### `Views/Appointments/Create.cshtml` — Form đặt lịch

```html
@model MedicalBookingSystem.Models.Appointment
@{
    ViewData["Title"] = "Đặt Lịch Khám Mới";
}

<div class="container mt-4">
    <div class="row justify-content-center">
        <div class="col-md-8">
            <div class="card">
                <div class="card-header bg-primary text-white">
                    <h4><i class="fas fa-calendar-plus me-2"></i>Đặt Lịch Khám Mới</h4>
                </div>
                <div class="card-body">
                    <form asp-action="Create">
                        <div class="row">
                            <div class="col-md-6">
                                <label class="form-label">Chọn Bác Sĩ *</label>
                                <select asp-for="DoctorId" class="form-select" required>
                                    <option value="">-- Chọn bác sĩ --</option>
                                    @foreach (var doctor in ViewBag.Doctors)
                                    {
                                        <option value="@doctor.Id">@doctor.Name - @doctor.Specialty?.Name</option>
                                    }
                                </select>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Họ tên bệnh nhân *</label>
                                <input asp-for="PatientName" class="form-control" required>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-md-6">
                                <label class="form-label">Ngày khám *</label>
                                <input asp-for="AppointmentDate" type="date" class="form-control"
                                       min="@DateTime.Today.ToString("yyyy-MM-dd")" required>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Giờ khám *</label>
                                <select asp-for="AppointmentTime" class="form-select" required>
                                    <option value="">-- Chọn giờ --</option>
                                    <option value="08:00">08:00</option>
                                    <option value="09:00">09:00</option>
                                    <option value="10:00">10:00</option>
                                    <option value="14:00">14:00</option>
                                    <option value="15:00">15:00</option>
                                    <option value="16:00">16:00</option>
                                </select>
                            </div>
                        </div>
                        <div class="mt-3">
                            <label class="form-label">Triệu chứng (nếu có)</label>
                            <textarea asp-for="Symptoms" class="form-control" rows="3"
                                      placeholder="Mô tả triệu chứng..."></textarea>
                        </div>
                        <div class="text-center mt-4">
                            <button type="submit" class="btn btn-primary btn-lg">
                                <i class="fas fa-calendar-check me-2"></i>Đặt Lịch
                            </button>
                            <a href="/Appointments" class="btn btn-secondary ms-2">Quay Lại</a>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>
```

#### `Views/Account/Register.cshtml` — Đăng ký tài khoản (trích đoạn chính)

```html
@model MedicalBookingSystem.Controllers.RegisterViewModel
@{
    ViewData["Title"] = "Đăng Ký Tài Khoản";
}

<div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-8 col-lg-6">
            <div class="card shadow-lg border-0">
                <div class="card-header bg-primary text-white text-center py-4">
                    <h2><i class="fas fa-user-plus me-2"></i>ĐĂNG KÝ TÀI KHOẢN</h2>
                </div>
                <div class="card-body p-5">
                    <form id="registerForm" method="post">
                        @Html.AntiForgeryToken()
                        <div class="row">
                            <div class="col-md-6">
                                <label asp-for="FullName" class="form-label fw-bold">Họ và tên *</label>
                                <input asp-for="FullName" class="form-control form-control-lg" />
                                <span asp-validation-for="FullName" class="text-danger small"></span>
                            </div>
                            <div class="col-md-6">
                                <label asp-for="Email" class="form-label fw-bold">Email *</label>
                                <input asp-for="Email" class="form-control form-control-lg" />
                                <span asp-validation-for="Email" class="text-danger small"></span>
                            </div>
                        </div>
                        <!-- Các field khác: PhoneNumber, Password, ConfirmPassword, Gender, DateOfBirth, Address -->
                        <div class="d-grid mt-4">
                            <button type="submit" class="btn btn-primary btn-lg fw-bold">
                                <i class="fas fa-user-plus me-2"></i>ĐĂNG KÝ NGAY
                            </button>
                        </div>
                        <div class="text-center mt-3">
                            Đã có tài khoản?
                            <a href="/Account/Login" class="text-primary fw-bold">Đăng nhập ngay</a>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
<script>
    // Password strength indicator
    $('#Password').on('input', function () {
        var password = $(this).val();
        var strength = 0;
        if (password.length >= 6) strength += 25;
        if (password.match(/[a-z]/)) strength += 25;
        if (password.match(/[A-Z]/)) strength += 25;
        if (password.match(/[0-9]/)) strength += 25;
        $('#passwordStrengthBar').css('width', strength + '%');
    });

    // Confirm password real-time check
    $('#ConfirmPassword').on('input', function () {
        var match = $('#Password').val() === $(this).val();
        $('#passwordMatchText')
            .text(match ? '✓ Mật khẩu khớp' : '✗ Mật khẩu không khớp')
            .removeClass('text-danger text-success')
            .addClass(match ? 'text-success' : 'text-danger');
    });
</script>
}
```

---

## ✅ Chức năng hệ thống

### Bệnh nhân (Patient)
- [x] Đăng ký / Đăng nhập tài khoản
- [x] Xem danh sách bác sĩ theo chuyên khoa
- [x] Xem chi tiết bác sĩ (kinh nghiệm, phí khám, đánh giá)
- [x] Đặt lịch hẹn khám bệnh trực tuyến
- [x] Xem danh sách lịch hẹn đã đặt

### Quản trị viên (Admin)
- [x] Quản lý thông tin bác sĩ
- [x] Quản lý danh mục chuyên khoa
- [x] Xem và quản lý toàn bộ lịch hẹn
- [x] Quản lý tài khoản người dùng

---

## 🖼 Giao diện

| Màn hình | Mô tả |
|---|---|
| Trang chủ | Hero section, thống kê tổng quan, bác sĩ nổi bật, dịch vụ |
| Danh sách bác sĩ | Hiển thị dạng card, thông tin chuyên khoa & kinh nghiệm |
| Chi tiết bác sĩ | Thông tin đầy đủ, bằng cấp, đánh giá, nút đặt lịch |
| Đặt lịch hẹn | Form đặt lịch với calendar picker và chọn khung giờ |
| Quản lý lịch hẹn | Danh sách lịch hẹn dạng bảng với trạng thái |
| Đăng ký | Validation real-time, kiểm tra độ mạnh mật khẩu |
| Đăng nhập | Form đơn giản với tùy chọn Remember Me |

---

## 📄 Giấy phép

Đồ án học tập — Trường Đại học Trà Vinh © 2024–2025
