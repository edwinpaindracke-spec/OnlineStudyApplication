using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OnlineStudyApplication.Models
{
    public class EligibilityViewModel
    {
        public int CourseId { get; set; }

        [Required]
        public double AverageMark { get; set; }

        public bool HasMath { get; set; }

        public double? MathMark { get; set; }

        [Required]
        public IFormFile CertificateFile { get; set; }

        public bool IsEligible { get; set; }
        public string Message { get; set; }
    }
}
