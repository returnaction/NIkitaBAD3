using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NIkitaBAD3.Data;
using NIkitaBAD3.Models;
using NIkitaBAD3.Models.Casino;
using NIkitaBAD3.Models.Casino.Enums;
using NIkitaBAD3.Models.Casino.LineBets;

namespace NIkitaBAD3.Controllers
{
    public class PlaceBetController : Controller
    {
        private const EGames currentGame = EGames.PlaceBet;

        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public PlaceBetController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public IActionResult Play()
        {
            PlaceBet placeBet = GeneratePlaceBet();
            return View(placeBet);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Play(PlaceBet placeBet)
        {
            User? user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is null)
                return NotFound();
            
            PlayerGames? placeBetGame = await _context.PlayerGames.FirstOrDefaultAsync(g => g.UserId == user!.Id && g.GameType == currentGame);

            // if game is null it means we need to create a new game with empty count
            if (placeBetGame is null)
            {
                placeBetGame = new PlayerGames { GameType = currentGame, UserId = user!.Id, TempBestResult = 0, LongestCorrectAsnwerStreak = 0, TotalAnswers = 0 };

                await _context.PlayerGames.AddAsync(placeBetGame);
                await _context.SaveChangesAsync();
            }

            switch (placeBet.RolledNumber)
            {
                case 5:
                case 9:
                    if (placeBet.Answer == CaluclateCorrectAnswerFor5or9(placeBet.Bet))
                    {
                        placeBetGame.TempBestResult++;
                        placeBetGame.TotalAnswers++;
                        if (placeBetGame.TempBestResult > placeBetGame.LongestCorrectAsnwerStreak)
                        {
                            placeBetGame.LongestCorrectAsnwerStreak = placeBetGame.TempBestResult;
                            await _context.SaveChangesAsync();
                        }

                        return RedirectToAction(nameof(Play));
                    }
                    else
                    {
                        placeBet.ErrorMessage = "Wrong Payout!";

                        placeBetGame.TempBestResult = 0;
                        placeBetGame.TotalAnswers++;

                        await _context.SaveChangesAsync();
                    }
                    break;
                case 6:
                case 8:
                    if (placeBet.Answer == CaluclateCorrectAnswerFor6or8(placeBet.Bet))
                    {
                        placeBetGame.TempBestResult++;
                        placeBetGame.TotalAnswers++;
                        if (placeBetGame.TempBestResult > placeBetGame.LongestCorrectAsnwerStreak)
                        {
                            placeBetGame.LongestCorrectAsnwerStreak = placeBetGame.TempBestResult;
                            await _context.SaveChangesAsync();
                        }

                        return RedirectToAction(nameof(Play));
                    }
                    else
                    {
                        placeBet.ErrorMessage = "Wrong Payout!";

                        placeBetGame.TempBestResult = 0;
                        placeBetGame.TotalAnswers++;

                        await _context.SaveChangesAsync();
                    }
                    break;
            }

            return View(placeBet);
        }

        private PlaceBet GeneratePlaceBet()
        {
            PlaceBet placeBet = new();
            placeBet.RolledNumber = RollDice();

            switch (placeBet.RolledNumber)
            {
                case 5:
                case 9:
                    placeBet.Bet = GenerateRandomBetFor4or10or5or9();
                    break;
                case 6:
                case 8:
                    placeBet.Bet = GenerateRandomBetFor6or8();
                    break;
            }

            return placeBet;
        }

        private int GenerateRandomBetFor6or8()
        {
            Random random = new();
            int bet = random.Next(1, 101) * 6;
            return bet;
        }

        private int CaluclateCorrectAnswerFor6or8(int bet)
        {
            return bet + (bet / 6);
        }

        private int GenerateRandomBetFor4or10or5or9()
        {
            Random random = new();
            int bet = random.Next(1, 101) * 5;
            return bet;
        }

        private int CaluclateCorrectAnswerFor5or9(int bet)
        {
            return bet / 5 * 7;
        }

        //TODO: did not implented yet
        private int CaluclateCorrectAnswerFor4or10(int bet)
        {
            if (bet <= 15)
                return bet / 5 * 9;
            else
                return bet * bet;
        }


        //TODO: Add 4 and 10
        private int RollDice()
        {
            Random random = new();
            int[] outcome = { 6, 8 };
            return outcome[random.Next(0, outcome.Length)];
        }
    }
}
