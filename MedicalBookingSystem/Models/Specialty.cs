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

        // Navigation property - FIX NULLABLE
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}