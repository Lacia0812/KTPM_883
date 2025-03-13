using System;
using ASC.Tests.TestUtilities;
using ASC.WEB.Configuration;
using ASC.WEB.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ASC.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<IOptions<ApplicationSettings>> optionsMock;
        private readonly Mock<ILogger<HomeController>> loggerMock;
        private readonly Mock<HttpContext> mockHttpContext;
        private readonly FakeSession fakeSession;

        public HomeControllerTests()
        {
            optionsMock = new Mock<IOptions<ApplicationSettings>>();
            loggerMock = new Mock<ILogger<HomeController>>();
            mockHttpContext = new Mock<HttpContext>();
            fakeSession = new FakeSession();

            // Set up fake ApplicationSettings
            optionsMock.Setup(ap => ap.Value).Returns(new ApplicationSettings
            {
                ApplicationTitle = "ASC"
            });

            // Set up HttpContext with FakeSession
            mockHttpContext.SetupGet(p => p.Session).Returns(fakeSession);
        }

        [Fact]
        public void HomeController_Index_View_Test()
        {
            var controller = new HomeController(loggerMock.Object, optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void HomeController_Index_NoModel_Test()
        {
            var controller = new HomeController(loggerMock.Object, optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
            Assert.Null(result.ViewData.Model);
        }

        [Fact]
        public void HomeController_Index_Validation_Test()
        {
            var controller = new HomeController(loggerMock.Object, optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(0, result.ViewData.ModelState.ErrorCount);
        }

        [Fact]
        public void HomeController_Index_Session_Test()
        {
            var controller = new HomeController(loggerMock.Object, optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            // Save value into the session
            var settings = new ApplicationSettings { ApplicationTitle = "Test ASC" };
            fakeSession.Set("Test", settings);

            // Retrieve value from session
            var retrievedSettings = fakeSession.Get<ApplicationSettings>("Test");

            Assert.NotNull(retrievedSettings);
            Assert.Equal("Test ASC", retrievedSettings.ApplicationTitle);
        }
    }
}
