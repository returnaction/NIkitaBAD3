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
    public class ComeBetController : Controller
    {
        private const EGames currentGame = EGames.ComeBet;

        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ComeBetController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Play()
        {
            ComeBet comeBet = GenerateComeBet();
            return View(comeBet);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Play(ComeBet comeBet)
        {
            User? user = await _userManager.GetUserAsync(HttpContext.User);

            if (user is null)
                return NotFound();
            
            PlayerGames? comeBetGame = await _context.PlayerGames.FirstOrDefaultAsync(g => g.UserId == user!.Id && g.GameType == currentGame);

            // if game is null it means we need to create a new game with empty count
            if (comeBetGame is null)
            {
                comeBetGame = new PlayerGames { GameType = currentGame, UserId = user!.Id, TempBestResult = 0, LongestCorrectAsnwerStreak = 0, TotalAnswers = 0 };

                await _context.PlayerGames.AddAsync(comeBetGame);
                await _context.SaveChangesAsync();
            }

            switch (comeBet.RolledNumber)
            {
                case 4:
                case 10:
                    if (comeBet.Answer == CaluclateCorrectAnswerFor4or10(comeBet.FlatBet, comeBet.Odds))
                    {
                        comeBetGame.TempBestResult++;
                        comeBetGame.TotalAnswers++;
                        if (comeBetGame.TempBestResult > comeBetGame.LongestCorrectAsnwerStreak)
                        {
                            comeBetGame.LongestCorrectAsnwerStreak = comeBetGame.TempBestResult;
                            await _context.SaveChangesAsync();
                        }

                        return RedirectToAction(nameof(Play));
                    }
                    else
                    {
                        comeBet.ErrorMessage = "Wrong Payout!";

                        comeBetGame.TempBestResult = 0;
                        comeBetGame.TotalAnswers++;
                        await _context.SaveChangesAsync();
                    }
                    break;

                case 5:
                case 9:
                    if (comeBet.Answer == CaluclateCorrectAnswerFor5or9(comeBet.FlatBet, comeBet.Odds))
                    {
                        comeBetGame.TempBestResult++;
                        comeBetGame.TotalAnswers++;
                        if (comeBetGame.TempBestResult > comeBetGame.LongestCorrectAsnwerStreak)
                        {
                            comeBetGame.LongestCorrectAsnwerStreak = comeBetGame.TempBestResult;
                            await _context.SaveChangesAsync();
                        }

                        return RedirectToAction(nameof(Play));
                    }
                    else
                    {
                        comeBet.ErrorMessage = "Wrong Payout!";

                        comeBetGame.TempBestResult = 0;
                        comeBetGame.TotalAnswers++;
                        await _context.SaveChangesAsync();
                    }
                    break;

                case 6:
                case 8:
                    if (comeBet.Answer == CaluclateCorrectAnswerFor6or8(comeBet.FlatBet, comeBet.Odds))
                    {
                        comeBetGame.TempBestResult++;
                        comeBetGame.TotalAnswers++;
                        if (comeBetGame.TempBestResult > comeBetGame.LongestCorrectAsnwerStreak)
                        {
                            comeBetGame.LongestCorrectAsnwerStreak = comeBetGame.TempBestResult;
                            await _context.SaveChangesAsync();
                        }

                        return RedirectToAction(nameof(Play));
                    }
                    else
                    {
                        comeBet.ErrorMessage = "Wrong Payout!";

                        comeBetGame.TempBestResult = 0;
                        comeBetGame.TotalAnswers++;
                        await _context.SaveChangesAsync();
                    }
                    break;
            }

            return View(comeBet);
        }

        private ComeBet GenerateComeBet()
        {
            int[] bet = new int[2];
            ComeBet comeBet = new();
            comeBet.RolledNumber = RollDice();

            switch (comeBet.RolledNumber)
            {

                case 4:
                case 10:
                    bet = GenerateRandomBetFor4or10();
                    comeBet.FlatBet = bet[0];
                    comeBet.Odds = bet[1];
                    break;
                case 5:
                case 9:
                    bet = GenerateRandomBetFor5or9();
                    comeBet.FlatBet = bet[0];
                    comeBet.Odds = bet[1];
                    break;
                case 6:
                case 8:
                    bet = GenerateRandomBetFor6or8();
                    comeBet.FlatBet = bet[0];
                    comeBet.Odds = bet[1];
                    break;

            }

            return comeBet;
        }

        private int RollDice()
        {
            Random random = new();
            int[] outcome = { 4, 5, 6, 8, 9, 10 };
            return outcome[random.Next(0, outcome.Length)];
        }

        // The array will have 2 numbers: 1st for flatBet and 2nd for Odds
        private int[] GenerateRandomBetFor4or10()
        {
            Random random = new Random();

            int flatBet = random.Next(1, 100) * 5;
            int odds = random.Next(1, (flatBet * 3 / 5) + 1) * 5;

            return [flatBet, odds];
        }

        private int CaluclateCorrectAnswerFor4or10(int flatBet, int odds)
        {
            return odds * 2 + flatBet;
        }

        private int[] GenerateRandomBetFor5or9()
        {
            Random random = new Random();

            int flatBet = random.Next(1, 100) * 5;
            int odds = random.Next(1, (flatBet * 4 / 10) + 1) * 10;

            return [flatBet, odds];
        }

        private int CaluclateCorrectAnswerFor5or9(int flatBet, int odds)
        {
            return odds + (odds / 2) + flatBet;
        }

        private int[] GenerateRandomBetFor6or8()
        {
            Random random = new Random();
            int flatBet = random.Next(1, 100) * 5;
            int odds = random.Next(1, (flatBet * 5 / 5) + 1) * 5;

            return [flatBet, odds];
        }

        private int CaluclateCorrectAnswerFor6or8(int flatBet, int odds)
        {
            return (odds + (odds / 5)) + flatBet;
        }
    }
}
