using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NIkitaBAD3.Data;
using NIkitaBAD3.Models;
using NIkitaBAD3.Models.Casino;
using NIkitaBAD3.Models.Casino.Enums;
using NIkitaBAD3.Models.Casino.PropBets;

namespace NIkitaBAD3.Controllers
{
    public class HornHighController : Controller
    {
        private const EGames currentGame = EGames.HornHighBet;

        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public HornHighController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Play()
        {
            PropBet hornHigh = GenerateNewHornHighBet();
            return View(hornHigh);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Play(PropBet hornHigh)
        {
            User? user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is null)
                return NotFound();

            PlayerGames? hornHighGame = await _context.PlayerGames.FirstOrDefaultAsync(g => g.UserId == user!.Id && g.GameType == currentGame);

            if (hornHighGame is null)
            {
                hornHighGame = new PlayerGames { GameType = currentGame, UserId = user!.Id, TempBestResult = 0, LongestCorrectAsnwerStreak = 0, TotalAnswers = 0 };
                await _context.PlayerGames.AddAsync(hornHighGame);
                await _context.SaveChangesAsync();
            }

            if (hornHigh.Answer == CalculateCorrectAnswer(hornHigh.Bet, hornHigh.PlacedBet, hornHigh.RolledNumber))
            {
                hornHighGame.TempBestResult++;
                hornHighGame.TotalAnswers++;
                if (hornHighGame.TempBestResult > hornHighGame.LongestCorrectAsnwerStreak)
                {
                    hornHighGame.LongestCorrectAsnwerStreak = hornHighGame.TempBestResult;
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Play));
            }
            else
            {
                hornHigh.ErrorMessage = "Wrong Payout!";
                hornHighGame.TempBestResult = 0;
                hornHighGame.TotalAnswers++;
                await _context.SaveChangesAsync();

                return View(hornHigh);
            }
        }

        private PropBet GenerateNewHornHighBet()
        {
            PropBet hornHigh = new();
            hornHigh.Bet = GenerateRandomBet();
            hornHigh.RolledNumber = RollDice();
            hornHigh.PlacedBet = GenerateRandomPlace();

            return hornHigh;
        }

        private int CalculateCorrectAnswer(int bet, int? placedBet, int rolledNumber)
        {
            int result = 0;

            switch (placedBet)
            {
                case 2:
                    switch (rolledNumber)
                    {
                        case 2:
                            result = (bet * 11) + bet / 5 * 2;
                            break;
                        case 12:
                            result = (bet * 5) + bet / 5;
                            break;
                        case 3:
                        case 11:
                            result = (bet * 2) + bet / 5;
                            break;
                    }
                    break;
                case 12:
                    switch (rolledNumber)
                    {
                        case 2:
                            result = (bet * 5) + bet / 5;
                            break;
                        case 12:
                            result = (bet * 11) + bet / 5 * 2;
                            break;
                        case 3:
                        case 11:
                            result = (bet * 2) + bet / 5;
                            break;
                    }
                    break;
                case 3:
                    switch (rolledNumber)
                    {
                        case 2:
                        case 12:
                            result = (bet * 5) + bet / 5;
                            break;
                        case 3:
                            result = (bet * 5) + bet / 5 * 2;
                            break;
                        case 11:
                            result = (bet * 2) + bet / 5;
                            break;
                    }
                    break;
                case 11:
                    switch (rolledNumber)
                    {
                        case 2:
                        case 12:
                            result = (bet * 5) + bet / 5;
                            break;
                        case 3:
                            result = (bet * 2) + bet / 5;
                            break;
                        case 11:
                            result = (bet * 5) + bet / 5 * 2;
                            break;
                    }
                    break;
            }

            return result;
        }

        private int RollDice()
        {
            Random random = new Random();
            int[] outcome = { 2, 3, 11, 12 };
            return outcome[random.Next(0, outcome.Length)];
        }

        private int GenerateRandomBet()
        {
            Random random = new();
            return random.Next(1, 41) * 5;
        }

        private int GenerateRandomPlace()
        {
            Random random = new Random();
            int[] betPlaced = { 2, 3, 11, 12 };
            return betPlaced[random.Next(0, betPlaced.Length)];
        }
    }
}
