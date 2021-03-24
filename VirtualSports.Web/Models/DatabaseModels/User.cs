using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualSports.Web.Models.DatabaseModels
{
    [Table("Users")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        [Column("FavouriteGameIds", TypeName = "jsonb")]
        public Dictionary<PlatformType, List<string>> FavouriteGameIds { get; set; }
        [Column("RecentGameIds", TypeName = "jsonb")]
        public Dictionary<PlatformType, Queue<string>> RecentGameIds { get; set; }
        [Column("Bets", TypeName = "jsonb")]
        public Dictionary<PlatformType, List<Bet>> Bets { get; set; }
    }
}
