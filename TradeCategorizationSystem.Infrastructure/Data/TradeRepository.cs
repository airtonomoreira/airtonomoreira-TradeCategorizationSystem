using Dapper;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using TradeCategorizationSystem.Domain;
using TradeCategorizationSystem.Domain.Interfaces;

namespace TradeCategorizationSystem.Infrastructure.Data
{
    public class TradeRepository : ITradeRepository
    {
        private readonly SQLiteConnection _connection;

        public TradeRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Trade> GetAll()
        {
            return _connection.Query<Trade>("SELECT * FROM Trades").ToList();
        }
    }
}