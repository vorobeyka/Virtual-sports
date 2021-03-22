using System.Threading;
using System.Threading.Tasks;

namespace VirtualSports.BE.Services.DatabaseServices
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDatabaseUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> RegisterUserAsync(string login, string password, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> LoginUserAsync(string login, string password, CancellationToken cancellationToken);
    }
}
