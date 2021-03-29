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
    public class AdminCategoriesTests
    {
        [Fact]
        public async Task Should_Return_Ok_When_Add()
        {
            //Arrange
            var categories = new List<CategoryRequest>();
            var categoriesDTO = new List<CategoryDTO>();
            var cancellationToken = new CancellationToken();

            var addService = new Mock<IAdminAddService>();
            var updateService = new Mock<IAdminUpdateService>();
            var deleteService = new Mock<IAdminDeleteService>();
            var logger = new Mock<ILogger<AdminController>>();
            var mapper = new Mock<IMapper>();

            mapper.Setup(m => m.Map<IEnumerable<CategoryDTO>>(categories)).Returns(categoriesDTO);
            addService.Setup(a => a.AddCategories(categoriesDTO, cancellationToken));

            var controller = new AdminController(addService.Object, updateService.Object, deleteService.Object,
                logger.Object, mapper.Object);

            //Act

            var result = await controller.AddCategories(categories, cancellationToken);

            //Assert
            mapper.Verify(m => m.Map<IEnumerable<CategoryDTO>>(categories), Times.Once);
            addService.Verify(a => a.AddCategories(categoriesDTO, cancellationToken), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Should_Return_Ok_When_Delete()
        {
            //Arrange
            var id = "category_id";
            var cancellationToken = new CancellationToken();

            var addService = new Mock<IAdminAddService>();
            var updateService = new Mock<IAdminUpdateService>();
            var deleteService = new Mock<IAdminDeleteService>();
            var logger = new Mock<ILogger<AdminController>>();
            var mapper = new Mock<IMapper>();

            deleteService.Setup(d => d.DeleteCategory(id, cancellationToken));

            var controller = new AdminController(addService.Object, updateService.Object, deleteService.Object,
                logger.Object, mapper.Object);

            //Act

            var result = await controller.DeleteCategory(id, cancellationToken);

            //Assert
            deleteService.Verify(d => d.DeleteCategory(id, cancellationToken));
            Assert.IsType<OkResult>(result);
        }
    }
}