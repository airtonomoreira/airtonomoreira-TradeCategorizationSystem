using System.Collections.Generic;
using TradeCategorizationSystem.Domain;

namespace TradeCategorizationSystem.Domain.Interfaces
{
    public interface ITradeRepository
    {
        IEnumerable<Trade> GetAll();
    }
}