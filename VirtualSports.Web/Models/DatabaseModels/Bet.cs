using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualSports.BE.Models.DatabaseModels
{
    public class Bet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public BetType BetType { get; set; }
        public int DroppedNumber { get; set; }
        public bool IsBetWon { get; set; }

        //Field name DateTime was asked by Mobile
        public string DateTime { get; set; }
    }

    public enum BetType
    {
        NUMBER1,
        NUMBER3,
        NUMBER4,
        NUMBER5,
        NUMBER6,
        EVEN,
        ODD
    }
}
