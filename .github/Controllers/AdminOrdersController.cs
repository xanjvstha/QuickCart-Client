using Microsoft.AspNetCore.Mvc;
using ExpressEats.Models;
using ExpressEats.Data;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExpressEats.Controllers
{
    public class AdminOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            return View(orders);
        }


        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            var orderToUpdate = await _context.Orders.FindAsync(id);
            if (orderToUpdate == null)
            {
                return NotFound();
            }

            orderToUpdate.Status = order.Status;
            orderToUpdate.DeliveryDate = order.DeliveryDate;
            orderToUpdate.DeliveryAddress = order.DeliveryAddress;

            _context.Orders.Update(orderToUpdate);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
