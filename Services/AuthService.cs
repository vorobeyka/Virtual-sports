using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VirtualSports.BE.Models;

namespace VirtualSports.BE.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly ConcurrentBag<Account> _users = new ConcurrentBag<Account>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> RegisterAsync(Account user)
        {
            var canAdd = _users.FirstOrDefault(u => u.Login == user.Login);
            if (canAdd != null) return false;
            _users.Add(user);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> FindAsync(Account user)
        {
            var isRegistered = _users.FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);
            return isRegistered != null;
        }
    }
}