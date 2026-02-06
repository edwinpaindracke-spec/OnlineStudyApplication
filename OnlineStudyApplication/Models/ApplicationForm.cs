using System.ComponentModel.DataAnnotations;

namespace OnlineStudyApplication.Models
{
    public class ApplicationForm
    {
        public int Id { get; set; }

        // 🔐 Server-side only (must be nullable)
        public string? UserId { get; set; }

        // ✅ Selected by user
        [Required(ErrorMessage = "Please select a course")]
        public int CourseId { get; set; }

        // 🧭 Navigation property (never required)
        public Course? Course { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Education { get; set; }

        // 🔐 Server-side only
        public string? Status { get; set; }
    }
}
