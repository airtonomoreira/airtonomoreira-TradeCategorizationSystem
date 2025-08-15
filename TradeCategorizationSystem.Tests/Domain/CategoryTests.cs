using TradeCategorizationSystem.Domain;
using Xunit;

namespace TradeCategorizationSystem.Tests.Domain
{
    public class CategoryTests
    {
        [Fact]
        public void Activate_ShouldSetActiveToTrue()
        {
            // Arrange
            var category = new Category("LOWRISK", 0, 1000000, "Public", false);

            // Act
            category.Activate();

            // Assert
            Assert.True(category.IsActive);
        }

        [Fact]
        public void Deactivate_ShouldSetActiveToFalse()
        {
            // Arrange
            var category = new Category("LOWRISK", 0, 1000000, "Public", true);

            // Act
            category.Deactivate();

            // Assert
            Assert.False(category.IsActive);
        }
    }
}