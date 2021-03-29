using Autofac.Extras.Moq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using VirtualSports.BLL.DTO;
using VirtualSports.BLL.Services.DatabaseServices;
using VirtualSports.Web.Contracts.ViewModels;
using VirtualSports.Web.Controllers;
using Xunit;


namespace VirtualSports.Tests.User.Tests
{
    public class RecentTests
    {
        [Fact]
        public async Task Should_Return_Ok_When_Requesting_Favourites()
        {
            using (var mock = AutoMock.GetLoose())
            {
                //Arrange
                var cancellationToken = new CancellationToken();
                var platform = "Android";
                var login = "test@pm";
                var gameDtos = new List<GameDTO>();

                var logger = mock.Mock<ILogger<UserController>>();
                var mapper = mock.Mock<IMapper>();
                var userService = mock.Mock<IUserService>();

                userService.Setup(u =>
                    u.GetRecentAsync(login, platform, cancellationToken)).ReturnsAsync(gameDtos);

                var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, login),
            };
                var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                var controller = new UserController(logger.Object, userService.Object, mapper.Object)
                {
                    ControllerContext = new ControllerContext()
                    {
                        HttpContext = new DefaultHttpContext()
                        {
                            User = new ClaimsPrincipal(new List<ClaimsIdentity>() { claimsIdentity }.AsEnumerable())
                        }
                    },
                    Platform = platform
                };

                //Act
                var result = await controller.GetRecent(cancellationToken);

                //Assert
                Assert.NotNull(result);
                userService.Verify(u =>
                    u.GetRecentAsync(login, platform, cancellationToken), Times.Once);
                Assert.IsType<ActionResult<IEnumerable<GameView>>>(result);
            }
        }
    }
}