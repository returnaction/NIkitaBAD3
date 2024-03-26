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
    public class WorldController : Controller
    {
        private const EGames currentGame = EGames.WorldBet;

        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        

        public WorldController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Play()
        {
            int? minBet = null;
            int? maxBet = null;
            int? incrementBet = null;

            if (TempData.ContainsKey("minBet"))
            {
                if (TempData["minBet"] is int minBetValue)
                    minBet = minBetValue;
            }

            if (TempData.ContainsKey("maxBet"))
            {
                if (TempData["maxBet"] is int maxBetValue)
                    maxBet = maxBetValue;
            }

            if (TempData.ContainsKey("incrementBet"))
            {
                if (TempData["incrementBet"] is int incrementBetValue)
                    incrementBet = incrementBetValue;
            }

            PropBet worldBet = GenerateWorlBet(minBet, maxBet, incrementBet);
            
            return View(worldBet);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Play(PropBet propBet, int minBet, int maxBet, int incrementBet)
        {
            TempData["minBet"] = minBet;
            TempData["maxBet"] = maxBet;
            TempData["incrementBet"] = incrementBet;


            User? user = await _userManager.GetUserAsync(HttpContext.User);

            PlayerGames? worldBetGame = await _context.PlayerGames.FirstOrDefaultAsync(g => g.UserId == user!.Id && g.GameType == currentGame);

            if (worldBetGame is null)
            {
                worldBetGame = new PlayerGames { GameType = currentGame, UserId = user!.Id, TempBestResult = 0, LongestCorrectAsnwerStreak = 0, TotalAnswers = 0 };

                await _context.PlayerGames.AddAsync(worldBetGame);
                await _context.SaveChangesAsync();
            }

            if (propBet.Answer == CalculateCorrectAnswer(propBet.Bet, propBet.RolledNumber))
            {
                worldBetGame.TempBestResult++;
                worldBetGame.TotalAnswers++;
                if (worldBetGame.TempBestResult > worldBetGame.LongestCorrectAsnwerStreak)
                {
                    worldBetGame.LongestCorrectAsnwerStreak = worldBetGame.TempBestResult;
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Play));
            }
            else
            {
                propBet.ErrorMessage = "Wrong Payout!";
                worldBetGame.TempBestResult = 0;
                worldBetGame.TotalAnswers++;
                await _context.SaveChangesAsync();
                return View(propBet);
            }
        }

        private int CalculateCorrectAnswer(int bet, int rolledNumber)
        {
            int result = 0;

            switch (rolledNumber)
            {
                case 3:
                case 11:
                    result = (bet * 2) + bet / 5;
                    break;

                case 2:
                case 12:
                    result = (bet * 5) + bet / 5;
                    break;
                case 7:
                    result = 0;
                    break;

            }

            return result;
        }

        private PropBet GenerateWorlBet(int? min, int? max, int? increment)
        {
            PropBet worldBet = new();
            worldBet.Bet = GenerateRandomBet(min, max, increment);
            worldBet.RolledNumber = RollDice();

            return worldBet;
        }

        private int RollDice()
        {
            Random random = new Random();
            int[] outcome = { 2, 3, 11, 12, 7 };
            return outcome[random.Next(0, outcome.Length)];
        }

        private int GenerateRandomBet(int? min, int? max, int? increment)
        {
            Random random = new();
            int minValue = min ?? 5;
            int maxValue = max ?? 200;
            int incValue = (increment.HasValue && (increment == 5 || increment == 10 || increment == 25 || increment == 100)) ? increment.Value : 5; // If increment is null or not in the allowed values, set it to 5


            return random.Next(minValue, maxValue / incValue + 1) * incValue;
        }
    }
}
