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
    public class HornController : Controller
    {
        private const EGames currentGame = EGames.HornBet;

        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public HornController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Play()
        {
            PropBet hornBet = GenerateNewHornBet();
            return View(hornBet);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Play(PropBet propBet)
        {
            // check latter if there is null in here
            User? user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is null)
                return NotFound();

            PlayerGames? hornGame = await _context.PlayerGames.FirstOrDefaultAsync(g => g.UserId == user!.Id && g.GameType == currentGame);

            // if game is null it means we need to create a new game with empty count
            if (hornGame is null)
            {
                hornGame = new PlayerGames { GameType = currentGame, UserId = user!.Id, TempBestResult = 0, LongestCorrectAsnwerStreak = 0, TotalAnswers = 0 };

                await _context.PlayerGames.AddAsync(hornGame);
                await _context.SaveChangesAsync();
            }
            // if not null we are going to user this game

            // if answer is correct 
            if (propBet.Answer == CalculateCorrectAnswer(propBet.Bet, propBet.RolledNumber))
            {
                //if(hornGame.BestResult)
                hornGame.TempBestResult++;
                hornGame.TotalAnswers++;
                if (hornGame.TempBestResult > hornGame.LongestCorrectAsnwerStreak)
                {
                    hornGame.LongestCorrectAsnwerStreak = hornGame.TempBestResult;
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Play));
            }
            // if answer is wrong
            else
            {
                propBet.ErrorMessage = "Wrong Payout!";
                hornGame.TempBestResult = 0;
                hornGame.TotalAnswers++;
                await _context.SaveChangesAsync();

                return View(propBet);
            }

        }

        private PropBet GenerateNewHornBet()
        {
            PropBet hornBet = new();
            hornBet.Bet = GeneratateRandomBet();
            hornBet.RolledNumber = RollDice();

            return hornBet;
        }

        private int CalculateCorrectAnswer(int bet, int rolledNumber)
        {
            if (rolledNumber == 3 || rolledNumber == 11)
            {
                return bet * 3;
            }
            else
            {
                return (bet * 7) - (bet / 4);
            }
        }


        private int RollDice()
        {
            Random radnom = new Random();
            int[] outcome = { 2, 3, 11, 12 };
            return outcome[radnom.Next(0, outcome.Length)];
        }

        private int GeneratateRandomBet()
        {
            Random random = new Random();

            return random.Next(1, 51) * 4;
        }
    }
}
