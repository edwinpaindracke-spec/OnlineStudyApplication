using System.ComponentModel.DataAnnotations;

namespace OnlineStudyApplication.Models
{
    public class ApplicationForm
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        public string Education { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
