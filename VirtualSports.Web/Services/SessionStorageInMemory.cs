using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using VirtualSports.Web.Contexts;
using VirtualSports.Web.Models.DatabaseModels;

namespace VirtualSports.Web.Services
{
    /// <inheritdoc />
    public class SessionStorageInMemory : ISessionStorage
    {
        private readonly ConcurrentDictionary<string, byte> _storage;

        /// <summary>
        /// 
        /// </summary>
        public SessionStorageInMemory()
        {
            _storage = new ConcurrentDictionary<string, byte>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public SessionStorageInMemory(DatabaseManagerContext dbContext)
        {
            foreach (var s in dbContext.ExpSessions.AsQueryable())
            {
                _storage.TryAdd(s.Token, 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public void Add(string token)
        {
            _storage.TryAdd(token, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool Contains(string token)
        {
            return _storage.TryGetValue(token, out _);
        }
    }
}