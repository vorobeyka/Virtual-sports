using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BE.Contexts;
using VirtualSports.BE.Models.DatabaseModels;

namespace VirtualSports.BE.Services.DatabaseServices
{
    public class DatabaseUserService : IDatabaseUserService
    {
        private readonly DatabaseManagerContext _dbContext;

        public DatabaseUserService(DatabaseManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> LoginUserAsync(string login, string password, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(user => user.Login == login && user.Password == password,
                cancellationToken);

            if (user == null) return false;
            return true;
        }

        public async Task<bool> RegisterUserAsync(string login, string password, CancellationToken cancellationToken)
        {
            if (await _dbContext.Users.AnyAsync(user => user.Login == login, cancellationToken))
            {
                return false;
            }

            await _dbContext.Users.AddAsync(new User
            {
                Login = login,
                Password = password,
                FavouriteGameIds = new List<string>(),
                FavouriteGameMobileIds = new List<string>(),
                RecentGameIds = new List<string>(),
                RecentMobileGameIds = new List<string>()
            }, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
