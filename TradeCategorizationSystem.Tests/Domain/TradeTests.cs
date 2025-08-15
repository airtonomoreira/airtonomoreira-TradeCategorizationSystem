using System;
using TradeCategorizationSystem.Domain;
using Xunit;

namespace TradeCategorizationSystem.Tests.Domain
{
    public class TradeTests
    {
        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange
            var value = 1000000;
            var clientSector = "Private";
            var nextPaymentDate = DateTime.Now.AddDays(30);

            // Act
            var trade = new Trade(value, clientSector, nextPaymentDate);

            // Assert
            Assert.Equal(value, trade.Value);
            Assert.Equal(clientSector, trade.ClientSector);
            Assert.Equal(nextPaymentDate, trade.NextPaymentDate);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_WithInvalidValue_ShouldThrowArgumentException(double value)
        {
            // Arrange
            var clientSector = "Private";
            var nextPaymentDate = DateTime.Now.AddDays(30);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Trade(value, clientSector, nextPaymentDate));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WithInvalidClientSector_ShouldThrowArgumentException(string clientSector)
        {
            // Arrange
            var value = 1000000;
            var nextPaymentDate = DateTime.Now.AddDays(30);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Trade(value, clientSector, nextPaymentDate));
        }
    }
}