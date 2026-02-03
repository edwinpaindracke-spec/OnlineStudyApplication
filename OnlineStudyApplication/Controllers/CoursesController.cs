using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineStudyApplication.Data;
using OnlineStudyApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStudyApplication.Controllers
{
    [AllowAnonymous]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // ADMIN ONLY
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id) => View();

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id) => View();

        public IActionResult CheckEligibility(int id)
        {
            var model = new EligibilityViewModel
            {
                CourseId = id
            };

            return View(model);
        }

   [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> CheckEligibility(EligibilityViewModel model)
{
    var course = await _context.Courses
        .FirstOrDefaultAsync(c => c.Id == model.CourseId);

    if (course == null)
    {
        model.IsEligible = false;
        model.Message = "Course not found.";
        return View(model);
    }

    // ❗ Default: NOT eligible
    model.IsEligible = false;

    // 🔒 Validate certificate upload
    if (model.CertificateFile == null || model.CertificateFile.Length == 0)
    {
        model.Message = "Please upload your certificate.";
        return View(model);
    }

    // ✅ Average mark check
    if (model.AverageMark < course.MinimumAverage)
    {
        model.Message =
            $"You do not qualify. Minimum average required is {course.MinimumAverage}%.";
        return View(model);
    }

    // ✅ Mathematics requirement
    if (course.RequiresMath)
    {
        if (!model.HasMath)
        {
            model.Message =
                "You do not qualify. Mathematics is required for this course.";
            return View(model);
        }

        if (model.MathMark == null || model.MathMark < course.MinimumMathMark)
        {
            model.Message =
                $"You do not qualify. Minimum Mathematics mark required is {course.MinimumMathMark}%.";
            return View(model);
        }
    }

    // ✅ PASSED ALL CHECKS
    model.IsEligible = true;
    model.Message = "You meet the requirements for this course!";

    // 🔐 Store eligibility for Apply lock
    TempData["IsEligible"] = true;
    TempData["EligibleCourseId"] = model.CourseId;

    return View(model);
}


    }
}

