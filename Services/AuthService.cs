using System.Threading.Tasks;

namespace VirtualSports.BE.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthService : IAuthService
    {
        public Task<string> Register(string login, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> Login(string login, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}