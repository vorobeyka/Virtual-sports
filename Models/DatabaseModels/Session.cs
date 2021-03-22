using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualSports.BE.Models.DatabaseModels
{
    /// <summary>
    /// Model for table sessions.
    /// </summary>
    [Table("Sessions")]
    public class Session
    {
        /// <summary>
        /// Unique id.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Jwt token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// True if jwt token is valid.
        /// </summary>
        public bool IsValid { get; set; }
    }
}