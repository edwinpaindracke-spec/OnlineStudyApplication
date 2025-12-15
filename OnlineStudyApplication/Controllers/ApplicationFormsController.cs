using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStudyApplication.Data;
using OnlineStudyApplication.Models;

namespace OnlineStudyApplication.Controllers
{
    [Authorize] // must be logged in to apply
    public class ApplicationFormsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ApplicationFormsController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================
        // MY APPLICATIONS
        // =========================
        public async Task<IActionResult> MyApplications()
        {
            var userId = _userManager.GetUserId(User);

            var applications = await _context.ApplicationForms
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return View(applications);
        }

        // =========================
        // CREATE (APPLY)
        // =========================

        // GET: ApplicationForms/Create?courseId=1
        public IActionResult Create(int courseId)
        {
            var application = new ApplicationForm
            {
                CourseId = courseId
            };

            return View(application);
        }

        // POST: ApplicationForms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationForm applicationForm)
        {
            if (!ModelState.IsValid)
            {
                return View(applicationForm);
            }

            applicationForm.UserId = _userManager.GetUserId(User);
            applicationForm.Status = "Pending";

            _context.ApplicationForms.Add(applicationForm);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyApplications));
        }

        // =========================
        // DETAILS (OPTIONAL)
        // =========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var applicationForm = await _context.ApplicationForms.FindAsync(id);
            if (applicationForm == null) return NotFound();

            return View(applicationForm);
        }
    }
}
