using Moq;
using NUnit.Framework;
using SimpleGraphCalculatorApp.Interfaces;
using SimpleGraphCalculatorApp.Models;
using SimpleGraphCalculatorApp.Services;
using System;
using System.IO;

namespace SimpleGraphCalculatorApp.Test
{
    [TestFixture]
    public class SettingsServiceTest
    {
        private Mock<IMessageService> mockMessageService;
        private string originalDirectory;
        private string originalFilePath;

        [SetUp]
        public void Setup()
        {
            mockMessageService = new Mock<IMessageService>();

            // Inject the mock into SettingsService
            SettingsService.SetMessageService(mockMessageService.Object);

            // SaveParameters the original paths so we can restore them later
            originalDirectory = SettingsService.FileDirectory;
            originalFilePath = SettingsService.FilePath;

            // Set custom paths for testing
            SettingsService.FileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles");
            SettingsService.FilePath = Path.Combine(SettingsService.FileDirectory, "testdata.json");

            // Ensure the directory exists before each test
            if (!Directory.Exists(SettingsService.FileDirectory))
            {
                Directory.CreateDirectory(SettingsService.FileDirectory);
            }
        }

        [TearDown]
        public void Cleanup()
        {
            // Cleanup: delete the test file and restore original paths
            if (File.Exists(SettingsService.FilePath))
            {
                File.Delete(SettingsService.FilePath);
            }

            // Restore original directory and file path
            SettingsService.FileDirectory = originalDirectory;
            SettingsService.FilePath = originalFilePath;

        }
            

        [Test]
        public void SaveAndLoad_ShouldPreserveParameters()
        {
            // Arrange     
            var input = new FunctionParameters
            {
                Amplitude = 2.5,
                Frequency = 1.2,
                Phase = 0.5,
                RangeStart = -5,
                RangeEnd = 5
            };

           
            // Act  
            SettingsService.SaveParameters(input);
            var output = SettingsService.LoadParameters();

            // Assert  
            Assert.Multiple(() =>
            {
                Assert.That(output.Amplitude, Is.EqualTo(input.Amplitude));
                Assert.That(output.Frequency, Is.EqualTo(input.Frequency));
                Assert.That(output.Phase, Is.EqualTo(input.Phase));
                Assert.That(output.RangeStart, Is.EqualTo(input.RangeStart));
                Assert.That(output.RangeEnd, Is.EqualTo(input.RangeEnd));
            });
        }


        [Test]
        public void Load_ShouldReturnDefault_WhenFileDoesNotExist()
        {            
            // Act
            var result = SettingsService.LoadParameters();

            // Assert
            Assert.That(result, Is.TypeOf<FunctionParameters>());
            Assert.That(result.Amplitude, Is.EqualTo(1.0)); // default
            Assert.That(result.Phase, Is.EqualTo(1.0)); // default
            Assert.That(result.Frequency, Is.EqualTo(1.0)); // default
            Assert.That(result.RangeStart, Is.EqualTo(-10.0)); // default
            Assert.That(result.RangeEnd, Is.EqualTo(10.0)); // default
        }

        [Test]
        public void GetParameterList_ShouldReturnEmptyListWhenFileIsEmpty()
        {
            // Arrange
            if (!File.Exists(SettingsService.FilePath))
            {
                File.WriteAllText(SettingsService.FilePath, "[]");  // Write empty JSON array
            }

            // Act
            var parameters = SettingsService.GetParameterList();

            // Assert
            Assert.IsNotNull(parameters);
            Assert.AreEqual(1, parameters.Count, "The parameters list should have 0 items, but the test asserts 1 item.");            
        }

        [Test]
        public void GetParameterList_ShouldFailIfFileDoesNotExist()
        {
            // Arrange
            if (File.Exists(SettingsService.FilePath))
            {
                File.Delete(SettingsService.FilePath);
            }

            // Act
            var parameters = SettingsService.GetParameterList();

            // Assert
            Assert.IsNull(parameters, "The parameters list should be null, but the test asserts an empty list.");
        }

        [Test]
        public void GetParameterList_ShouldShowErrorMessageWhenFileIsNotAccessible()
        {
            // Arrange   
            if (File.Exists(SettingsService.FilePath))
            {
                File.Delete(SettingsService.FilePath);
            }

            string capturedMessage = null;
            mockMessageService.Setup(m => m.ShowMessage(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((msg, title) => capturedMessage = msg);

            // Act  
            var parameters = SettingsService.GetParameterList();

            // Assert  
            mockMessageService.Verify(m => m.ShowMessage(It.Is<string>(msg => msg.Contains("Error accessing file")), "Error"), Times.Once);
            Assert.AreEqual("Error accessing file: The system cannot find the file specified.", capturedMessage);
        }

        [Test]
        public void GetParameterFromLoad_ShouldShowErrorMessageWhenFileIsNotAccessible()
        {
            // Arrange   
            if (File.Exists(SettingsService.FilePath))
            {
                File.Delete(SettingsService.FilePath);
            }

            string capturedMessage = null;
            mockMessageService.Setup(m => m.ShowMessage(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((msg, title) => capturedMessage = msg);

            // Act  
            var parameters = SettingsService.LoadParameters();

            // Assert  
            mockMessageService.Verify(m => m.ShowMessage(It.Is<string>(msg => msg.Contains("Error accessing file")), "Error"), Times.Once);
            Assert.AreEqual("Error accessing file: The system cannot find the file specified.", capturedMessage);
        }
    }
}
