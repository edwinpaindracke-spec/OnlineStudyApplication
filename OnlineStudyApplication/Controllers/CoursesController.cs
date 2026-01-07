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
            var course = await _context.Courses.FindAsync(model.CourseId);
            if (course == null) return NotFound();

            if (model.AverageMark < course.MinimumAverage)
            {
                model.IsEligible = false;
                model.Message = "Your average mark does not meet the minimum requirement.";
                return View(model);
            }

            if (course.RequiresMath)
            {
                if (!model.HasMath || model.MathMark < course.MinimumMathMark)
                {
                    model.IsEligible = false;
                    model.Message = "This course requires Mathematics with sufficient marks.";
                    return View(model);
                }
            }

            model.IsEligible = true;
            model.Message = "You meet the requirements for this course!";
            return View(model);
        }

    }
}

