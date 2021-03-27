namespace VirtualSports.Web.Contracts.ViewModels
{
    /// <summary>
    /// Model for table expired sessions.
    /// </summary>
    public class ExpSessionView
    {
        /// <summary>
        /// Unique id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Jwt token.
        /// </summary>
        public string Token { get; set; }

        public ExpSessionView(string token)
        {
            Token = token;
        }
    }
}