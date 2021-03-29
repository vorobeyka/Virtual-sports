using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using VirtualSports.BLL.Services.DatabaseServices.Impl;
using VirtualSports.DAL.Contexts;
using Xunit;

namespace VirtualSports.Tests.DatabaseServices.Tests.Auth.Tests
{
    public class RegisterTest
    {
        [Fact]
        public async Task Should_Return_Token_When_New_User()
        {
            //Arrange
            var login = "test@gmail.com";
            var password = "12345678";
            var cancellationToken = new CancellationToken();
            var usersMock = new Mock<DbSet<DAL.Entities.User>>();
            usersMock.Setup(u => u.AnyAsync(cancellationToken)).ReturnsAsync(false); 
            usersMock.Setup(u => u.AddAsync(new DAL.Entities.User(), cancellationToken));
            var options = new DbContextOptions<DatabaseManagerContext>();
            var dbContext = new DatabaseManagerContext(options) {Users = usersMock.Object};

            var authService = new AuthService(dbContext);


            //Act
            var token = await authService.RegisterUserAsync(login, password, cancellationToken);

            //Assert
            Assert.NotNull(token);
            var securityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var loginResult = securityToken.Claims.ToList()[0].Value;
            Assert.Equal(login, loginResult);
        }
    }
}