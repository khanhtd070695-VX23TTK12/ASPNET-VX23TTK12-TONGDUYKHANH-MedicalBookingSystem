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

        // Navigation properties - FIX NULLABLE
        [ForeignKey("SpecialtyId")]
        public virtual Specialty? Specialty { get; set; }
    }
}