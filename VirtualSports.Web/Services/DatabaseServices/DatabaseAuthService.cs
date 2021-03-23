using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using VirtualSports.BE.Contexts;
using VirtualSports.BE.Models;
using VirtualSports.BE.Models.DatabaseModels;
using VirtualSports.BE.Options;

namespace VirtualSports.BE.Services.DatabaseServices
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

            await _dbContext.Users.AddAsync(new User
            {
                Login = account.Login,
                PasswordHash = GetPasswordHash(account.Password),
                FavouriteGameIds = new List<string>(),
                FavouriteGameMobileIds = new List<string>(),
                RecentGameIds = new List<string>(),
                RecentMobileGameIds = new List<string>()
            }, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return await GetJwtTokenAsync(account);
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
            var identity = await GetIdentityAsync(user);

            var now = DateTime.UtcNow;
            var jwtToken =
                new JwtSecurityToken(JwtOptions.Issuer, JwtOptions.Audience, notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromDays(JwtOptions.LifeTime)),
                    signingCredentials: new SigningCredentials(JwtOptions.GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return encodedJwt;
        }

        private async Task<ClaimsIdentity> GetIdentityAsync(Account user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        /// <inheritdoc />
        public async Task ExpireToken(string token, CancellationToken cancellationToken)
        {
            await _dbContext.ExpSessions.AddAsync(new ExpSession(token), cancellationToken);
        }
    }
}
