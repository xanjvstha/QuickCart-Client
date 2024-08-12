using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpressEats.Models;
using System.Linq;
using System.Threading.Tasks;
using ExpressEats.Data;
using Microsoft.AspNetCore.Identity;

namespace ExpressEats.Controllers
{
    public class PublicMenuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PublicMenuController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: PublicMenus
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.Restaurant).ToListAsync();
            return View(products); // Renders the Index view with a list of products
        }

        // GET: PublicProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Restaurant)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: PublicProducts/Buy/5
        [Authorize]
        public async Task<IActionResult> Buy(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Details", new { id });
            }
        }
    }
}
