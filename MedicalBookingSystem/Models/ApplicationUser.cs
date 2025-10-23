using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MedicalBookingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;  // THÊM = string.Empty

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Giới tính")]
        [StringLength(10)]
        public string? Gender { get; set; }  // THÊM ?

        [Display(Name = "Địa chỉ")]
        [StringLength(200)]
        public string? Address { get; set; }  // THÊM ?

        [Required]
        [Display(Name = "Vai trò")]
        [StringLength(20)]
        public string Role { get; set; } = "Patient";  // THÊM = "Patient"

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Ngày cập nhật")]
        public DateTime? LastModifiedDate { get; set; }

        // Navigation properties
        public virtual ICollection<Appointment> PatientAppointments { get; set; } = new List<Appointment>();
        public virtual Doctor? Doctor { get; set; }  // THÊM ?
    }
}