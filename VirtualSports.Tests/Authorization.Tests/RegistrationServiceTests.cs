using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VirtualSports.BLL.Services.DatabaseServices.Impl;
using VirtualSports.DAL.Contexts;
using Xunit;

namespace VirtualSports.Tests.Authorization.Tests
{
    public class RegistrationServiceTests : IDisposable
    {
        private readonly DatabaseManagerContext _context;

        public RegistrationServiceTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkNpgsql()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<DatabaseManagerContext>();
            builder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=qwerty")
                .UseInternalServiceProvider(serviceProvider);
            
            _context = new DatabaseManagerContext(builder.Options);
            
            _context.Database.Migrate();

        }

        //[Fact]
        public async Task Register()
        {
            //Arrange
            
            var dbAuthService = new AuthService(_context);
            var login = "test@gmail.com";

            //Act
            var token =
                await dbAuthService.RegisterUserAsync(login, "12345678", new CancellationToken());
            var securityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var loginResult = securityToken.Claims.ToList()[0].Value;

            //Verify the results
            Assert.Equal(login, loginResult);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}