using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualSports.Lib.Models
{
    public class Bet
    {
        public string Id { get; set; }
        public BetType BetType { get; set; }
        public int DroppedNumber { get; set; }
        public bool IsBetWon { get; set; }
        public string DateTime { get; set; }
    }
}
