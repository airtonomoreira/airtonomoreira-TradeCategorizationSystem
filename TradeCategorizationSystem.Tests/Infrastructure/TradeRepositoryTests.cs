using Dapper;
using System.Data.SQLite;
using System.Linq;
using TradeCategorizationSystem.Domain;
using TradeCategorizationSystem.Infrastructure.Data;
using Xunit;

namespace TradeCategorizationSystem.Tests.Infrastructure
{
    public class TradeRepositoryTests : IDisposable
    {
        private readonly SQLiteConnection _connection;

        public TradeRepositoryTests()
        {
            _connection = new SQLiteConnection("DataSource=:memory:");
            _connection.Open();
            _connection.Execute("CREATE TABLE Trades (Value REAL, ClientSector TEXT, NextPaymentDate DATETIME);");
        }

        [Fact]
        public void GetAll_ShouldReturnAllTrades()
        {
            // Arrange
            _connection.Execute("INSERT INTO Trades (Value, ClientSector, NextPaymentDate) VALUES (1000000, 'Private', '2025-12-31');");
            _connection.Execute("INSERT INTO Trades (Value, ClientSector, NextPaymentDate) VALUES (500000, 'Public', '2025-12-31');");

            var repository = new TradeRepository(_connection);

            // Act
            var trades = repository.GetAll();

            // Assert
            Assert.Equal(2, trades.Count());
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}