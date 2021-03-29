using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VirtualSports.BLL.DTO;
using VirtualSports.BLL.Services.AdminServices;
using VirtualSports.Web.Contracts.AdminContracts;
using VirtualSports.Web.Controllers;
using Xunit;

namespace VirtualSports.Tests.Admin.Tests
{
    public class AdminProvidersTests
    {
        [Fact]
        public async Task Should_Return_Ok_When_Add()
        {
            //Arrange
            var providers = new List<ProviderRequest>();
            var providersDTO = new List<ProviderDTO>();
            var cancellationToken = new CancellationToken();

            var addService = new Mock<IAdminAddService>();
            var updateService = new Mock<IAdminUpdateService>();
            var deleteService = new Mock<IAdminDeleteService>();
            var logger = new Mock<ILogger<AdminController>>();
            var mapper = new Mock<IMapper>();

            mapper.Setup(m => m.Map<IEnumerable<ProviderDTO>>(providers)).Returns(providersDTO);
            addService.Setup(a => a.AddProviders(providersDTO, cancellationToken));

            var controller = new AdminController(addService.Object, updateService.Object, deleteService.Object,
                logger.Object, mapper.Object);

            //Act

            var result = await controller.AddProviders(providers, cancellationToken);

            //Assert
            mapper.Verify(m => m.Map<IEnumerable<ProviderDTO>>(providers), Times.Once);
            addService.Verify(a => a.AddProviders(providersDTO, cancellationToken), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Should_Return_Ok_When_Delete()
        {
            //Arrange
            var id = "provider_id";
            var cancellationToken = new CancellationToken();

            var addService = new Mock<IAdminAddService>();
            var updateService = new Mock<IAdminUpdateService>();
            var deleteService = new Mock<IAdminDeleteService>();
            var logger = new Mock<ILogger<AdminController>>();
            var mapper = new Mock<IMapper>();

            deleteService.Setup(d => d.DeleteProvider(id, cancellationToken));

            var controller = new AdminController(addService.Object, updateService.Object, deleteService.Object,
                logger.Object, mapper.Object);

            //Act

            var result = await controller.DeleteProvider(id, cancellationToken);

            //Assert
            deleteService.Verify(d => d.DeleteProvider(id, cancellationToken));
            Assert.IsType<OkResult>(result);
        }
    }
}