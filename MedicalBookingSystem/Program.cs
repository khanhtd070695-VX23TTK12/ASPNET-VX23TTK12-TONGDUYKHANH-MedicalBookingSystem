using Microsoft.EntityFrameworkCore;
using MedicalBookingSystem.Data;
using MedicalBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext với SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Thêm Identity với cấu hình đầy đủ
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    // Cấu hình mật khẩu
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Cấu hình lockout
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình user
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

    // Cấu hình signin
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
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

// Thêm session (nếu cần)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Thêm HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Thêm Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Thêm session
app.UseSession();

// Map controllers và Razor Pages cho Identity
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// SEED DATA - THÊM DỮ LIỆU MẪU
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Đảm bảo database được tạo
    context.Database.EnsureCreated();
    Console.WriteLine("Database created successfully!");

    // Tạo roles nếu chưa có
    string[] roleNames = { "Admin", "Doctor", "Patient" };
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
            Console.WriteLine($"Role '{roleName}' created successfully!");
        }
    }

    // THÊM CHUYÊN KHOA MẪU
    if (!context.Specialties.Any())
    {
        Console.WriteLine("Adding sample specialties...");
        context.Specialties.AddRange(
            new Specialty
            {
                Name = "Tim mạch",
                Description = "Chuyên khoa tim mạch",
                SortOrder = 1,
                IsActive = true
            },
            new Specialty
            {
                Name = "Thần kinh",
                Description = "Chuyên khoa thần kinh",
                SortOrder = 2,
                IsActive = true
            },
            new Specialty
            {
                Name = "Tiêu hóa",
                Description = "Chuyên khoa tiêu hóa",
                SortOrder = 3,
                IsActive = true
            },
            new Specialty
            {
                Name = "Da liễu",
                Description = "Chuyên khoa da liễu",
                SortOrder = 4,
                IsActive = true
            },
            new Specialty
            {
                Name = "Nhi khoa",
                Description = "Chuyên khoa nhi",
                SortOrder = 5,
                IsActive = true
            },
            new Specialty
            {
                Name = "Sản phụ khoa",
                Description = "Chuyên khoa sản phụ khoa",
                SortOrder = 6,
                IsActive = true
            }
        );
        await context.SaveChangesAsync();
        Console.WriteLine("Specialties added successfully!");
    }

    // THÊM BÁC SĨ MẪU
    if (!context.Doctors.Any())
    {
        Console.WriteLine("Adding sample doctors...");

        var specialties = context.Specialties.ToList();

        // Tạo user cho bác sĩ
        var doctorUser = new IdentityUser
        {
            UserName = "doctor@medical.com",
            Email = "doctor@medical.com",
            EmailConfirmed = true,
            PhoneNumber = "0912345678"
        };

        var result = await userManager.CreateAsync(doctorUser, "Doctor123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(doctorUser, "Doctor");
            Console.WriteLine("Doctor user created successfully!");
        }

        context.Doctors.AddRange(
            new Doctor
            {
                Name = "BS. Nguyễn Văn A",
                UserId = doctorUser.Id,
                SpecialtyId = specialties[0].Id,
                MedicalLicense = "GL001",
                Qualifications = "Tiến sĩ Y khoa, Chuyên khoa Tim mạch - Bệnh viện Bạch Mai",
                Experience = 15,
                ConsultationFee = 500000,
                Description = "Bác sĩ chuyên khoa Tim mạch với 15 năm kinh nghiệm. Chuyên điều trị các bệnh lý về tim mạch, huyết áp, suy tim. Từng tu nghiệp tại Pháp và Nhật Bản.",
                AverageRating = 4.8m,
                TotalRatings = 124,
                IsAvailable = true,
                IsActive = true
            },
            new Doctor
            {
                Name = "BS. Trần Thị B",
                UserId = doctorUser.Id,
                SpecialtyId = specialties[1].Id,
                MedicalLicense = "GL002",
                Qualifications = "Thạc sĩ Thần kinh học - Đại học Y Hà Nội",
                Experience = 12,
                ConsultationFee = 450000,
                Description = "Chuyên gia Thần kinh với 12 năm kinh nghiệm. Điều trị các bệnh đau đầu, động kinh, Parkinson, đột quỵ. Phương pháp điều trị tiên tiến, hiện đại.",
                AverageRating = 4.7m,
                TotalRatings = 89,
                IsAvailable = true,
                IsActive = true
            },
            new Doctor
            {
                Name = "BS. Lê Văn C",
                UserId = doctorUser.Id,
                SpecialtyId = specialties[2].Id,
                MedicalLicense = "GL003",
                Qualifications = "Tiến sĩ Tiêu hóa, Bác sĩ Nội khoa - Bệnh viện Việt Đức",
                Experience = 10,
                ConsultationFee = 400000,
                Description = "Chuyên gia Tiêu hóa với 10 năm kinh nghiệm. Nội soi tiêu hóa, điều trị viêm loét dạ dày, gan mật, đại tràng. Thăm khám tận tình, chu đáo.",
                AverageRating = 4.9m,
                TotalRatings = 156,
                IsAvailable = true,
                IsActive = true
            },
            new Doctor
            {
                Name = "BS. Phạm Thị D",
                UserId = doctorUser.Id,
                SpecialtyId = specialties[3].Id,
                MedicalLicense = "GL004",
                Qualifications = "Bác sĩ Da liễu, Thẩm mỹ da - Đại học Y Dược TP.HCM",
                Experience = 8,
                ConsultationFee = 350000,
                Description = "Chuyên gia Da liễu và Thẩm mỹ. Điều trị mụn, nám, tàn nhang, viêm da và các bệnh da liễu khác. Phương pháp điều trị hiện đại, an toàn.",
                AverageRating = 4.6m,
                TotalRatings = 67,
                IsAvailable = true,
                IsActive = true
            },
            new Doctor
            {
                Name = "BS. Hoàng Văn E",
                UserId = doctorUser.Id,
                SpecialtyId = specialties[4].Id,
                MedicalLicense = "GL005",
                Qualifications = "Bác sĩ Nhi khoa, Chuyên gia Dinh dưỡng trẻ em - Bệnh viện Nhi Trung ương",
                Experience = 14,
                ConsultationFee = 300000,
                Description = "Bác sĩ Nhi khoa với 14 năm kinh nghiệm. Chuyên khám và tư vấn sức khỏe, dinh dưỡng cho trẻ em. Điều trị các bệnh về hô hấp, tiêu hóa ở trẻ.",
                AverageRating = 4.8m,
                TotalRatings = 203,
                IsAvailable = true,
                IsActive = true
            },
            new Doctor
            {
                Name = "BS. Nguyễn Thị F",
                UserId = doctorUser.Id,
                SpecialtyId = specialties[5].Id,
                MedicalLicense = "GL006",
                Qualifications = "Thạc sĩ Sản phụ khoa - Đại học Y Phạm Ngọc Thạch",
                Experience = 9,
                ConsultationFee = 420000,
                Description = "Chuyên gia Sản phụ khoa với 9 năm kinh nghiệm. Khám thai, siêu âm, tư vấn sức khỏe sinh sản, điều trị các bệnh phụ khoa. Tận tâm với bệnh nhân.",
                AverageRating = 4.7m,
                TotalRatings = 78,
                IsAvailable = true,
                IsActive = true
            }
        );
        await context.SaveChangesAsync();
        Console.WriteLine("Doctors added successfully!");
    }

    // THÊM LỊCH HẸN MẪU
    if (!context.Appointments.Any())
    {
        Console.WriteLine("Adding sample appointments...");

        var doctors = context.Doctors.Take(3).ToList();
        var today = DateTime.Today;

        context.Appointments.AddRange(
            new Appointment
            {
                DoctorId = doctors[0].Id,
                PatientName = "Nguyễn Văn Khách",
                PatientEmail = "khach@email.com",
                PatientPhone = "0912345678",
                AppointmentDate = today.AddDays(1),
                AppointmentTime = "09:00",
                Symptoms = "Đau ngực, khó thở, hồi hộp",
                Status = "Chờ xác nhận",
                CreatedDate = DateTime.Now
            },
            new Appointment
            {
                DoctorId = doctors[1].Id,
                PatientName = "Trần Thị Bệnh",
                PatientEmail = "benh@email.com",
                PatientPhone = "0923456789",
                AppointmentDate = today.AddDays(2),
                AppointmentTime = "14:00",
                Symptoms = "Đau đầu, chóng mặt, buồn nôn",
                Status = "Đã xác nhận",
                CreatedDate = DateTime.Now.AddHours(-2)
            },
            new Appointment
            {
                DoctorId = doctors[2].Id,
                PatientName = "Lê Văn Bệnh",
                PatientEmail = "benhnhan@email.com",
                PatientPhone = "0934567890",
                AppointmentDate = today.AddDays(3),
                AppointmentTime = "10:00",
                Symptoms = "Đau bụng, khó tiêu, ợ chua",
                Status = "Chờ xác nhận",
                CreatedDate = DateTime.Now.AddHours(-1)
            },
            new Appointment
            {
                DoctorId = doctors[0].Id,
                PatientName = "Phạm Thị Hồng",
                PatientEmail = "hong@email.com",
                PatientPhone = "0945678901",
                AppointmentDate = today.AddDays(-1),
                AppointmentTime = "15:00",
                Symptoms = "Tăng huyết áp, mệt mỏi",
                Status = "Đã hoàn thành",
                CreatedDate = DateTime.Now.AddDays(-2)
            }
        );
        await context.SaveChangesAsync();
        Console.WriteLine("Appointments added successfully!");
    }

    // Tạo admin user
    var adminUser = await userManager.FindByEmailAsync("admin@medical.com");
    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = "admin@medical.com",
            Email = "admin@medical.com",
            EmailConfirmed = true,
            PhoneNumber = "0909123456"
        };

        var adminResult = await userManager.CreateAsync(adminUser, "Admin123!");
        if (adminResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
            Console.WriteLine("Admin user created successfully!");
        }
    }

    Console.WriteLine("Seed data completed successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"Seed data error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    // Không throw exception để ứng dụng vẫn chạy được
}

// Middleware xử lý lỗi
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/Home/Error";
        await next();
    }
});

app.Run();