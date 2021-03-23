﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtualSports.Web.Controllers;
using VirtualSports.Web.Models;
using VirtualSports.Web.Services;
using VirtualSports.Web.Services.DatabaseServices;
using Xunit;

namespace VirtualSports.Tests.Authorization.Test
{
    public class LoginTests
    {
        [Fact]
        public async Task Should_Return_OkResult_And_Token_When_Correct_Login()
        {
            //Arrange
            var user = new Account("virtual.sports", "qwerty123456");
            var cancellationToken = new CancellationTokenSource().Token;
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";

            var authService = new Mock<IDatabaseAuthService>();
            var sessionStorage = new Mock<ISessionStorage>();

            authService.Setup(r =>
                r.LoginUserAsync(user, cancellationToken)).ReturnsAsync(token);

            var authController = new AuthController(authService.Object, sessionStorage.Object);

            //Act
            var result = await authController.LoginAsync(user, cancellationToken);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnToken = Assert.IsType<string>(okResult.Value);
            authService.Verify(service => service.LoginUserAsync(user, cancellationToken), Times.Once);
            Assert.Equal(token, returnToken);
        }

        [Fact]
        public async Task Should_Return_BadRequest_When_ModelState_Is_InValid()
        {
            //Arrange
            var user = new Account("virtual.sports", "qw");
            var cancellationToken = new CancellationTokenSource().Token;

            var authService = new Mock<IDatabaseAuthService>();
            var sessionStorage = new Mock<ISessionStorage>();

            authService.Setup(r =>
                r.LoginUserAsync(user, cancellationToken));

            var authController = new AuthController(authService.Object, sessionStorage.Object);
            authController.ModelState.AddModelError("error", "small length for password");
            
            //Act
            var result = await authController.LoginAsync(user, cancellationToken);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            authService.Verify(service => service.RegisterUserAsync(user, cancellationToken), Times.Never);
        }

        [Fact]
        public async Task Should_Return_NotFound_When_Login_Or_Password_Does_Not_Exist()
        {
            //Arrange
            var user = new Account("virtual.sports", "qwerty2228");
            var cancellationToken = new CancellationTokenSource().Token;

            var authService = new Mock<IDatabaseAuthService>();
            var sessionStorage = new Mock<ISessionStorage>();

            authService.Setup(r =>
                    r.LoginUserAsync(user, cancellationToken))
                .ReturnsAsync(default(string));

            var authController = new AuthController(authService.Object, sessionStorage.Object);
            
            //Act
            var result = await authController.LoginAsync(user, cancellationToken);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            authService.Verify(service => service.LoginUserAsync(user, cancellationToken), Times.Once);
            Assert.Equal("Wrong username or password.", notFoundResult.Value);
        }
    }
}
