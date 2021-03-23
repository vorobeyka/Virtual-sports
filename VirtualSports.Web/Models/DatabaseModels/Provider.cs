using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualSports.BE.Models.DatabaseModels
{
    [Table("Providers")]
    public class Provider
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
    }
}
