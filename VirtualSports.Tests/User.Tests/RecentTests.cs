/*using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.TestPlatform.Common.Exceptions;
using Moq;
using VirtualSports.Web.Controllers;
using VirtualSports.Web.Models;
using VirtualSports.Web.Models.DatabaseModels;
using VirtualSports.Web.Services;
using VirtualSports.Web.Services.DatabaseServices;
using Xunit;

namespace VirtualSports.Tests.User.Tests
{
    public class RecentTests
    {
        [Fact]
        public async Task Should_Return_OkResult_And_Recent_Games()
        {
            //Arrange
            var login = "test@pm.com";
            var platform = PlatformType.Andriod;
            var cancellationToken = new CancellationTokenSource().Token;
            var recent = new List<Game>();

            var userService = new Mock<IDatabaseUserService>();
            var logger = new Mock<ILogger<UserController>>();

            userService.Setup(u =>
                u.GetRecentAsync(login, platform, cancellationToken)).ReturnsAsync(recent);

            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, login),
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            var controller = new UserController(logger.Object, userService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new List<ClaimsIdentity>() { claimsIdentity }.AsEnumerable())
                    }
                },
                Platform = "android"
            };

            //Act
            var result = await controller.GetRecent(cancellationToken);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnRecent = Assert.IsType<List<Game>>(okResult.Value);
            userService.Verify(service => service.GetRecentAsync(login, platform, cancellationToken), Times.Once);
            Assert.Equal(recent, returnRecent);
        }

        [Fact]
        public async Task Should_Return_NotFound()
        {
            //Arrange
            var login = "test@pm.com";
            var platform = PlatformType.Andriod;
            var cancellationToken = new CancellationTokenSource().Token;
            var recent = default(List<Game>);

            var userService = new Mock<IDatabaseUserService>();
            var logger = new Mock<ILogger<UserController>>();

            userService.Setup(u =>
                u.GetRecentAsync(login, platform, cancellationToken)).ReturnsAsync(recent);

            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, login),
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            var controller = new UserController(logger.Object, userService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new List<ClaimsIdentity>() { claimsIdentity }.AsEnumerable())
                    }
                },
                Platform = "android"
            };

            //Act
            var result = await controller.GetRecent(cancellationToken);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            userService.Verify(service => service.GetRecentAsync(login, platform, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Should_Return_BadRequest_When_Platform_Unknown()
        {
            //Arrange
            var login = "test@pm.com";
            var platform = PlatformType.UnknownPlatform;
            var cancellationToken = new CancellationTokenSource().Token;
            var recent = new List<Game>();

            var userService = new Mock<IDatabaseUserService>();
            var logger = new Mock<ILogger<UserController>>();

            userService.Setup(u =>
                u.GetRecentAsync(login, platform, cancellationToken)).ReturnsAsync(recent);

            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, login),
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            var controller = new UserController(logger.Object, userService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new List<ClaimsIdentity>() { claimsIdentity }.AsEnumerable())
                    }
                },
                Platform = "unknown platform"
            };

            //Act
            var result = await controller.GetRecent(cancellationToken);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(badRequestResult.Value);
            userService.Verify(service => service.GetRecentAsync(login, platform, cancellationToken), Times.Never);
        }
    }
}*/