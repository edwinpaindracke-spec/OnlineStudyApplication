using System.ComponentModel.DataAnnotations;

namespace OnlineStudyApplication.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        public string CourseName { get; set; }

        public string Description { get; set; }

        public string Duration { get; set; }
        public int MinimumAverage { get; set; }
        public bool RequiresMath { get; set; }
        public int MinimumMathMark { get; set; }

    }
}
