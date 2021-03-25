using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VirtualSports.Web.Controllers;
using VirtualSports.Web.Models;
using VirtualSports.Web.Models.DatabaseModels;
using VirtualSports.Web.Services.DatabaseServices;
using Xunit;

namespace VirtualSports.Tests.User.Tests
{
    public class FavouritesTests
    {
        [Fact]
        public async Task Should_Return_OkResult_And_All_Favourites()
        {
            //Arrange
            var login = "test@pm.com";
            var platform = PlatformType.Ios;
            var cancellationToken = new CancellationTokenSource().Token;
            var favourites = new List<Game>();

            var userService = new Mock<IDatabaseUserService>();
            var logger = new Mock<ILogger<UserController>>();

            userService.Setup(u =>
                u.GetFavouritesAsync(login, platform, cancellationToken)).ReturnsAsync(favourites);

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
                Platform = "ios"
            };

            //Act
            var result = await controller.GetFavourites(cancellationToken);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnFav = Assert.IsType<List<Game>>(okResult.Value);
            userService.Verify(service => service.GetFavouritesAsync(login, platform, cancellationToken), Times.Once);
            Assert.Equal(favourites, returnFav);
        }

        [Fact]
        public async Task Should_Return_NotFound()
        {
            //Arrange
            var login = "test@pm.com";
            var platform = PlatformType.Ios;
            var cancellationToken = new CancellationTokenSource().Token;
            var favourites = default(List<Game>);

            var userService = new Mock<IDatabaseUserService>();
            var logger = new Mock<ILogger<UserController>>();

            userService.Setup(u =>
                u.GetFavouritesAsync(login, platform, cancellationToken)).ReturnsAsync(favourites);

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
                Platform = "ios"
            };

            //Act
            var result = await controller.GetFavourites(cancellationToken);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            userService.Verify(service => service.GetFavouritesAsync(login, platform, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Should_Return_Ok_When_Can_Be_Added()
        {
            //Arrange
            var login = "test@pm.com";
            var platform = PlatformType.Ios;
            var cancellationToken = new CancellationTokenSource().Token;
            var gameId = "id";

            var userService = new Mock<IDatabaseUserService>();
            var logger = new Mock<ILogger<UserController>>();

            userService.Setup(u =>
                u.TryAddFavouriteAsync(login, gameId, platform, cancellationToken)).ReturnsAsync(true);

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
                Platform = "ios"
            };

            //Act
            var result = await controller.AddToFavourites(gameId, cancellationToken);

            //Assert
            Assert.IsType<OkResult>(result);
            userService.Verify(service => service.TryAddFavouriteAsync(login, gameId, platform, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Should_Return_BadRequest_When_Can_Not_Be_Added()
        {
            //Arrange
            var login = "test@pm.com";
            var platform = PlatformType.Ios;
            var cancellationToken = new CancellationTokenSource().Token;
            var gameId = "id";

            var userService = new Mock<IDatabaseUserService>();
            var logger = new Mock<ILogger<UserController>>();

            userService.Setup(u =>
                u.TryAddFavouriteAsync(login, gameId, platform, cancellationToken)).ReturnsAsync(false);

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
                Platform = "ios"
            };

            //Act
            var result = await controller.AddToFavourites(gameId, cancellationToken);

            //Assert
            var nfResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<string>(nfResult.Value);
            userService.Verify(service => service.TryAddFavouriteAsync(login, gameId, platform, cancellationToken), Times.Once);
        }
    }
}