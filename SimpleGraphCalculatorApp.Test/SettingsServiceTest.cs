using NUnit.Framework;
using SimpleGraphCalculatorApp.Models;
using SimpleGraphCalculatorApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculatorApp.Test
{
    [TestFixture]
    public class SettingsServiceTest
    {
        private string originalDirectory;
        private string originalFilePath;

        [SetUp]
        public void Setup()
        {
            // Save the original paths so we can restore them later
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
            SettingsService.Save(input);
            var output = SettingsService.Load();

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
            var result = SettingsService.Load();

            // Assert
            Assert.That(result, Is.TypeOf<FunctionParameters>());
            Assert.That(result.Amplitude, Is.EqualTo(1.0)); // default
            Assert.That(result.Phase, Is.EqualTo(1.0)); // default
            Assert.That(result.Frequency, Is.EqualTo(1.0)); // default
            Assert.That(result.RangeStart, Is.EqualTo(-10.0)); // default
            Assert.That(result.RangeEnd, Is.EqualTo(10.0)); // default
        }
    }
}
