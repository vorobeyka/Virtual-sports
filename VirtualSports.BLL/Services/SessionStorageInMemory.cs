using System.Collections.Concurrent;
using VirtualSports.DAL.Contexts;

namespace VirtualSports.BLL.Services
{
    /// <inheritdoc />
    public class SessionStorageInMemory : ISessionStorage
    {
        private readonly ConcurrentDictionary<string, byte> _storage = new ConcurrentDictionary<string, byte>();


        public SessionStorageInMemory(DatabaseManagerContext dbContext)
        {
            foreach (var s in dbContext.ExpSessions.AsQueryable())
            {
                _storage.TryAdd(s.Token, 1);
            }
        }

        public void Add(string token)
        {
            _storage.TryAdd(token, 1);
        }

        public bool Contains(string token)
        {
            return _storage.TryGetValue(token, out _);
        }
    }
}