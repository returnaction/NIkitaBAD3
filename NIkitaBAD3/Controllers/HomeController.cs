using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NIkitaBAD3.Data;
using NIkitaBAD3.Models;
using NIkitaBAD3.Models.Casino;
using System.Diagnostics;

namespace NIkitaBAD3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<PlayerGames> playerGamesBoard = await _context.PlayerGames.Include(u => u.User).ToListAsync();
            return View(playerGamesBoard);
        }

        [Authorize]
        public async Task<IActionResult> Employees()
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
