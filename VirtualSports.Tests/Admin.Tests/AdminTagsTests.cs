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
    public class AdminTagsTests
    {
        [Fact]
        public async Task Should_Return_Ok_When_Add()
        {
            //Arrange
            var tags = new List<TagRequest>();
            var tagsDTO = new List<TagDTO>();
            var cancellationToken = new CancellationToken();

            var addService = new Mock<IAdminAddService>();
            var updateService = new Mock<IAdminUpdateService>();
            var deleteService = new Mock<IAdminDeleteService>();
            var logger = new Mock<ILogger<AdminController>>();
            var mapper = new Mock<IMapper>();

            mapper.Setup(m => m.Map<IEnumerable<TagDTO>>(tags)).Returns(tagsDTO);
            addService.Setup(a => a.AddTags(tagsDTO, cancellationToken));

            var controller = new AdminController(addService.Object, updateService.Object, deleteService.Object,
                logger.Object, mapper.Object);

            //Act

            var result = await controller.AddTags(tags, cancellationToken);

            //Assert
            mapper.Verify(m => m.Map<IEnumerable<TagDTO>>(tags), Times.Once);
            addService.Verify(a => a.AddTags(tagsDTO, cancellationToken), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Should_Return_Ok_When_Update()
        {
            //Arrange
            var tag = new TagRequest();
            var tagDTO = new TagDTO();
            var cancellationToken = new CancellationToken();

            var addService = new Mock<IAdminAddService>();
            var updateService = new Mock<IAdminUpdateService>();
            var deleteService = new Mock<IAdminDeleteService>();
            var logger = new Mock<ILogger<AdminController>>();
            var mapper = new Mock<IMapper>();

            mapper.Setup(m => m.Map<TagDTO>(tag)).Returns(tagDTO);
            updateService.Setup(u => u.UpdateTag(tagDTO, cancellationToken));

            var controller = new AdminController(addService.Object, updateService.Object, deleteService.Object,
                logger.Object, mapper.Object);

            //Act

            var result = await controller.UpdateTag(tag, cancellationToken);

            //Assert
            mapper.Verify(m => m.Map<TagDTO>(tag), Times.Once);
            updateService.Verify(u => u.UpdateTag(tagDTO, cancellationToken));
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Should_Return_Ok_When_Delete()
        {
            //Arrange
            var id = "tag_id";
            var cancellationToken = new CancellationToken();

            var addService = new Mock<IAdminAddService>();
            var updateService = new Mock<IAdminUpdateService>();
            var deleteService = new Mock<IAdminDeleteService>();
            var logger = new Mock<ILogger<AdminController>>();
            var mapper = new Mock<IMapper>();

            deleteService.Setup(d => d.DeleteTag(id, cancellationToken));

            var controller = new AdminController(addService.Object, updateService.Object, deleteService.Object,
                logger.Object, mapper.Object);

            //Act

            var result = await controller.DeleteTag(id, cancellationToken);

            //Assert
            deleteService.Verify(d => d.DeleteTag(id, cancellationToken));
            Assert.IsType<OkResult>(result);
        }
    }
}