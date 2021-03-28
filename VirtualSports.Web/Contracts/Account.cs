using System.ComponentModel.DataAnnotations;

namespace VirtualSports.Web.Contracts
{
    /// <summary>
    /// User model.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// User's login.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty email")]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "Not valid email length")]
        [EmailAddress(ErrorMessage = "Not valid email address")]
        public string Login { get; set; }

        /// <summary>
        /// User's password.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty password")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Not valid password length")]
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
