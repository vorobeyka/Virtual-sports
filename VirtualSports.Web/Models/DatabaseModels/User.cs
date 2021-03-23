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
        public List<string> FavouriteGameIds { get; set; }
        public List<string> FavouriteGameMobileIds { get; set; }
        [Column("RecentGameIds", TypeName = "jsonb")]
        public Queue<string> RecentGameIds { get; set; }
        [Column("RecentGameIds", TypeName = "jsonb")]
        public Queue<string> RecentMobileGameIds { get; set; }
        [Column("Bets", TypeName = "jsonb")]
        public List<Bet> Bets { get; set; }
        [Column("Bets", TypeName = "jsonb")]
        public List<Bet> MobileBets { get; set; }
    }
}
