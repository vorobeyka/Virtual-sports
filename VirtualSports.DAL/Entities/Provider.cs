using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using VirtualSports.DAL.Models;
using VirtualSports.Lib.Models;

namespace VirtualSports.DAL.Entities
{
    [Table("Providers")]
    public class Provider
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        [Column("PlatformTypes", TypeName = "jsonb")]
        public List<PlatformType> PlatformTypes { get; set; }
    }
}
