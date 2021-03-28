using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VirtualSports.BLL.Services;
using VirtualSports.BLL.Services.DatabaseServices;
using VirtualSports.Web.Contracts;
using VirtualSports.Web.Controllers;
using Xunit;

namespace VirtualSports.Tests.Authorization.Tests
{
    public class RegistrationTests
    {
        [Fact]
        public async Task Should_Return_OkResult_And_Token_When_Correct_Register()
        {
            //Arrange
            var login = "virtual.sports";
            var password = "qwerty123456";
            var cancellationToken = new CancellationTokenSource().Token;
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";

            var logger = new Mock<ILogger<AuthController>>();
            var authService = new Mock<IAuthService>();
            var sessionStorage = new Mock<ISessionStorage>();

            authService.Setup(r =>
                r.RegisterUserAsync(login, password, cancellationToken)).ReturnsAsync(token);
            
            var authController = new AuthController(logger.Object, authService.Object, sessionStorage.Object);
            
            //Act

            var result = await authController.RegisterAsync(new Account(login, password), cancellationToken);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnToken = Assert.IsType<string>(okResult.Value);
            authService.Verify(service =>
                service.RegisterUserAsync(login, password, cancellationToken), Times.Once);
            Assert.Equal(token, returnToken);
        }

        [Fact]
        public async Task Should_Return_BadRequest_When_ModelState_Is_InValid()
        {
            //Arrange
            var login = "virtual.sports";
            var password = "qw";
            var cancellationToken = new CancellationTokenSource().Token;

            var logger = new Mock<ILogger<AuthController>>();
            var authService = new Mock<IAuthService>();
            var sessionStorage = new Mock<ISessionStorage>();

            authService.Setup(r =>
                r.RegisterUserAsync(login, password, cancellationToken));

            var authController = new AuthController(logger.Object, authService.Object, sessionStorage.Object);
            authController.ModelState.AddModelError("error", "small length for password");
            //Act

            var result = await authController.RegisterAsync(new Account(login, password), cancellationToken);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            authService.Verify(service =>
                service.RegisterUserAsync(login, password, cancellationToken), Times.Never);
        }

        [Fact]
        public async Task Should_Return_Conflict_When_Login_Was_Already_Used()
        {
            //Arrange
            var login = "virtual.sports";
            var password = "qwerty123456";
            var cancellationToken = new CancellationTokenSource().Token;

            var logger = new Mock<ILogger<AuthController>>();
            var authService = new Mock<IAuthService>();
            var sessionStorage = new Mock<ISessionStorage>();

            authService.Setup(r =>
                    r.RegisterUserAsync(login, password, cancellationToken))
                .ReturnsAsync(default(string));

            var authController = new AuthController(logger.Object, authService.Object, sessionStorage.Object);
            //Act

            var result = await authController.RegisterAsync(new Account(login, password), cancellationToken);

            //Assert
            var conflictResult =  Assert.IsType<ConflictObjectResult>(result);
            authService.Verify(service =>
                service.RegisterUserAsync(login, password, cancellationToken), Times.Once);
            Assert.Equal("Login has been used already.", conflictResult.Value);
        }
    }
}