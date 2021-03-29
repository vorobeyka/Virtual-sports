using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Moq;
using VirtualSports.BLL.Services;
using VirtualSports.BLL.Services.DatabaseServices;
using VirtualSports.Web.Controllers;
using Xunit;

namespace VirtualSports.Tests.Authorization.Tests
{
    public class LogoutTests
    {
        [Fact]
        public async Task Should_Return_Unauthorized_When_AuthorizationHeader_Does_Not_Contain_Token()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var logger = new Mock<ILogger<AuthController>>();
            var authService = new Mock<IAuthService>();
            var sessionStorage = new Mock<ISessionStorage>();


            var authController = new AuthController(logger.Object, authService.Object, sessionStorage.Object);
            authController.Unauthorized();
            //Act
            var result = await authController.LogoutAsync(cancellationToken);

            //Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Should_Return_Ok_When_Authorized()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            var token = "asdFkjkljafF32fdsF";

            var authService = new Mock<IAuthService>();
            var sessionStorage = new Mock<ISessionStorage>();
            var logger = new Mock<ILogger<AuthController>>();

            sessionStorage.Setup(s => s.Contains(token)).Returns(false);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer " + token;
            var authController = new AuthController(logger.Object, authService.Object, sessionStorage.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            //Act

            var result = await authController.LogoutAsync(cancellationToken);


            //Assert
            Assert.IsType<OkResult>(result);
        }
    }
}