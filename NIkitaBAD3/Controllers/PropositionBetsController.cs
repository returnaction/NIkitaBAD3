using Microsoft.AspNetCore.Mvc;
using NIkitaBAD3.Data;
using NIkitaBAD3.Models.Casino.PropBets;

namespace NIkitaBAD3.Controllers
{
    public class PropositionBetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PropositionBetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Play()
        {
            List<PropositionBet> propBets = GeneratePropositionBets();

            return View(propBets);
        }



        // Generate Random dice Roll
        private int RollDice()
        {
            Random random = new();
            int[] outcome = { 2, 3, 11, 12 };
            return outcome[random.Next(0, outcome.Length)];
        }

        // Generate how many bets will be generated, so far we have only 2 bets so I will generate two but, in the future it should vary from 2 to 5
        private List<PropositionBet> GeneratePropositionBets()
        {
            List<PropositionBet> propositionBets = new List<PropositionBet>();
            int rollNumber = RollDice();

            PropositionBet worldBet = new();
            worldBet.Bet = GenerateBetForWorldBet(null, null, null);
            worldBet.RolledNumber = rollNumber;
            worldBet.CorrectAnswer = CalculateCorrectAnswerForWorldBet(worldBet.Bet, rollNumber);

            PropositionBet hornHighBet = new();
            hornHighBet.Bet = GenerateBetForHornHighBet(null, null, null);
            hornHighBet.PlacedOnBet = GenerateBetForHornHighBetPlacedOn();
            hornHighBet.RolledNumber = rollNumber;
            hornHighBet.CorrectAnswer = CalculateCorrectAnswerForHornHighBet(worldBet.Bet, hornHighBet.PlacedOnBet, rollNumber);


            propositionBets.Add(worldBet);
            propositionBets.Add(hornHighBet);

            return propositionBets;
        }

        // 1.WorldBet
        // 2.HornHighBet
        //_______
        // 3.HornBet a bit tricky because it increment of 4


        //1.WORLD BET LOGIC
        private int GenerateBetForWorldBet(int? min, int? max, int? increment)
        {
            Random random = new();
            int minValue = min ?? 5;
            int maxValue = max ?? 200;
            int incValue = (increment.HasValue && (increment == 5 || increment == 10 || increment == 25 || increment == 100)) ? increment.Value : 5; // If increment is null or not in the allowed values, set it to 5

            return random.Next(minValue, maxValue / incValue + 1) * incValue;
        }

        private int CalculateCorrectAnswerForWorldBet(int bet, int rolledNumber)
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

        //2.HORNHIGH BET LOGIC
        private int GenerateBetForHornHighBet(int? min, int? max, int? increment)
        {
            Random random = new();
            int minValue = min ?? 5;
            int maxValue = max ?? 200;
            int incValue = (increment.HasValue && (increment == 5 || increment == 10 || increment == 25 || increment == 100)) ? increment.Value : 5; // If increment is null or not in the allowed values, set it to 5

            return random.Next(minValue, maxValue / incValue + 1) * incValue;
        }

        // extension method for hornhigh bet.
        private int GenerateBetForHornHighBetPlacedOn()
        {
            Random random = new Random();
            int[] betPlaced = { 2, 3, 11, 12 };
            return betPlaced[random.Next(0, betPlaced.Length)];
        }

        // calculate correct answer for hornHighBet
        private int CalculateCorrectAnswerForHornHighBet(int bet, int? placedBet, int rolledNumber)
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
    }
}
