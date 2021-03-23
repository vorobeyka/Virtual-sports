using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BE.Models;

namespace VirtualSports.Web.Services.DatabaseServices
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDatabaseAuthService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Jwt token.</returns>
        Task<string> RegisterUserAsync(Account account, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Jwt token.</returns>
        Task<string> LoginUserAsync(Account account, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExpireToken(string token, CancellationToken cancellationToken);
    }
}
