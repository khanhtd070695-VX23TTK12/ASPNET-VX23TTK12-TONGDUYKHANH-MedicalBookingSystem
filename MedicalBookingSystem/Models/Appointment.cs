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

        // FIX NULLABLE: Thêm dấu ? để cho phép null
        [ForeignKey("DoctorId")]
        public virtual Doctor? Doctor { get; set; }
    }
}