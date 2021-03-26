using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using VirtualSports.Lib.Models;

namespace VirtualSports.DAL.Entities
{
    [Table("Users")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        [Column("FavouriteGameIds", TypeName = "jsonb")]
        public List<string> FavouriteGameIds { get; set; }
        [Column("RecentGameIds", TypeName = "jsonb")]
        public Dictionary<PlatformType, Queue<string>> RecentGameIds { get; set; }
        [Column("Bets", TypeName = "jsonb")]
        public List<Bet> Bets { get; set; }
    }
}
