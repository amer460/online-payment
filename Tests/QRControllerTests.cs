using Application.Common.Interfaces;
using Application.HelperModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using XCoreAssignment.Controllers;
using XCoreAssignment.Services;

namespace XCoreAssignment.Tests
{
    [TestFixture]
    public class QRControllerTests
    {

        private Mock<IQRService> _qrServiceMock;
        private Mock<IPaymentService> _paymentServiceMock;
        private QRControllerService _service;

        [SetUp]
        public void SetUp()
        {
            _qrServiceMock = new Mock<IQRService>();
            _paymentServiceMock = new Mock<IPaymentService>();

            _service = new QRControllerService(_qrServiceMock.Object, _paymentServiceMock.Object);
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var expectedViewName = "Index";
            object? expectedModel = null;
            var mockService = new Mock<IQRControllerService>();
            var ViewResultHelper = new ViewResultHelper("Index", null) ;
            mockService.Setup(x => x.IndexGet()).Returns(ViewResultHelper);
            var controller = new QRController(mockService.Object);

            // Act
            var result = controller.Index() as ViewResult;
            result.ViewData.Model = expectedModel;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedViewName, result.ViewName);
            Assert.AreEqual(expectedModel, result.ViewData.Model);
        }

        [Test]
        public void IndexGet_ReturnsViewResultHelper()
        {
            // Arrange
            var expectedViewName = "Index";

            // Act
            var result = _service.IndexGet();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedViewName, result.View);
        }



        [Test]
        public void IndexPost_WithInvalidQRCode_ReturnsExceptionViewResultHelper()
        {
            // Arrange
            var formFileMock = new Mock<IFormFile>();
            var qrServiceExceptionMessage = "Error reading QR code.";
            _qrServiceMock.Setup(x => x.ReadImage(formFileMock.Object)).Throws(new Exception(qrServiceExceptionMessage));
            var expectedViewName = "Exception";
            var expectedMessage = qrServiceExceptionMessage;

            // Act
            var result = _service.IndexPost(formFileMock.Object);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedViewName, result.View);
            Assert.AreEqual(expectedMessage, result.Model);
        }


        [Test]
        public void IndexPost_WithInvalidPaymentInfo_ReturnsExceptionViewResultHelper()
        {
            // Arrange
            var formFileMock = new Mock<IFormFile>();
            var qrServiceResult = "R:1703001370600358";
            _qrServiceMock.Setup(x => x.ReadImage(formFileMock.Object)).Returns(qrServiceResult);
            var paymentServiceExceptionMessage = "Error getting payment info.";
            _paymentServiceMock.Setup(x => x.Parse(qrServiceResult)).Throws(new Exception(paymentServiceExceptionMessage));
            var expectedViewName = "Exception";
            var expectedMessage = paymentServiceExceptionMessage;

            // Act
            var result = _service.IndexPost(formFileMock.Object);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedViewName, result.View);
            Assert.AreEqual(expectedMessage, result.Model);
        }
    }
}
