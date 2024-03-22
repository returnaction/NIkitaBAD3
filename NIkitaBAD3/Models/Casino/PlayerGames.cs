using Microsoft.AspNetCore.Identity;
using NIkitaBAD3.Models.Casino.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NIkitaBAD3.Models.Casino
{
    public class PlayerGames
    {
        [Key]
        public int Id { get; set; }

        public EGames GameType { get; set; }

        public int LongestCorrectAsnwerStreak { get; set; }

        public int TempBestResult { get; set; } = 0;

        public int TotalAnswers { get; set; }

        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
