using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineStudyApplication.Data;
using OnlineStudyApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineStudyApplication.Controllers
{
    [Authorize]
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

        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName");
            return View();
        }

        // POST: ApplicationForms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationForm applicationForm)
        {
            // 🔍 Safety check – show validation errors if any
            if (!ModelState.IsValid)
            {
                ViewData["CourseId"] = new SelectList(
                    _context.Courses,
                    "Id",
                    "CourseName",
                    applicationForm.CourseId
                );

                return View(applicationForm);
            }

            // 🔐 SERVER-SIDE VALUES ONLY
            applicationForm.UserId = _userManager.GetUserId(User); // sets Identity user ID
            applicationForm.Status = "Pending"; // default status


            // 💾 Save
            _context.ApplicationForms.Add(applicationForm);
            await _context.SaveChangesAsync();

            // 🚀 Redirect so page does NOT reload
            return RedirectToAction("MyApplications", "Applications");
        }

        // GET: ApplicationForms
        public async Task<IActionResult> Index()
        {
            var applications = await _context.ApplicationForms
                .Include(a => a.Course)
                .ToListAsync();

            return View(applications);
        }


        // GET: ApplicationForms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationForm = await _context.ApplicationForms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationForm == null)
            {
                return NotFound();
            }

            return View(applicationForm);
        }

        

      




        // GET: ApplicationForms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationForm = await _context.ApplicationForms.FindAsync(id);
            if (applicationForm == null)
            {
                return NotFound();
            }
            return View(applicationForm);
        }

        // POST: ApplicationForms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,CourseId,FullName,Email,Education,Status")] ApplicationForm applicationForm)
        {
            if (id != applicationForm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationForm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationFormExists(applicationForm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationForm);
        }

        // GET: ApplicationForms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationForm = await _context.ApplicationForms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationForm == null)
            {
                return NotFound();
            }

            return View(applicationForm);
        }

        // POST: ApplicationForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicationForm = await _context.ApplicationForms.FindAsync(id);
            if (applicationForm != null)
            {
                _context.ApplicationForms.Remove(applicationForm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationFormExists(int id)
        {
            return _context.ApplicationForms.Any(e => e.Id == id);
        }
    }
}
