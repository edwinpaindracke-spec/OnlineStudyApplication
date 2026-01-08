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
            // Load course properly
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == model.CourseId);

            if (course == null)
            {
                model.IsEligible = false;
                model.Message = "Course not found.";
                return View(model);
            }

            // Defensive validation
            if (!ModelState.IsValid)
            {
                model.IsEligible = false;
                model.Message = "Please enter all required marks correctly.";
                return View(model);
            }

            // Default = eligible (prove otherwise)
            model.IsEligible = true;

            // ✅ Average mark check
            if (model.AverageMark < course.MinimumAverage)
            {
                model.IsEligible = false;
                model.Message =
                    $"Minimum average required is {course.MinimumAverage}%.";
                return View(model);
            }

            // ✅ Mathematics requirement check
            if (course.RequiresMath)
            {
                if (!model.HasMath)
                {
                    model.IsEligible = false;
                    model.Message = "Mathematics is required for this course.";
                    return View(model);
                }

                if (model.MathMark < course.MinimumMathMark)
                {
                    model.IsEligible = false;
                    model.Message =
                        $"Mathematics requires at least {course.MinimumMathMark}%.";
                    return View(model);
                }
            }

            // ✅ PASSED ALL CHECKS
            model.Message = "You meet all the requirements for this course.";

            // 🔐 Store eligibility for Apply lock
            TempData["IsEligible"] = true;
            TempData["EligibleCourseId"] = model.CourseId;

            return View(model);
        }


       
        }

    }


