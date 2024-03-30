namespace NIkitaBAD3.Models.Casino.PropBets
{
    public class PropositionBet
    {
        public int Bet { get; set; }

        // For example when we place the bet on HornHigh it tells us where is the bet is placed 2,3,11,12 for other bets we don't need this.
        public int? PlacedOnBet { get; set; }

        public int Answer { get; set; }
        public int CorrectAnswer { get; set; }

        public int RolledNumber { get; set; }

        public string ErrorMessage { get; set; } = null!;
    }
}
