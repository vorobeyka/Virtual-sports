using System.Threading;
using System.Threading.Tasks;

namespace VirtualSports.BLL.Services.DatabaseServices
{
    /// <summary>
    /// Database authorization service.
    /// </summary>
    public interface AuthService
    {
        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Jwt token.</returns>
        Task<string> RegisterUserAsync(string login, string password, CancellationToken cancellationToken);

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Jwt token.</returns>
        Task<string> LoginUserAsync(string login, string password, CancellationToken cancellationToken);

        /// <summary>
        /// Expire token.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExpireToken(string token, CancellationToken cancellationToken);
    }
}
