using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ConvertDigits.Controllers;
using ConvertDigits.Model;

namespace ConvertDigits.Tests
{
    // Test for Integration of ConvertController
    public class ConvertControllerTests
    {
        // Prepare a test context
        private NumberContext GetTestContext()
        {
            var options = new DbContextOptionsBuilder<NumberContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new NumberContext(options);

            if (!context.Numbers.Any())
            {
                context.Numbers.Add(new Numbers { Dollars = "ONE", Cents = "" });
                context.SaveChanges();
            }

            return context;
        }

        // IT: 1
        // Test for function to get all numbers
        [Fact]
        public async Task GetAllNumbers_ReturnsAllNumbers()
        {
            // Arrange
            var context = GetTestContext();
            var controller = new ConvertController(context);

            // Act
            var result = await controller.GetAllNumbers();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Numbers>>>(result);
            var returnValue = Assert.IsType<List<Numbers>>(actionResult.Value);
            Assert.Single(returnValue);
        }

        // IT: 2
        // Test for a valid get number input
        [Fact]
        public async Task GetNumber_ReturnsCorrectNumber()
        {
            // Arrange
            var context = GetTestContext();
            var controller = new ConvertController(context);

            // Act
            var result = await controller.GetNumber(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Numbers>>(result);
            var returnValue = Assert.IsType<Numbers>(actionResult.Value);
            Assert.Equal("ONE", returnValue.Dollars);
            Assert.Equal("", returnValue.Cents);
        }

        // IT: 3
        // Test for a valid post number input
        [Fact]
        public async Task PostNumber_ValidInput_CreatesNumber()
        {
            // Arrange
            var context = GetTestContext();
            var controller = new ConvertController(context);

            // Act
            var number = new Numbers { Dollars = "123", Cents = "45" };
            var result = await controller.PostNumber(number);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Numbers>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Numbers>(createdResult.Value);
            Assert.Equal("ONE HUNDRED AND TWENTY-THREE", returnValue.Dollars);
            Assert.Equal("FORTY-FIVE", returnValue.Cents);
        }

        // IT: 4
        // Test for a Invalid post number input
        [Fact]
        public async Task PostNumber_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var context = GetTestContext();
            var controller = new ConvertController(context);
            controller.ModelState.AddModelError("Dollars", "Required");

            // Act
            var number = new Numbers { Dollars = null, Cents = null };
            var result = await controller.PostNumber(number);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }

    // Test for unit tests of NumberConverter
    public class NumberConverterTests
    {
        // UT: 1
        // Test for converting words with valid input (dollars and cents)
        [Fact]
        public void CheckString_ValidInput_ReturnsCorrectWords()
        {
            // Arrange
            NumberConverter converter = new NumberConverter();

            // Act
            var result = converter.ConvertingWords("123", "45");

            // Assert
            Assert.Equal("ONE HUNDRED AND TWENTY-THREE", result.Dollars);
            Assert.Equal("FORTY-FIVE", result.Cents);
        }

        // UT: 2
        // Test for converting words with valid input (dollars)
        [Fact]
        public void CheckString_EmptyCents_ReturnsCorrectWords()
        {
            // Arrange
            NumberConverter converter = new NumberConverter();

            // Act
            var result = converter.ConvertingWords("123", "");

            // Assert
            Assert.Equal("ONE HUNDRED AND TWENTY-THREE", result.Dollars);
            Assert.Equal("", result.Cents);
        }

        // UT: 3
        // Test for converting words with valid input (dollars only)
        [Fact]
        public void ConvertIntegersToWords_ValidInput_ReturnsCorrectWords()
        {
            // Arrange
            NumberConverter converter = new NumberConverter();

            // Act
            var result = converter.ConvertIntegersToWords(123);

            // Assert
            Assert.Equal("ONE HUNDRED AND TWENTY-THREE", result);
        }

        // UT: 4
        // Test for converting words with valid input (cents only)
        [Fact]
        public void ConvertFloatingPointToWords_ValidInput_ReturnsCorrectWords()
        {
            // Arrange
            NumberConverter converter = new NumberConverter();

            // Act
            var result = converter.ConvertFloatingPointToWords(45);

            // Assert
            Assert.Equal("FORTY-FIVE", result);
        }

        // UT: 5
        // Test for converting words with invalid input (dollars)
        [Fact]
        public void ConvertIntegersToWords_ZeroInput_ReturnsZero()
        {
            // Arrange
            NumberConverter converter = new NumberConverter();

            // Act
            var result = converter.ConvertIntegersToWords(0);

            // Assert
            Assert.Equal("ZERO", result);
        }

        // UT: 6
        // Test for converting words with negative input (dollars)
        [Fact]
        public void ConvertIntegersToWords_NegativeInput_ReturnsMinusWords()
        {
            // Arrange
            NumberConverter converter = new NumberConverter();

            // Act
            var result = converter.ConvertIntegersToWords(-123);

            // Assert
            Assert.Equal("MINUS ONE HUNDRED AND TWENTY-THREE", result);
        }

        // UT: 7
        // Test for converting integer with tens and units
        [Fact]
        public void ConvertIntegersToWords_TensAndUnits_ReturnsCorrectWords()
        {
            // Arrange
            NumberConverter converter = new NumberConverter();

            // Act
            var result = converter.ConvertIntegersToWords(23);

            // Assert
            Assert.Equal("TWENTY-THREE", result);
        }

        // UT: 8
        // Test for converting integer with 11-19 tens
        [Fact] 
        public void ConvertIntegersToWords_ElevenToNineteen_ReturnsCorrectWords()
        {
            // Arrange
            NumberConverter converter = new NumberConverter();

            // Act
            var result = converter.ConvertIntegersToWords(15);

            // Assert
            Assert.Equal("FIFTEEN", result);
        }

        // UT: 9
        // Test for converting integer with hundreds
        [Fact]
        public void ConvertIntegersToWords_Hundreds_ReturnsCorrectWords()
        {
            // Arrange
            NumberConverter converter = new NumberConverter();

            // Act
            var result = converter.ConvertIntegersToWords(300);

            // Assert
            Assert.Equal("THREE HUNDRED", result);
        }

        // UT: 10
        // Test for converting integer with thousands
        [Fact]
        public void ConvertIntegersToWords_Thousands_ReturnsCorrectWords()
        {
            // Arrange
            NumberConverter converter = new NumberConverter();

            // Act
            var result = converter.ConvertIntegersToWords(4000);

            // Assert
            Assert.Equal("FOUR THOUSAND", result);
        }

        // UT: 11
        // Test for converting integer with millions
        [Fact]
        public void ConvertIntegersToWords_Millions_ReturnsCorrectWords()
        {
            // Arrange
            NumberConverter converter = new NumberConverter();

            // Act
            var result = converter.ConvertIntegersToWords(5000000);

            // Assert
            Assert.Equal("FIVE MILLION", result);
        }
    }
}