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
    public class HistoryTests
    {
        [Fact]
        public async Task Should_Return_OkResult_And_Bets_List()
        {
            //Arrange
            var login = "test@pm.com";
            var platform = PlatformType.Andriod;
            var cancellationToken = new CancellationTokenSource().Token;
            var history = new List<Bet>();

            var userService = new Mock<IDatabaseUserService>();
            var logger = new Mock<ILogger<UserController>>();

            userService.Setup(u => 
                u.GetBetsStoryAsync(login, platform, cancellationToken)).ReturnsAsync(history);

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
                        User = new ClaimsPrincipal(new List<ClaimsIdentity>(){claimsIdentity}.AsEnumerable())
                    }
                },
                Platform = "android"
            };

            //Act
            var result = await controller.GetBetHistory(cancellationToken);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnHistory = Assert.IsType<List<Bet>>(okResult.Value);
            userService.Verify(service => service.GetBetsStoryAsync(login, platform, cancellationToken), Times.Once);
            Assert.Equal(history, returnHistory);
        }
    }
}*/