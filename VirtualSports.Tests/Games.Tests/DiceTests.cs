using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VirtualSports.BLL.Services;
using VirtualSports.BLL.Services.DatabaseServices;
using VirtualSports.Lib.Models;
using VirtualSports.Web.Contracts;
using VirtualSports.Web.Controllers;
using Xunit;

namespace VirtualSports.Tests.Games.Tests
{
    public class DiceTests
    {
        [Fact]
        public async Task Should_Return_Ok()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            var platform = "android";
            var diceBet = new DiceBetValidationModel {BetType = 0};
            var login = "test@pm";

            var logger = new Mock<ILogger<GamesController>>();
            var mapper = new Mock<IMapper>();
            var rootService = new Mock<IRootService>();
            var userService = new Mock<IUserService>();
            var diceService = new Mock<IDiceService>();

            for (var i = 1; i <= 6; i++)
            {
                diceService.Setup(d => d.GetBetResultAsync(i, (BetType) diceBet.BetType));
            }

            userService.Setup(u =>
                u.AddRecentAsync(login, "original_dice_game", platform, cancellationToken));

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
                        User = new ClaimsPrincipal(new List<ClaimsIdentity>() {claimsIdentity}.AsEnumerable())
                    }
                },
                Platform = platform
            };
            //Act
            var result = await controller.PlayDice(cancellationToken, diceBet, userService.Object, diceService.Object);

            //Assert

            for (var i = 1; i <= 6; i++)
            {
                diceService.Verify(d => d.GetBetResultAsync(i, (BetType) diceBet.BetType), Times.AtMostOnce);
            }

            userService.Verify(u =>
                u.AddRecentAsync(login, "original_dice_game", platform, cancellationToken), Times.Once);
            var resultBet = Assert.IsType<OkObjectResult>(result.Result);
            var bet = Assert.IsType<Bet>(resultBet.Value);
            Assert.Equal((BetType) diceBet.BetType, bet.BetType);
        }

        [Fact]
        public async Task Should_Return_BadRequest_When_Login_Null()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            var platform = "android";
            var diceBet = new DiceBetValidationModel { BetType = 0 };
            var login = "test@pm";

            var logger = new Mock<ILogger<GamesController>>();
            var mapper = new Mock<IMapper>();
            var rootService = new Mock<IRootService>();
            var userService = new Mock<IUserService>();
            var diceService = new Mock<IDiceService>();

            var controller = new GamesController(logger.Object, mapper.Object, rootService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                },
                Platform = platform
            };
            //Act
            var result = await controller.PlayDice(cancellationToken, diceBet, userService.Object, diceService.Object);

            //Assert

            for (var i = 1; i <= 6; i++)
            {
                diceService.Verify(d => d.GetBetResultAsync(i, (BetType)diceBet.BetType), Times.Never);
            }

            userService.Verify(u =>
                u.AddRecentAsync(login, "original_dice_game", platform, cancellationToken), Times.Never);
            var resultBet = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid user!", resultBet.Value);
        }
    }
}