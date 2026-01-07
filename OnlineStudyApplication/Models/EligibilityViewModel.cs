namespace OnlineStudyApplication.Models
{
    public class EligibilityViewModel
    {
        public int CourseId { get; set; }

        public int AverageMark { get; set; }
        public bool HasMath { get; set; }
        public int MathMark { get; set; }

        public bool IsEligible { get; set; }
        public string Message { get; set; }
    }
}
