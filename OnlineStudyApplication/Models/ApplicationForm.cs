using System.ComponentModel.DataAnnotations;

namespace OnlineStudyApplication.Models
{
    public class ApplicationForm
    {
        public int Id { get; set; }

        
        public string UserId { get; set; }

        // ✅ Selected from dropdown
        [Required(ErrorMessage = "Please select a course")]
        public int CourseId { get; set; }

        // ✅ ADD THIS
        public Course Course { get; set; }


        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        public string Education { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
