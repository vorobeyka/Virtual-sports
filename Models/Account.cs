using System.ComponentModel.DataAnnotations;

namespace VirtualSports.BE.Models
{
    /// <summary>
    /// User model.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// User's login.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(64, MinimumLength = 6)]
        public string Login { get; set; }
        
        /// <summary>
        /// User's password.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 8)]
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public Account(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}