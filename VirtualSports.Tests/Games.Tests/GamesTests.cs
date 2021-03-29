using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.TestPlatform.Common.Exceptions;
using Moq;
using VirtualSports.BLL.DTO;
using VirtualSports.BLL.Services.DatabaseServices;
using VirtualSports.Web.Contracts.ViewModels;
using VirtualSports.Web.Controllers;
using Xunit;

namespace VirtualSports.Tests.Games.Tests
{
    public class GamesTests
    {
        [Fact]
        public async Task Should_Return_Ok_And_AllData()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            var platform = "android";
            var rootDTO = new RootDTO();
            var rootView = new RootView();

            var logger = new Mock<ILogger<GamesController>>();
            var mapper = new Mock<IMapper>();
            var rootService = new Mock<IRootService>();

            rootService.Setup(s =>
                s.GetRootAsync(platform, cancellationToken)).ReturnsAsync(rootDTO);
            mapper.Setup(m => m.Map<RootView>(rootDTO)).Returns(rootView);

            var controller =
                new GamesController(logger.Object, mapper.Object, rootService.Object) {Platform = platform};
            //Act
            var result = await controller.GetAllData(cancellationToken);

            //Assert
            rootService.Verify(s => s.GetRootAsync(platform, cancellationToken), Times.Once);
            mapper.Verify(m => m.Map<RootView>(rootDTO), Times.Once);
            var view = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(rootView, view.Value);
        }

        [Fact]
        public async Task Should_Return_Ok_And_GameView()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            var platform = "android";
            var gameDTO = new GameDTO();
            var gameView = new GameView();
            var gameId = "test";
            var login = "test@pm";

            var logger = new Mock<ILogger<GamesController>>();
            var mapper = new Mock<IMapper>();
            var rootService = new Mock<IRootService>();
            var userService = new Mock<IUserService>();

            userService.Setup(u => u.AddRecentAsync(login, gameId, platform, cancellationToken));
            rootService.Setup(s => s.GetGameAsync(gameId, cancellationToken)).ReturnsAsync(gameDTO);
            mapper.Setup(m => m.Map<GameView>(gameDTO)).Returns(gameView);

            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, login),
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            var controller = new GamesController(logger.Object, mapper.Object, rootService.Object)
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
            var result = await controller.PlayGame(cancellationToken, gameId, userService.Object);

            //Assert
            rootService.Verify(s => s.GetGameAsync(gameId, cancellationToken), Times.Once);
            userService.Verify(s => s.AddRecentAsync(login, gameId, platform, cancellationToken), Times.Once);
            mapper.Verify(m => m.Map<GameView>(gameDTO), Times.Once);
            
            var view = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(gameView, view.Value);
        }

        [Fact]
        public async Task Should_Return_BadRequest_When_Login_Is_NullOrEmpty()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            var platform = "android";
            var gameDTO = new GameDTO();
            var gameView = new GameView();
            var gameId = "test";
            var login = "test@pm";

            var logger = new Mock<ILogger<GamesController>>();
            var mapper = new Mock<IMapper>();
            var rootService = new Mock<IRootService>();
            var userService = new Mock<IUserService>();

            var controller = new GamesController(logger.Object, mapper.Object, rootService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                },
                Platform = platform
            };
            //Act
            var result = await controller.PlayGame(cancellationToken, gameId, userService.Object);

            //Assert
            rootService.Verify(s => s.GetGameAsync(gameId, cancellationToken), Times.Never);
            userService.Verify(s => s.AddRecentAsync(login, gameId, platform, cancellationToken), Times.Never);
            mapper.Verify(m => m.Map<GameView>(gameDTO), Times.Never);

            var view = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid user!", view.Value);
        }
    }
}