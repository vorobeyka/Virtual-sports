using System.Threading.Tasks;
using VirtualSports.BE.Models;

namespace VirtualSports.BE.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> RegisterAsync(Account user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> FindAsync(Account user);
    }
}