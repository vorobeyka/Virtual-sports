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
using VirtualSports.DAL.Contexts;
using VirtualSports.DAL.Entities;
using VirtualSports.Lib.Models;
using VirtualSports.Web.Options;

namespace VirtualSports.BLL.Services.DatabaseServices.Impl
{
    /// <inheritdoc />
    public class AuthService : DatabaseServices.AuthService
    {
        private readonly DatabaseManagerContext _dbContext;

#pragma warning disable 1591
        public AuthService(DatabaseManagerContext dbContext)
#pragma warning restore 1591
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<string> LoginUserAsync(string login, string password, CancellationToken cancellationToken)
        {
            var passwordHash = GetPasswordHash(password);
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == login && u.PasswordHash == passwordHash,
                cancellationToken);

            if (user != null)
            {
                return await GetJwtTokenAsync(user);
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<string> RegisterUserAsync(string login, string password, CancellationToken cancellationToken)
        {
            if (await _dbContext.Users.AnyAsync(user => user.Login == login, cancellationToken))
            {
                return null;
            }

            var user = NewUser(login, password);
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return await GetJwtTokenAsync(user);
        }

        private static User NewUser(string login, string password)
        {
            var user = new User
            {
                Login = login,
                PasswordHash = GetPasswordHash(password),
                FavouriteGames = new List<Game>(),
                RecentGames = new Dictionary<string, List<Game>>(),
                Bets = new List<Bet>()
            };
            foreach (var i in AppTools.Platforms)
            {
                user.RecentGames.Add(i.ToLower(), new List<Game>());
            }
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

        private async Task<string> GetJwtTokenAsync(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, user.Login)
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
