using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineStudyApplication.Models
{
    public class ApplicationForm
    {
        public int Id { get; set; }

        // ✅ USER SELECTS COURSE VIA CourseId
        [Required]
        public int CourseId { get; set; }


        
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        public string Education { get; set; }

    }
}
