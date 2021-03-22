using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BE.Contexts;
using VirtualSports.BE.Models.DatabaseModels;

namespace VirtualSports.BE.Services.DatabaseServices
{
    /// <inheritdoc />
    public class DatabaseAuthService : IDatabaseAuthService
    {
        private readonly DatabaseManagerContext _dbContext;

        public DatabaseAuthService(DatabaseManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<bool> LoginUserAsync(string login, string password, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == login && u.Password == password,
                cancellationToken);

            return user != null;
        }

        /// <inheritdoc />
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
