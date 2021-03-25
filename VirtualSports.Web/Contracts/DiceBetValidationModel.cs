using System.ComponentModel.DataAnnotations;
using VirtualSports.Web.Models.DatabaseModels;

namespace VirtualSports.Web.Contracts
{
    public class DiceBetValidationModel
    {
        [Required]
        [StringLength(19, MinimumLength =19)]
        public string DateTime { get; set; }

        [Required]
        [Range(0,7)]
        public BetType BetType { get; set; }
    }
}
