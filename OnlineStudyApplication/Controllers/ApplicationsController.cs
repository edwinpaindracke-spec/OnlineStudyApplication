using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStudyApplication.Data;
using OnlineStudyApplication.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStudyApplication.Controllers
{
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ApplicationsController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Applications/MyApplications
        public async Task<IActionResult> MyApplications()
        {
            var userId = _userManager.GetUserId(User);

            var applications = await _context.ApplicationForms
                .Include(a => a.Course)
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return View(applications);
        }
    }
}
