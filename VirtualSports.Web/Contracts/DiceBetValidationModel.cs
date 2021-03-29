using System.ComponentModel.DataAnnotations;

namespace VirtualSports.Web.Contracts
{
    /// <summary>
    /// Model for validation dice bet.
    /// </summary>
    public class DiceBetValidationModel
    {
        /// <summary>
        /// Date time when bet was made.
        /// </summary>
        [Required]
        [StringLength(19, MinimumLength =19)]
        public string DateTime { get; set; }

        /// <summary>
        /// Bet type.
        /// </summary>
        [Required]
        [Range(0,7)]
        public int BetType { get; set; }
    }
}
