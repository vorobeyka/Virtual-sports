using System.Threading;
using System.Threading.Tasks;
using VirtualSports.Web.Models;

namespace VirtualSports.Web.Services.DatabaseServices
{
    /// <summary>
    /// Database authorization service.
    /// </summary>
    public interface IDatabaseAuthService
    {
        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Jwt token.</returns>
        Task<string> RegisterUserAsync(Account account, CancellationToken cancellationToken);

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Jwt token.</returns>
        Task<string> LoginUserAsync(Account account, CancellationToken cancellationToken);

        /// <summary>
        /// Expire token.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExpireToken(string token, CancellationToken cancellationToken);
    }
}
