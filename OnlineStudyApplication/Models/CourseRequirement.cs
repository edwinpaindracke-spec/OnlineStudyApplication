namespace OnlineStudyApplication.Models
{
    public class CourseRequirement
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string SubjectName { get; set; }
        public int MinimumMark { get; set; }


    }
}
