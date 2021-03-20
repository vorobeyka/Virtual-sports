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
        private readonly ConcurrentBag<User> _users = new ConcurrentBag<User>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> Register(User user)
        {
            _users.Add(user);
            //throw new System.NotImplementedException();
            return Task.FromResult("ok");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> FindAsync(User user)
        {
            var isRegistered = _users.FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);
            return isRegistered != null;
        }
    }
}