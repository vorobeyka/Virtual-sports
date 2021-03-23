using System.Collections.Generic;
using System.Net.Cache;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtualSports.BE.Models;
using VirtualSports.BE.Services;
using VirtualSports.Web.Controllers;
using VirtualSports.Web.Services.DatabaseServices;
using Xunit;

namespace VirtualSports.Tests.Authorization.Test
{
    public class LogoutTests
    {
        [Fact]
        public async Task Should_Return_Unauthorized_When_AuthorizationHeader_Does_Not_Contain_Token()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var authService = new Mock<IDatabaseAuthService>();
            var sessionStorage = new Mock<ISessionStorage>();

            var authController = new AuthController(authService.Object, sessionStorage.Object);

            //Act
            var result = await authController.LogoutAsync(cancellationToken);

            //Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}