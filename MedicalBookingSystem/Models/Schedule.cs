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

        // FIX NULLABLE: Thêm dấu ?
        [ForeignKey("DoctorId")]
        public virtual Doctor? Doctor { get; set; }
    }
}