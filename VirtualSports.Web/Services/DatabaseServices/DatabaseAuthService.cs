using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VirtualSports.Web.Contexts;
using VirtualSports.Web.Models;
using VirtualSports.Web.Models.DatabaseModels;
using VirtualSports.Web.Options;

namespace VirtualSports.Web.Services.DatabaseServices
{
    /// <inheritdoc />
    public class DatabaseAuthService : IDatabaseAuthService
    {
        private readonly DatabaseManagerContext _dbContext;

#pragma warning disable 1591
        public DatabaseAuthService(DatabaseManagerContext dbContext)
#pragma warning restore 1591
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<string> LoginUserAsync(Account account, CancellationToken cancellationToken)
        {
            var passwordHash = GetPasswordHash(account.Password);
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == account.Login && u.PasswordHash == passwordHash,
                cancellationToken);

            if (user != null)
            {
                return await GetJwtTokenAsync(account);
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<string> RegisterUserAsync(Account account, CancellationToken cancellationToken)
        {
            if (await _dbContext.Users.AnyAsync(user => user.Login == account.Login, cancellationToken))
            {
                return null;
            }

            var user = NewUser(account.Login, account.Password);
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return await GetJwtTokenAsync(account);
        }

        private static User NewUser(string login, string password)
        {
            var user = new User
            {
                Login = login,
                PasswordHash = GetPasswordHash(password),
                FavouriteGameIds = new Dictionary<PlatformType, List<string>>(),
                RecentGameIds = new Dictionary<PlatformType, Queue<string>>(),
                Bets = new Dictionary<PlatformType, List<Bet>>(),
            };
            user.FavouriteGameIds.Add(PlatformType.WebMobile, new List<string>());
            user.FavouriteGameIds.Add(PlatformType.WebDesktop, new List<string>());
            user.FavouriteGameIds.Add(PlatformType.Ios, new List<string>());
            user.FavouriteGameIds.Add(PlatformType.Andriod, new List<string>());
            user.RecentGameIds.Add(PlatformType.WebMobile, new Queue<string>());
            user.RecentGameIds.Add(PlatformType.WebDesktop, new Queue<string>());
            user.RecentGameIds.Add(PlatformType.Ios, new Queue<string>());
            user.RecentGameIds.Add(PlatformType.Andriod, new Queue<string>());
            user.Bets.Add(PlatformType.WebMobile, new List<Bet>());
            user.Bets.Add(PlatformType.WebDesktop, new List<Bet>());
            user.Bets.Add(PlatformType.Ios, new List<Bet>());
            user.Bets.Add(PlatformType.Andriod, new List<Bet>());
            return user;
        }

        private static string GetPasswordHash(string password)
        {
            var md5 = MD5.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = md5.ComputeHash(passwordBytes);
            var sb = new StringBuilder();
            foreach (var t in hashBytes)
            {
                sb.Append(t.ToString("x2"));
            }

            return sb.ToString();
        }

        private async Task<string> GetJwtTokenAsync(Account user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
            };

            var now = DateTime.UtcNow;
            var jwtToken =
                new JwtSecurityToken(JwtOptions.Issuer, JwtOptions.Audience, notBefore: now,
                    claims: claims,
                    expires: now.Add(TimeSpan.FromDays(JwtOptions.LifeTime)),
                    signingCredentials: new SigningCredentials(JwtOptions.GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return encodedJwt;
        }

        /// <inheritdoc />
        public async Task ExpireToken(string token, CancellationToken cancellationToken)
        {
            await _dbContext.ExpSessions.AddAsync(new ExpSession(token), cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
