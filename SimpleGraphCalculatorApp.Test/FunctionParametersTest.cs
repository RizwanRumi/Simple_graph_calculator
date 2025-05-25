using NUnit.Framework;
using SimpleGraphCalculatorApp.Models;
using System;

namespace SimpleGraphCalculatorApp.Test
{
    [TestFixture]
    public class FunctionParametersTest
    {
        private FunctionParameters _funcParams;

        [SetUp]
        public void Setup()
        {
            _funcParams = new FunctionParameters();
        }

        [TestCase("abc")]
        [TestCase("1.2.3")]
        [TestCase("")]        
        public void ParametersShouldNotAcceptInvalidDoubleStrings(string invalidInput)
        {
            // Arrange  
            double result;

            // Act  
            bool amplitudeSuccess = double.TryParse(invalidInput, out result);
            if (amplitudeSuccess)
            {
                _funcParams.Amplitude = result;
            }

            bool phaseSuccess = double.TryParse(invalidInput, out result);
            if (phaseSuccess)
            {
                _funcParams.Phase = result;
            }

            bool frequencySuccess = double.TryParse(invalidInput, out result);
            if (frequencySuccess)
            {
                _funcParams.Frequency = result;
            }

            // Assert  
            Assert.IsFalse(amplitudeSuccess, $"'{invalidInput}' should not be parsed as double for Amplitude.");
            Assert.IsFalse(phaseSuccess, $"'{invalidInput}' should not be parsed as double for Phase.");
            Assert.IsFalse(frequencySuccess, $"'{invalidInput}' should not be parsed as double for Frequency.");
        }


        [TestCase("3.14")]
        [TestCase("-1.0")]
        [TestCase("0")]
        public void ParametersShouldAcceptValidDoubleStrings(string validInput)
        {
            // Arrange  
            double result;

            // Act  
            bool amplitudeSuccess = double.TryParse(validInput, out result);
            if (amplitudeSuccess)
            {
                _funcParams.Amplitude = result;
            }

            bool phaseSuccess = double.TryParse(validInput, out result);
            if (phaseSuccess)
            {
                _funcParams.Phase = result;
            }

            bool frequencySuccess = double.TryParse(validInput, out result);
            if (frequencySuccess)
            {
                _funcParams.Frequency = result;
            }

            // Assert  
            Assert.IsTrue(amplitudeSuccess, $"'{validInput}' should be parsed as double for Amplitude.");
            Assert.IsTrue(phaseSuccess, $"'{validInput}' should be parsed as double for Phase.");
            Assert.IsTrue(frequencySuccess, $"'{validInput}' should be parsed as double for Frequency.");
        }

        

        [Test]
        public void FunctionParameters_DefaultValues_AreCorrect()
        {
            // Assert
            Assert.That(_funcParams.Amplitude, Is.EqualTo(1.0));
            Assert.That(_funcParams.Phase, Is.EqualTo(1.0));
            Assert.That(_funcParams.Frequency, Is.EqualTo(1.0));
            Assert.That(_funcParams.RangeStart, Is.EqualTo(-10.0));
            Assert.That(_funcParams.RangeEnd, Is.EqualTo(10.0));
        }

        [Test]
        public void FunctionParameters_SetValidValues_UpdatesCorrectly()
        {
            // Act
            _funcParams.Amplitude = 5.5;
            _funcParams.Phase = 2.3;
            _funcParams.Frequency = -45.0;

            // Assert
            Assert.That(_funcParams.Amplitude, Is.EqualTo(5.5));
            Assert.That(_funcParams.Phase, Is.EqualTo(2.3));
            Assert.That(_funcParams.Frequency, Is.EqualTo(-45.0));
        }

        [Test]
        public void FunctionParameters_PropertyChanged_FiresCorrectly()
        {
            // Arrange
            string changedProperty = null;
            _funcParams.PropertyChanged += (sender, e) => changedProperty = e.PropertyName;

            // Act
            _funcParams.Amplitude = 2.5;

            // Assert
            Assert.That(changedProperty, Is.EqualTo(nameof(FunctionParameters.Amplitude)));
        }

      

        [Test]
        [TestCase("Amplitude", "a")]
        [TestCase("Phase", "3.0")]
        [TestCase("Frequency", "180.0")]
        [TestCase("RangeStart", "-10.0")]
        [TestCase("RangeEnd", "10.0")]
        public void FunctionParameters_ValidValues_ReturnsCastError(string propertyName, object value)
        {
            // Act and Assert
            switch (propertyName)
            {
                case "Amplitude":
                    Assert.Throws<InvalidCastException>(() => _funcParams.Amplitude = (double)value); break;
                case "Phase":
                    Assert.Throws<InvalidCastException>(() => _funcParams.Phase = (double)value); break;
                case "Frequency":
                    Assert.Throws<InvalidCastException>(() => _funcParams.Frequency = (double)value); break;
            }            
        }
    }
}
