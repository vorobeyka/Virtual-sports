using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualSports.DAL.Entities
{
    /// <summary>
    /// Model for table expired sessions.
    /// </summary>
    [Table("Expired Sessions")]
    public class ExpSession
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

        public ExpSession(string token)
        {
            Token = token;
        }
    }
}