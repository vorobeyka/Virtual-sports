using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VirtualSports.BLL.DTO;
using VirtualSports.BLL.Services.AdminServices;
using VirtualSports.Web.Contracts.AdminRequests;
using VirtualSports.Web.Controllers;
using Xunit;

namespace VirtualSports.Tests.Admin.Tests
{
    public class AdminGamesTests
    {
        [Fact]
        public async Task Should_Return_Ok_When_Add()
        {
            //Arrange
            var games = new List<GameRequest>();
            var gamesDTO = new List<GameDTO>();
            var cancellationToken = new CancellationToken();

            var addService = new Mock<IAdminAddService>();
            var updateService = new Mock<IAdminUpdateService>();
            var deleteService = new Mock<IAdminDeleteService>();
            var logger = new Mock<ILogger<AdminController>>();
            var mapper = new Mock<IMapper>();

            mapper.Setup(m => m.Map<IEnumerable<GameDTO>>(games)).Returns(gamesDTO);
            addService.Setup(a => a.AddGames(gamesDTO, cancellationToken));

            var controller = new AdminController(addService.Object, updateService.Object, deleteService.Object,
                logger.Object, mapper.Object);

            //Act

            var result = await controller.AddGames(games, cancellationToken);

            //Assert
            mapper.Verify(m => m.Map<IEnumerable<GameDTO>>(games), Times.Once);
            addService.Verify(a => a.AddGames(gamesDTO, cancellationToken), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Should_Return_Ok_When_Update()
        {
            //Arrange
            var game = new GameRequest();
            var gameDTO = new GameDTO();
            var cancellationToken = new CancellationToken();

            var addService = new Mock<IAdminAddService>();
            var updateService = new Mock<IAdminUpdateService>();
            var deleteService = new Mock<IAdminDeleteService>();
            var logger = new Mock<ILogger<AdminController>>();
            var mapper = new Mock<IMapper>();

            mapper.Setup(m => m.Map<GameDTO>(game)).Returns(gameDTO);
            updateService.Setup(u => u.UpdateGame(gameDTO, cancellationToken));

            var controller = new AdminController(addService.Object, updateService.Object, deleteService.Object,
                logger.Object, mapper.Object);

            //Act

            var result = await controller.UpdateGame(game, cancellationToken);

            //Assert
            mapper.Verify(m => m.Map<GameDTO>(game), Times.Once);
            updateService.Verify(u => u.UpdateGame(gameDTO, cancellationToken));
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Should_Return_Ok_When_Delete()
        {
            //Arrange
            var id = "game_id";
            var cancellationToken = new CancellationToken();

            var addService = new Mock<IAdminAddService>();
            var updateService = new Mock<IAdminUpdateService>();
            var deleteService = new Mock<IAdminDeleteService>();
            var logger = new Mock<ILogger<AdminController>>();
            var mapper = new Mock<IMapper>();

            deleteService.Setup(d => d.DeleteGame(id, cancellationToken));

            var controller = new AdminController(addService.Object, updateService.Object, deleteService.Object,
                logger.Object, mapper.Object);

            //Act

            var result = await controller.DeleteGame(id, cancellationToken);

            //Assert
            deleteService.Verify(d => d.DeleteGame(id, cancellationToken));
            Assert.IsType<OkResult>(result);
        }
    }
}